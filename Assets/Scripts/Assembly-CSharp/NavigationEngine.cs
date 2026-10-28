using System.Collections.Generic;
using UnityEngine;

public class NavigationEngine : ScriptableObject
{
	public virtual void Awake()
	{
	}

	public virtual Vector3[] GetPointsArray(Vector3 startPosition, Vector3 targetPosition)
	{
		List<Vector3> list = new List<Vector3>();
		list.Add(targetPosition);
		return list.ToArray();
	}

	public virtual string GetPrefabName()
	{
		return string.Empty;
	}

	public virtual void SetVisibility(bool visibility)
	{
	}

	public virtual void SceneSettingsGUI()
	{
	}
}
