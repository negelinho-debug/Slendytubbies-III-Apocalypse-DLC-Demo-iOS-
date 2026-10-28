using System;

namespace AC
{
	[Serializable]
	public class ActionCameraShake : Action
	{
		public int shakeIntensity;

		public float duration = 1f;

		public ActionCameraShake()
		{
			isDisplayed = true;
			title = "Camera: Shake";
		}

		public override float Run()
		{
			MainCamera mainCamera = KickStarter.mainCamera;
			if ((bool)mainCamera)
			{
				if (isRunning)
				{
					isRunning = false;
					return 0f;
				}
				isRunning = true;
				if (mainCamera.attachedCamera is GameCamera)
				{
					mainCamera.Shake((float)shakeIntensity / 67f, duration, true);
				}
				else if (mainCamera.attachedCamera is GameCamera25D)
				{
					mainCamera.Shake((float)shakeIntensity / 67f, duration, true);
					GameCamera25D gameCamera25D = (GameCamera25D)mainCamera.attachedCamera;
					if ((bool)gameCamera25D.backgroundImage)
					{
						gameCamera25D.backgroundImage.Shake((float)shakeIntensity / 0.67f, duration);
					}
				}
				else if (mainCamera.attachedCamera is GameCamera2D)
				{
					mainCamera.Shake((float)shakeIntensity / 33f, duration, false);
				}
				else
				{
					mainCamera.Shake((float)shakeIntensity / 67f, duration, false);
				}
				if (willWait)
				{
					return duration;
				}
			}
			return 0f;
		}

		public override void Skip()
		{
		}
	}
}
