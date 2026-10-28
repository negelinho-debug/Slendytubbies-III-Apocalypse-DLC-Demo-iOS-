using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class toggleFPS : Action
	{
		public toggleFPS()
		{
			isDisplayed = true;
			title = "Custom: Toggle FPS";
		}

		public override float Run()
		{
			GameObject gameObject = GameObject.FindWithTag("Player");
			gameObject.GetComponent<playerFPS>().enabled = true;
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
