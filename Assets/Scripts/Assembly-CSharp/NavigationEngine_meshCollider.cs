using System.Collections.Generic;
using AC;
using UnityEngine;

public class NavigationEngine_meshCollider : NavigationEngine
{
	private bool pathFailed;

	private SettingsManager settingsManager;

	private SceneSettings sceneSettings;

	public override void Awake()
	{
		GetReferences();
	}

	public override Vector3[] GetPointsArray(Vector3 originPos, Vector3 targetPos)
	{
		List<Vector3> list = new List<Vector3>();
		if ((bool)sceneSettings && (bool)sceneSettings.navMesh && (bool)sceneSettings.navMesh.GetComponent<Collider>())
		{
			Vector3 vector = originPos;
			Vector3 value = targetPos;
			originPos = GetNearestToMesh(originPos);
			targetPos = GetNearestToMesh(targetPos);
			list.Add(originPos);
			if (!IsLineClear(targetPos, originPos, false))
			{
				list = FindComplexPath(originPos, targetPos, false);
				if (pathFailed)
				{
					Vector3 lineBreak = GetLineBreak(list[list.Count - 1], targetPos);
					if (lineBreak != Vector3.zero)
					{
						targetPos = lineBreak;
						if (!IsLineClear(targetPos, originPos, true))
						{
							list = FindComplexPath(originPos, targetPos, true);
							if (pathFailed)
							{
								list.Clear();
								list.Add(originPos);
							}
						}
						else
						{
							list.Clear();
							list.Add(originPos);
						}
					}
				}
			}
			if (list.Count > 2)
			{
				for (int i = 0; i < list.Count; i++)
				{
					for (int j = i; j < list.Count; j++)
					{
						if (IsLineClear(list[i], list[j], false) && j > i + 1)
						{
							list.RemoveRange(i + 1, j - i - 1);
							j = 0;
							i = 0;
						}
					}
				}
			}
			list.Add(targetPos);
			if (list[0] == vector)
			{
				list.RemoveAt(0);
			}
			if (list.Count == 1 && list[0] == originPos)
			{
				list[0] = value;
			}
		}
		else
		{
			list.Add(targetPos);
		}
		return list.ToArray();
	}

	public override string GetPrefabName()
	{
		return "NavMesh";
	}

	public override void SetVisibility(bool visibility)
	{
		NavigationMesh[] array = Object.FindObjectsOfType(typeof(NavigationMesh)) as NavigationMesh[];
		NavigationMesh[] array2 = array;
		foreach (NavigationMesh navigationMesh in array2)
		{
			if (visibility)
			{
				navigationMesh.Show();
			}
			else
			{
				navigationMesh.Hide();
			}
		}
	}

	public override void SceneSettingsGUI()
	{
	}

	private bool IsVertexImperfect(Vector3 vertex, Vector3[] blackList)
	{
		bool result = false;
		foreach (Vector3 vector in blackList)
		{
			if (vertex == vector)
			{
				result = true;
			}
		}
		return result;
	}

	private float GetPathLength(List<Vector3> _pointsList, Vector3 candidatePoint, Vector3 endPos)
	{
		float num = 0f;
		List<Vector3> list = new List<Vector3>();
		foreach (Vector3 _points in _pointsList)
		{
			list.Add(_points);
		}
		list.Add(candidatePoint);
		list.Add(endPos);
		for (int i = 1; i < list.Count; i++)
		{
			num += Vector3.Distance(list[i], list[i - 1]);
		}
		return num;
	}

	private bool IsLineClear(Vector3 startPos, Vector3 endPos, bool ignoreOthers)
	{
		if (startPos.y > endPos.y)
		{
			endPos.y = startPos.y;
		}
		else
		{
			startPos.y = endPos.y;
		}
		Vector3 vector = startPos;
		RaycastHit hitInfo = default(RaycastHit);
		Ray ray = default(Ray);
		for (float num = 0f; num < 1f; num += 0.01f)
		{
			vector = startPos + (endPos - startPos) * num;
			ray = new Ray(vector + new Vector3(0f, 2f, 0f), new Vector3(0f, -1f, 0f));
			if ((bool)settingsManager && Physics.Raycast(ray, out hitInfo, settingsManager.navMeshRaycastLength, 1 << sceneSettings.navMesh.gameObject.layer))
			{
				if (hitInfo.collider.gameObject != sceneSettings.navMesh.gameObject && !ignoreOthers)
				{
					return false;
				}
				continue;
			}
			return false;
		}
		return true;
	}

	private Vector3 GetLineBreak(Vector3 startPos, Vector3 endPos)
	{
		if (startPos.y > endPos.y)
		{
			endPos.y = startPos.y;
		}
		else
		{
			startPos.y = endPos.y;
		}
		Vector3 vector = startPos;
		RaycastHit hitInfo = default(RaycastHit);
		Ray ray = default(Ray);
		for (float num = 0f; num < 1f; num += 0.01f)
		{
			vector = startPos + (endPos - startPos) * num;
			ray = new Ray(vector + new Vector3(0f, 2f, 0f), new Vector3(0f, -1f, 0f));
			if ((bool)settingsManager && Physics.Raycast(ray, out hitInfo, settingsManager.navMeshRaycastLength, 1 << sceneSettings.navMesh.gameObject.layer) && hitInfo.collider.gameObject != sceneSettings.navMesh.gameObject)
			{
				return vector;
			}
		}
		return Vector3.zero;
	}

	private Vector3[] CreateVertexArray(Vector3 targetPos)
	{
		Mesh sharedMesh = sceneSettings.navMesh.transform.GetComponent<MeshCollider>().sharedMesh;
		if (sharedMesh == null)
		{
			Debug.LogWarning("Active NavMesh has no mesh!");
			return null;
		}
		Vector3[] vertices = sharedMesh.vertices;
		List<NavMeshData> list = new List<NavMeshData>();
		Vector3[] array = vertices;
		foreach (Vector3 vertex in array)
		{
			list.Add(new NavMeshData(vertex, targetPos, sceneSettings.navMesh.transform));
		}
		list.Sort((NavMeshData a, NavMeshData b) => a.distance.CompareTo(b.distance));
		List<Vector3> list2 = new List<Vector3>();
		foreach (NavMeshData item in list)
		{
			list2.Add(item.vertex);
		}
		return list2.ToArray();
	}

	private List<Vector3> FindComplexPath(Vector3 originPos, Vector3 targetPos, bool ignoreOthers)
	{
		targetPos = GetNearestToMesh(targetPos);
		pathFailed = false;
		List<Vector3> list = new List<Vector3>();
		list.Add(originPos);
		bool flag = false;
		Vector3[] array = CreateVertexArray(targetPos);
		int num = 0;
		float num2 = 0f;
		bool flag2 = false;
		bool flag3 = false;
		Vector3 item = Vector3.zero;
		List<Vector3> list2 = new List<Vector3>();
		while (!flag)
		{
			num2 = 0f;
			flag2 = false;
			flag3 = false;
			Vector3[] array2 = array;
			foreach (Vector3 vector in array2)
			{
				if (IsVertexImperfect(vector, list2.ToArray()) || !IsLineClear(vector, list[list.Count - 1], ignoreOthers))
				{
					continue;
				}
				if (IsLineClear(targetPos, vector, ignoreOthers))
				{
					if (!flag3)
					{
						float pathLength = GetPathLength(list, vector, targetPos);
						if (pathLength < num2 || !flag2)
						{
							flag2 = true;
							flag3 = true;
							item = vector;
							num2 = pathLength;
						}
					}
					else
					{
						float pathLength2 = GetPathLength(list, vector, targetPos);
						if (pathLength2 < num2)
						{
							item = vector;
							num2 = pathLength2;
						}
					}
				}
				else
				{
					if (flag3)
					{
						continue;
					}
					if (!flag2)
					{
						item = vector;
						flag2 = true;
						num2 = GetPathLength(list, vector, targetPos);
						continue;
					}
					float pathLength3 = GetPathLength(list, vector, targetPos);
					if (pathLength3 < num2)
					{
						item = vector;
						num2 = pathLength3;
					}
				}
			}
			if (flag2)
			{
				list.Add(item);
				if (flag3)
				{
					flag = true;
				}
				else
				{
					list2.Add(item);
				}
			}
			num++;
			if (num > array.Length)
			{
				pathFailed = true;
				return list;
			}
		}
		return list;
	}

	private Vector3 GetNearestToMesh(Vector3 point)
	{
		RaycastHit hitInfo = default(RaycastHit);
		Ray ray = default(Ray);
		ray = new Ray(point + new Vector3(0f, 2f, 0f), new Vector3(0f, -1f, 0f));
		if ((bool)settingsManager && !Physics.Raycast(ray, out hitInfo, settingsManager.navMeshRaycastLength, 1 << sceneSettings.navMesh.gameObject.layer))
		{
			Vector3[] array = CreateVertexArray(point);
			return array[0];
		}
		return point;
	}

	private void GetReferences()
	{
		settingsManager = AdvGame.GetReferences().settingsManager;
		sceneSettings = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
	}

	private void OnDestroy()
	{
		settingsManager = null;
		sceneSettings = null;
	}
}
