using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionContainerSet : Action
	{
		public enum ContainerAction
		{
			Add,
			Remove,
			RemoveAll
		}

		public ContainerAction containerAction;

		public int invParameterID = -1;

		public int invID;

		private int invNumber;

		public bool useActive;

		public int constantID;

		public int parameterID = -1;

		public Container container;

		public bool setAmount;

		public int amount = 1;

		public bool transferToPlayer;

		private InventoryManager inventoryManager;

		public override void AssignValues(List<ActionParameter> parameters)
		{
			container = AssignFile(parameters, parameterID, constantID, container);
			invID = AssignInvItemID(parameters, invParameterID, invID);
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
			if (!setAmount)
			{
				amount = 1;
			}
			if (containerAction == ContainerAction.Add)
			{
				container.Add(invID, amount);
			}
			else if (containerAction == ContainerAction.Remove)
			{
				if (transferToPlayer)
				{
					RuntimeInventory component = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
					component.Add(invID, amount, false, -1);
				}
				container.Remove(invID, amount);
			}
			else if (containerAction == ContainerAction.RemoveAll)
			{
				if (transferToPlayer)
				{
					RuntimeInventory component2 = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
					foreach (ContainerItem item in container.items)
					{
						component2.Add(item.linkedID, item.count, false, -1);
					}
				}
				container.items.Clear();
			}
			PlayerMenus.ResetInventoryBoxes();
			return 0f;
		}
	}
}
