using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class InvItem
	{
		public int count;

		public Texture2D tex;

		public Texture2D activeTex;

		public bool carryOnStart;

		public bool canCarryMultiple;

		public bool useSeparateSlots;

		public string label;

		public string altLabel;

		public int id;

		public int lineID = -1;

		public int useIconID;

		public int binID;

		public bool isEditing;

		public int recipeSlot = -1;

		public ActionListAsset useActionList;

		public ActionListAsset lookActionList;

		public List<InvInteraction> interactions = new List<InvInteraction>();

		public List<ActionListAsset> combineActionList = new List<ActionListAsset>();

		public ActionListAsset unhandledActionList;

		public ActionListAsset unhandledCombineActionList;

		public List<int> combineID = new List<int>();

		public InvItem(int[] idArray)
		{
			count = 0;
			tex = null;
			activeTex = null;
			id = 0;
			binID = -1;
			recipeSlot = -1;
			useSeparateSlots = false;
			interactions = new List<InvInteraction>();
			combineActionList = new List<ActionListAsset>();
			combineID = new List<int>();
			foreach (int num in idArray)
			{
				if (id == num)
				{
					id++;
				}
			}
			label = "Inventory item " + (id + 1);
			altLabel = string.Empty;
		}

		public InvItem(InvItem assetItem)
		{
			count = assetItem.count;
			tex = assetItem.tex;
			activeTex = assetItem.activeTex;
			carryOnStart = assetItem.carryOnStart;
			canCarryMultiple = assetItem.canCarryMultiple;
			label = assetItem.label;
			altLabel = assetItem.altLabel;
			id = assetItem.id;
			lineID = assetItem.lineID;
			useIconID = assetItem.useIconID;
			binID = assetItem.binID;
			useSeparateSlots = assetItem.useSeparateSlots;
			isEditing = false;
			recipeSlot = -1;
			useActionList = assetItem.useActionList;
			lookActionList = assetItem.lookActionList;
			interactions = assetItem.interactions;
			combineActionList = assetItem.combineActionList;
			unhandledActionList = assetItem.unhandledActionList;
			unhandledCombineActionList = assetItem.unhandledCombineActionList;
			combineID = assetItem.combineID;
		}

		public bool DoesHaveInventoryInteraction(InvItem invItem)
		{
			if (invItem != null)
			{
				foreach (int item in combineID)
				{
					if (item == invItem.id)
					{
						return true;
					}
				}
			}
			return false;
		}

		public string GetLabel()
		{
			if (Options.GetLanguage() > 0)
			{
				return SpeechManager.GetTranslation(lineID, Options.GetLanguage());
			}
			if (altLabel != string.Empty)
			{
				return altLabel;
			}
			return label;
		}

		public int GetNextInteraction(int i, int numInvInteractions)
		{
			if (i < interactions.Count)
			{
				i++;
				if (i >= interactions.Count + numInvInteractions)
				{
					return 0;
				}
				return i;
			}
			if (i == interactions.Count - 1 + numInvInteractions)
			{
				return 0;
			}
			return i + 1;
		}

		public int GetPreviousInteraction(int i, int numInvInteractions)
		{
			if (i > interactions.Count && numInvInteractions > 0)
			{
				return i - 1;
			}
			if (i == 0)
			{
				return GetNumInteractions(numInvInteractions) - 1;
			}
			if (i <= interactions.Count)
			{
				i--;
				if (i < 0)
				{
					return GetNumInteractions(numInvInteractions) - 1;
				}
				return i;
			}
			return i - 1;
		}

		private int GetNumInteractions(int numInvInteractions)
		{
			return interactions.Count + numInvInteractions;
		}
	}
}
