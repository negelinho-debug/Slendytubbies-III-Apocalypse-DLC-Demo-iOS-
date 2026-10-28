using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class InventoryManager : ScriptableObject
	{
		public List<InvItem> items = new List<InvItem>();

		public List<InvBin> bins = new List<InvBin>();

		public ActionListAsset unhandledCombine;

		public ActionListAsset unhandledHotspot;

		public List<Recipe> recipes = new List<Recipe>();

		public string GetLabel(int _id)
		{
			string result = string.Empty;
			foreach (InvItem item in items)
			{
				if (item.id == _id)
				{
					result = item.label;
				}
			}
			return result;
		}

		public InvItem GetItem(int _id)
		{
			foreach (InvItem item in items)
			{
				if (item.id == _id)
				{
					return item;
				}
			}
			return null;
		}

		public bool CanCarryMultiple(int _id)
		{
			foreach (InvItem item in items)
			{
				if (item.id == _id)
				{
					return item.canCarryMultiple;
				}
			}
			return false;
		}
	}
}
