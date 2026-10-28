using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class TurnSpeed : Action
	{
		public float speed;

		public TurnSpeed()
		{
			isDisplayed = true;
			title = "Custom: Turn Speed";
		}

		public override float Run()
		{
			GameObject gameObject = GameObject.FindWithTag("Player");
			gameObject.GetComponent<Player>().turnSpeed = speed;
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
