using UnityEngine;

namespace AC
{
	public class LimitVisibility : MonoBehaviour
	{
		public _Camera limitToCamera;

		public bool affectChildren;

		private _Camera activeCamera;

		private bool isVisible;

		private void Start()
		{
			activeCamera = KickStarter.mainCamera.attachedCamera;
			if (activeCamera == limitToCamera)
			{
				SetVisibility(true);
			}
			else if (activeCamera != limitToCamera)
			{
				SetVisibility(false);
			}
		}

		private void Update()
		{
			activeCamera = KickStarter.mainCamera.attachedCamera;
			if (activeCamera == limitToCamera && !isVisible)
			{
				SetVisibility(true);
			}
			else if (activeCamera != limitToCamera && isVisible)
			{
				SetVisibility(false);
			}
		}

		private void SetVisibility(bool state)
		{
			if ((bool)GetComponent<Renderer>())
			{
				GetComponent<Renderer>().enabled = state;
			}
			else if ((bool)gameObject.GetComponent<SpriteRenderer>())
			{
				gameObject.GetComponent<SpriteRenderer>().enabled = state;
			}
			if (affectChildren)
			{
				SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
				SpriteRenderer[] array = componentsInChildren;
				foreach (SpriteRenderer spriteRenderer in array)
				{
					spriteRenderer.enabled = state;
				}
			}
			isVisible = state;
		}
	}
}
