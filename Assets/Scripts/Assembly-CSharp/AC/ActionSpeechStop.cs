using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionSpeechStop : Action
	{
		public bool forceMenus;

		public ActionSpeechStop()
		{
			isDisplayed = true;
			title = "Dialogue: Stop speech";
		}

		public override float Run()
		{
			Dialog component = GameObject.FindWithTag("GameEngine").GetComponent<Dialog>();
			component.KillDialog(forceMenus);
			return 0f;
		}
	}
}
