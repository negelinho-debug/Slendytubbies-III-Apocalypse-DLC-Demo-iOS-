using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace FSG.MeshAnimator
{
	[Serializable]
	public class MeshAnimation : ScriptableObject
	{
		public enum RootMotionMode
		{
			None,
			Baked,
			AppliedToTransform
		}

		private sealed class VertexEntry
		{
			private int _reserved = 8;

			public int[] TriangleIndex;

			public int[] VertexIndex;

			private int _count;

			public int Count => _count;

			public void Add(int vertIndex, int triIndex)
			{
				if (_reserved == _count)
				{
					_reserved *= 2;
					Array.Resize(ref TriangleIndex, _reserved);
					Array.Resize(ref VertexIndex, _reserved);
				}
				TriangleIndex[_count] = triIndex;
				VertexIndex[_count] = vertIndex;
				_count++;
			}

			public void PopulateArrays()
			{
				TriangleIndex = AllocatedArray<int>.Get(_reserved);
				VertexIndex = AllocatedArray<int>.Get(_reserved);
			}

			public void Clear()
			{
				_count = 0;
				AllocatedArray<int>.Return(TriangleIndex, false);
				AllocatedArray<int>.Return(VertexIndex, false);
				TriangleIndex = null;
				VertexIndex = null;
			}
		}

		internal static Dictionary<Mesh, Dictionary<MeshAnimation, Mesh[]>> generatedFrames = new Dictionary<Mesh, Dictionary<MeshAnimation, Mesh[]>>();

		public string animationName;

		public bool preGenerateFrames;

		public float playbackSpeed = 1f;

		public int smoothNormalsAngle = -1;

		public bool instantNormalCalculation;

		public bool recalculateNormalsOnRotation;

		public WrapMode wrapMode;

		[HideInInspector]
		public RootMotionMode rootMotionMode;

		[HideInInspector]
		public DeltaCompressedFrameData compressedFrameData;

		[HideInInspector]
		public string[] exposedTransforms;

		public MeshAnimationEvent[] events;

		public float length;

		[HideInInspector]
		public int frameSkip = 1;

		[NonSerialized]
		public Mesh[] frames = new Mesh[0];

		[NonSerialized]
		public int totalFrames;

		[NonSerialized]
		private bool[] generatedMeshes;

		[NonSerialized]
		private bool completelyGenerated;

		[NonSerialized]
		private MeshFrameData[] decompressedFrameData;

		[NonSerialized]
		private Quaternion lastRotation;

		private Queue<Action> mainThreadActions;

		private Dictionary<Mesh, KeyValuePair<int[], Vector3[]>> meshInfoCache;

		public static IEqualityComparer<Vector3> VectorComparer = Comparers.Create((Vector3 x, Vector3 y) => x.x == y.x && x.y == y.y && x.z == y.z, (Vector3 x) => x.GetHashCode());

		[HideInInspector]
		public MeshFrameData[] frameData
		{
			get
			{
				if (compressedFrameData == null)
				{
					return new MeshFrameData[0];
				}
				if (decompressedFrameData == null || decompressedFrameData.Length == 0)
				{
					decompressedFrameData = compressedFrameData;
				}
				return decompressedFrameData;
			}
		}

		private void OnEnable()
		{
			totalFrames = frameData.Length * frameSkip;
			generatedMeshes = new bool[totalFrames];
			if (string.IsNullOrEmpty(animationName))
			{
				animationName = name;
			}
		}

		public void GenerateFrames(Mesh baseMesh)
		{
			if (generatedFrames.ContainsKey(baseMesh) && generatedFrames[baseMesh].ContainsKey(this))
			{
				frames = generatedFrames[baseMesh][this];
				return;
			}
			int num = ((!preGenerateFrames) ? 1 : (frameData.Length * frameSkip));
			for (int i = 0; i < num; i++)
			{
				GenerateFrame(baseMesh, i);
			}
			if (!generatedFrames.ContainsKey(baseMesh))
			{
				generatedFrames.Add(baseMesh, new Dictionary<MeshAnimation, Mesh[]>());
			}
			generatedFrames[baseMesh].Add(this, frames);
		}

		public void GenerateFrameIfNeeded(Mesh baseMesh, int frame)
		{
			if (mainThreadActions != null && mainThreadActions.Count > 0)
			{
				try
				{
					mainThreadActions.Dequeue()?.Invoke();
				}
				catch (Exception message)
				{
					Debug.LogError(message);
				}
			}
			if (completelyGenerated || (frames.Length > frame && (bool)frames[frame]))
			{
				return;
			}
			if (generatedFrames.ContainsKey(baseMesh) && generatedFrames[baseMesh].ContainsKey(this) && generatedFrames[baseMesh][this].Length > frame && (bool)generatedFrames[baseMesh][this][frame])
			{
				frames[frame] = generatedFrames[baseMesh][this][frame];
				if (!completelyGenerated)
				{
					CheckFullGeneration(frame);
				}
			}
			else
			{
				GenerateFrame(baseMesh, frame);
			}
		}

		public Vector3[] GetFrame(int frame)
		{
			if (frame % frameSkip != 0)
			{
				float num = (float)frame / (float)(frameSkip * frameData.Length);
				int num2 = (int)(num * (float)frameData.Length);
				float num3 = (float)num2 / (float)frameData.Length;
				float num4 = Mathf.Clamp01((float)(num2 + 1) / (float)frameData.Length);
				if (num2 >= frameData.Length)
				{
					num2 = frameData.Length - 1;
				}
				if (num2 <= 0)
				{
					num2 = 0;
				}
				Vector3[] verts = frameData[num2].verts;
				Vector3[] array = ((num2 + 1 < frameData.Length) ? frameData[num2 + 1].verts : frameData[0].verts);
				float t = Mathf.Lerp(0f, 1f, (num - num3) / (num4 - num3));
				Vector3[] array2 = new Vector3[verts.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = Vector3.Slerp(verts[i], array[i], t);
				}
				return array2;
			}
			int num5 = frame / frameSkip;
			if (num5 >= 0 && num5 < frameData.Length)
			{
				return frameData[frame / frameSkip].verts;
			}
			return frameData[0].verts;
		}

		public MeshFrameData GetNearestFrame(int frame)
		{
			if (frame % frameSkip != 0)
			{
				float f = (float)frame / (float)frameSkip;
				frame = Mathf.RoundToInt(f);
			}
			int num = frame / frameSkip;
			if (num >= 0 && num < frameData.Length)
			{
				return frameData[frame / frameSkip];
			}
			return frameData[0];
		}

		public void DisplayFrame(MeshFilter meshFilter, int frame, int previousFrame)
		{
			if (frame != previousFrame)
			{
				meshFilter.mesh = frames[frame];
				if (recalculateNormalsOnRotation)
				{
					Quaternion rotation = meshFilter.transform.rotation;
					if (lastRotation != rotation)
					{
						lastRotation = rotation;
						RecalculateNormals(frames[frame], smoothNormalsAngle, null, null, instantNormalCalculation);
					}
				}
			}
			previousFrame = frame;
		}

		public void FireEvents(GameObject eventReciever, int frame)
		{
			for (int i = 0; i < events.Length; i++)
			{
				if (events[i].frame == frame)
				{
					events[i].FireEvent(eventReciever);
				}
			}
		}

		public void Reset()
		{
			completelyGenerated = false;
			if (generatedMeshes != null)
			{
				for (int i = 0; i < generatedMeshes.Length; i++)
				{
					generatedMeshes[i] = false;
				}
			}
			Mesh[] array = frames;
			foreach (Mesh mesh in array)
			{
				if ((bool)mesh)
				{
					UnityEngine.Object.Destroy(mesh);
				}
			}
			frames = new Mesh[0];
		}

		private void CheckFullGeneration(int newFrame)
		{
			if (completelyGenerated)
			{
				return;
			}
			generatedMeshes[newFrame] = true;
			completelyGenerated = true;
			for (int i = 0; i < generatedMeshes.Length; i++)
			{
				if (!generatedMeshes[i])
				{
					completelyGenerated = false;
					break;
				}
			}
		}

		private Mesh GenerateFrame(Mesh baseMesh, int frame)
		{
			if (frames.Length == 0)
			{
				frames = new Mesh[frameData.Length * frameSkip];
			}
			if (frame % frameSkip != 0)
			{
				float num = (float)frame / (float)(frameSkip * frameData.Length);
				int num2 = (int)(num * (float)frameData.Length);
				float num3 = (float)num2 / (float)frameData.Length;
				float num4 = Mathf.Clamp01((float)(num2 + 1) / (float)frameData.Length);
				Vector3[] verts = frameData[num2].verts;
				Vector3[] array = ((num2 + 1 < frameData.Length) ? frameData[num2 + 1].verts : frameData[0].verts);
				float t = Mathf.Lerp(0f, 1f, (num - num3) / (num4 - num3));
				Vector3[] array2 = new Vector3[verts.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = Vector3.Lerp(verts[i], array[i], t);
				}
				Mesh mesh = UnityEngine.Object.Instantiate(baseMesh);
				mesh.vertices = array2;
				mesh.RecalculateBounds();
				RecalculateNormals(mesh, smoothNormalsAngle);
				frames[frame] = mesh;
			}
			else
			{
				Mesh mesh2 = UnityEngine.Object.Instantiate(baseMesh);
				mesh2.vertices = frameData[frame / frameSkip].verts;
				mesh2.RecalculateBounds();
				RecalculateNormals(mesh2, smoothNormalsAngle);
				frames[frame] = mesh2;
			}
			if (!completelyGenerated)
			{
				CheckFullGeneration(frame);
			}
			return frames[frame];
		}

		private void RecalculateNormals(Mesh mesh, float angle)
		{
			if (angle == -1f)
			{
				mesh.RecalculateNormals();
				return;
			}
			int[] triangles = mesh.GetTriangles(0);
			Vector3[] vertices = mesh.vertices;
			if (meshInfoCache == null)
			{
				meshInfoCache = new Dictionary<Mesh, KeyValuePair<int[], Vector3[]>>();
			}
			if (!meshInfoCache.ContainsKey(mesh))
			{
				meshInfoCache.Add(mesh, new KeyValuePair<int[], Vector3[]>(triangles, vertices));
			}
			if (instantNormalCalculation)
			{
				RecalculateNormals(mesh, angle, triangles, vertices, instantNormalCalculation);
				return;
			}
			ThreadPool.QueueUserWorkItem(delegate
			{
				RecalculateNormals(mesh, angle, triangles, vertices, instantNormalCalculation);
			});
		}

		private void RecalculateNormals(Mesh mesh, float angle, int[] triangles, Vector3[] vertices, bool instant = false)
		{
			if (triangles == null)
			{
				if (meshInfoCache == null)
				{
					meshInfoCache = new Dictionary<Mesh, KeyValuePair<int[], Vector3[]>>();
				}
				if (meshInfoCache.ContainsKey(mesh))
				{
					triangles = meshInfoCache[mesh].Key;
					vertices = meshInfoCache[mesh].Value;
				}
				else
				{
					triangles = mesh.GetTriangles(0);
					vertices = mesh.vertices;
					meshInfoCache.Add(mesh, new KeyValuePair<int[], Vector3[]>(triangles, vertices));
				}
			}
			Vector3[] array = AllocatedArray<Vector3>.Get(triangles.Length / 3);
			Vector3[] normals = AllocatedArray<Vector3>.Get(vertices.Length);
			angle *= (float)Math.PI / 180f;
			PooledDictionary<Vector3, VertexEntry> pooledDictionary = PooledDictionary<Vector3, VertexEntry>.Get(vertices.Length, VectorComparer);
			for (int i = 0; i < triangles.Length; i += 3)
			{
				int num = triangles[i];
				int num2 = triangles[i + 1];
				int num3 = triangles[i + 2];
				Vector3 lhs = vertices[num2] - vertices[num];
				Vector3 rhs = vertices[num3] - vertices[num];
				Vector3 normalized = Vector3.Cross(lhs, rhs).normalized;
				int num4 = i / 3;
				array[num4] = normalized;
				VertexEntry value;
				if (!pooledDictionary.TryGetValue(vertices[num], out value))
				{
					value = GenericObjectPool<VertexEntry>.Get();
					value.PopulateArrays();
					pooledDictionary.Add(vertices[num], value);
				}
				value.Add(num, num4);
				if (!pooledDictionary.TryGetValue(vertices[num2], out value))
				{
					value = GenericObjectPool<VertexEntry>.Get();
					value.PopulateArrays();
					pooledDictionary.Add(vertices[num2], value);
				}
				value.Add(num2, num4);
				if (!pooledDictionary.TryGetValue(vertices[num3], out value))
				{
					value = GenericObjectPool<VertexEntry>.Get();
					value.PopulateArrays();
					pooledDictionary.Add(vertices[num3], value);
				}
				value.Add(num3, num4);
			}
			foreach (KeyValuePair<Vector3, VertexEntry> item in pooledDictionary)
			{
				VertexEntry value2 = item.Value;
				for (int j = 0; j < value2.Count; j++)
				{
					Vector3 vector = default(Vector3);
					for (int k = 0; k < value2.Count; k++)
					{
						if (value2.VertexIndex[j] == value2.VertexIndex[k])
						{
							vector += array[value2.TriangleIndex[k]];
							continue;
						}
						float value3 = Vector3.Dot(array[value2.TriangleIndex[j]], array[value2.TriangleIndex[k]]);
						value3 = Mathf.Clamp(value3, -0.99999f, 0.99999f);
						float num5 = Mathf.Acos(value3);
						if (num5 <= angle)
						{
							vector += array[value2.TriangleIndex[k]];
						}
					}
					normals[value2.VertexIndex[j]] = vector.normalized;
				}
				value2.Clear();
				GenericObjectPool<VertexEntry>.Return(value2);
			}
			pooledDictionary.ReturnToPool();
			if (!instant)
			{
				if (mainThreadActions == null)
				{
					mainThreadActions = new Queue<Action>();
				}
				mainThreadActions.Enqueue(() =>
				{
					if ((bool)mesh)
					{
						mesh.normals = normals;
					}
					AllocatedArray<Vector3>.Return(normals, false);
				});
			}
			else
			{
				mesh.normals = normals;
				AllocatedArray<Vector3>.Return(normals, false);
			}
		}
	}
}
