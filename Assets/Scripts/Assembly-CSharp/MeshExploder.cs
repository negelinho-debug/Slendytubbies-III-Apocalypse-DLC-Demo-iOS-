using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Mesh/Mesh Exploder")]
public class MeshExploder : MonoBehaviour
{
	public enum ExplosionType
	{
		Visual,
		Physics
	}

	public struct MeshExplosionPreparation
	{
		public Mesh startMesh;

		public Vector3[] triangleNormals;

		public Vector3[] triangleCentroids;

		public int totalFrontTriangles;

		public Mesh[] physicsMeshes;

		public Quaternion[] rotations;

		public int[] frontMeshTrianglesPerSubMesh;
	}

	public ExplosionType type;

	public float minSpeed = 1f;

	public float maxSpeed = 5f;

	public float minRotationSpeed = 90f;

	public float maxRotationSpeed = 360f;

	public float fadeWaitTime = 0.5f;

	public float fadeTime = 2f;

	public bool useGravity;

	public float colliderThickness = 0.125f;

	public bool useNormals;

	public bool useMeshBoundsCenter;

	public bool allowShadows;

	public bool shadersAlreadyHandleTransparency;

	private MeshExplosionPreparation preparation;

	private static Dictionary<Mesh, MeshExplosionPreparation> cache = new Dictionary<Mesh, MeshExplosionPreparation>();

	private string ComponentName => GetType().Name;

	private void Start()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		if (component == null)
		{
			SkinnedMeshRenderer component2 = GetComponent<SkinnedMeshRenderer>();
			if (!(component2 != null))
			{
				Debug.LogError(ComponentName + " must be on a GameObject with a MeshFilter or SkinnedMeshRenderer component.");
			}
		}
		else
		{
			Mesh sharedMesh = component.sharedMesh;
			if (sharedMesh == null)
			{
				Debug.LogError("The MeshFilter does not reference a mesh.");
			}
			else
			{
				Prepare(sharedMesh);
			}
		}
	}

	private void PrepareWithoutCaching(Mesh oldMesh)
	{
		Prepare(oldMesh, false);
	}

	private void Prepare(Mesh oldMesh, bool cachePreparation = true)
	{
		if (!oldMesh.isReadable)
		{
			Debug.LogError("The mesh is not readable. Switch on the \"Read/Write Enabled\" option on the mesh's import settings.");
			return;
		}
		bool flag = type == ExplosionType.Physics;
		MeshExplosionPreparation value;
		if (cache.TryGetValue(oldMesh, out value) && ((flag && value.physicsMeshes != null) || (!flag && value.startMesh != null)))
		{
			preparation = value;
			return;
		}
		Vector3[] vertices = oldMesh.vertices;
		Vector3[] normals = oldMesh.normals;
		Vector4[] tangents = oldMesh.tangents;
		Vector2[] uv = oldMesh.uv;
		Vector2[] uv2 = oldMesh.uv2;
		Color[] colors = oldMesh.colors;
		int subMeshCount = oldMesh.subMeshCount;
		int[][] array = new int[subMeshCount][];
		int[] array2 = ((!flag) ? null : (value.frontMeshTrianglesPerSubMesh = new int[subMeshCount]));
		int num = 0;
		for (int i = 0; i < subMeshCount; i++)
		{
			int num2 = (array[i] = oldMesh.GetTriangles(i)).Length / 3;
			if (flag)
			{
				array2[i] = num2;
			}
			num += num2;
		}
		value.totalFrontTriangles = num;
		int num3 = num * 2;
		int num4 = ((!flag) ? (num3 * 3) : 6);
		if (num4 > 65534)
		{
			Debug.LogError("The mesh has too many triangles to explode. It must have " + 10922 + " or fewer triangles.");
			return;
		}
		Vector3[] array3 = new Vector3[num4];
		Vector3[] array4 = ((normals != null && normals.Length != 0) ? new Vector3[num4] : null);
		Vector4[] array5 = ((tangents != null && tangents.Length != 0) ? new Vector4[num4] : null);
		Vector2[] array6 = ((uv != null && uv.Length != 0) ? new Vector2[num4] : null);
		Vector2[] array7 = ((uv2 != null && uv2.Length != 0) ? new Vector2[num4] : null);
		Color[] array8 = ((colors != null && colors.Length != 0) ? new Color[num4] : null);
		Vector3[] array9 = (value.triangleCentroids = new Vector3[num]);
		Mesh[] array10 = (value.physicsMeshes = ((!flag) ? null : new Mesh[num]));
		Quaternion[] array11 = (value.rotations = ((!flag) ? null : new Quaternion[num]));
		int[] triangles = ((!flag) ? null : new int[6] { 0, 1, 2, 3, 4, 5 });
		int num5 = 0;
		int num6 = 0;
		Quaternion quaternion = Quaternion.identity;
		for (int j = 0; j < subMeshCount; j++)
		{
			int[] array12 = array[j];
			int num7 = array12.Length;
			int num8 = 0;
			while (num8 < num7)
			{
				int num9 = num8;
				Vector3 vector = Vector3.zero;
				for (int k = 0; k < 2; k++)
				{
					num8 = num9;
					bool flag2 = k == 1;
					while (num8 < num7)
					{
						int num10 = array12[(!flag2) ? num8 : (num9 + (2 - (num8 - num9)))];
						if (flag && num5 % 6 == 0)
						{
							Vector3 vector2 = vertices[num10];
							Vector3 vector3 = vertices[array12[num8 + 1]];
							Vector3 vector4 = vertices[array12[num8 + 2]];
							Vector3 vector5 = Vector3.Cross(vector3 - vector2, vector4 - vector2);
							array11[num6] = Quaternion.FromToRotation(Vector3.up, vector5);
							quaternion = Quaternion.FromToRotation(vector5, Vector3.up);
							vector = (array9[num6] = (vector2 + vector3 + vector4) / 3f);
						}
						if (!flag2)
						{
							array3[num5] = quaternion * (vertices[num10] - vector);
							if (array4 != null)
							{
								array4[num5] = quaternion * normals[num10];
							}
							if (array5 != null)
							{
								array5[num5] = quaternion * tangents[num10];
							}
						}
						if (array6 != null)
						{
							array6[num5] = uv[num10];
						}
						if (array7 != null)
						{
							array7[num5] = uv2[num10];
						}
						if (array8 != null)
						{
							array8[num5] = colors[num10];
						}
						num8++;
						num5++;
						if (num5 % 6 == 0)
						{
							if (flag)
							{
								MeshExplosion.SetBackTriangleVertices(array3, array4, array5, 1);
								Mesh mesh = new Mesh();
								mesh.vertices = array3;
								if (array4 != null)
								{
									mesh.normals = array4;
								}
								if (array5 != null)
								{
									mesh.tangents = array5;
								}
								if (array6 != null)
								{
									mesh.uv = array6;
								}
								if (array7 != null)
								{
									mesh.uv2 = array7;
								}
								if (array8 != null)
								{
									mesh.colors = array8;
								}
								mesh.triangles = triangles;
								array10[num6] = mesh;
								num5 = 0;
							}
							break;
						}
						if (num5 % 3 == 0 && !flag2)
						{
							break;
						}
					}
				}
				num6++;
			}
		}
		Vector3 vector6 = Vector3.zero;
		if (!flag)
		{
			Mesh mesh2 = (value.startMesh = new Mesh());
			mesh2.MarkDynamic();
			mesh2.vertices = array3;
			if (array4 != null)
			{
				mesh2.normals = array4;
			}
			if (array5 != null)
			{
				mesh2.tangents = array5;
			}
			if (array6 != null)
			{
				mesh2.uv = array6;
			}
			if (array7 != null)
			{
				mesh2.uv2 = array7;
			}
			if (array8 != null)
			{
				mesh2.colors = array8;
			}
			mesh2.subMeshCount = subMeshCount;
			num5 = 0;
			for (int l = 0; l < subMeshCount; l++)
			{
				int num11 = array[l].Length * 2;
				int[] array13 = new int[num11];
				int num12 = 0;
				while (num12 < num11)
				{
					array13[num12] = num5;
					num12++;
					num5++;
				}
				mesh2.SetTriangles(array13, l);
			}
			if (useMeshBoundsCenter)
			{
				vector6 = mesh2.bounds.center;
			}
		}
		Vector3[] array14 = (value.triangleNormals = new Vector3[num]);
		int num13 = 0;
		int num14 = 0;
		while (num14 < num)
		{
			Vector3 vector7;
			if (flag)
			{
				vector7 = array9[num14];
			}
			else
			{
				vector7 = (array3[num13] + array3[num13 + 1] + array3[num13 + 2]) / 3f;
				array9[num14] = vector7;
			}
			Vector3 vector8;
			if (useNormals && array4 != null)
			{
				if (flag)
				{
					array4 = array10[num14].normals;
					num13 = 0;
				}
				vector8 = ((array4[num13] + array4[num13 + 1] + array4[num13 + 2]) / 3f).normalized;
			}
			else
			{
				vector8 = vector7;
				if (useMeshBoundsCenter)
				{
					vector8 -= vector6;
				}
				vector8.Normalize();
			}
			array14[num14] = vector8;
			num14++;
			num13 += 6;
		}
		preparation = value;
		if (cachePreparation)
		{
			cache[oldMesh] = value;
		}
		if (fadeTime == 0f || shadersAlreadyHandleTransparency)
		{
			return;
		}
		Material[] sharedMaterials = GetComponent<Renderer>().sharedMaterials;
		foreach (Material material in sharedMaterials)
		{
			Shader shader = material.shader;
			Shader replacementFor = Fade.GetReplacementFor(shader);
			if (replacementFor == null || !replacementFor.name.StartsWith("Transparent/"))
			{
				Debug.LogWarning("Couldn't find an explicitly transparent version of shader '" + shader.name + "' so fading may not work. If the shader does support transparency then this warning can be avoided by enabling the 'Shaders Already Handle Transparency' option.");
			}
		}
	}

	public GameObject Explode()
	{
		if (preparation.startMesh == null && preparation.physicsMeshes == null)
		{
			SkinnedMeshRenderer component = GetComponent<SkinnedMeshRenderer>();
			if (component == null)
			{
				return null;
			}
			if (component.sharedMesh == null)
			{
				Debug.LogError("The SkinnedMeshRenderer does not reference a mesh.");
				return null;
			}
			Mesh mesh = new Mesh();
			component.BakeMesh(mesh);
			PrepareWithoutCaching(mesh);
		}
		string text = base.gameObject.name + " (Mesh Explosion)";
		if (type == ExplosionType.Physics)
		{
			Mesh[] physicsMeshes = preparation.physicsMeshes;
			Quaternion[] rotations = preparation.rotations;
			float num = maxSpeed - minSpeed;
			float num2 = maxRotationSpeed - minRotationSpeed;
			bool flag = minSpeed == maxSpeed;
			bool flag2 = minRotationSpeed == maxRotationSpeed;
			Vector3[] triangleCentroids = preparation.triangleCentroids;
			Vector3[] triangleNormals = preparation.triangleNormals;
			Transform transform = base.transform;
			Quaternion rotation = transform.rotation;
			Vector3 position = transform.position;
			Vector3 lossyScale = transform.lossyScale;
			bool flag3 = lossyScale != Vector3.one;
			int num3 = physicsMeshes.Length;
			int num4 = 0;
			Material[] materials = GetComponent<Renderer>().materials;
			int[] frontMeshTrianglesPerSubMesh = preparation.frontMeshTrianglesPerSubMesh;
			int num5 = frontMeshTrianglesPerSubMesh[0];
			Material material = null;
			float waitTime = fadeWaitTime + fadeTime;
			int num6 = 0;
			int num7 = 0;
			while (num6 < num3)
			{
				if (num7 == num5)
				{
					num7 = 0;
					num4++;
					num5 = frontMeshTrianglesPerSubMesh[num4];
					material = null;
				}
				GameObject gameObject = SetUpExplosionPiece(text, material == null);
				if (material == null)
				{
					material = materials[num4];
					if (fadeTime != 0f)
					{
						gameObject.GetComponent<Fade>().materials = new Material[1] { material };
					}
				}
				else if (fadeTime != 0f)
				{
					gameObject.AddComponent<DestroyAfterTime>().waitTime = waitTime;
				}
				gameObject.GetComponent<MeshRenderer>().sharedMaterials = new Material[1] { material };
				Vector3 vector = Vector3.Scale(triangleCentroids[num6], lossyScale);
				Quaternion quaternion = rotations[num6];
				vector = rotation * vector + position;
				quaternion = rotation * quaternion;
				Transform transform2 = gameObject.transform;
				transform2.localPosition = vector;
				transform2.localRotation = quaternion;
				Mesh mesh2 = preparation.physicsMeshes[num6];
				MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
				if (flag3)
				{
					mesh2 = (component2.sharedMesh = Object.Instantiate(mesh2));
					Vector3[] vertices = mesh2.vertices;
					Vector3[] normals = mesh2.normals;
					int num8 = vertices.Length;
					Quaternion quaternion2 = rotations[num6];
					Quaternion quaternion3 = Quaternion.Inverse(quaternion2);
					for (int i = 0; i < num8; i++)
					{
						vertices[i] = quaternion3 * Vector3.Scale(quaternion2 * vertices[i], lossyScale);
					}
					mesh2.vertices = vertices;
					mesh2.normals = normals;
				}
				else
				{
					component2.sharedMesh = mesh2;
				}
				Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
				rigidbody.angularVelocity = Quaternion.AngleAxis((!flag2) ? (minRotationSpeed + Random.value * num2) : minRotationSpeed, Random.onUnitSphere).eulerAngles;
				rigidbody.velocity = ((!flag) ? (minSpeed + Random.value * num) : minSpeed) * triangleNormals[num6];
				BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
				Vector3 size = mesh2.bounds.size;
				size.y = colliderThickness;
				boxCollider.size = size;
				rigidbody.SetDensity(1f);
				num6++;
				num7++;
			}
			return null;
		}
		GameObject gameObject2 = SetUpExplosionPiece(text);
		gameObject2.AddComponent<MeshExplosion>().Go(preparation, minSpeed, maxSpeed, minRotationSpeed, maxRotationSpeed, useGravity, base.transform.lossyScale);
		return gameObject2;
	}

	private GameObject SetUpExplosionPiece(string name, bool addFade = true)
	{
		GameObject gameObject = new GameObject(name);
		Transform transform = base.transform;
		Transform transform2 = gameObject.transform;
		transform2.localPosition = transform.position;
		transform2.localRotation = transform.rotation;
		gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		Renderer component = GetComponent<Renderer>();
		meshRenderer.castShadows = component.castShadows;
		meshRenderer.receiveShadows = component.receiveShadows;
		meshRenderer.sharedMaterials = component.sharedMaterials;
		meshRenderer.useLightProbes = component.useLightProbes;
		if (fadeTime != 0f)
		{
			if (addFade)
			{
				Fade fade = gameObject.AddComponent<Fade>();
				fade.waitTime = fadeWaitTime;
				fade.fadeTime = fadeTime;
				fade.replaceShaders = !shadersAlreadyHandleTransparency;
				gameObject.AddComponent<DestroyOnFadeCompletion>();
			}
			if (!allowShadows)
			{
				meshRenderer.castShadows = false;
				meshRenderer.receiveShadows = false;
			}
		}
		return gameObject;
	}
}
