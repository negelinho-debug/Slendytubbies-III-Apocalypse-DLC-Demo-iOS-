using UnityEngine;

namespace AC
{
	public class GameCamera25D : _Camera
	{
		public BackgroundImage backgroundImage;

		public void SetActiveBackground()
		{
			if (!this.backgroundImage)
			{
				return;
			}
			BackgroundImage[] array = Object.FindObjectsOfType(typeof(BackgroundImage)) as BackgroundImage[];
			BackgroundImage[] array2 = array;
			foreach (BackgroundImage backgroundImage in array2)
			{
				if (backgroundImage == this.backgroundImage)
				{
					backgroundImage.TurnOn();
				}
				else
				{
					backgroundImage.TurnOff();
				}
			}
			if ((bool)GameObject.FindWithTag("BackgroundCamera") && (bool)GameObject.FindWithTag("BackgroundCamera").GetComponent<BackgroundCamera>())
			{
				BackgroundCamera component = GameObject.FindWithTag("BackgroundCamera").GetComponent<BackgroundCamera>();
				component.TurnOn();
			}
			else
			{
				Debug.LogWarning("No BackgroundCamera found - please use the Scene Manager to Organise Room Objects with 2.5D Camera Projection.");
			}
			KickStarter.mainCamera.PrepareForBackground();
		}

		public new void ResetTarget()
		{
		}

		public void SnapCameraInEditor()
		{
			if ((bool)KickStarter.mainCamera)
			{
				MainCamera mainCamera = KickStarter.mainCamera;
				mainCamera.transform.parent = transform;
				mainCamera.transform.localPosition = Vector3.zero;
				mainCamera.transform.localEulerAngles = Vector3.zero;
				mainCamera.GetComponent<Camera>().orthographic = GetComponent<Camera>().orthographic;
				mainCamera.GetComponent<Camera>().fieldOfView = GetComponent<Camera>().fieldOfView;
				mainCamera.GetComponent<Camera>().farClipPlane = GetComponent<Camera>().farClipPlane;
				mainCamera.GetComponent<Camera>().nearClipPlane = GetComponent<Camera>().nearClipPlane;
				mainCamera.GetComponent<Camera>().orthographicSize = GetComponent<Camera>().orthographicSize;
			}
		}
	}
}
