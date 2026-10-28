using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionObjectCheck : ActionCheck
	{
		public GameObject gameObject;

		public int parameterID = -1;

		public int constantID;

		public ActionObjectCheck()
		{
			isDisplayed = true;
			title = "Object: Check presence";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			gameObject = AssignFile(parameters, parameterID, constantID, gameObject);
		}

		public override bool CheckCondition()
		{
			if (gameObject != null && gameObject.activeInHierarchy)
			{
				return true;
			}
			return false;
		}
	}
}
