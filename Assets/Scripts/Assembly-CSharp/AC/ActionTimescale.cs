using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionTimescale : Action
	{
		public float timeScale;

		public ActionTimescale()
		{
			isDisplayed = true;
			title = "Engine: Change timescale";
		}

		public override float Run()
		{
			if (timeScale >= 0f)
			{
				GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>().timeScale = timeScale;
				Time.timeScale = timeScale;
			}
			return 0f;
		}
	}
}
