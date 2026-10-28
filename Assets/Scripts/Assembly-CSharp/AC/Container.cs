using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class Container : MonoBehaviour
	{
		public List<ContainerItem> items = new List<ContainerItem>();

		private InventoryManager inventoryManager;

		public void Awake()
		{
			if ((bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().inventoryManager)
			{
				inventoryManager = AdvGame.GetReferences().inventoryManager;
			}
		}

		public void Interact()
		{
			if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>())
			{
				GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>().activeContainer = this;
			}
		}

		public void Add(int _id, int amount)
		{
			foreach (ContainerItem item in items)
			{
				if (item.linkedID == _id)
				{
					if (inventoryManager.CanCarryMultiple(item.linkedID))
					{
						item.count += amount;
					}
					return;
				}
			}
			foreach (InvItem item2 in inventoryManager.items)
			{
				if (item2.id == _id)
				{
					if (!inventoryManager.CanCarryMultiple(_id))
					{
						amount = 1;
					}
					items.Add(new ContainerItem(_id, amount, GetIDArray()));
				}
			}
		}

		public void Remove(int _id, int amount)
		{
			foreach (ContainerItem item in items)
			{
				if (item.linkedID == _id)
				{
					if (item.count > 0)
					{
						item.count -= amount;
					}
					if (item.count < 1)
					{
						items.Remove(item);
					}
					break;
				}
			}
		}

		public int GetCount(int _id)
		{
			foreach (ContainerItem item in items)
			{
				if (item.linkedID == _id)
				{
					return item.count;
				}
			}
			return 0;
		}

		public void InsertAt(InvItem _item, int _index)
		{
			ContainerItem containerItem = new ContainerItem(_item.id, GetIDArray());
			containerItem.count = _item.count;
			if (items.Count <= _index)
			{
				items.Add(containerItem);
			}
			else
			{
				items.Insert(_index, containerItem);
			}
		}

		public int[] GetIDArray()
		{
			List<int> list = new List<int>();
			foreach (ContainerItem item in items)
			{
				list.Add(item.id);
			}
			list.Sort();
			return list.ToArray();
		}

		private void OnDestroy()
		{
			inventoryManager = null;
		}
	}
}
