using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionInventoryCheck : ActionCheck
	{
		public enum IntCondition
		{
			EqualTo,
			NotEqualTo,
			LessThan,
			MoreThan
		}

		public int parameterID = -1;

		public int invID;

		private int invNumber;

		public bool doCount;

		public int intValue = 1;

		public IntCondition intCondition;

		public bool setPlayer;

		public int playerID;

		private InventoryManager inventoryManager;

		private SettingsManager settingsManager;

		public ActionInventoryCheck()
		{
			isDisplayed = true;
			title = "Inventory: Check";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			invID = AssignInvItemID(parameters, parameterID, invID);
		}

		public override bool CheckCondition()
		{
			RuntimeInventory component = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
			settingsManager = AdvGame.GetReferences().settingsManager;
			int num = 0;
			num = ((settingsManager.playerSwitching != PlayerSwitching.Allow || settingsManager.shareInventory || !setPlayer) ? component.GetCount(invID) : component.GetCount(invID, playerID));
			if (doCount)
			{
				if (intCondition == IntCondition.EqualTo)
				{
					if (num == intValue)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.NotEqualTo)
				{
					if (num != intValue)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.LessThan)
				{
					if (num < intValue)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.MoreThan && num > intValue)
				{
					return true;
				}
			}
			else if (num > 0)
			{
				return true;
			}
			return false;
		}
	}
}
