using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionSetPlayerPrefs : Action
	{
		public string playerPref;

		public string value;

		public ActionSetPlayerPrefs()
		{
			isDisplayed = true;
			title = "Custom: SetPlayerPrefs";
		}

		public override float Run()
		{
			PlayerPrefs.SetString(playerPref, value);
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
