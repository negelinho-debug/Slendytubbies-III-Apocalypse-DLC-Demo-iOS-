using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionPlayMaker : Action
	{
		public bool isPlayer;

		public int constantID;

		public int parameterID = -1;

		public GameObject linkedObject;

		public string fsmName;

		public string eventName;

		public ActionPlayMaker()
		{
			isDisplayed = true;
			title = "Third-Party: PlayMaker";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			if (isPlayer)
			{
				linkedObject = GameObject.FindWithTag("Player");
			}
			else
			{
				linkedObject = AssignFile(parameters, parameterID, constantID, linkedObject);
			}
		}

		public override float Run()
		{
			if (linkedObject != null && eventName != string.Empty)
			{
				if (fsmName != string.Empty)
				{
					PlayMakerIntegration.CallEvent(linkedObject, eventName, fsmName);
				}
				else
				{
					PlayMakerIntegration.CallEvent(linkedObject, eventName);
				}
			}
			return 0f;
		}
	}
}
