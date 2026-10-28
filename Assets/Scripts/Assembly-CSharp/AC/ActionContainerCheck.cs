using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionContainerCheck : ActionCheck
	{
		public enum IntCondition
		{
			EqualTo,
			NotEqualTo,
			LessThan,
			MoreThan
		}

		public int invParameterID = -1;

		public int invID;

		private int invNumber;

		public bool useActive;

		public int parameterID = -1;

		public int constantID;

		public Container container;

		public bool doCount;

		public int intValue = 1;

		public IntCondition intCondition;

		private InventoryManager inventoryManager;

		public ActionContainerCheck()
		{
			isDisplayed = true;
			title = "Container: Check";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			container = AssignFile(parameters, parameterID, constantID, container);
			invID = AssignInvItemID(parameters, invParameterID, invID);
			if (useActive)
			{
				container = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>().activeContainer;
			}
		}

		public override bool CheckCondition()
		{
			if (container == null)
			{
				return false;
			}
			int count = container.GetCount(invID);
			if (doCount)
			{
				if (intCondition == IntCondition.EqualTo)
				{
					if (count == intValue)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.NotEqualTo)
				{
					if (count != intValue)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.LessThan)
				{
					if (count < intValue)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.MoreThan && count > intValue)
				{
					return true;
				}
			}
			else if (count > 0)
			{
				return true;
			}
			return false;
		}
	}
}
