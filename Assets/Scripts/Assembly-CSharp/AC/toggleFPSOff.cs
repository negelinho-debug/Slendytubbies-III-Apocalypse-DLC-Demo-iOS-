using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class toggleFPSOff : Action
	{
		public toggleFPSOff()
		{
			isDisplayed = true;
			title = "Custom: Toggle FPS Off";
		}

		public override float Run()
		{
			GameObject gameObject = GameObject.FindWithTag("Player");
			gameObject.GetComponent<playerFPS>().enabled = false;
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
