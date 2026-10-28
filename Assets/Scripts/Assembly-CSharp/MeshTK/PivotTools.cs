using UnityEngine;

namespace MeshTK
{
	public static class PivotTools
	{
		public static Vector3 Center(Mesh mesh)
		{
			mesh.RecalculateBounds();
			Vector3 center = mesh.bounds.center;
			Move(mesh, center);
			return center;
		}

		public static void Move(Mesh mesh, Vector3 offset)
		{
			Vector3[] vertices = mesh.vertices;
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = new Vector3(vertices[i].x - offset.x, vertices[i].y - offset.y, vertices[i].z - offset.z);
			}
			mesh.vertices = vertices;
			mesh.RecalculateBounds();
		}

		public static Vector3 Set(Mesh mesh, Vector3 offset)
		{
			mesh.RecalculateBounds();
			Vector3 vector = mesh.bounds.center - offset;
			Move(mesh, vector);
			return vector;
		}
	}
}
