using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MeshTK
{
	public class TriangleTools
	{
		public static GameObject DetachToNewObject(ref EJMMesh ejmmesh, HashSet<int> indices)
		{
			GameObject gameObject = new GameObject();
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = new Mesh();
			gameObject.AddComponent<MeshRenderer>();
			EJMMesh eJMMesh = new EJMMesh(meshFilter);
			eJMMesh.ImportData(ejmmesh.SharedMesh);
			int num = 0;
			for (int i = 0; i < ejmmesh.SharedMesh.subMeshCount; i++)
			{
				List<int> list = ejmmesh.SharedMesh.GetTriangles(i).ToList();
				int count = list.Count;
				List<int> list2 = new List<int>();
				for (int num2 = num + count - 3; num2 >= num; num2 -= 3)
				{
					if (indices.Contains(num2 / 3))
					{
						list2.AddRange(list.GetRange(num2 - num, 3));
						list.RemoveRange(num2 - num, 3);
					}
				}
				ejmmesh.SharedMesh.SetTriangles(list.ToArray(), i);
				eJMMesh.SharedMesh.SetTriangles(list2.ToArray(), i);
				num += count;
			}
			return gameObject;
		}

		public static void Detach(Mesh mesh, int[] indices, bool newSubmesh = false)
		{
			bool flag = mesh.uv.Length > 0;
			bool flag2 = mesh.uv2.Length > 0;
			bool flag3 = mesh.colors.Length > 0;
			bool flag4 = mesh.tangents.Length > 0;
			bool flag5 = mesh.boneWeights.Length > 0;
			int vertexCount = mesh.vertexCount;
			int[] triangles = mesh.triangles;
			List<Vector3> list = new List<Vector3>();
			List<Vector2> list2 = new List<Vector2>();
			List<Vector2> list3 = new List<Vector2>();
			List<Color> list4 = new List<Color>();
			List<Vector3> list5 = new List<Vector3>();
			List<Vector4> list6 = new List<Vector4>();
			List<BoneWeight> list7 = new List<BoneWeight>();
			List<int> list8 = new List<int>();
			foreach (int num in indices)
			{
				for (int j = 0; j < 3; j++)
				{
					int num2 = triangles[num * 3 + j];
					Vector3 vert = mesh.vertices[num2];
					int num3 = list.FindIndex((Vector3 p) => Vector3.Distance(vert, p) <= 0.001f);
					if (num3 == -1)
					{
						list.Add(vert);
						list5.Add(mesh.normals[num2]);
						if (flag)
						{
							list2.Add(mesh.uv[num2]);
						}
						if (flag2)
						{
							list3.Add(mesh.uv2[num2]);
						}
						if (flag3)
						{
							list4.Add(mesh.colors[num2]);
						}
						if (flag4)
						{
							list6.Add(mesh.tangents[num2]);
						}
						if (flag5)
						{
							list7.Add(mesh.boneWeights[num2]);
						}
						num3 = list.Count - 1;
					}
					triangles[num * 3 + j] = vertexCount + num3;
					if (newSubmesh)
					{
						list8.Add(vertexCount + num3);
					}
				}
			}
			int num4 = mesh.vertexCount + list.Count;
			Vector3[] array = new Vector3[num4];
			mesh.vertices.CopyTo(array, 0);
			list.ToArray().CopyTo(array, vertexCount);
			mesh.vertices = array;
			Vector3[] array2 = new Vector3[num4];
			mesh.normals.CopyTo(array2, 0);
			list5.ToArray().CopyTo(array2, vertexCount);
			mesh.normals = array2;
			if (flag)
			{
				Vector2[] array3 = new Vector2[num4];
				mesh.uv.CopyTo(array3, 0);
				list2.ToArray().CopyTo(array3, vertexCount);
				mesh.uv = array3;
			}
			if (flag2)
			{
				Vector2[] array4 = new Vector2[num4];
				mesh.uv2.CopyTo(array4, 0);
				list3.ToArray().CopyTo(array4, vertexCount);
				mesh.uv2 = array4;
			}
			if (flag3)
			{
				Color[] array5 = new Color[num4];
				mesh.colors.CopyTo(array5, 0);
				list4.ToArray().CopyTo(array5, vertexCount);
				mesh.colors = array5;
			}
			if (flag4)
			{
				Vector4[] array6 = new Vector4[num4];
				mesh.tangents.CopyTo(array6, 0);
				list6.ToArray().CopyTo(array6, vertexCount);
				mesh.tangents = array6;
			}
			if (flag5)
			{
				BoneWeight[] array7 = new BoneWeight[num4];
				mesh.boneWeights.CopyTo(array7, 0);
				list7.ToArray().CopyTo(array7, vertexCount);
				mesh.boneWeights = array7;
			}
			int num5 = 0;
			for (int num6 = 0; num6 < mesh.subMeshCount; num6++)
			{
				int num7 = mesh.GetTriangles(num6).Length;
				mesh.SetTriangles(triangles.Skip(num5).Take(num7).ToArray(), num6);
				num5 += num7;
			}
			if (newSubmesh)
			{
				Remove(mesh, indices);
				mesh.subMeshCount++;
				mesh.SetTriangles(list8.ToArray(), mesh.subMeshCount - 1);
			}
		}

		public static void Extrude(Mesh mesh, int[] indices, float dist)
		{
			bool flag = mesh.uv.Length > 0;
			bool flag2 = mesh.uv2.Length > 0;
			bool flag3 = mesh.colors.Length > 0;
			bool flag4 = mesh.tangents.Length > 0;
			bool flag5 = mesh.boneWeights.Length > 0;
			int[] triangles = mesh.triangles;
			Matrix4x4[] bindposes = mesh.bindposes;
			int[] array = new int[triangles.Length + 18 * indices.Length];
			Vector3[] array2 = mesh.vertices;
			Vector2[] array3 = mesh.uv;
			Vector2[] array4 = mesh.uv2;
			Color[] array5 = mesh.colors;
			Vector3[] array6 = mesh.normals;
			Vector4[] array7 = mesh.tangents;
			BoneWeight[] array8 = mesh.boneWeights;
			int num = 0;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (Array.IndexOf(indices, i / 3) == -1)
				{
					array[num++] = triangles[i];
					array[num++] = triangles[i + 1];
					array[num++] = triangles[i + 2];
					continue;
				}
				Vector3 normalized = Vector3.Cross(array2[triangles[i + 1]] - array2[triangles[i]], array2[triangles[i + 2]] - array2[triangles[i]]).normalized;
				Array.Resize(ref array2, array2.Length + 15);
				Array.Resize(ref array6, array6.Length + 15);
				Array.Resize(ref array3, array3.Length + 15);
				if (flag2)
				{
					Array.Resize(ref array4, array4.Length + 15);
				}
				if (flag3)
				{
					Array.Resize(ref array5, array5.Length + 15);
				}
				if (flag4)
				{
					Array.Resize(ref array7, array7.Length + 15);
				}
				if (flag5)
				{
					Array.Resize(ref array8, array8.Length + 15);
				}
				for (int j = 0; j < 3; j++)
				{
					int num2 = array2.Length - 15 + j * 4;
					int num3 = array2.Length - 14 + j * 4;
					int num4 = array2.Length - 13 + j * 4;
					int num5 = array2.Length - 12 + j * 4;
					array2[num2] = array2[triangles[i + j]];
					array2[num3] = array2[triangles[(j != 2) ? (i + j + 1) : i]];
					array2[num4] = array2[triangles[(j != 2) ? (i + j + 1) : i]] + dist * normalized;
					array2[num5] = array2[triangles[i + j]] + dist * normalized;
					Vector3 normalized2 = Vector3.Cross(array2[num3] - array2[num2], array2[num4] - array2[num2]).normalized;
					array6[num2] = normalized2;
					array6[num3] = normalized2;
					array6[num4] = normalized2;
					array6[num5] = normalized2;
					array3[num2] = array3[triangles[i + j]];
					array3[num3] = array3[triangles[(j != 2) ? (i + j + 1) : i]];
					array3[num4] = array3[triangles[(j != 2) ? (i + j + 1) : i]];
					array3[num5] = array3[triangles[i + j]];
					if (flag2)
					{
						array4[num2] = array4[triangles[i + j]];
						array4[num3] = array4[triangles[(j != 2) ? (i + j + 1) : i]];
						array4[num4] = array4[triangles[(j != 2) ? (i + j + 1) : i]];
						array4[num5] = array4[triangles[i + j]];
					}
					if (flag3)
					{
						array5[num2] = array5[triangles[i + j]];
						array5[num3] = array5[triangles[(j != 2) ? (i + j + 1) : i]];
						array5[num4] = array5[triangles[(j != 2) ? (i + j + 1) : i]];
						array5[num5] = array5[triangles[i + j]];
					}
					if (flag4)
					{
						array7[num2] = array7[triangles[i + j]];
						array7[num3] = array7[triangles[(j != 2) ? (i + j + 1) : i]];
						array7[num4] = array7[triangles[(j != 2) ? (i + j + 1) : i]];
						array7[num5] = array7[triangles[i + j]];
					}
					if (flag5)
					{
						array8[num2] = array8[triangles[i + j]];
						array8[num3] = array8[triangles[(j != 2) ? (i + j + 1) : i]];
						array8[num4] = array8[triangles[(j != 2) ? (i + j + 1) : i]];
						array8[num5] = array8[triangles[i + j]];
					}
					array[num++] = num2;
					array[num++] = num3;
					array[num++] = num4;
					array[num++] = num2;
					array[num++] = num4;
					array[num++] = num5;
				}
				for (int k = 0; k < 3; k++)
				{
					array2[array2.Length - 3 + k] = array2[triangles[i + k]] + dist * normalized;
					array6[array2.Length - 3 + k] = array6[triangles[i + k]];
					array3[array2.Length - 3 + k] = array3[triangles[i + k]];
					if (flag2)
					{
						array4[array2.Length - 3 + k] = array4[triangles[i + k]];
					}
					if (flag3)
					{
						array5[array2.Length - 3 + k] = array5[triangles[i + k]];
					}
					if (flag4)
					{
						array7[array2.Length - 3 + k] = array7[triangles[i + k]];
					}
					if (flag5)
					{
						array8[array2.Length - 3 + k] = array8[triangles[i + k]];
					}
				}
				array[num++] = array2.Length - 3;
				array[num++] = array2.Length - 2;
				array[num++] = array2.Length - 1;
			}
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
			if (flag3)
			{
				mesh.colors = array5;
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
			int originalindex = 0;
			int num6 = 0;
			for (int l = 0; l < mesh.subMeshCount; l++)
			{
				int originallength = mesh.GetTriangles(l).Length;
				int num7 = originallength + indices.Count((int p) => p >= originalindex && p < originalindex + originallength) * 18;
				mesh.SetTriangles(array.Skip(num6).Take(num7).ToArray(), l);
				originalindex += originallength;
				num6 += num7;
			}
		}

		public static void Flip(Mesh mesh, int[] indices = null)
		{
			int[] triangles = mesh.triangles;
			if (indices == null)
			{
				for (int i = 0; i < triangles.Length; i += 3)
				{
					int num = triangles[i];
					triangles[i] = triangles[i + 2];
					triangles[i + 2] = num;
				}
			}
			else
			{
				foreach (int num2 in indices)
				{
					if (triangles.Length >= num2 * 3 + 3)
					{
						int num3 = triangles[num2 * 3];
						triangles[num2 * 3] = triangles[num2 * 3 + 2];
						triangles[num2 * 3 + 2] = num3;
					}
				}
			}
			int num4 = 0;
			for (int k = 0; k < mesh.subMeshCount; k++)
			{
				int num5 = mesh.GetTriangles(k).Length;
				mesh.SetTriangles(triangles.Skip(num4).Take(num5).ToArray(), k);
				num4 += num5;
			}
		}

		public static void Remove(Mesh mesh, int[] indices)
		{
			List<int> list = mesh.triangles.ToList();
			Array.Sort(indices, (int a, int b) => b.CompareTo(a));
			foreach (int num2 in indices)
			{
				list.RemoveRange(num2 * 3, 3);
			}
			int originalindex = 0;
			int num3 = 0;
			for (int num4 = 0; num4 < mesh.subMeshCount; num4++)
			{
				int originallength = mesh.GetTriangles(num4).Length;
				int num5 = originallength - indices.Count((int p) => p >= originalindex && p < originalindex + originallength) * 3;
				mesh.SetTriangles(list.Skip(num3).Take(num5).ToArray(), num4);
				originalindex += originallength;
				num3 += num5;
			}
		}

		public static void SubdivideByCenter(Mesh mesh, int[] indices)
		{
			bool flag = mesh.uv.Length > 0;
			bool flag2 = mesh.uv2.Length > 0;
			bool flag3 = mesh.colors.Length > 0;
			bool flag4 = mesh.tangents.Length > 0;
			bool flag5 = mesh.boneWeights.Length > 0;
			int[] triangles = mesh.triangles;
			Matrix4x4[] bindposes = mesh.bindposes;
			int[] array = new int[triangles.Length + 6 * indices.Length];
			Vector3[] array2 = mesh.vertices;
			Vector3[] array3 = mesh.normals;
			Vector2[] array4 = mesh.uv;
			Vector2[] array5 = mesh.uv2;
			Color[] array6 = mesh.colors;
			Vector4[] array7 = mesh.tangents;
			BoneWeight[] array8 = mesh.boneWeights;
			int num = 0;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (Array.IndexOf(indices, i / 3) == -1 && indices != null)
				{
					array[num++] = triangles[i];
					array[num++] = triangles[i + 1];
					array[num++] = triangles[i + 2];
					continue;
				}
				Array.Resize(ref array2, array2.Length + 1);
				Array.Resize(ref array3, array3.Length + 1);
				Array.Resize(ref array4, array4.Length + 1);
				if (flag2)
				{
					Array.Resize(ref array5, array5.Length + 1);
				}
				if (flag3)
				{
					Array.Resize(ref array6, array6.Length + 1);
				}
				if (flag4)
				{
					Array.Resize(ref array7, array7.Length + 1);
				}
				if (flag5)
				{
					Array.Resize(ref array8, array8.Length + 1);
				}
				int num2 = triangles[i];
				int num3 = triangles[i + 1];
				int num4 = triangles[i + 2];
				int num5 = array2.Length - 1;
				array2[num5] = (array2[num2] + array2[num3] + array2[num4]) / 3f;
				array3[num5] = (array3[num2] + array3[num3] + array3[num4]) / 3f;
				array4[num5] = (array4[num2] + array4[num3] + array4[num4]) / 3f;
				if (flag2)
				{
					array5[num5] = (array5[num2] + array5[num3] + array5[num4]) / 3f;
				}
				if (flag3)
				{
					array6[num5] = (array6[num2] + array6[num3] + array6[num4]) / 3f;
				}
				if (flag4)
				{
					array7[num5] = (array7[num2] + array7[num3] + array7[num4]) / 3f;
				}
				if (flag5)
				{
					array8[num5] = array8[triangles[i]];
				}
				array[num++] = num2;
				array[num++] = num3;
				array[num++] = num5;
				array[num++] = num3;
				array[num++] = num4;
				array[num++] = num5;
				array[num++] = num4;
				array[num++] = num2;
				array[num++] = num5;
			}
			mesh.vertices = array2;
			mesh.normals = array3;
			if (flag)
			{
				mesh.uv = array4;
			}
			if (flag2)
			{
				mesh.uv2 = array5;
			}
			if (flag3)
			{
				mesh.colors = array6;
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
			int originalindex = 0;
			int num6 = 0;
			for (int j = 0; j < mesh.subMeshCount; j++)
			{
				int originallength = mesh.GetTriangles(j).Length;
				int num7 = originallength + indices.Count((int p) => p >= originalindex && p < originalindex + originallength) * 6;
				mesh.SetTriangles(array.Skip(num6).Take(num7).ToArray(), j);
				originalindex += originallength;
				num6 += num7;
			}
		}

		public static void SubdivideByEdge(Mesh mesh, int[] indices, bool weld = true, float threshold = 0.001f)
		{
			bool flag = mesh.uv.Length > 0;
			bool flag2 = mesh.uv2.Length > 0;
			bool flag3 = mesh.colors.Length > 0;
			bool flag4 = mesh.tangents.Length > 0;
			bool flag5 = mesh.boneWeights.Length > 0;
			int[] triangles = mesh.triangles;
			Matrix4x4[] bindposes = mesh.bindposes;
			int[] array = new int[triangles.Length + 9 * indices.Length];
			Vector3[] array2 = mesh.vertices;
			Vector3[] array3 = mesh.normals;
			Vector2[] array4 = mesh.uv;
			Vector2[] array5 = mesh.uv2;
			Color[] array6 = mesh.colors;
			Vector4[] array7 = mesh.tangents;
			BoneWeight[] array8 = mesh.boneWeights;
			int num = 0;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (Array.IndexOf(indices, i / 3) == -1)
				{
					array[num++] = triangles[i];
					array[num++] = triangles[i + 1];
					array[num++] = triangles[i + 2];
					continue;
				}
				int num2 = triangles[i];
				int num3 = triangles[i + 1];
				int num4 = triangles[i + 2];
				Vector3 vert1 = (array2[triangles[i]] + array2[triangles[i + 1]]) / 2f;
				Vector3 vert2 = (array2[triangles[i + 1]] + array2[triangles[i + 2]]) / 2f;
				Vector3 vert3 = (array2[triangles[i + 2]] + array2[triangles[i]]) / 2f;
				int num5 = Array.FindIndex(array2, (Vector3 p) => Vector3.Distance(p, vert1) <= threshold);
				int num6 = Array.FindIndex(array2, (Vector3 p) => Vector3.Distance(p, vert2) <= threshold);
				int num7 = Array.FindIndex(array2, (Vector3 p) => Vector3.Distance(p, vert3) <= threshold);
				int newSize = array2.Length + (weld ? (((num5 == -1) ? 1 : 0) + ((num6 == -1) ? 1 : 0) + ((num7 == -1) ? 1 : 0)) : 3);
				Array.Resize(ref array2, newSize);
				Array.Resize(ref array3, newSize);
				Array.Resize(ref array4, newSize);
				if (flag2)
				{
					Array.Resize(ref array5, newSize);
				}
				if (flag3)
				{
					Array.Resize(ref array6, newSize);
				}
				if (flag4)
				{
					Array.Resize(ref array7, newSize);
				}
				if (flag5)
				{
					Array.Resize(ref array8, newSize);
				}
				if (!weld || num5 == -1)
				{
					num5 = array2.Length - 1 - ((num6 == -1) ? 1 : 0) - ((num7 == -1) ? 1 : 0);
					array2[num5] = vert1;
					array3[num5] = (array3[num2] + array3[num3]) / 2f;
					array4[num5] = (array4[num2] + array4[num3]) / 2f;
					if (flag2)
					{
						array5[num5] = (array5[num2] + array5[num3]) / 2f;
					}
					if (flag3)
					{
						array6[num5] = (array6[num2] + array6[num3]) / 2f;
					}
					if (flag4)
					{
						array7[num5] = (array7[num2] + array7[num3]) / 2f;
					}
					if (flag5)
					{
						array8[num5] = array8[num2];
					}
				}
				if (!weld || num6 == -1)
				{
					num6 = array2.Length - 1 - ((num7 == -1) ? 1 : 0);
					array2[num6] = vert2;
					array3[num6] = (array3[num3] + array3[num4]) / 2f;
					array4[num6] = (array4[num3] + array4[num4]) / 2f;
					if (flag2)
					{
						array5[num6] = (array5[num3] + array5[num4]) / 2f;
					}
					if (flag3)
					{
						array6[num6] = (array6[num3] + array6[num4]) / 2f;
					}
					if (flag4)
					{
						array7[num6] = (array7[num3] + array7[num4]) / 2f;
					}
					if (flag5)
					{
						array8[num6] = array8[num3];
					}
				}
				if (!weld || num7 == -1)
				{
					num7 = array2.Length - 1;
					array2[num7] = vert3;
					array3[num7] = (array3[num4] + array3[num2]) / 2f;
					array4[num7] = (array4[num4] + array4[num2]) / 2f;
					if (flag2)
					{
						array5[num7] = (array5[num4] + array5[num2]) / 2f;
					}
					if (flag3)
					{
						array6[num7] = (array6[num4] + array6[num2]) / 2f;
					}
					if (flag4)
					{
						array7[num7] = (array7[num4] + array7[num2]) / 2f;
					}
					if (flag5)
					{
						array8[num7] = array8[num4];
					}
				}
				Debug.Log("1: " + num5 + ", 2: " + num6 + ", 3: " + num7);
				array[num++] = num2;
				array[num++] = num5;
				array[num++] = num7;
				array[num++] = num3;
				array[num++] = num6;
				array[num++] = num5;
				array[num++] = num4;
				array[num++] = num7;
				array[num++] = num6;
				array[num++] = num5;
				array[num++] = num6;
				array[num++] = num7;
			}
			mesh.vertices = array2;
			mesh.normals = array3;
			if (flag)
			{
				mesh.uv = array4;
			}
			if (flag2)
			{
				mesh.uv2 = array5;
			}
			if (flag3)
			{
				mesh.colors = array6;
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
			int originalindex = 0;
			int num8 = 0;
			for (int num9 = 0; num9 < mesh.subMeshCount; num9++)
			{
				int originallength = mesh.GetTriangles(num9).Length;
				int num10 = originallength + indices.Count((int p) => p >= originalindex && p < originalindex + originallength) * 3 * 3;
				mesh.SetTriangles(array.Skip(num8).Take(num10).ToArray(), num9);
				originalindex += originallength;
				num8 += num10;
			}
		}
	}
}
