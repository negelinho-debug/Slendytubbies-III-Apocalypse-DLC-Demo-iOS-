using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class NavigationMesh : MonoBehaviour
	{
		public List<PolygonCollider2D> polygonColliderHoles = new List<PolygonCollider2D>();

		public bool showInEditor = true;

		public bool disableRenderer = true;

		private SettingsManager settingsManager;

		private SceneSettings sceneSettings;

		private void Awake()
		{
			if (disableRenderer)
			{
				Hide();
			}
			settingsManager = AdvGame.GetReferences().settingsManager;
			sceneSettings = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
			ResetHoles();
		}

		public void AddHole(PolygonCollider2D newHole)
		{
			foreach (PolygonCollider2D polygonColliderHole in polygonColliderHoles)
			{
				if (polygonColliderHole == newHole)
				{
					return;
				}
			}
			polygonColliderHoles.Add(newHole);
			ResetHoles();
			if (GetComponent<RememberNavMesh2D>() == null)
			{
				Debug.LogWarning("Changes to " + gameObject.name + "'s holes will not be saved because it has no RememberNavMesh2D script");
			}
		}

		public void RemoveHole(PolygonCollider2D oldHole)
		{
			foreach (PolygonCollider2D polygonColliderHole in polygonColliderHoles)
			{
				if (polygonColliderHole == oldHole)
				{
					polygonColliderHoles.Remove(polygonColliderHole);
					ResetHoles();
					break;
				}
			}
		}

		private void ResetHoles()
		{
			if ((bool)GetComponent<PolygonCollider2D>())
			{
				PolygonCollider2D component = GetComponent<PolygonCollider2D>();
				component.pathCount = 1;
				if (polygonColliderHoles.Count == 0)
				{
					return;
				}
				{
					foreach (PolygonCollider2D polygonColliderHole in polygonColliderHoles)
					{
						if (polygonColliderHole != null)
						{
							component.pathCount++;
							List<Vector2> list = new List<Vector2>();
							Vector2[] points = polygonColliderHole.points;
							foreach (Vector2 vector in points)
							{
								list.Add(polygonColliderHole.transform.TransformPoint(vector) - transform.position);
							}
							component.SetPath(component.pathCount - 1, list.ToArray());
							polygonColliderHole.gameObject.layer = LayerMask.NameToLayer(settingsManager.deactivatedLayer);
							polygonColliderHole.isTrigger = true;
						}
					}
					return;
				}
			}
			if ((bool)GetComponent<MeshCollider>() && GetComponent<MeshCollider>().sharedMesh == null)
			{
				if ((bool)GetComponent<MeshFilter>() && (bool)GetComponent<MeshFilter>().sharedMesh)
				{
					GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
					Debug.LogWarning(gameObject.name + " has no MeshCollider mesh - temporarily using MeshFilter mesh instead.");
				}
				else
				{
					Debug.LogWarning(gameObject.name + " has no MeshCollider mesh.");
				}
			}
		}

		public void TurnOn()
		{
			if (sceneSettings == null)
			{
				sceneSettings = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
			}
			if (settingsManager == null)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			if ((bool)sceneSettings && (sceneSettings.navigationMethod == AC_NavigationMethod.meshCollider || sceneSettings.navigationMethod == AC_NavigationMethod.PolygonCollider))
			{
				if (LayerMask.NameToLayer(settingsManager.navMeshLayer) == -1)
				{
					Debug.LogWarning("Can't find layer " + settingsManager.navMeshLayer + " - please define it in the Tags Manager and list it in the Settings Manager.");
				}
				else if (settingsManager.navMeshLayer != string.Empty)
				{
					gameObject.layer = LayerMask.NameToLayer(settingsManager.navMeshLayer);
				}
				if (sceneSettings.navigationMethod == AC_NavigationMethod.meshCollider && GetComponent<Collider>() == null)
				{
					Debug.LogWarning("A Collider component must be attached to " + name + " for pathfinding to work - please attach one.");
				}
				else if (sceneSettings.navigationMethod == AC_NavigationMethod.PolygonCollider && GetComponent<Collider2D>() == null)
				{
					Debug.LogWarning("A 2D Collider component must be attached to " + name + " for pathfinding to work - please attach one.");
				}
			}
			else if ((bool)sceneSettings)
			{
				Debug.LogWarning("Cannot enable NavMesh " + name + " as this scene's Navigation Method is Unity Navigation.");
			}
			else
			{
				Debug.LogWarning("Cannot enable NavMesh - no SceneSettings found.");
			}
		}

		public void TurnOff()
		{
			if (settingsManager == null)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			gameObject.layer = LayerMask.NameToLayer(settingsManager.deactivatedLayer);
		}

		public void Hide()
		{
			if ((bool)GetComponent<MeshRenderer>())
			{
				GetComponent<MeshRenderer>().enabled = false;
			}
		}

		public void Show()
		{
			if ((bool)GetComponent<MeshRenderer>() && (bool)GetComponent<MeshFilter>() && (bool)GetComponent<MeshCollider>() && (bool)GetComponent<MeshCollider>().sharedMesh)
			{
				GetComponent<MeshFilter>().mesh = GetComponent<MeshCollider>().sharedMesh;
				GetComponent<MeshRenderer>().enabled = true;
				GetComponent<MeshRenderer>().castShadows = false;
				GetComponent<MeshRenderer>().receiveShadows = false;
			}
		}

		protected void OnDrawGizmos()
		{
			if (showInEditor)
			{
				DrawGizmos();
			}
		}

		protected void OnDrawGizmosSelected()
		{
			DrawGizmos();
		}

		public virtual void DrawGizmos()
		{
			if ((bool)GetComponent<PolygonCollider2D>())
			{
				AdvGame.DrawPolygonCollider(transform, GetComponent<PolygonCollider2D>(), Color.white);
			}
		}

		private void OnDestroy()
		{
			sceneSettings = null;
			settingsManager = null;
		}
	}
}
