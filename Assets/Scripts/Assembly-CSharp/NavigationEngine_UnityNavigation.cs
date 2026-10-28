using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationEngine_UnityNavigation : NavigationEngine
{
	public override Vector3[] GetPointsArray(Vector3 startPosition, Vector3 targetPosition)
	{
		NavMeshHit hit = default(NavMeshHit);
		NavMeshPath navMeshPath = new NavMeshPath();
		if (!NavMesh.CalculatePath(startPosition, targetPosition, -1, navMeshPath))
		{
			float num = 0.001f;
			for (num = 0.001f; num < 1f; num += 0.05f)
			{
				if (NavMesh.SamplePosition(startPosition, out hit, num, -1))
				{
					startPosition = hit.position;
					break;
				}
			}
			for (num = 0.001f; num < 1f; num += 0.05f)
			{
				if (NavMesh.SamplePosition(targetPosition, out hit, num, -1))
				{
					targetPosition = hit.position;
					break;
				}
			}
			NavMesh.CalculatePath(startPosition, targetPosition, -1, navMeshPath);
		}
		List<Vector3> list = new List<Vector3>();
		Vector3[] corners = navMeshPath.corners;
		foreach (Vector3 item in corners)
		{
			list.Add(item);
		}
		if (list.Count > 1 && list[0].x == startPosition.x && list[0].z == startPosition.z)
		{
			list.RemoveAt(0);
		}
		else if (list.Count == 0)
		{
			list.Clear();
			list.Add(targetPosition);
		}
		return list.ToArray();
	}

	public override string GetPrefabName()
	{
		return "NavMeshSegment";
	}

	public override void SetVisibility(bool visibility)
	{
		NavMeshSegment[] array = Object.FindObjectsOfType(typeof(NavMeshSegment)) as NavMeshSegment[];
		NavMeshSegment[] array2 = array;
		foreach (NavMeshSegment navMeshSegment in array2)
		{
			if (visibility)
			{
				navMeshSegment.Show();
			}
			else
			{
				navMeshSegment.Hide();
			}
		}
	}
}
