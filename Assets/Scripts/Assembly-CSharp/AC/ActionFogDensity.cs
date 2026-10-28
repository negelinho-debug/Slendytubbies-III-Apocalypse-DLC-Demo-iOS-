using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionFogDensity : Action
	{
		public float fog;

		public ActionFogDensity()
		{
			isDisplayed = true;
			title = "Custom: Change Fog";
		}

		public override float Run()
		{
			RenderSettings.fogDensity = fog;
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
