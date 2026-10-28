using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionFadeIn : Action
	{
		public GameObject myActivate;

		public ActionFadeIn()
		{
			isDisplayed = true;
			title = "Custom: Fade In";
		}

		public override float Run()
		{
			FadeScreen component = GameObject.Find("_Fade").GetComponent<FadeScreen>();
			component.FadeIn();
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
