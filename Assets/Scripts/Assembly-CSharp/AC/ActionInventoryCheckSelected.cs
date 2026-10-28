using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionInventoryCheckSelected : ActionCheck
	{
		public int parameterID = -1;

		public int invID;

		public bool checkNothing;

		private InventoryManager inventoryManager;

		public override void AssignValues(List<ActionParameter> parameters)
		{
			invID = AssignInvItemID(parameters, parameterID, invID);
		}

		public override bool CheckCondition()
		{
			RuntimeInventory component = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
			if ((bool)component)
			{
				if (checkNothing)
				{
					if (component.selectedItem == null)
					{
						return true;
					}
				}
				else if (component.selectedItem != null && component.selectedItem.id == invID)
				{
					return true;
				}
			}
			return false;
		}
	}
}
