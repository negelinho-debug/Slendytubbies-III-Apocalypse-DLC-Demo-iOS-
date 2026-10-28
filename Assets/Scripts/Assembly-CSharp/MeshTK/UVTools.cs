using UnityEngine;

namespace MeshTK
{
	public class UVTools
	{
		public static void Rotate(Mesh mesh, float rot)
		{
			Vector2[] uv = mesh.uv;
			Quaternion quaternion = Quaternion.Euler(0f, 0f, rot);
			for (int i = 0; i < uv.Length; i++)
			{
				uv[i] = quaternion * (uv[i] - new Vector2(0.5f, 0.5f)) + new Vector3(0.5f, 0.5f);
			}
			mesh.uv = uv;
		}

		public static void Export(Mesh mesh, string filepath, int w = 1024, int h = 1024)
		{
			Texture2D texture2D = new Texture2D(w, h);
			int[] triangles = mesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector2 vector = mesh.uv[triangles[i]];
				Vector2 vector2 = mesh.uv[triangles[i + 1]];
				Vector2 vector3 = mesh.uv[triangles[i + 2]];
				TextureTools.DrawLine(texture2D, (int)(vector.x * (float)w), (int)(vector.y * (float)h), (int)(vector2.x * (float)w), (int)(vector2.y * (float)h), Color.red);
				TextureTools.DrawLine(texture2D, (int)(vector2.x * (float)w), (int)(vector2.y * (float)h), (int)(vector3.x * (float)w), (int)(vector3.y * (float)h), Color.red);
				TextureTools.DrawLine(texture2D, (int)(vector3.x * (float)w), (int)(vector3.y * (float)h), (int)(vector.x * (float)w), (int)(vector.y * (float)h), Color.red);
			}
			texture2D.Apply();
			TextureTools.SaveTexture(texture2D, filepath);
		}
	}
}
