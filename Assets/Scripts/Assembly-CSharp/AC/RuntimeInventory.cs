using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class RuntimeInventory : MonoBehaviour
	{
		[HideInInspector]
		public List<InvItem> localItems = new List<InvItem>();

		[HideInInspector]
		public List<InvItem> craftingItems = new List<InvItem>();

		[HideInInspector]
		public ActionListAsset unhandledCombine;

		[HideInInspector]
		public ActionListAsset unhandledHotspot;

		[HideInInspector]
		public InvItem selectedItem;

		[HideInInspector]
		public InvItem hoverItem;

		[HideInInspector]
		public List<int> matchingInvInteractions = new List<int>();

		private GUIStyle countStyle;

		private TextEffects countTextEffects;

		private InventoryManager inventoryManager;

		private SettingsManager settingsManager;

		private PlayerInteraction playerInteraction;

		[HideInInspector]
		public InvItem highlightItem;

		private HighlightState highlightState;

		private float pulse;

		private int pulseDirection;

		public void Awake()
		{
			selectedItem = null;
			GetReferences();
			craftingItems.Clear();
			localItems.Clear();
			GetItemsOnStart();
			if ((bool)inventoryManager)
			{
				unhandledCombine = inventoryManager.unhandledCombine;
				unhandledHotspot = inventoryManager.unhandledHotspot;
			}
			else
			{
				Debug.LogError("An Inventory Manager is required - please use the Adventure Creator window to create one.");
			}
		}

		private void OnLevelWasLoaded()
		{
			if (!settingsManager.IsInLoadingScene())
			{
				GetReferences();
				SetNull();
			}
		}

		private void GetReferences()
		{
			if ((bool)AdvGame.GetReferences())
			{
				if ((bool)AdvGame.GetReferences().inventoryManager)
				{
					inventoryManager = AdvGame.GetReferences().inventoryManager;
				}
				if ((bool)AdvGame.GetReferences().settingsManager)
				{
					settingsManager = AdvGame.GetReferences().settingsManager;
				}
			}
			playerInteraction = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInteraction>();
		}

		public void SetNull()
		{
			selectedItem = null;
			highlightItem = null;
			PlayerMenus.ResetInventoryBoxes();
		}

		public void SelectItemByID(int _id)
		{
			if (_id == -1)
			{
				SetNull();
				return;
			}
			foreach (InvItem localItem in localItems)
			{
				if (localItem != null && localItem.id == _id)
				{
					selectedItem = localItem;
					PlayerMenus.ResetInventoryBoxes();
					return;
				}
			}
			SetNull();
			GetReferences();
			Debug.LogWarning("Want to select inventory item " + inventoryManager.GetLabel(_id) + " but player is not carrying it.");
		}

		public void SelectItem(InvItem item)
		{
			if (selectedItem == item)
			{
				selectedItem = null;
				GameObject.FindWithTag("GameEngine").GetComponent<PlayerCursor>().ResetSelectedCursor();
			}
			else
			{
				selectedItem = item;
			}
			PlayerMenus.ResetInventoryBoxes();
		}

		private void GetItemsOnStart()
		{
			if ((bool)inventoryManager)
			{
				foreach (InvItem item in inventoryManager.items)
				{
					if (item.carryOnStart)
					{
						if (!item.canCarryMultiple)
						{
							item.count = 1;
						}
						if (item.count >= 1)
						{
							item.recipeSlot = -1;
							if (item.canCarryMultiple && item.useSeparateSlots)
							{
								for (int i = 0; i < item.count; i++)
								{
									InvItem invItem = new InvItem(item);
									invItem.count = 1;
									localItems.Add(invItem);
								}
							}
							else
							{
								localItems.Add(new InvItem(item));
							}
						}
					}
				}
				return;
			}
			Debug.LogError("No Inventory Manager found - please use the Adventure Creator window to create one.");
		}

		public void Add(int _id, int amount, bool selectAfter, int playerID)
		{
			if (playerID >= 0 && KickStarter.player.ID != playerID)
			{
				AddToOtherPlayer(_id, amount, playerID);
			}
			else
			{
				localItems = Add(_id, amount, localItems, selectAfter);
			}
		}

		public void Remove(int _id, int amount, bool setAmount, int playerID)
		{
			if (playerID >= 0 && KickStarter.player.ID != playerID)
			{
				RemoveFromOtherPlayer(_id, amount, setAmount, playerID);
			}
			else
			{
				localItems = Remove(_id, amount, setAmount, localItems);
			}
		}

		private void AddToOtherPlayer(int invID, int amount, int playerID)
		{
			SaveSystem component = GetComponent<SaveSystem>();
			List<InvItem> itemsFromPlayer = component.GetItemsFromPlayer(playerID);
			itemsFromPlayer = Add(invID, amount, itemsFromPlayer, false);
			component.AssignItemsToPlayer(itemsFromPlayer, playerID);
		}

		private void RemoveFromOtherPlayer(int invID, int amount, bool setAmount, int playerID)
		{
			SaveSystem component = GetComponent<SaveSystem>();
			List<InvItem> itemsFromPlayer = component.GetItemsFromPlayer(playerID);
			itemsFromPlayer = Remove(invID, amount, setAmount, itemsFromPlayer);
			component.AssignItemsToPlayer(itemsFromPlayer, playerID);
		}

		public List<InvItem> Add(int _id, int amount, List<InvItem> itemList, bool selectAfter)
		{
			GetReferences();
			itemList = ReorderItems(itemList);
			foreach (InvItem item in itemList)
			{
				if (item == null || item.id != _id)
				{
					continue;
				}
				if (item.canCarryMultiple)
				{
					if (item.useSeparateSlots)
					{
						break;
					}
					item.count += amount;
				}
				if (selectAfter)
				{
					SelectItem(item);
				}
				return itemList;
			}
			foreach (InvItem item2 in inventoryManager.items)
			{
				if (item2.id != _id)
				{
					continue;
				}
				InvItem invItem = new InvItem(item2);
				if (!invItem.canCarryMultiple)
				{
					amount = 1;
				}
				invItem.recipeSlot = -1;
				invItem.count = amount;
				if (settingsManager.canReorderItems)
				{
					for (int i = 0; i < itemList.Count; i++)
					{
						if (itemList[i] != null)
						{
							continue;
						}
						itemList[i] = invItem;
						if (selectAfter)
						{
							SelectItem(invItem);
						}
						if (invItem.canCarryMultiple && invItem.useSeparateSlots)
						{
							int num = invItem.count - 1;
							invItem.count = 1;
							for (int j = 0; j < num; j++)
							{
								itemList.Add(invItem);
							}
						}
						return itemList;
					}
				}
				if (invItem.canCarryMultiple && invItem.useSeparateSlots)
				{
					int count = invItem.count;
					invItem.count = 1;
					for (int k = 0; k < count; k++)
					{
						itemList.Add(invItem);
					}
				}
				else
				{
					itemList.Add(invItem);
				}
				if (selectAfter)
				{
					SelectItem(invItem);
				}
				return itemList;
			}
			itemList = RemoveEmptySlots(itemList);
			return itemList;
		}

		public void Remove(InvItem _item)
		{
			if (_item != null && localItems.Contains(_item))
			{
				if (_item == selectedItem)
				{
					SetNull();
				}
				localItems[localItems.IndexOf(_item)] = null;
				localItems = ReorderItems(localItems);
				localItems = RemoveEmptySlots(localItems);
			}
		}

		private List<InvItem> Remove(int _id, int amount, bool setAmount, List<InvItem> itemList)
		{
			if (amount <= 0)
			{
				return itemList;
			}
			foreach (InvItem item in itemList)
			{
				if (item == null || item.id != _id)
				{
					continue;
				}
				if (item.canCarryMultiple && item.useSeparateSlots)
				{
					itemList[itemList.IndexOf(item)] = null;
					amount--;
					if (amount == 0)
					{
						break;
					}
					continue;
				}
				if (!item.canCarryMultiple || !setAmount)
				{
					itemList[itemList.IndexOf(item)] = null;
					amount = 0;
				}
				else
				{
					if (item.count > 0)
					{
						int num = item.count - amount;
						item.count -= amount;
						amount = num;
					}
					if (item.count < 1)
					{
						itemList[itemList.IndexOf(item)] = null;
					}
				}
				itemList = ReorderItems(itemList);
				itemList = RemoveEmptySlots(itemList);
				if (itemList.Count == 0)
				{
					return itemList;
				}
				if (amount <= 0)
				{
					return itemList;
				}
			}
			itemList = ReorderItems(itemList);
			itemList = RemoveEmptySlots(itemList);
			return itemList;
		}

		private List<InvItem> ReorderItems(List<InvItem> invItems)
		{
			if (!settingsManager.canReorderItems)
			{
				bool flag = false;
				while (!flag)
				{
					bool flag2 = false;
					for (int i = 0; i < invItems.Count; i++)
					{
						if (invItems[i] == null)
						{
							invItems.RemoveAt(i);
							flag2 = true;
						}
					}
					if (!flag2)
					{
						flag = true;
					}
				}
			}
			return invItems;
		}

		private void RemoveEmptyCraftingSlots()
		{
			int num = craftingItems.Count - 1;
			while (num >= 0 && localItems[num] == null)
			{
				localItems.RemoveAt(num);
				num--;
			}
		}

		private List<InvItem> RemoveEmptySlots(List<InvItem> itemList)
		{
			int num = itemList.Count - 1;
			while (num >= 0)
			{
				if (itemList[num] == null)
				{
					itemList.RemoveAt(num);
					num--;
					continue;
				}
				return itemList;
			}
			return itemList;
		}

		public string GetLabel(InvItem item)
		{
			if (Options.GetLanguage() > 0)
			{
				return SpeechManager.GetTranslation(item.lineID, Options.GetLanguage());
			}
			if (item.altLabel != string.Empty)
			{
				return item.altLabel;
			}
			return item.label;
		}

		public int GetCount(int _invID)
		{
			foreach (InvItem localItem in localItems)
			{
				if (localItem != null && localItem.id == _invID)
				{
					return localItem.count;
				}
			}
			return 0;
		}

		public int GetCount(int _invID, int _playerID)
		{
			List<InvItem> itemsFromPlayer = GetComponent<SaveSystem>().GetItemsFromPlayer(_playerID);
			if (itemsFromPlayer != null)
			{
				foreach (InvItem item in itemsFromPlayer)
				{
					if (item != null && item.id == _invID)
					{
						return item.count;
					}
				}
			}
			return 0;
		}

		public InvItem GetCraftingItem(int _id)
		{
			foreach (InvItem craftingItem in craftingItems)
			{
				if (craftingItem.id == _id)
				{
					return craftingItem;
				}
			}
			return null;
		}

		public InvItem GetItem(int _id)
		{
			foreach (InvItem localItem in localItems)
			{
				if (localItem != null && localItem.id == _id)
				{
					return localItem;
				}
			}
			return null;
		}

		public void Look(InvItem item)
		{
			if (item != null && item.recipeSlot <= -1)
			{
				GetReferences();
				if ((bool)item.lookActionList)
				{
					AdvGame.RunActionListAsset(item.lookActionList);
				}
			}
		}

		public void Use(InvItem item)
		{
			if (item != null && item.recipeSlot <= -1)
			{
				GetReferences();
				if ((bool)item.useActionList)
				{
					selectedItem = null;
					AdvGame.RunActionListAsset(item.useActionList);
				}
				else
				{
					SelectItem(item);
				}
			}
		}

		public void RunInteraction(InvItem invItem, int iconID)
		{
			if (invItem == null || invItem.recipeSlot > -1)
			{
				return;
			}
			GetReferences();
			foreach (InvInteraction interaction in invItem.interactions)
			{
				if (interaction.icon.id == iconID)
				{
					if ((bool)interaction.actionList)
					{
						AdvGame.RunActionListAsset(interaction.actionList);
					}
					break;
				}
			}
		}

		public void RunInteraction(bool isHover, int iconID)
		{
			GetReferences();
			InvItem invItem = selectedItem;
			if (isHover)
			{
				invItem = hoverItem;
			}
			foreach (InvInteraction interaction in invItem.interactions)
			{
				if (interaction.icon.id == iconID)
				{
					if ((bool)interaction.actionList)
					{
						AdvGame.RunActionListAsset(interaction.actionList);
					}
					break;
				}
			}
			selectedItem = null;
		}

		public void ShowInteractions(InvItem item)
		{
			selectedItem = item;
			PlayerMenus component = GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>();
			component.SetInteractionMenus(true);
		}

		public void Combine(int ID)
		{
			Combine(GetItem(ID));
		}

		public void Combine(InvItem item)
		{
			if (item == null || item.recipeSlot > -1)
			{
				return;
			}
			GetReferences();
			if (item == selectedItem)
			{
				selectedItem = null;
				if ((settingsManager.interactionMethod != AC_InteractionMethod.ChooseHotspotThenInteraction || settingsManager.inventoryInteractions == InventoryInteractions.Single) && settingsManager.inventoryDragDrop && settingsManager.inventoryDropLook)
				{
					Look(item);
				}
			}
			else
			{
				for (int i = 0; i < item.combineID.Count; i++)
				{
					if (item.combineID[i] == selectedItem.id && item.combineActionList[i] != null)
					{
						PlayerMenus.ForceOffAllMenus(true);
						selectedItem = null;
						AdvGame.RunActionListAsset(item.combineActionList[i]);
						return;
					}
				}
				if (settingsManager.reverseInventoryCombinations || settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot)
				{
					for (int j = 0; j < selectedItem.combineID.Count; j++)
					{
						if (selectedItem.combineID[j] == item.id && selectedItem.combineActionList[j] != null)
						{
							ActionListAsset actionListAsset = selectedItem.combineActionList[j];
							PlayerMenus.ForceOffAllMenus(true);
							selectedItem = null;
							AdvGame.RunActionListAsset(actionListAsset);
							return;
						}
					}
				}
				if ((bool)selectedItem.unhandledCombineActionList)
				{
					ActionListAsset unhandledCombineActionList = selectedItem.unhandledCombineActionList;
					selectedItem = null;
					AdvGame.RunActionListAsset(unhandledCombineActionList);
				}
				else if ((bool)unhandledCombine)
				{
					selectedItem = null;
					PlayerMenus.ForceOffAllMenus(true);
					AdvGame.RunActionListAsset(unhandledCombine);
				}
				else
				{
					selectedItem = null;
				}
			}
			GameObject.FindWithTag("GameEngine").GetComponent<PlayerCursor>().ResetSelectedCursor();
		}

		public List<InvItem> GetSelected()
		{
			List<InvItem> list = new List<InvItem>();
			if (selectedItem != null)
			{
				list.Add(selectedItem);
			}
			return list;
		}

		public bool IsItemCarried(InvItem _item)
		{
			if (_item == null)
			{
				return false;
			}
			foreach (InvItem localItem in localItems)
			{
				if (localItem == _item)
				{
					return true;
				}
			}
			return false;
		}

		public void RemoveRecipes()
		{
			while (craftingItems.Count > 0)
			{
				for (int i = 0; i < craftingItems.Count; i++)
				{
					Add(craftingItems[i].id, craftingItems[i].count, false, -1);
					craftingItems.RemoveAt(i);
				}
			}
			PlayerMenus.ResetInventoryBoxes();
		}

		public void TransferCraftingToLocal(int _recipeSlot, bool selectAfter)
		{
			foreach (InvItem craftingItem in craftingItems)
			{
				if (craftingItem.recipeSlot == _recipeSlot)
				{
					Add(craftingItem.id, craftingItem.count, selectAfter, -1);
					SelectItemByID(craftingItem.id);
					craftingItems.Remove(craftingItem);
					break;
				}
			}
		}

		public void TransferLocalToContainer(Container _container, InvItem _item, int _index)
		{
			if (_item != null && localItems.Contains(_item))
			{
				localItems[localItems.IndexOf(_item)] = null;
				localItems = ReorderItems(localItems);
				localItems = RemoveEmptySlots(localItems);
				SetNull();
			}
		}

		public void TransferLocalToCrafting(InvItem _item, int _slot)
		{
			if (_item != null && localItems.Contains(_item))
			{
				_item.recipeSlot = _slot;
				craftingItems.Add(_item);
				localItems[localItems.IndexOf(_item)] = null;
				localItems = ReorderItems(localItems);
				localItems = RemoveEmptySlots(localItems);
				SetNull();
			}
		}

		public List<InvItem> MatchInteractions()
		{
			List<InvItem> list = new List<InvItem>();
			matchingInvInteractions = new List<int>();
			if (settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot && hoverItem != null)
			{
				list = MatchInteractionsFromItem(list, hoverItem);
			}
			else if (settingsManager.SelectInteractionMethod() != SelectInteractions.CyclingCursorAndClickingHotspot && selectedItem != null)
			{
				list = MatchInteractionsFromItem(list, selectedItem);
			}
			else if ((bool)playerInteraction.GetActiveHotspot())
			{
				List<Button> invButtons = playerInteraction.GetActiveHotspot().invButtons;
				foreach (Button item in invButtons)
				{
					foreach (InvItem localItem in localItems)
					{
						if (localItem != null && localItem.id == item.invID && !item.isDisabled)
						{
							matchingInvInteractions.Add(invButtons.IndexOf(item));
							list.Add(localItem);
							break;
						}
					}
				}
			}
			return list;
		}

		private List<InvItem> MatchInteractionsFromItem(List<InvItem> items, InvItem _item)
		{
			if (_item != null && _item.combineID != null && _item.combineID.Count > 0)
			{
				foreach (int item in _item.combineID)
				{
					foreach (InvItem localItem in localItems)
					{
						if (localItem != null && localItem.id == item)
						{
							matchingInvInteractions.Add(_item.combineID.IndexOf(item));
							items.Add(localItem);
							break;
						}
					}
				}
			}
			return items;
		}

		public Recipe CalculateRecipe(bool autoCreateMatch)
		{
			if (inventoryManager == null)
			{
				return null;
			}
			foreach (Recipe recipe in inventoryManager.recipes)
			{
				if (autoCreateMatch && !recipe.autoCreate)
				{
					break;
				}
				foreach (InvItem craftingItem2 in craftingItems)
				{
					bool flag = false;
					foreach (Ingredient ingredient in recipe.ingredients)
					{
						if (ingredient.itemID == craftingItem2.id)
						{
							flag = true;
						}
					}
					if (flag)
					{
					}
				}
				bool flag2 = true;
				while (flag2)
				{
					foreach (Ingredient ingredient2 in recipe.ingredients)
					{
						InvItem craftingItem = GetCraftingItem(ingredient2.itemID);
						if (craftingItem == null)
						{
							flag2 = false;
							break;
						}
						if ((recipe.useSpecificSlots && craftingItem.recipeSlot == ingredient2.slotNumber - 1) || !recipe.useSpecificSlots)
						{
							if ((craftingItem.canCarryMultiple && craftingItem.count >= ingredient2.amount) || !craftingItem.canCarryMultiple)
							{
								if (flag2 && recipe.ingredients.IndexOf(ingredient2) == recipe.ingredients.Count - 1)
								{
									return recipe;
								}
							}
							else
							{
								flag2 = false;
							}
						}
						else
						{
							flag2 = false;
						}
					}
				}
			}
			return null;
		}

		public void PerformCrafting(Recipe recipe, bool selectAfter)
		{
			foreach (Ingredient ingredient in recipe.ingredients)
			{
				for (int i = 0; i < craftingItems.Count; i++)
				{
					if (craftingItems[i].id != ingredient.itemID)
					{
						continue;
					}
					if (ingredient.amount > 1)
					{
						craftingItems[i].count -= ingredient.amount;
						if (craftingItems[i].count < 1)
						{
							craftingItems.RemoveAt(i);
						}
					}
					else
					{
						craftingItems.RemoveAt(i);
					}
				}
			}
			RemoveEmptyCraftingSlots();
			Add(recipe.resultID, 1, selectAfter, -1);
		}

		public void MoveItemToIndex(InvItem item, List<InvItem> items, int index)
		{
			if (item == null)
			{
				return;
			}
			if (settingsManager.canReorderItems)
			{
				int num = items.IndexOf(item);
				while (items.Count <= Mathf.Max(index, num))
				{
					items.Add(null);
				}
				if (items[index] == null)
				{
					items[index] = item;
					items[num] = null;
				}
				SetNull();
				items = RemoveEmptySlots(items);
			}
			else if (items.IndexOf(item) == index)
			{
				SetNull();
			}
		}

		public void SetFont(Font font, int size, Color color, TextEffects textEffects)
		{
			countStyle = new GUIStyle();
			countStyle.font = font;
			countStyle.fontSize = size;
			countStyle.normal.textColor = color;
			countStyle.alignment = TextAnchor.MiddleCenter;
			countTextEffects = textEffects;
		}

		public void DrawHighlighted(Rect _rect)
		{
			if (highlightItem == null || highlightItem.activeTex == null)
			{
				return;
			}
			if (highlightState == HighlightState.None)
			{
				GUI.DrawTexture(_rect, highlightItem.activeTex, ScaleMode.StretchToFill, true, 0f);
				return;
			}
			if (pulseDirection == 0)
			{
				pulse = 0f;
				pulseDirection = 1;
			}
			else if (pulseDirection == 1)
			{
				pulse += settingsManager.inventoryPulseSpeed * Time.deltaTime;
			}
			else if (pulseDirection == -1)
			{
				pulse -= settingsManager.inventoryPulseSpeed * Time.deltaTime;
			}
			if (pulse > 1f)
			{
				pulse = 1f;
				if (highlightState == HighlightState.Normal)
				{
					highlightState = HighlightState.None;
					GUI.DrawTexture(_rect, highlightItem.activeTex, ScaleMode.StretchToFill, true, 0f);
					return;
				}
				pulseDirection = -1;
			}
			else if (pulse < 0f)
			{
				pulse = 0f;
				if (highlightState != HighlightState.Pulse)
				{
					highlightState = HighlightState.None;
					GUI.DrawTexture(_rect, highlightItem.tex, ScaleMode.StretchToFill, true, 0f);
					highlightItem = null;
					return;
				}
				pulseDirection = 1;
			}
			Color color = GUI.color;
			Color color2 = GUI.color;
			color2.a = pulse;
			GUI.color = color2;
			GUI.DrawTexture(_rect, highlightItem.activeTex, ScaleMode.StretchToFill, true, 0f);
			GUI.color = color;
			GUI.DrawTexture(_rect, highlightItem.tex, ScaleMode.StretchToFill, true, 0f);
		}

		public void HighlightItemOnInstant(int _id)
		{
			highlightItem = GetItem(_id);
			highlightState = HighlightState.None;
			pulse = 1f;
		}

		public void HighlightItemOffInstant()
		{
			highlightItem = null;
			highlightState = HighlightState.None;
			pulse = 0f;
		}

		public void HighlightItem(int _id, HighlightType _type)
		{
			highlightItem = GetItem(_id);
			if (highlightItem != null)
			{
				switch (_type)
				{
				case HighlightType.Enable:
					highlightState = HighlightState.Normal;
					pulseDirection = 1;
					break;
				case HighlightType.Disable:
					highlightState = HighlightState.Normal;
					pulseDirection = -1;
					break;
				case HighlightType.PulseOnce:
					highlightState = HighlightState.Flash;
					pulse = 0f;
					pulseDirection = 1;
					break;
				case HighlightType.PulseContinually:
					highlightState = HighlightState.Pulse;
					pulse = 0f;
					pulseDirection = 1;
					break;
				}
			}
		}

		public void StopHighlighting()
		{
			highlightItem = null;
			highlightState = HighlightState.None;
		}

		public void DrawInventoryCount(Vector2 cursorPosition, float cursorSize, int count)
		{
			if (count > 1)
			{
				if (countTextEffects != TextEffects.None)
				{
					AdvGame.DrawTextEffect(AdvGame.GUIBox(cursorPosition, cursorSize), count.ToString(), countStyle, Color.black, countStyle.normal.textColor, 2f, countTextEffects);
				}
				else
				{
					GUI.Label(AdvGame.GUIBox(cursorPosition, cursorSize), count.ToString(), countStyle);
				}
			}
		}

		private void OnEnable()
		{
			inventoryManager = null;
		}

		private void OnDestroy()
		{
			inventoryManager = null;
			settingsManager = null;
		}
	}
}
