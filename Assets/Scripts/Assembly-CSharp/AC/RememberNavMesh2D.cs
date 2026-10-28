using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class RememberNavMesh2D : ConstantID
	{
		public NavMesh2DData SaveData()
		{
			NavMesh2DData navMesh2DData = new NavMesh2DData();
			navMesh2DData.objectID = constantID;
			if ((bool)GetComponent<NavigationMesh>())
			{
				NavigationMesh component = GetComponent<NavigationMesh>();
				navMesh2DData.linkedIDs = new List<int>();
				for (int i = 0; i < component.polygonColliderHoles.Count; i++)
				{
					if ((bool)component.polygonColliderHoles[i].GetComponent<ConstantID>())
					{
						navMesh2DData.linkedIDs.Add(component.polygonColliderHoles[i].GetComponent<ConstantID>().constantID);
						continue;
					}
					Debug.LogWarning("Cannot save " + gameObject.name + "'s holes because " + component.polygonColliderHoles[i].gameObject.name + " has no Constant ID!");
				}
			}
			return navMesh2DData;
		}

		public void LoadData(NavMesh2DData data)
		{
			if (!GetComponent<NavigationMesh>())
			{
				return;
			}
			NavigationMesh component = GetComponent<NavigationMesh>();
			component.polygonColliderHoles.Clear();
			for (int i = 0; i < data.linkedIDs.Count; i++)
			{
				PolygonCollider2D polygonCollider2D = Serializer.returnComponent<PolygonCollider2D>(data.linkedIDs[i]);
				if (polygonCollider2D != null)
				{
					component.AddHole(polygonCollider2D);
				}
			}
		}
	}
}
