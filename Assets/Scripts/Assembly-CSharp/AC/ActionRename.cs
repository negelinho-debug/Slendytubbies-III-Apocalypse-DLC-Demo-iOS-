using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionRename : Action
	{
		public int constantID;

		public int parameterID = -1;

		public Hotspot hotspot;

		public string newName;

		public ActionRename()
		{
			isDisplayed = true;
			title = "Hotspot: Rename";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			hotspot = AssignFile(parameters, parameterID, constantID, hotspot);
		}

		public override float Run()
		{
			if ((bool)hotspot && newName != string.Empty)
			{
				hotspot.hotspotName = newName;
			}
			return 0f;
		}
	}
}
