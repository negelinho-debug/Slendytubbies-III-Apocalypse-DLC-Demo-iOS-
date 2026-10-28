using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionEnable : Action
	{
		public GameObject myActivate;

		public ActionEnable()
		{
			isDisplayed = true;
			title = "Object: Activate Object";
		}

		public override float Run()
		{
			myActivate.SetActive(true);
			isRunning = true;
			isRunning = false;
			return 0f;
		}
	}
}
