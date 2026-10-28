using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionFadeOut : Action
	{
		public GameObject myActivate;

		public ActionFadeOut()
		{
			isDisplayed = true;
			title = "Custom: Fade Out";
		}

		public override float Run()
		{
			FadeScreen component = GameObject.Find("_Fade").GetComponent<FadeScreen>();
			component.FadeOut();
			if (!isRunning)
			{
				isRunning = true;
				return defaultPauseTime;
			}
			isRunning = false;
			return 0f;
		}
	}
}
