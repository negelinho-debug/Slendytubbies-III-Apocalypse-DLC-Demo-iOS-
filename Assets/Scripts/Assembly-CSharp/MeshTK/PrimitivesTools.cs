using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeshTK
{
	public class PrimitivesTools
	{
		public static Mesh CreateCircle(float radius, int sides)
		{
			List<Vector3> list = new List<Vector3>();
			List<Vector2> list2 = new List<Vector2>();
			List<int> list3 = new List<int>();
			list.Add(new Vector3(0f, 0f, 0f));
			list2.Add(new Vector2(0.5f, 0.5f));
			list.Add(new Vector3(radius, 0f, 0f));
			list2.Add(new Vector2(1f, 0.5f));
			for (int i = 1; i <= sides; i++)
			{
				float f = (float)i / (float)sides * 2f * (float)Math.PI;
				list.Add(new Vector3(Mathf.Cos(f) * radius, 0f, Mathf.Sin(f) * radius));
				list2.Add(new Vector2(0.5f + Mathf.Cos(f) / 2f, 0.5f + Mathf.Sin(f) / 2f));
				list3.AddRange(new int[3]
				{
					0,
					(i == sides) ? 1 : (i + 1),
					i
				});
			}
			Mesh mesh = new Mesh();
			mesh.vertices = list.ToArray();
			mesh.uv = list2.ToArray();
			mesh.triangles = list3.ToArray();
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			return mesh;
		}

		public static Mesh CreatePlane(float width, float height)
		{
			List<Vector3> list = new List<Vector3>();
			List<Vector2> list2 = new List<Vector2>();
			List<int> list3 = new List<int>();
			list.Add(new Vector3((0f - width) / 2f, 0f, (0f - height) / 2f));
			list.Add(new Vector3((0f - width) / 2f, 0f, height / 2f));
			list.Add(new Vector3(width / 2f, 0f, height / 2f));
			list.Add(new Vector3(width / 2f, 0f, (0f - height) / 2f));
			list2.AddRange(new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f),
				new Vector2(1f, 0f)
			});
			list3.AddRange(new int[6] { 0, 1, 2, 0, 2, 3 });
			Mesh mesh = new Mesh();
			mesh.vertices = list.ToArray();
			mesh.uv = list2.ToArray();
			mesh.triangles = list3.ToArray();
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			return mesh;
		}

		public static Mesh CreateFromImage(Texture2D outlineTex, float maxDeviance, float meshWidth, float meshHeight, Vector3 pivotOffset)
		{
			if (outlineTex == null)
			{
				return new Mesh();
			}
			float num = 200f;
			Color[] pixels = outlineTex.GetPixels();
			ImageProcessor imageProcessor = new ImageProcessor(pixels, outlineTex.width, outlineTex.height, num / 255f);
			imageProcessor.SimplifyEdges(maxDeviance);
			ArrayList allPoints = imageProcessor.GetAllPoints();
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (Vector2[] item in allPoints)
			{
				int[] array2 = new Triangulator(item).Triangulate();
				Vector2[] array3 = new Vector2[item.Length];
				Vector3[] array4 = new Vector3[item.Length];
				for (int i = 0; i < item.Length; i++)
				{
					float num5 = 1f - item[i].x / (float)outlineTex.width - 0.5f / (float)outlineTex.width;
					float num6 = item[i].y / (float)outlineTex.height + 0.5f / (float)outlineTex.height;
					num5 = num5 * meshWidth - meshWidth / 2f;
					num6 = num6 * meshHeight - meshHeight / 2f;
					array4[i] = new Vector3(0f - num5, 0f, num6) - pivotOffset;
					array3[i] = imageProcessor.GetUVAtIndex(num4, i);
				}
				arrayList.AddRange(array4);
				arrayList3.AddRange(array3);
				for (int j = 0; j < array2.Length; j++)
				{
					arrayList2.Add(array2[j] + num2);
				}
				num2 += array4.Length;
				num3 += array2.Length;
				num4++;
			}
			Mesh mesh = new Mesh();
			mesh.vertices = (Vector3[])arrayList.ToArray(typeof(Vector3));
			mesh.triangles = (int[])arrayList2.ToArray(typeof(int));
			mesh.uv = (Vector2[])arrayList3.ToArray(typeof(Vector2));
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			return mesh;
		}
	}
}
