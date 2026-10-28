using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionDisable : Action
	{
		public GameObject myActivate;

		public ActionDisable()
		{
			isDisplayed = true;
			title = "Object: Deactivate Object";
		}

		public override float Run()
		{
			myActivate.SetActive(false);
			isRunning = true;
			isRunning = false;
			return 0f;
		}
	}
}
