using UnityEngine;

namespace MeshTK
{
	public class EJMMesh
	{
		public Renderer mRenderer;

		public Transform targettransform;

		public Matrix4x4 LocalToWorld;

		public Mesh SharedMesh;

		private bool isSkinned;

		private MeshFilter _mFilter;

		private SkinnedMeshRenderer _smRenderer;

		public bool IsSkinned => isSkinned;

		public int vertexCount => SharedMesh.vertexCount;

		public Vector3[] vertices
		{
			get
			{
				return SharedMesh.vertices;
			}
			set
			{
				SharedMesh.vertices = value;
			}
		}

		public Vector2[] uv
		{
			get
			{
				return SharedMesh.uv;
			}
			set
			{
				SharedMesh.uv = value;
			}
		}

		public Vector3[] normals
		{
			get
			{
				return SharedMesh.normals;
			}
			set
			{
				SharedMesh.normals = value;
			}
		}

		public Vector4[] tangents
		{
			get
			{
				return SharedMesh.tangents;
			}
			set
			{
				SharedMesh.tangents = value;
			}
		}

		public Color[] colors
		{
			get
			{
				return SharedMesh.colors;
			}
			set
			{
				SharedMesh.colors = value;
			}
		}

		public int[] triangles
		{
			get
			{
				return SharedMesh.triangles;
			}
			set
			{
				SharedMesh.triangles = value;
			}
		}

		public EJMMesh(MeshFilter component)
		{
			isSkinned = false;
			_mFilter = component;
			mRenderer = component.GetComponent<Renderer>();
			targettransform = component.transform;
			SetSharedMesh(_mFilter.sharedMesh);
			RefreshMatrix();
		}

		public EJMMesh(SkinnedMeshRenderer component)
		{
			isSkinned = true;
			_smRenderer = component;
			mRenderer = component.GetComponent<Renderer>();
			targettransform = component.transform;
			SetSharedMesh(_smRenderer.sharedMesh);
			RefreshMatrix();
		}

		public void RefreshMatrix()
		{
			if (isSkinned)
			{
				Mesh mesh = new Mesh();
				_smRenderer.BakeMesh(mesh);
				Vector3 vector = mesh.bounds.center - _smRenderer.sharedMesh.bounds.center;
				if (mesh.vertices.Length > 0 && _smRenderer.sharedMesh.vertices.Length > 0)
				{
					vector = mesh.vertices[0] - _smRenderer.sharedMesh.vertices[0];
				}
				LocalToWorld = Matrix4x4.TRS(_smRenderer.transform.position + vector, _smRenderer.transform.rotation, Vector3.one);
			}
			else
			{
				LocalToWorld = _mFilter.transform.localToWorldMatrix;
			}
		}

		private static Vector3 ExtractScaleFromMatrix(Matrix4x4 matrix)
		{
			Vector3 result = new Vector3(matrix.GetColumn(0).magnitude, matrix.GetColumn(1).magnitude, matrix.GetColumn(2).magnitude);
			if (Vector3.Cross(matrix.GetColumn(0), matrix.GetColumn(1)).normalized != (Vector3)matrix.GetColumn(2).normalized)
			{
				result.x *= -1f;
			}
			return result;
		}

		public void MakeUnique()
		{
			SetSharedMesh(Object.Instantiate((Object)SharedMesh) as Mesh);
		}

		public void SetSharedMesh(Mesh mesh)
		{
			SharedMesh = mesh;
			if (isSkinned)
			{
				_smRenderer.sharedMesh = SharedMesh;
			}
			else
			{
				_mFilter.sharedMesh = SharedMesh;
			}
		}

		public void UpdateCollider()
		{
			MeshCollider component = targettransform.GetComponent<MeshCollider>();
			if (component != null)
			{
				component.sharedMesh = null;
				component.sharedMesh = SharedMesh;
				return;
			}
			BoxCollider component2 = targettransform.GetComponent<BoxCollider>();
			if (component2 != null)
			{
				component2.size = SharedMesh.bounds.size;
				component2.center = SharedMesh.bounds.center;
			}
		}

		public void ImportData(Mesh dataMesh)
		{
			SharedMesh.Clear();
			SharedMesh.vertices = dataMesh.vertices;
			SharedMesh.uv = dataMesh.uv;
			SharedMesh.uv2 = dataMesh.uv2;
			SharedMesh.colors = dataMesh.colors;
			SharedMesh.normals = dataMesh.normals;
			SharedMesh.tangents = dataMesh.tangents;
			SharedMesh.triangles = new int[dataMesh.triangles.Length];
			SharedMesh.subMeshCount = dataMesh.subMeshCount;
			for (int i = 0; i < dataMesh.subMeshCount; i++)
			{
				SharedMesh.SetTriangles(dataMesh.GetTriangles(i), i);
			}
			SharedMesh.bindposes = dataMesh.bindposes;
			SharedMesh.boneWeights = dataMesh.boneWeights;
			SharedMesh.RecalculateBounds();
		}
	}
}
