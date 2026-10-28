using System.Collections;
using UnityEngine;

namespace MeshTK
{
	internal class ImageProcessor
	{
		private ArrayList edges;

		private ArrayList points;

		private ArrayList polygons;

		public ImageProcessor(Color[] pixels, int imageWidth, int imageHeight, float threshold)
		{
			edges = new ArrayList();
			points = new ArrayList();
			polygons = new ArrayList();
			for (int i = 0; i < imageWidth; i++)
			{
				float num = (float)i + 0.5f;
				for (int j = 0; j < imageHeight; j++)
				{
					float num2 = (float)j + 0.5f;
					if (pixels[i + imageWidth * j].a < threshold)
					{
						continue;
					}
					float num3 = 0f;
					float num4 = 0f;
					float num5 = 0f;
					float num6 = 0f;
					float num7 = 0f;
					float num8 = 0f;
					float num9 = 0f;
					if (i > 0)
					{
						num6 = pixels[i - 1 + imageWidth * j].a;
						if (j < imageHeight - 1)
						{
							num8 = pixels[i - 1 + imageWidth * (j + 1)].a;
						}
					}
					if (i < imageWidth - 1)
					{
						if (j > 0)
						{
							num9 = pixels[i + 1 + imageWidth * (j - 1)].a;
						}
						num5 = pixels[i + 1 + imageWidth * j].a;
						if (j < imageHeight - 1)
						{
							num7 = pixels[i + 1 + imageWidth * (j + 1)].a;
						}
					}
					if (j > 0)
					{
						num4 = pixels[i + imageWidth * (j - 1)].a;
					}
					if (j < imageHeight - 1)
					{
						num3 = pixels[i + imageWidth * (j + 1)].a;
					}
					if (num3 >= threshold)
					{
						if (num7 < threshold && num5 < threshold)
						{
							if (num8 >= threshold || num6 >= threshold)
							{
								ImageEdge value = new ImageEdge(GetPoint(i, j, num / (float)imageWidth, num2 / (float)imageHeight), GetPoint(i, j + 1, num / (float)imageWidth, (num2 + 1f) / (float)imageHeight));
								edges.Add(value);
							}
						}
						else if (num8 < threshold && num6 < threshold && (num7 >= threshold || num5 >= threshold))
						{
							ImageEdge value2 = new ImageEdge(GetPoint(i, j, num / (float)imageWidth, num2 / (float)imageHeight), GetPoint(i, j + 1, num / (float)imageWidth, (num2 + 1f) / (float)imageHeight));
							edges.Add(value2);
						}
					}
					if (num7 >= threshold)
					{
						if (num3 < threshold && num5 >= threshold)
						{
							ImageEdge value3 = new ImageEdge(GetPoint(i, j, num / (float)imageWidth, num2 / (float)imageHeight), GetPoint(i + 1, j + 1, (num + 1f) / (float)imageWidth, (num2 + 1f) / (float)imageHeight));
							edges.Add(value3);
						}
						else if (num3 >= threshold && num5 < threshold)
						{
							ImageEdge value4 = new ImageEdge(GetPoint(i, j, num / (float)imageWidth, num2 / (float)imageHeight), GetPoint(i + 1, j + 1, (num + 1f) / (float)imageWidth, (num2 + 1f) / (float)imageHeight));
							edges.Add(value4);
						}
					}
					if (num5 >= threshold)
					{
						if (num3 < threshold && num7 < threshold)
						{
							if (num4 >= threshold || num9 >= threshold)
							{
								ImageEdge value5 = new ImageEdge(GetPoint(i, j, num / (float)imageWidth, num2 / (float)imageHeight), GetPoint(i + 1, j, (num + 1f) / (float)imageWidth, num2 / (float)imageHeight));
								edges.Add(value5);
							}
						}
						else if (num4 < threshold && num9 < threshold && (num3 >= threshold || num7 >= threshold))
						{
							ImageEdge value6 = new ImageEdge(GetPoint(i, j, num / (float)imageWidth, num2 / (float)imageHeight), GetPoint(i + 1, j, (num + 1f) / (float)imageWidth, num2 / (float)imageHeight));
							edges.Add(value6);
						}
					}
					if (num9 >= threshold)
					{
						if (num5 < threshold && num4 >= threshold)
						{
							ImageEdge value7 = new ImageEdge(GetPoint(i, j, num / (float)imageWidth, num2 / (float)imageHeight), GetPoint(i + 1, j - 1, (num + 1f) / (float)imageWidth, (num2 - 1f) / (float)imageHeight));
							edges.Add(value7);
						}
						else if (num5 >= threshold && num4 < threshold)
						{
							ImageEdge value8 = new ImageEdge(GetPoint(i, j, num / (float)imageWidth, num2 / (float)imageHeight), GetPoint(i + 1, j - 1, (num + 1f) / (float)imageWidth, (num2 - 1f) / (float)imageHeight));
							edges.Add(value8);
						}
					}
				}
			}
			CreateFaces();
		}

		private ImagePoint GetPoint(float x, float y, float u, float v)
		{
			for (int i = 0; i < points.Count; i++)
			{
				ImagePoint imagePoint = (ImagePoint)points[i];
				if (imagePoint.x == x && imagePoint.y == y)
				{
					return imagePoint;
				}
			}
			ImagePoint imagePoint2 = new ImagePoint(x, y, u, v);
			points.Add(imagePoint2);
			return imagePoint2;
		}

		public void SimplifyEdges(float maxDeviance = 0.448f)
		{
			foreach (ImagePolygon polygon in polygons)
			{
				polygon.ReduceEdge(maxDeviance);
			}
		}

		private void CreateFaces()
		{
			ImagePolygon imagePolygon = new ImagePolygon((ImageEdge)edges[0]);
			edges.RemoveAt(0);
			polygons.Add(imagePolygon);
			while (edges.Count > 0)
			{
				if (imagePolygon.IsClosed())
				{
					ImagePolygon imagePolygon2 = new ImagePolygon((ImageEdge)edges[0]);
					edges.RemoveAt(0);
					polygons.Add(imagePolygon2);
					imagePolygon = imagePolygon2;
				}
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < edges.Count; i++)
				{
					ImageEdge imageEdge = (ImageEdge)edges[i];
					if (imagePolygon.AddEdge(imageEdge))
					{
						arrayList.Add(imageEdge);
					}
				}
				for (int j = 0; j < arrayList.Count; j++)
				{
					edges.Remove((ImageEdge)arrayList[j]);
				}
			}
		}

		public ArrayList GetAllPoints()
		{
			ArrayList arrayList = new ArrayList();
			foreach (ImagePolygon polygon in polygons)
			{
				arrayList.Add(polygon.GetPoints());
			}
			return arrayList;
		}

		public Vector2 GetUVAtIndex(int loopIndex, int i)
		{
			ImagePolygon imagePolygon = (ImagePolygon)polygons[loopIndex];
			return imagePolygon.GetUVAtIndex(i);
		}
	}
}
