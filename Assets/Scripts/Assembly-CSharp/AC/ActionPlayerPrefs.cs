using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionPlayerPrefs : ActionCheck
	{
		public string playerPref;

		public string value;

		public ActionPlayerPrefs()
		{
			isDisplayed = true;
			title = "Custom: PlayerPrefs";
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				return defaultPauseTime;
			}
			isRunning = false;
			return 0f;
		}

		public override bool CheckCondition()
		{
			if (PlayerPrefs.GetString(playerPref) == value)
			{
				return true;
			}
			return false;
		}
	}
}
