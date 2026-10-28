using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MeshTK
{
	public class VertexTools
	{
		public static void MergeDoubles(Mesh mesh, float threshold = 0.001f)
		{
			bool flag = mesh.uv.Length > 0;
			bool flag2 = mesh.uv2.Length > 0;
			bool flag3 = mesh.colors.Length > 0;
			bool flag4 = mesh.tangents.Length > 0;
			bool flag5 = mesh.boneWeights.Length > 0;
			Vector3[] vertices = mesh.vertices;
			Matrix4x4[] bindposes = mesh.bindposes;
			int[] array = new int[mesh.subMeshCount];
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				array[i] = mesh.GetTriangles(i).Length;
			}
			List<Vector3> list = new List<Vector3>();
			List<Vector2> list2 = new List<Vector2>();
			List<Vector2> list3 = new List<Vector2>();
			List<Color> list4 = new List<Color>();
			List<Vector3> list5 = new List<Vector3>();
			List<Vector4> list6 = new List<Vector4>();
			List<BoneWeight> list7 = new List<BoneWeight>();
			for (int j = 0; j < vertices.Length; j++)
			{
				Vector3 vert = vertices[j];
				if (!list.Exists((Vector3 p) => Vector3.Distance(vert, p) <= threshold))
				{
					list.Add(vert);
					if (flag)
					{
						list2.Add(mesh.uv[j]);
					}
					if (flag2)
					{
						list3.Add(mesh.uv2[j]);
					}
					list5.Add(mesh.normals[j]);
					if (flag3)
					{
						list4.Add(mesh.colors[j]);
					}
					if (flag4)
					{
						list6.Add(mesh.tangents[j]);
					}
					if (flag5)
					{
						list7.Add(mesh.boneWeights[j]);
					}
				}
			}
			int[] triangles = mesh.triangles;
			for (int num = 0; num < triangles.Length; num++)
			{
				for (int num2 = 0; num2 < list.Count; num2++)
				{
					if (Vector3.Distance(list[num2], vertices[triangles[num]]) <= threshold)
					{
						triangles[num] = num2;
						break;
					}
				}
			}
			mesh.Clear();
			mesh.vertices = list.ToArray();
			mesh.normals = list5.ToArray();
			if (flag)
			{
				mesh.uv = list2.ToArray();
			}
			if (flag2)
			{
				mesh.uv2 = list3.ToArray();
			}
			if (flag4)
			{
				mesh.tangents = list6.ToArray();
			}
			if (flag5)
			{
				mesh.boneWeights = list7.ToArray();
			}
			mesh.bindposes = bindposes;
			mesh.subMeshCount = array.Length;
			int num3 = 0;
			for (int num4 = 0; num4 < array.Length; num4++)
			{
				mesh.SetTriangles(triangles.Skip(num3).Take(array[num4]).ToArray(), num4);
				num3 += array[num4];
			}
		}

		public static void MirrorAxis(Mesh mesh, string axis)
		{
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;
			for (int i = 0; i < vertices.Length; i++)
			{
				switch (axis)
				{
				case "x":
					vertices[i].x = 0f - vertices[i].x;
					normals[i].x = 0f - normals[i].x;
					break;
				case "y":
					vertices[i].y = 0f - vertices[i].y;
					normals[i].y = 0f - normals[i].y;
					break;
				case "z":
					vertices[i].z = 0f - vertices[i].z;
					normals[i].z = 0f - normals[i].z;
					break;
				}
			}
			mesh.vertices = vertices;
			mesh.normals = normals;
			TriangleTools.Flip(mesh);
			mesh.RecalculateBounds();
		}

		public static void Remove(Mesh mesh, int[] indices)
		{
			if (indices == null || indices.Length <= 0)
			{
				return;
			}
			bool flag = mesh.uv.Length > 0;
			bool flag2 = mesh.uv2.Length > 0;
			bool flag3 = mesh.colors.Length > 0;
			bool flag4 = mesh.tangents.Length > 0;
			bool flag5 = mesh.boneWeights.Length > 0;
			int[] array = new int[mesh.subMeshCount];
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				array[i] = mesh.GetTriangles(i).Length;
			}
			int[] tris = mesh.triangles;
			List<int> list = new List<int>();
			for (int j = 0; j < tris.Length; j += 3)
			{
				if (Array.Exists(indices, (int p) => p == tris[j] || p == tris[j + 1] || p == tris[j + 2]))
				{
					list.Add(j / 3);
				}
			}
			TriangleTools.Remove(mesh, list.ToArray());
			int num = mesh.vertexCount - indices.Length;
			Vector3[] array2 = new Vector3[num];
			Vector2[] array3 = new Vector2[num];
			Vector2[] array4 = new Vector2[num];
			Color[] array5 = new Color[num];
			Vector3[] array6 = new Vector3[num];
			Vector4[] array7 = new Vector4[num];
			BoneWeight[] array8 = new BoneWeight[num];
			tris = mesh.triangles;
			int num2 = 0;
			int num3 = 0;
			int i2;
			for (i2 = 0; i2 < mesh.vertexCount; i2++)
			{
				if (!Array.Exists(indices, (int p) => p == i2))
				{
					array2[num2] = mesh.vertices[i2];
					if (flag)
					{
						array3[num2] = mesh.uv[i2];
					}
					if (flag2)
					{
						array4[num2] = mesh.uv2[i2];
					}
					if (flag3)
					{
						array5[num2] = mesh.colors[i2];
					}
					if (flag4)
					{
						array7[num2] = mesh.tangents[i2];
					}
					if (flag5)
					{
						array8[num2] = mesh.boneWeights[i2];
					}
					array6[num2] = mesh.normals[i2];
					num2++;
					if (num3 <= 0)
					{
						continue;
					}
					while (Array.Exists(tris, (int p) => p == i2))
					{
						tris[Array.FindIndex(tris, (int p) => p == i2)] -= num3;
					}
				}
				else
				{
					num3++;
				}
			}
			mesh.Clear();
			Matrix4x4[] bindposes = mesh.bindposes;
			mesh.vertices = array2;
			mesh.normals = array6;
			if (flag)
			{
				mesh.uv = array3;
			}
			if (flag2)
			{
				mesh.uv2 = array4;
			}
			if (flag4)
			{
				mesh.tangents = array7;
			}
			if (flag5)
			{
				mesh.boneWeights = array8;
			}
			mesh.bindposes = bindposes;
			mesh.subMeshCount = array.Length;
			int num4 = 0;
			for (int num5 = 0; num5 < array.Length; num5++)
			{
				mesh.SetTriangles(tris.Skip(num4).Take(array[num5]).ToArray(), num5);
				num4 += array[num5];
			}
		}

		public static void RemoveUnused(Mesh mesh)
		{
			List<int> list = new List<int>();
			int i;
			for (i = 0; i < mesh.vertexCount; i++)
			{
				if (!Array.Exists(mesh.triangles, (int p) => p == i))
				{
					list.Add(i);
				}
			}
			Remove(mesh, list.ToArray());
		}

		public static void SplitShared(Mesh mesh)
		{
			bool flag = mesh.uv.Length > 0;
			bool flag2 = mesh.uv2.Length > 0;
			bool flag3 = mesh.colors.Length > 0;
			bool flag4 = mesh.tangents.Length > 0;
			bool flag5 = mesh.boneWeights.Length > 0;
			int[] triangles = mesh.triangles;
			Matrix4x4[] bindposes = mesh.bindposes;
			int[] array = new int[mesh.subMeshCount];
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				array[i] = mesh.GetTriangles(i).Length;
			}
			int num = mesh.triangles.Length;
			int[] array2 = new int[num];
			Vector3[] array3 = new Vector3[num];
			Vector2[] array4 = new Vector2[num];
			Vector2[] array5 = new Vector2[num];
			Vector3[] array6 = new Vector3[num];
			Vector4[] array7 = new Vector4[num];
			Color[] array8 = new Color[num];
			BoneWeight[] array9 = new BoneWeight[num];
			for (int j = 0; j < triangles.Length; j++)
			{
				array2[j] = j;
				int num2 = triangles[j];
				array3[j] = mesh.vertices[num2];
				if (flag)
				{
					array4[j] = mesh.uv[num2];
				}
				if (flag2)
				{
					array5[j] = mesh.uv2[num2];
				}
				array6[j] = mesh.normals[num2];
				if (flag3)
				{
					array8[j] = mesh.colors[num2];
				}
				if (flag4)
				{
					array7[j] = mesh.tangents[num2];
				}
				if (flag5)
				{
					array9[j] = mesh.boneWeights[num2];
				}
			}
			mesh.Clear();
			mesh.vertices = array3;
			mesh.normals = array6;
			if (flag)
			{
				mesh.uv = array4;
			}
			if (flag2)
			{
				mesh.uv2 = array5;
			}
			if (flag4)
			{
				mesh.tangents = array7;
			}
			if (flag5)
			{
				mesh.boneWeights = array9;
			}
			mesh.bindposes = bindposes;
			mesh.subMeshCount = array.Length;
			int num3 = 0;
			for (int k = 0; k < array.Length; k++)
			{
				mesh.SetTriangles(array2.Skip(num3).Take(array[k]).ToArray(), k);
				num3 += array[k];
			}
		}

		public static void Weld(Mesh mesh, int[] indices)
		{
			if (indices.Length <= 1)
			{
				return;
			}
			bool flag = mesh.uv2.Length > 0;
			bool flag2 = mesh.colors.Length > 0;
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;
			Vector2[] uv = mesh.uv;
			Vector2[] uv2 = mesh.uv2;
			Color[] colors = mesh.colors;
			Vector3 zero = Vector3.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 zero3 = Vector2.zero;
			Vector3 zero4 = Vector3.zero;
			Color black = Color.black;
			foreach (int num in indices)
			{
				zero += vertices[num];
				zero4 += normals[num];
				zero2 += uv[num];
				if (flag)
				{
					zero3 += uv2[num];
				}
				if (flag2)
				{
					black += colors[num];
				}
			}
			zero /= (float)indices.Length;
			zero4 /= (float)indices.Length;
			zero2 /= (float)indices.Length;
			if (flag)
			{
				zero3 /= (float)indices.Length;
			}
			if (flag2)
			{
				black /= (float)indices.Length;
			}
			foreach (int num2 in indices)
			{
				vertices[num2] = zero;
				normals[num2] = zero4;
				uv[num2] = zero2;
				if (flag)
				{
					uv2[num2] = zero3;
				}
				if (flag2)
				{
					colors[num2] = black;
				}
			}
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uv;
			if (flag)
			{
				mesh.uv2 = uv2;
			}
			if (flag2)
			{
				mesh.colors = colors;
			}
			mesh.RecalculateBounds();
		}
	}
}
