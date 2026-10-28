using System.Collections;
using UnityEngine;

namespace MeshTK
{
	internal class ImagePolygon
	{
		public ArrayList orderedEdges;

		public ImagePolygon(ImageEdge e)
		{
			orderedEdges = new ArrayList();
			orderedEdges.Add(e);
		}

		public bool AddEdge(ImageEdge e)
		{
			ImagePoint v = ((ImageEdge)orderedEdges[orderedEdges.Count - 1]).v2;
			if (e.v1 == v)
			{
				orderedEdges.Add(e);
				return true;
			}
			if (e.v2 == v)
			{
				e.Reverse();
				orderedEdges.Add(e);
				return true;
			}
			ImagePoint v2 = ((ImageEdge)orderedEdges[0]).v1;
			if (e.v2 == v2)
			{
				orderedEdges.Insert(0, e);
				return true;
			}
			if (e.v1 == v2)
			{
				e.Reverse();
				orderedEdges.Insert(0, e);
				return true;
			}
			return false;
		}

		public bool IsClosed()
		{
			if (orderedEdges.Count < 2)
			{
				return false;
			}
			ImagePoint v = ((ImageEdge)orderedEdges[orderedEdges.Count - 1]).v2;
			ImagePoint v2 = ((ImageEdge)orderedEdges[0]).v1;
			if (v2.x == v.x && v2.y == v.y)
			{
				return true;
			}
			return false;
		}

		public Vector2[] GetPoints()
		{
			Vector2[] array = new Vector2[orderedEdges.Count];
			for (int i = 0; i < orderedEdges.Count; i++)
			{
				ImagePoint v = ((ImageEdge)orderedEdges[i]).v1;
				array[i] = new Vector2(v.x, v.y);
			}
			return array;
		}

		public Vector2 GetUVAtIndex(int i)
		{
			if (i >= orderedEdges.Count)
			{
				return default(Vector2);
			}
			ImagePoint v = ((ImageEdge)orderedEdges[i]).v1;
			return new Vector2(v.u, v.v);
		}

		public void ReduceEdge(float maxDeviance = 0.448f)
		{
			ArrayList arrayList = new ArrayList();
			ImageEdge imageEdge = (ImageEdge)orderedEdges[0];
			for (int i = 1; i < orderedEdges.Count; i++)
			{
				ImageEdge imageEdge2 = (ImageEdge)orderedEdges[i];
				ImagePoint v = imageEdge.v1;
				ImagePoint v2 = imageEdge.v2;
				ImagePoint v3 = imageEdge2.v2;
				float num = Mathf.Abs((v3.x - v.x) * (v.y - v2.y) - (v.x - v2.x) * (v3.y - v.y)) / Mathf.Sqrt(Mathf.Pow(v3.x - v.x, 2f) + Mathf.Pow(v3.y - v.y, 2f));
				if (num <= maxDeviance)
				{
					imageEdge.v2 = v3;
					continue;
				}
				arrayList.Add(imageEdge);
				imageEdge = imageEdge2;
			}
			orderedEdges = arrayList;
		}
	}
}
