using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionContainerOpen : Action
	{
		public bool useActive;

		public int parameterID = -1;

		public int constantID;

		public Container container;

		public ActionContainerOpen()
		{
			isDisplayed = true;
			title = "Container: Open";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			container = AssignFile(parameters, parameterID, constantID, container);
			if (useActive)
			{
				container = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>().activeContainer;
			}
		}

		public override float Run()
		{
			if (container == null)
			{
				return 0f;
			}
			container.Interact();
			return 0f;
		}
	}
}
