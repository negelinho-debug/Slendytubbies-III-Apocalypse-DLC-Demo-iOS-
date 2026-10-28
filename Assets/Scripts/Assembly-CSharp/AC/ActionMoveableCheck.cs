using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionMoveableCheck : ActionCheck
	{
		public DragBase dragObject;

		public int constantID;

		public int parameterID = -1;

		public ActionMoveableCheck()
		{
			isDisplayed = true;
			title = "Moveable: Check held by player";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			dragObject = AssignFile(parameters, parameterID, constantID, dragObject);
		}

		public override bool CheckCondition()
		{
			if ((bool)dragObject)
			{
				PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
				return component.IsDragObjectHeld(dragObject);
			}
			return false;
		}
	}
}
