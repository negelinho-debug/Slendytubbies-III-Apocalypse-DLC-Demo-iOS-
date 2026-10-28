using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionHotspotEnable : Action
	{
		public int parameterID = -1;

		public int constantID;

		public Hotspot hotspot;

		public ChangeType changeType;

		public ActionHotspotEnable()
		{
			isDisplayed = true;
			title = "Hotspot: Enable or disable";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			hotspot = AssignFile(parameters, parameterID, constantID, hotspot);
		}

		public override float Run()
		{
			if (hotspot == null)
			{
				return 0f;
			}
			if (changeType == ChangeType.Enable)
			{
				hotspot.TurnOn();
			}
			else
			{
				hotspot.TurnOff();
			}
			return 0f;
		}
	}
}
