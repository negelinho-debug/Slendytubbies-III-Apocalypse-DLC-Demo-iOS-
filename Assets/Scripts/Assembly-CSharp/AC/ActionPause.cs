using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionPause : Action
	{
		public int parameterID = -1;

		public float timeToPause;

		public ActionPause()
		{
			isDisplayed = true;
			title = "Engine: Pause game";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			timeToPause = AssignFloat(parameters, parameterID, timeToPause);
			timeToPause = Mathf.Max(0f, timeToPause);
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				return timeToPause;
			}
			isRunning = false;
			return 0f;
		}
	}
}
