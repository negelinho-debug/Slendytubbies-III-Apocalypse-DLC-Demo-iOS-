using System.IO;
using System.Text;
using UnityEngine;

namespace MeshTK
{
	public class ObjTools
	{
		public static string ExportToString(Mesh mesh, Renderer renderer, bool uselhcoords = true, bool separateSubmeshes = true)
		{
			Material[] sharedMaterials = renderer.sharedMaterials;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("o ").Append("Plane").Append("\n");
			Vector3[] vertices = mesh.vertices;
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 vector = vertices[i];
				stringBuilder.Append($"v {((!uselhcoords) ? vector.x : (0f - vector.x)):0.000000} {vector.y:0.000000} {vector.z:0.000000}\n");
			}
			stringBuilder.Append("\n");
			Vector3[] normals = mesh.normals;
			for (int j = 0; j < normals.Length; j++)
			{
				Vector3 vector2 = normals[j];
				stringBuilder.Append($"vn {vector2.x:0.000000} {vector2.y:0.000000} {vector2.z:0.000000}\n");
			}
			stringBuilder.Append("\n");
			Vector2[] uv = mesh.uv;
			for (int k = 0; k < uv.Length; k++)
			{
				Vector3 vector3 = uv[k];
				stringBuilder.Append($"vt {vector3.x:0.000000} {vector3.y:0.000000}\n");
			}
			for (int l = 0; l < mesh.subMeshCount; l++)
			{
				stringBuilder.Append("\n");
				if (separateSubmeshes)
				{
					stringBuilder.Append("g ").Append(sharedMaterials[l].name).Append("\n");
				}
				stringBuilder.Append("usemtl ").Append(sharedMaterials[l].name).Append("\n");
				stringBuilder.Append("usemap ").Append(sharedMaterials[l].name).Append("\n");
				int[] triangles = mesh.GetTriangles(l);
				for (int m = 0; m < triangles.Length; m += 3)
				{
					stringBuilder.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[(!uselhcoords) ? m : (m + 1)] + 1, triangles[(!uselhcoords) ? (m + 1) : m] + 1, triangles[m + 2] + 1));
				}
			}
			return stringBuilder.ToString();
		}

		public static void ExportToFile(Mesh mesh, Renderer renderer, string filename, bool uselhcoords = true)
		{
			if (string.IsNullOrEmpty(filename))
			{
				return;
			}
			using (StreamWriter streamWriter = new StreamWriter(filename))
			{
				streamWriter.Write(ExportToString(mesh, renderer, uselhcoords));
			}
		}
	}
}
