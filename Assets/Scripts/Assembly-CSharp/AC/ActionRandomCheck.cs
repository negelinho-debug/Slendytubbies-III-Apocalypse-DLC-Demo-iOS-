using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionRandomCheck : ActionCheckMultiple
	{
		public ActionRandomCheck()
		{
			isDisplayed = true;
			title = "Variable: Check random number";
		}

		public override int End(List<Action> actions)
		{
			if (numSockets <= 0)
			{
				Debug.LogWarning("Could not compute Random check because no values were possible!");
				return -1;
			}
			int i = UnityEngine.Random.Range(0, numSockets);
			return ProcessResult(i, actions);
		}
	}
}
