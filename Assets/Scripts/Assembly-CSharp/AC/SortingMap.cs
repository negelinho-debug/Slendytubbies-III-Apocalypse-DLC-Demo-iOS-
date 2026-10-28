using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class SortingMap : MonoBehaviour
	{
		public SortingMapType mapType = SortingMapType.OrderInLayer;

		public List<SortingArea> sortingAreas = new List<SortingArea>();

		public bool affectScale;

		public bool affectSpeed = true;

		public int originScale = 100;

		private FollowSortingMap[] followers;

		private void Start()
		{
			GetAllFollowers();
		}

		public void GetAllFollowers()
		{
			followers = UnityEngine.Object.FindObjectsOfType(typeof(FollowSortingMap)) as FollowSortingMap[];
		}

		private void OnDrawGizmos()
		{
			for (int i = 0; i < sortingAreas.Count; i++)
			{
				Gizmos.DrawIcon(GetAreaPosition(i), string.Empty, true);
				Gizmos.color = sortingAreas[i].color;
				if (i == 0)
				{
					Gizmos.DrawLine(transform.position, GetAreaPosition(i));
				}
				else
				{
					Gizmos.DrawLine(GetAreaPosition(i - 1), GetAreaPosition(i));
				}
			}
		}

		public void UpdateSimilarFollowers(FollowSortingMap follower)
		{
			if (followers == null || followers.Length <= 1 || !follower.followSortingMap)
			{
				return;
			}
			string order = follower.GetOrder();
			List<FollowSortingMap> list = new List<FollowSortingMap>();
			FollowSortingMap[] array = followers;
			foreach (FollowSortingMap followSortingMap in array)
			{
				if (followSortingMap.followSortingMap && !followSortingMap.lockSorting && followSortingMap != follower && followSortingMap.GetOrder() == order)
				{
					list.Add(followSortingMap);
				}
			}
			if (list.Count > 0)
			{
				list.Add(follower);
				list.Sort(SortByScreenPosition);
				for (int j = 0; j < list.Count; j++)
				{
					list[j].SetDepth((float)j * 0.001f);
				}
			}
			else
			{
				follower.SetDepth(0f);
			}
		}

		private static int SortByScreenPosition(FollowSortingMap o1, FollowSortingMap o2)
		{
			return Camera.main.WorldToScreenPoint(o1.transform.position).y.CompareTo(Camera.main.WorldToScreenPoint(o2.transform.position).y);
		}

		public Vector3 GetAreaPosition(int i)
		{
			return transform.position + transform.forward * sortingAreas[i].z;
		}

		public float GetScale(Vector3 followPosition)
		{
			if (!affectScale)
			{
				return 1f;
			}
			if (sortingAreas.Count == 0)
			{
				return originScale;
			}
			if (Vector3.Angle(transform.forward, transform.position - followPosition) < 90f)
			{
				return originScale;
			}
			if (Vector3.Angle(transform.forward, GetAreaPosition(sortingAreas.Count - 1) - followPosition) > 90f)
			{
				return sortingAreas[sortingAreas.Count - 1].scale;
			}
			for (int i = 0; i < sortingAreas.Count; i++)
			{
				float num = Vector3.Angle(transform.forward, GetAreaPosition(i) - followPosition);
				if (num < 90f)
				{
					float num2 = 0f;
					if (i > 0)
					{
						num2 = sortingAreas[i - 1].z;
					}
					float num3 = 1f - Vector3.Distance(GetAreaPosition(i), followPosition) / (sortingAreas[i].z - num2) * Mathf.Cos((float)Math.PI / 180f * num);
					float num4 = originScale;
					if (i > 0)
					{
						num4 = sortingAreas[i - 1].scale;
					}
					return num4 + num3 * ((float)sortingAreas[i].scale - num4);
				}
			}
			return 1f;
		}

		public void SetInBetweenScales()
		{
			if (sortingAreas.Count >= 2)
			{
				float num = sortingAreas[sortingAreas.Count - 1].scale;
				float z = sortingAreas[sortingAreas.Count - 1].z;
				for (int i = 0; i < sortingAreas.Count - 1; i++)
				{
					float num2 = sortingAreas[i].z / z * (num - (float)originScale) + (float)originScale;
					sortingAreas[i].scale = (int)num2;
				}
			}
		}
	}
}
