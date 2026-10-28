using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace FSG.MeshAnimator
{
	[AddComponentMenu("Miscellaneous/Mesh Animator")]
	[RequireComponent(typeof(MeshFilter))]
	public class MeshAnimator : MonoBehaviour
	{
		[Serializable]
		public class MeshAnimatorLODLevel
		{
			public int fps;

			public float distance;
		}

		private struct CurrentCrossFade
		{
			public MeshFrameData fromFrame;

			public MeshFrameData toFrame;

			public int framesNeeded;

			public int currentFrame;

			public int generatedFrame;

			public bool isFading;

			public float endTime;

			public CrossFadeFrameData frame;

			public void Reset()
			{
				fromFrame = null;
				toFrame = null;
				isFading = false;
				endTime = 0f;
				currentFrame = 0;
				generatedFrame = -1;
				framesNeeded = 0;
				ReturnFrame();
			}

			public void PopulateFrame(int length)
			{
				if (frame == null)
				{
					frame = new CrossFadeFrameData();
				}
				if (frame.positions == null)
				{
					frame.positions = AllocatedArray<Vector3>.Get(length);
				}
				if (frame.normals == null)
				{
					frame.normals = AllocatedArray<Vector3>.Get(length);
				}
			}

			public void ReturnFrame()
			{
				if (frame != null)
				{
					if (frame.positions != null)
					{
						AllocatedArray<Vector3>.Return(frame.positions, false);
					}
					if (frame.normals != null)
					{
						AllocatedArray<Vector3>.Return(frame.normals, false);
					}
					frame.positions = null;
					frame.normals = null;
				}
			}
		}

		private class CrossFadeFrameData
		{
			public Vector3[] positions;

			public Vector3[] normals;

			public Bounds bounds;
		}

		public Mesh baseMesh;

		public MeshAnimation defaultAnimation;

		public MeshAnimation[] animations;

		public float speed = 1f;

		public bool updateWhenOffscreen;

		public bool playAutomatically = true;

		public bool resetOnEnable = true;

		public GameObject eventReciever;

		public int FPS = 30;

		public bool skipLastLoopFrame;

		public bool recalculateCrossfadeNormals;

		[HideInInspector]
		public MeshFilter meshFilter;

		public Action<string> OnAnimationFinished;

		public Action OnFrameUpdated;

		public Action<bool> OnVisibilityChanged;

		public int currentFrame;

		public Transform LODCamera;

		public MeshAnimatorLODLevel[] LODLevels = new MeshAnimatorLODLevel[0];

		[HideInInspector]
		public float nextTick;

		private Dictionary<string, int> animIndexes = new Dictionary<string, int>();

		private int currentAnimIndex;

		private bool isVisible = true;

		private float lastFrameTime;

		private bool pingPong;

		private bool isPaused;

		private float currentAnimTime;

		private Mesh crossFadeMesh;

		private Queue<string> queuedAnims;

		private CurrentCrossFade currentCrossFade;

		private int currentLodLevel;

		private Transform mTransform;

		private Dictionary<string, Transform> childMap;

		private bool initialized;

		private int previousEventFrame = -1;

		private static List<Thread> cfThreads = new List<Thread>();

		private static bool shutDownThreads = false;

		private static Queue<MeshAnimator> crossfadeAnimators = new Queue<MeshAnimator>();

		private static AutoResetEvent crossfadeWaitHandle = new AutoResetEvent(false);

		private static Dictionary<Mesh, int> mMeshCount = new Dictionary<Mesh, int>();

		private static Dictionary<Mesh, Stack<Mesh>> crossFadePool = new Dictionary<Mesh, Stack<Mesh>>();

		public MeshAnimation currentAnimation
		{
			get
			{
				if (currentAnimIndex >= animations.Length)
				{
					currentAnimIndex = animations.Length - 1;
				}
				else if (currentAnimIndex < 0)
				{
					currentAnimIndex = 0;
				}
				return animations[currentAnimIndex];
			}
		}

		private void Start()
		{
			if (animations.Length == 0)
			{
				Debug.LogWarning("No animations for MeshAnimator on object: " + name + ". Disabling.", this);
				enabled = false;
				return;
			}
			for (int i = 0; i < animations.Length; i++)
			{
				if (animations[i] == null)
				{
					continue;
				}
				if (!animIndexes.ContainsKey(animations[i].name))
				{
					animIndexes.Add(animations[i].name, i);
				}
				animations[i].GenerateFrames(baseMesh);
				if (animations[i].exposedTransforms == null)
				{
					continue;
				}
				for (int j = 0; j < animations[i].exposedTransforms.Length; j++)
				{
					string key = animations[i].exposedTransforms[j];
					Transform transform = base.transform.Find(key);
					if ((bool)transform)
					{
						if (childMap == null)
						{
							childMap = new Dictionary<string, Transform>();
						}
						if (!childMap.ContainsKey(key))
						{
							childMap.Add(key, transform);
						}
					}
				}
			}
			if (!meshFilter)
			{
				meshFilter = GetComponent<MeshFilter>();
			}
			if (!mMeshCount.ContainsKey(baseMesh))
			{
				mMeshCount.Add(baseMesh, 1);
			}
			else
			{
				mMeshCount[baseMesh]++;
			}
			if (playAutomatically)
			{
				Play(defaultAnimation.name);
			}
			else
			{
				isPaused = true;
			}
			if ((float)cfThreads.Count < (float)MeshAnimatorManager.AnimatorCount / 15f && cfThreads.Count < 20)
			{
				shutDownThreads = false;
				Thread thread = new Thread(GenerateThreadedCrossfade);
				thread.Start();
				cfThreads.Add(thread);
			}
			initialized = true;
		}

		private void OnBecameVisible()
		{
			isVisible = true;
			if (OnVisibilityChanged != null)
			{
				OnVisibilityChanged(isVisible);
			}
		}

		private void OnBecameInvisible()
		{
			isVisible = false;
			if (OnVisibilityChanged != null)
			{
				OnVisibilityChanged(isVisible);
			}
		}

		private void OnEnable()
		{
			mTransform = transform;
			if (resetOnEnable && (bool)meshFilter)
			{
				if (playAutomatically)
				{
					Play(defaultAnimation.name);
				}
				else
				{
					isPaused = true;
				}
				if (currentAnimation != null)
				{
					currentAnimation.GenerateFrameIfNeeded(baseMesh, currentFrame);
					currentAnimation.DisplayFrame(meshFilter, currentFrame, -1);
				}
			}
			MeshAnimatorManager.AddAnimator(this);
			lastFrameTime = Time.time;
		}

		private void OnDisable()
		{
			MeshAnimatorManager.RemoveAnimator(this);
			currentCrossFade.Reset();
			currentAnimIndex = -1;
			pingPong = false;
			if (queuedAnims != null)
			{
				queuedAnims.Clear();
			}
		}

		private void OnDestroy()
		{
			if (!mMeshCount.ContainsKey(baseMesh))
			{
				return;
			}
			mMeshCount[baseMesh]--;
			ReturnCrossfadeToPool();
			if (mMeshCount[baseMesh] <= 0)
			{
				mMeshCount.Remove(baseMesh);
				foreach (KeyValuePair<MeshAnimation, Mesh[]> item in MeshAnimation.generatedFrames[baseMesh])
				{
					for (int i = 0; i < item.Value.Length; i++)
					{
						UnityEngine.Object.DestroyImmediate(item.Value[i]);
					}
				}
				MeshAnimation.generatedFrames.Remove(baseMesh);
				for (int j = 0; j < animations.Length; j++)
				{
					animations[j].Reset();
				}
				if (crossFadePool.ContainsKey(baseMesh))
				{
					while (crossFadePool[baseMesh].Count > 0)
					{
						UnityEngine.Object.Destroy(crossFadePool[baseMesh].Pop());
					}
					crossFadePool.Remove(baseMesh);
				}
			}
			if (mMeshCount.Count == 0)
			{
				cfThreads.Clear();
				shutDownThreads = true;
				crossfadeAnimators.Clear();
			}
		}

		private void FireAnimationEvents(MeshAnimation cAnim, float totalSpeed, bool finished)
		{
			if (cAnim.events.Length <= 0 || !(eventReciever != null) || previousEventFrame == currentFrame)
			{
				return;
			}
			if (finished)
			{
				if (totalSpeed < 0f)
				{
					for (int i = previousEventFrame; i >= 0; i++)
					{
						cAnim.FireEvents(eventReciever, i);
					}
					previousEventFrame = 0;
				}
				else
				{
					for (int j = previousEventFrame; j <= cAnim.totalFrames; j++)
					{
						cAnim.FireEvents(eventReciever, j);
					}
					previousEventFrame = -1;
				}
				return;
			}
			if (totalSpeed < 0f)
			{
				for (int num = currentFrame; num > previousEventFrame; num--)
				{
					cAnim.FireEvents(eventReciever, num);
				}
			}
			else
			{
				for (int k = previousEventFrame + 1; k <= currentFrame; k++)
				{
					cAnim.FireEvents(eventReciever, k);
				}
			}
			previousEventFrame = currentFrame;
		}

		private Mesh GetCrossfadeFromPool()
		{
			if (crossFadePool.ContainsKey(baseMesh) && crossFadePool[baseMesh].Count > 0)
			{
				return crossFadePool[baseMesh].Pop();
			}
			return UnityEngine.Object.Instantiate(baseMesh);
		}

		private void ReturnCrossfadeToPool()
		{
			if ((bool)crossFadeMesh)
			{
				if (!crossFadePool.ContainsKey(baseMesh))
				{
					crossFadePool.Add(baseMesh, new Stack<Mesh>());
				}
				crossFadePool[baseMesh].Push(crossFadeMesh);
				crossFadeMesh = null;
			}
			currentCrossFade.Reset();
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private void GenerateCrossfadeFrame()
		{
			if (currentCrossFade.generatedFrame == currentCrossFade.currentFrame)
			{
				return;
			}
			int length = currentCrossFade.toFrame.verts.Length;
			currentCrossFade.PopulateFrame(length);
			Vector3[] verts = currentCrossFade.fromFrame.verts;
			Vector3[] verts2 = currentCrossFade.toFrame.verts;
			CrossFadeFrameData frame = currentCrossFade.frame;
			Vector3 zero = Vector3.zero;
			Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			float t = (float)currentCrossFade.currentFrame / (float)currentCrossFade.framesNeeded;
			for (int i = 0; i < frame.positions.Length; i++)
			{
				Vector3 vector3 = Vector3.Lerp(verts[i], verts2[i], t);
				if (vector3.x < vector.x)
				{
					vector.x = vector3.x;
				}
				if (vector3.y < vector.y)
				{
					vector.y = vector3.y;
				}
				if (vector3.z < vector.z)
				{
					vector.z = vector3.z;
				}
				if (vector3.x > vector2.x)
				{
					vector2.x = vector3.x;
				}
				if (vector3.y > vector2.y)
				{
					vector2.y = vector3.y;
				}
				if (vector3.z > vector2.z)
				{
					vector2.z = vector3.z;
				}
				zero += vector3;
				frame.positions[i] = vector3;
			}
			zero /= (float)frame.positions.Length;
			currentCrossFade.frame = frame;
			currentCrossFade.frame.bounds = new Bounds(zero, vector2 - vector);
			currentCrossFade.generatedFrame = currentCrossFade.currentFrame;
		}

		private static void EnqueueAnimatorForCrossfade(MeshAnimator animator)
		{
			lock (crossfadeAnimators)
			{
				crossfadeAnimators.Enqueue(animator);
			}
			crossfadeWaitHandle.Set();
		}

		private static MeshAnimator DequeueAnimatorForCrossfade()
		{
			lock (crossfadeAnimators)
			{
				if (crossfadeAnimators.Count == 0)
				{
					return null;
				}
				return crossfadeAnimators.Dequeue();
			}
		}

		private static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
		{
			Matrix4x4 result = default(Matrix4x4);
			for (int i = 0; i < 16; i++)
			{
				result[i] = Mathf.Lerp(from[i], to[i], time);
			}
			return result;
		}

		private static void GenerateThreadedCrossfade()
		{
			while (!shutDownThreads)
			{
				try
				{
					MeshAnimator meshAnimator = null;
					while ((meshAnimator = DequeueAnimatorForCrossfade()) != null)
					{
						if (meshAnimator.currentCrossFade.isFading)
						{
							meshAnimator.GenerateCrossfadeFrame();
						}
					}
				}
				catch
				{
				}
				crossfadeWaitHandle.WaitOne();
			}
		}

		public void UpdateTick(float time)
		{
			if (!initialized)
			{
				return;
			}
			MeshAnimation meshAnimation = currentAnimation;
			if ((!isVisible && !updateWhenOffscreen) || isPaused || speed == 0f || meshAnimation.playbackSpeed == 0f)
			{
				return;
			}
			float num = ((LODLevels.Length <= currentLodLevel) ? FPS : LODLevels[currentLodLevel].fps);
			float num2 = Mathf.Abs(meshAnimation.playbackSpeed * speed);
			float num3 = Mathf.Max(0.0001f, 1f / num / num2);
			float num4 = time - lastFrameTime;
			bool flag = false;
			float num5 = ((!pingPong) ? 1 : (-1));
			if (speed * meshAnimation.playbackSpeed < 0f)
			{
				currentAnimTime -= num4 * num5 * num2;
			}
			else
			{
				currentAnimTime += num4 * num5 * num2;
			}
			if (currentAnimTime < 0f)
			{
				currentAnimTime = meshAnimation.length;
				flag = true;
			}
			else if (currentAnimTime > meshAnimation.length)
			{
				if (meshAnimation.wrapMode == WrapMode.Loop)
				{
					currentAnimTime = 0f;
				}
				flag = true;
			}
			nextTick = time + num3;
			lastFrameTime = time;
			float num6 = currentAnimTime / meshAnimation.length;
			int num7 = currentFrame;
			currentFrame = Mathf.Min(Mathf.RoundToInt(num6 * (float)meshAnimation.totalFrames), meshAnimation.totalFrames - 1);
			if (meshAnimation.wrapMode == WrapMode.PingPong && flag)
			{
				pingPong = !pingPong;
			}
			if (flag)
			{
				bool flag2 = false;
				if (queuedAnims != null && queuedAnims.Count > 0)
				{
					Play(queuedAnims.Dequeue());
					flag2 = true;
				}
				else if (meshAnimation.wrapMode != WrapMode.Loop && meshAnimation.wrapMode != WrapMode.PingPong)
				{
					nextTick = float.MaxValue;
					flag2 = true;
				}
				if (OnAnimationFinished != null)
				{
					OnAnimationFinished(meshAnimation.animationName);
				}
				if (flag2)
				{
					FireAnimationEvents(meshAnimation, num2, flag);
					return;
				}
			}
			meshAnimation.GenerateFrameIfNeeded(baseMesh, currentFrame);
			if (currentCrossFade.isFading)
			{
				if (currentCrossFade.currentFrame >= currentCrossFade.framesNeeded)
				{
					currentFrame = 0;
					num7 = -1;
					currentAnimTime = 0f;
					ReturnCrossfadeToPool();
				}
				else if (currentCrossFade.generatedFrame >= currentCrossFade.currentFrame)
				{
					if (crossFadeMesh == null)
					{
						crossFadeMesh = GetCrossfadeFromPool();
					}
					crossFadeMesh.vertices = currentCrossFade.frame.positions;
					crossFadeMesh.bounds = currentCrossFade.frame.bounds;
					if (recalculateCrossfadeNormals)
					{
						crossFadeMesh.RecalculateNormals();
					}
					meshFilter.sharedMesh = crossFadeMesh;
					currentCrossFade.ReturnFrame();
					currentCrossFade.currentFrame++;
					if (currentCrossFade.currentFrame < currentCrossFade.framesNeeded)
					{
						EnqueueAnimatorForCrossfade(this);
					}
					bool flag3 = childMap != null;
					bool flag4 = meshAnimation.rootMotionMode == MeshAnimation.RootMotionMode.AppliedToTransform;
					if (flag3 || flag4)
					{
						float num8 = (float)currentCrossFade.currentFrame / (float)currentCrossFade.framesNeeded;
						MeshFrameData fromFrame = currentCrossFade.fromFrame;
						MeshFrameData toFrame = currentCrossFade.toFrame;
						if (flag3)
						{
							for (int i = 0; i < meshAnimation.exposedTransforms.Length; i++)
							{
								Transform value = null;
								if (fromFrame.exposedTransforms.Length > i && toFrame.exposedTransforms.Length > i && childMap != null && childMap.TryGetValue(meshAnimation.exposedTransforms[i], out value))
								{
									Matrix4x4 matrix4x = fromFrame.exposedTransforms[i];
									Matrix4x4 to = toFrame.exposedTransforms[i];
									Matrix4x4 matrix = MatrixLerp(matrix4x, to, num8);
									MatrixUtils.FromMatrix4x4(value, matrix);
								}
							}
						}
						if (flag4)
						{
							Vector3 translation = Vector3.Lerp(fromFrame.rootMotionPosition, toFrame.rootMotionPosition, num8);
							Quaternion quaternion = Quaternion.Lerp(fromFrame.rootMotionRotation, toFrame.rootMotionRotation, num8);
							transform.Translate(translation, Space.Self);
							transform.Rotate(quaternion.eulerAngles * Time.deltaTime, Space.Self);
						}
					}
				}
			}
			if (!currentCrossFade.isFading)
			{
				meshAnimation.DisplayFrame(meshFilter, currentFrame, num7);
				if (currentFrame != num7)
				{
					bool flag5 = childMap != null;
					bool flag6 = meshAnimation.rootMotionMode == MeshAnimation.RootMotionMode.AppliedToTransform;
					if (flag5 || flag6)
					{
						MeshFrameData nearestFrame = meshAnimation.GetNearestFrame(currentFrame);
						if (flag5)
						{
							for (int j = 0; j < meshAnimation.exposedTransforms.Length; j++)
							{
								Transform value2 = null;
								if (nearestFrame.exposedTransforms.Length > j && childMap != null && childMap.TryGetValue(meshAnimation.exposedTransforms[j], out value2))
								{
									MatrixUtils.FromMatrix4x4(value2, nearestFrame.exposedTransforms[j]);
								}
							}
						}
						if (flag6)
						{
							transform.Translate(nearestFrame.rootMotionPosition, Space.Self);
							transform.Rotate(nearestFrame.rootMotionRotation.eulerAngles * Time.deltaTime, Space.Self);
						}
					}
				}
			}
			if (OnFrameUpdated != null)
			{
				OnFrameUpdated();
			}
			FireAnimationEvents(meshAnimation, num2, flag);
			if (LODLevels.Length <= 0 || (!(LODCamera != null) && !Camera.main))
			{
				return;
			}
			if (LODCamera == null)
			{
				LODCamera = Camera.main.transform;
			}
			float sqrMagnitude = (LODCamera.position - mTransform.position).sqrMagnitude;
			int num9 = 0;
			for (int k = 0; k < LODLevels.Length; k++)
			{
				if (sqrMagnitude > LODLevels[k].distance * LODLevels[k].distance)
				{
					num9 = k;
				}
			}
			if (currentLodLevel != num9)
			{
				currentLodLevel = num9;
			}
		}

		public void Crossfade(int index)
		{
			Crossfade(index, 0.1f);
		}

		public void Crossfade(string anim)
		{
			Crossfade(anim, 0.1f);
		}

		public void Crossfade(int index, float speed)
		{
			currentCrossFade.Reset();
			currentCrossFade.framesNeeded = (int)(speed * (float)FPS);
			currentCrossFade.isFading = true;
			currentCrossFade.endTime = Time.time + speed;
			if (currentAnimation == null)
			{
				currentCrossFade.fromFrame = defaultAnimation.GetNearestFrame(0);
			}
			else
			{
				currentCrossFade.fromFrame = currentAnimation.GetNearestFrame(currentFrame);
			}
			Play(index);
			currentCrossFade.toFrame = currentAnimation.GetNearestFrame(0);
			EnqueueAnimatorForCrossfade(this);
		}

		public void Crossfade(string anim, float speed)
		{
			int value = -1;
			if (animIndexes.TryGetValue(anim, out value))
			{
				Crossfade(value, speed);
			}
		}

		public void Play()
		{
			isPaused = false;
		}

		public void Play(string anim)
		{
			int value = -1;
			if (animIndexes.TryGetValue(anim, out value))
			{
				Play(value);
			}
		}

		public void Play(int index)
		{
			if (animations.Length > index && currentAnimIndex != index)
			{
				if (queuedAnims != null)
				{
					queuedAnims.Clear();
				}
				currentAnimIndex = index;
				currentFrame = 0;
				currentAnimTime = 0f;
				previousEventFrame = -1;
				pingPong = false;
				isPaused = false;
				nextTick = Time.time;
			}
		}

		public void PlayRandom(params string[] anim)
		{
			int num = UnityEngine.Random.Range(0, anim.Length);
			string key = anim[num];
			if (animIndexes.ContainsKey(key))
			{
				Play(animIndexes[key]);
			}
		}

		public void PlayQueued(string anim)
		{
			if (queuedAnims == null)
			{
				queuedAnims = new Queue<string>();
			}
			queuedAnims.Enqueue(anim);
		}

		public void Pause()
		{
			isPaused = true;
		}

		public void RestartAnim()
		{
			currentFrame = 0;
		}

		public void SetTime(float time, bool instantUpdate = false)
		{
			MeshAnimation meshAnimation = currentAnimation;
			if (!(meshAnimation == null))
			{
				time = Mathf.Clamp(time, 0f, meshAnimation.length);
				currentAnimTime = time;
				if (instantUpdate)
				{
					UpdateTick(Time.time);
				}
			}
		}

		public void SetTimeNormalized(float time, bool instantUpdate = false)
		{
			MeshAnimation meshAnimation = currentAnimation;
			if (!(meshAnimation == null))
			{
				time = Mathf.Clamp01(time);
				currentAnimTime = time * meshAnimation.length;
				if (instantUpdate)
				{
					UpdateTick(Time.time);
				}
			}
		}

		public MeshAnimation GetClip(string clipname)
		{
			if (animIndexes.ContainsKey(clipname))
			{
				return animations[animIndexes[clipname]];
			}
			return null;
		}
	}
}
