using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionInventorySet : Action
	{
		public InvAction invAction;

		public int parameterID = -1;

		public int invID;

		private int invNumber;

		public bool setAmount;

		public int amount = 1;

		public bool setPlayer;

		public int playerID;

		private InventoryManager inventoryManager;

		private SettingsManager settingsManager;

		public override void AssignValues(List<ActionParameter> parameters)
		{
			invID = AssignInvItemID(parameters, parameterID, invID);
		}

		public override float Run()
		{
			RuntimeInventory component = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
			if ((bool)component)
			{
				if (!setAmount)
				{
					amount = 1;
				}
				settingsManager = AdvGame.GetReferences().settingsManager;
				if (settingsManager.playerSwitching == PlayerSwitching.Allow && !settingsManager.shareInventory && setPlayer)
				{
					if (invAction == InvAction.Add)
					{
						component.Add(invID, amount, false, playerID);
					}
					else
					{
						component.Remove(invID, amount, setAmount, playerID);
					}
				}
				else if (invAction == InvAction.Add)
				{
					component.Add(invID, amount, false, -1);
				}
				else
				{
					component.Remove(invID, amount, setAmount, -1);
				}
				PlayerMenus.ResetInventoryBoxes();
			}
			return 0f;
		}
	}
}
