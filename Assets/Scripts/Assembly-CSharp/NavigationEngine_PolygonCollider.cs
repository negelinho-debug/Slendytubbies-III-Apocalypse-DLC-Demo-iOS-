using System.Collections.Generic;
using AC;
using UnityEngine;

public class NavigationEngine_PolygonCollider : NavigationEngine
{
	private int MAXNODES = 1000;

	private SceneSettings sceneSettings;

	public override void Awake()
	{
		GetReferences();
	}

	public override Vector3[] GetPointsArray(Vector3 _originPos, Vector3 _targetPos)
	{
		List<Vector3> list = new List<Vector3>();
		if (IsLineClear(_originPos, _targetPos))
		{
			list.Add(_targetPos);
			return list.ToArray();
		}
		Vector2[] array = CreateVertexArray();
		Vector2 nearestToMesh = GetNearestToMesh(_originPos, array);
		Vector2 nearestToMesh2 = GetNearestToMesh(_targetPos, array);
		array = AddEndsToList(array, nearestToMesh, nearestToMesh2);
		float[,] weight = pointsToWeight(array);
		int[] array2 = buildSpanningTree(0, 1, weight);
		if (array2 == null)
		{
			Debug.LogWarning("Pathfinding error");
			list.Add(_targetPos);
			return list.ToArray();
		}
		int[] shortestPath = getShortestPath(0, 1, array2);
		int[] array3 = shortestPath;
		foreach (int num in array3)
		{
			Vector3 item = new Vector3(array[num].x, array[num].y, _originPos.z);
			list.Insert(0, item);
		}
		if (list[0] == _originPos)
		{
			list.RemoveAt(0);
		}
		return list.ToArray();
	}

	private int[] buildSpanningTree(int source, int destination, float[,] weight)
	{
		int num = (int)Mathf.Sqrt(weight.Length);
		bool[] array = new bool[num];
		float[] array2 = new float[num];
		int[] array3 = new int[num];
		for (int i = 0; i < num; i++)
		{
			array2[i] = float.PositiveInfinity;
			array3[i] = 100000;
		}
		array2[source] = 0f;
		int num2 = source;
		while (num2 != destination)
		{
			if (num2 < 0)
			{
				return null;
			}
			float num3 = array2[num2];
			float num4 = float.PositiveInfinity;
			int num5 = -1;
			array[num2] = true;
			for (int j = 0; j < num; j++)
			{
				if (!array[j])
				{
					float num6 = num3 + weight[num2, j];
					if (weight[num2, j] == -1f)
					{
						num6 = float.PositiveInfinity;
					}
					if (num6 < array2[j])
					{
						array2[j] = num6;
						array3[j] = num2;
					}
					if (array2[j] < num4)
					{
						num4 = array2[j];
						num5 = j;
					}
				}
			}
			num2 = num5;
		}
		return array3;
	}

	private int[] getShortestPath(int source, int destination, int[] precede)
	{
		int num = destination;
		int num2 = 0;
		int[] array = new int[MAXNODES];
		array[num2] = destination;
		num2++;
		while (precede[num] != source)
		{
			num = (array[num2] = precede[num]);
			num2++;
		}
		array[num2] = source;
		int[] array2 = new int[num2 + 1];
		for (int i = 0; i < num2 + 1; i++)
		{
			array2[i] = array[i];
		}
		return array2;
	}

	private float[,] pointsToWeight(Vector2[] points)
	{
		int num = points.Length;
		float[,] array = new float[num, num];
		for (int i = 0; i < num; i++)
		{
			for (int j = i; j < num; j++)
			{
				if (i == j)
				{
					array[i, j] = -1f;
					continue;
				}
				if (!IsLineClear(points[i], points[j]))
				{
					array[i, j] = (array[j, i] = -1f);
					continue;
				}
				array[i, j] = Vector2.Distance(points[i], points[j]);
				array[j, i] = Vector2.Distance(points[i], points[j]);
			}
		}
		return array;
	}

	private Vector2 GetNearestToMesh(Vector2 vertex, Vector2[] pointsList)
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(vertex - new Vector2(0.005f, 0f), new Vector2(1f, 0f), 0.01f, 1 << sceneSettings.navMesh.gameObject.layer);
		if (!raycastHit2D)
		{
			float num = -1f;
			Vector2 result = vertex;
			foreach (Vector2 vector in pointsList)
			{
				float num2 = Vector2.Distance(vertex, vector);
				if (num2 < num || num < 0f)
				{
					num = num2;
					result = vector;
				}
			}
			return result;
		}
		return vertex;
	}

	private Vector2[] CreateVertexArray()
	{
		List<Vector2> list = new List<Vector2>();
		PolygonCollider2D component = sceneSettings.navMesh.transform.GetComponent<PolygonCollider2D>();
		Transform transform = sceneSettings.navMesh.transform;
		for (int i = 0; i < component.pathCount; i++)
		{
			Vector2[] path = component.GetPath(i);
			Vector2[] array = path;
			for (int j = 0; j < array.Length; j++)
			{
				Vector2 vector = array[j];
				Vector3 vector2 = transform.TransformPoint(new Vector3(vector.x, vector.y, transform.position.z));
				list.Add(new Vector2(vector2.x, vector2.y));
			}
		}
		return list.ToArray();
	}

	private Vector2[] AddEndsToList(Vector2[] points, Vector2 originPos, Vector2 targetPos)
	{
		List<Vector2> list = new List<Vector2>();
		foreach (Vector2 vector in points)
		{
			if (vector != originPos && vector != targetPos)
			{
				list.Add(vector);
			}
		}
		list.Insert(0, targetPos);
		list.Insert(0, originPos);
		return list.ToArray();
	}

	private bool IsLineClear(Vector2 startPos, Vector2 endPos)
	{
		Vector2 vector = startPos;
		for (float num = 0f; num < 1f; num += 0.01f)
		{
			vector = startPos + (endPos - startPos) * num;
			RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, endPos - vector, 0.01f, 1 << sceneSettings.navMesh.gameObject.layer);
			if ((bool)raycastHit2D)
			{
				if (raycastHit2D.collider.gameObject != sceneSettings.navMesh.gameObject)
				{
					return ArePointsConnected(startPos, endPos);
				}
				continue;
			}
			return ArePointsConnected(startPos, endPos);
		}
		return true;
	}

	private bool ArePointsConnected(Vector2 startPos, Vector2 endPos)
	{
		PolygonCollider2D component = sceneSettings.navMesh.transform.GetComponent<PolygonCollider2D>();
		for (int i = 0; i < component.pathCount; i++)
		{
			Vector2[] path = component.GetPath(i);
			for (int j = 0; j < path.Length; j++)
			{
				Vector2 vector = component.transform.TransformPoint(path[j]);
				if (vector == startPos)
				{
					int num = j + 1;
					if (num == path.Length)
					{
						num = 0;
					}
					vector = component.transform.TransformPoint(path[num]);
					if (vector == endPos)
					{
						return true;
					}
					num = j - 1;
					if (num == -1)
					{
						num = path.Length - 1;
					}
					vector = component.transform.TransformPoint(path[num]);
					if (vector == endPos)
					{
						return true;
					}
					return false;
				}
			}
		}
		return false;
	}

	public override string GetPrefabName()
	{
		return "NavMesh2D";
	}

	public override void SetVisibility(bool visibility)
	{
	}

	public override void SceneSettingsGUI()
	{
	}

	private void GetReferences()
	{
		sceneSettings = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
	}

	private void OnDestroy()
	{
		sceneSettings = null;
	}
}
