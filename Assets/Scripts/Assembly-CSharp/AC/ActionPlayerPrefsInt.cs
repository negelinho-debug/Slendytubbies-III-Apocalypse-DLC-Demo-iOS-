using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionPlayerPrefsInt : ActionCheck
	{
		public string playerPref;

		public int value;

		public ActionPlayerPrefsInt()
		{
			isDisplayed = true;
			title = "Custom: PlayerPrefs INT";
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
			if (PlayerPrefs.GetInt(playerPref) == value)
			{
				return true;
			}
			return false;
		}
	}
}
