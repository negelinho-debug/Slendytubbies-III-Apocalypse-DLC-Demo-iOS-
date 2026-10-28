using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionFade : Action
	{
		public FadeType fadeType;

		public bool isInstant;

		public float fadeSpeed = 0.5f;

		public bool setTexture;

		public Texture2D tempTexture;

		public ActionFade()
		{
			isDisplayed = true;
			title = "Camera: Fade";
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				MainCamera mainCamera = KickStarter.mainCamera;
				RunSelf(mainCamera, fadeSpeed);
				if (willWait && !isInstant)
				{
					return fadeSpeed;
				}
				return 0f;
			}
			isRunning = false;
			return 0f;
		}

		public override void Skip()
		{
			RunSelf(KickStarter.mainCamera, 0f);
		}

		private void RunSelf(MainCamera mainCam, float _time)
		{
			if (mainCam == null)
			{
				return;
			}
			mainCam.StopCrossfade();
			if (fadeType == FadeType.fadeIn)
			{
				if (isInstant)
				{
					mainCam.FadeIn(0f);
				}
				else
				{
					mainCam.FadeIn(_time);
				}
				return;
			}
			Texture2D tempTex = tempTexture;
			if (!setTexture)
			{
				tempTex = null;
			}
			float timeToFade = _time;
			if (isInstant)
			{
				timeToFade = 0f;
			}
			mainCam.FadeOut(timeToFade, tempTex);
		}
	}
}
