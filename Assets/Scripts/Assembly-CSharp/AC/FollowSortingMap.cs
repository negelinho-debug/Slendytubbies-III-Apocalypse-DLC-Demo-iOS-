using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class FollowSortingMap : MonoBehaviour
	{
		private enum DepthAxis
		{
			Y,
			Z
		}

		public bool lockSorting;

		public bool affectChildren = true;

		public bool followSortingMap;

		public bool offsetOriginal;

		private float originalDepth;

		private DepthAxis depthAxis;

		private List<int> offsets = new List<int>();

		private int sortingOrder;

		private string sortingLayer = string.Empty;

		private SortingMap sortingMap;

		private Vector2 oldPosition;

		private SettingsManager settingsManager;

		private void Awake()
		{
			settingsManager = AdvGame.GetReferences().settingsManager;
			if (!settingsManager.IsInLoadingScene())
			{
				SetOriginalDepth();
			}
		}

		private void Start()
		{
			if (!settingsManager.IsInLoadingScene())
			{
				oldPosition = transform.position;
				UpdateSortingMap();
				SetOriginalOffsets();
				StartCoroutine("_Update");
			}
		}

		private IEnumerator _Update()
		{
			while (Application.isPlaying)
			{
				UpdateRenderers();
				yield return new WaitForFixedUpdate();
			}
		}

		private void SetOriginalOffsets()
		{
			offsets = new List<int>();
			if (!offsetOriginal || !GetComponent<Renderer>())
			{
				return;
			}
			if (affectChildren)
			{
				Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
				Renderer[] array = componentsInChildren;
				foreach (Renderer renderer in array)
				{
					offsets.Add(renderer.sortingOrder);
				}
			}
			else
			{
				offsets.Add(GetComponent<Renderer>().sortingOrder);
			}
		}

		public string GetOrder()
		{
			if (sortingMap == null)
			{
				return string.Empty;
			}
			if (sortingMap.mapType == SortingMapType.OrderInLayer)
			{
				return sortingOrder.ToString();
			}
			return sortingLayer;
		}

		private void OnLevelWasLoaded()
		{
			if (!AdvGame.GetReferences().settingsManager.IsInLoadingScene())
			{
				UpdateSortingMap();
				SetOriginalOffsets();
				StopCoroutine("_Update");
				StartCoroutine("_Update");
			}
		}

		private void SetOriginalDepth()
		{
			if (!(settingsManager == null))
			{
				if (settingsManager.IsTopDown())
				{
					depthAxis = DepthAxis.Y;
					originalDepth = transform.position.y;
				}
				else
				{
					depthAxis = DepthAxis.Z;
					originalDepth = transform.position.z;
				}
			}
		}

		public void SetDepth(float depth)
		{
			if (depthAxis == DepthAxis.Y)
			{
				transform.position = new Vector3(transform.position.x, originalDepth + depth, transform.position.z);
				oldPosition = new Vector2(transform.position.x, transform.position.z);
			}
			else
			{
				transform.position = new Vector3(transform.position.x, transform.position.y, originalDepth + depth);
				oldPosition = new Vector2(transform.position.x, transform.position.y);
			}
		}

		public void UpdateSortingMap()
		{
			if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>() && GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>().sortingMap != null)
			{
				sortingMap = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>().sortingMap;
				SetOriginalDepth();
				sortingMap.UpdateSimilarFollowers(this);
			}
			else
			{
				Debug.Log("Cannot find sorting map to follow!");
			}
		}

		private void UpdateRenderers()
		{
			if (lockSorting || !followSortingMap || sortingMap == null || GetComponent<Renderer>() == null)
			{
				return;
			}
			if ((depthAxis == DepthAxis.Y && oldPosition != new Vector2(transform.position.x, transform.position.z)) || (depthAxis == DepthAxis.Z && oldPosition != new Vector2(transform.position.x, transform.position.y)))
			{
				sortingMap.UpdateSimilarFollowers(this);
			}
			if (depthAxis == DepthAxis.Y)
			{
				oldPosition = new Vector2(transform.position.x, transform.position.z);
			}
			else
			{
				oldPosition = new Vector2(transform.position.x, transform.position.y);
			}
			if (sortingMap.sortingAreas.Count > 0)
			{
				if (sortingMap.mapType == SortingMapType.OrderInLayer)
				{
					sortingOrder = sortingMap.sortingAreas[sortingMap.sortingAreas.Count - 1].order;
				}
				else if (sortingMap.mapType == SortingMapType.SortingLayer)
				{
					sortingLayer = sortingMap.sortingAreas[sortingMap.sortingAreas.Count - 1].layer;
				}
				for (int i = 0; i < sortingMap.sortingAreas.Count; i++)
				{
					if (Vector3.Angle(sortingMap.transform.forward, sortingMap.GetAreaPosition(i) - transform.position) < 90f)
					{
						if (sortingMap.mapType == SortingMapType.OrderInLayer)
						{
							sortingOrder = sortingMap.sortingAreas[i].order;
						}
						else if (sortingMap.mapType == SortingMapType.SortingLayer)
						{
							sortingLayer = sortingMap.sortingAreas[i].layer;
						}
						break;
					}
				}
			}
			if (!affectChildren)
			{
				if (sortingMap.mapType == SortingMapType.OrderInLayer)
				{
					GetComponent<Renderer>().sortingOrder = sortingOrder;
					if (offsetOriginal && offsets.Count > 0)
					{
						GetComponent<Renderer>().sortingOrder += offsets[0];
					}
				}
				else if (sortingMap.mapType == SortingMapType.SortingLayer)
				{
					GetComponent<Renderer>().sortingLayerName = sortingLayer;
				}
				return;
			}
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (sortingMap.mapType == SortingMapType.OrderInLayer)
				{
					componentsInChildren[j].sortingOrder = sortingOrder;
					if (offsetOriginal)
					{
						componentsInChildren[j].sortingOrder += offsets[j];
					}
				}
				else if (sortingMap.mapType == SortingMapType.SortingLayer)
				{
					componentsInChildren[j].sortingLayerName = sortingLayer;
				}
			}
		}

		public void LockSortingOrder(int order)
		{
			if (GetComponent<Renderer>() == null)
			{
				return;
			}
			lockSorting = true;
			if (!affectChildren)
			{
				GetComponent<Renderer>().sortingOrder = order;
				return;
			}
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				renderer.sortingOrder = order;
			}
		}

		public void LockSortingLayer(string layer)
		{
			if (GetComponent<Renderer>() == null)
			{
				return;
			}
			lockSorting = true;
			if (!affectChildren)
			{
				GetComponent<Renderer>().sortingLayerName = layer;
				return;
			}
			Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				renderer.sortingLayerName = layer;
			}
		}

		public float GetLocalScale()
		{
			if (followSortingMap && sortingMap != null && sortingMap.affectScale)
			{
				return sortingMap.GetScale(transform.position) / 100f;
			}
			return 0f;
		}

		public float GetLocalSpeed()
		{
			if (followSortingMap && sortingMap != null && sortingMap.affectScale && sortingMap.affectSpeed)
			{
				return sortingMap.GetScale(transform.position) / 100f;
			}
			return 1f;
		}
	}
}
