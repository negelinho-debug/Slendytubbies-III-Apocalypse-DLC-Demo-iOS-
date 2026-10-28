using UnityEngine;

namespace AC
{
	public class _Collision : MonoBehaviour
	{
		[HideInInspector]
		public bool showInEditor;

		private void TurnOn()
		{
			if ((bool)GetComponent<Collider>())
			{
				GetComponent<Collider>().enabled = true;
			}
			else if ((bool)GetComponent<Collider2D>())
			{
				GetComponent<Collider2D>().enabled = true;
			}
			gameObject.layer = LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer);
		}

		private void TurnOff()
		{
			if ((bool)GetComponent<Collider>())
			{
				GetComponent<Collider>().enabled = false;
			}
			else if ((bool)GetComponent<Collider2D>())
			{
				GetComponent<Collider2D>().enabled = false;
			}
			gameObject.layer = LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.deactivatedLayer);
		}

		private void OnDrawGizmos()
		{
			if (showInEditor)
			{
				DrawGizmos();
			}
		}

		private void OnDrawGizmosSelected()
		{
			DrawGizmos();
		}

		private void DrawGizmos()
		{
			AdvGame.DrawCubeCollider(transform, new Color(0f, 1f, 1f, 0.8f));
		}
	}
}
