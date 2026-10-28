using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionInventorySelect : Action
	{
		public enum InventorySelectType
		{
			SelectItem,
			DeselectActive
		}

		public InventorySelectType selectType;

		public bool giveToPlayer;

		public int parameterID = -1;

		public int invID;

		private int invNumber;

		private InventoryManager inventoryManager;

		public override void AssignValues(List<ActionParameter> parameters)
		{
			invID = AssignInvItemID(parameters, parameterID, invID);
		}

		public override float Run()
		{
			RuntimeInventory component = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
			if ((bool)component)
			{
				if (selectType == InventorySelectType.DeselectActive)
				{
					component.SetNull();
				}
				else
				{
					if (giveToPlayer)
					{
						component.Add(invID, 1, false, -1);
					}
					component.SelectItemByID(invID);
				}
			}
			return 0f;
		}
	}
}
