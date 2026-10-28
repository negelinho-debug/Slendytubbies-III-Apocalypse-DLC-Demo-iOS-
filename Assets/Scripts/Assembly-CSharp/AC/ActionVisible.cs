using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionVisible : Action
	{
		public enum VisState
		{
			Visible,
			Invisible
		}

		public int parameterID = -1;

		public int constantID;

		public GameObject obToAffect;

		public bool affectChildren;

		public VisState visState;

		public ActionVisible()
		{
			isDisplayed = true;
			title = "Object: Visibility";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			obToAffect = AssignFile(parameters, parameterID, constantID, obToAffect);
		}

		public override float Run()
		{
			bool enabled = false;
			if (visState == VisState.Visible)
			{
				enabled = true;
			}
			if ((bool)obToAffect)
			{
				if ((bool)obToAffect.GetComponent<Renderer>())
				{
					obToAffect.GetComponent<Renderer>().enabled = enabled;
				}
				if (affectChildren)
				{
					foreach (Transform item in obToAffect.transform)
					{
						if ((bool)item.gameObject.GetComponent<Renderer>())
						{
							item.gameObject.GetComponent<Renderer>().enabled = enabled;
						}
					}
				}
			}
			return 0f;
		}
	}
}
