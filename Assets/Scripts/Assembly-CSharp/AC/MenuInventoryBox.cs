using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class MenuInventoryBox : MenuElement
	{
		public TextEffects textEffects;

		public AC_InventoryBoxType inventoryBoxType;

		public bool limitSlots;

		public int maxSlots;

		public bool limitToCategory;

		public int categoryID;

		public List<InvItem> items = new List<InvItem>();

		public bool selectItemsAfterTaking = true;

		public ConversationDisplayType displayType = ConversationDisplayType.IconOnly;

		private RuntimeInventory runtimeInventory;

		public override void Declare()
		{
			isVisible = true;
			isClickable = true;
			inventoryBoxType = AC_InventoryBoxType.Default;
			numSlots = 0;
			SetSize(new Vector2(6f, 10f));
			limitSlots = false;
			maxSlots = 10;
			limitToCategory = false;
			selectItemsAfterTaking = true;
			categoryID = -1;
			displayType = ConversationDisplayType.IconOnly;
			textEffects = TextEffects.None;
			items = new List<InvItem>();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuInventoryBox menuInventoryBox = ScriptableObject.CreateInstance<MenuInventoryBox>();
			menuInventoryBox.Declare();
			menuInventoryBox.CopyInventoryBox(this);
			return menuInventoryBox;
		}

		public void CopyInventoryBox(MenuInventoryBox _element)
		{
			isClickable = _element.isClickable;
			textEffects = _element.textEffects;
			inventoryBoxType = _element.inventoryBoxType;
			numSlots = _element.numSlots;
			limitSlots = _element.limitSlots;
			maxSlots = _element.maxSlots;
			limitToCategory = _element.limitToCategory;
			categoryID = _element.categoryID;
			selectItemsAfterTaking = _element.selectItemsAfterTaking;
			displayType = _element.displayType;
			PopulateList();
			base.Copy(_element);
		}

		public override void Display(GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			base.Display(_style, _slot, zoom, isActive);
			if (items.Count <= 0 || items.Count <= _slot + offset || items[_slot + offset] == null)
			{
				return;
			}
			if (displayType == ConversationDisplayType.IconOnly)
			{
				GUI.Label(GetSlotRectRelative(_slot), string.Empty, _style);
				DrawTexture(ZoomRect(GetSlotRectRelative(_slot), zoom), items[_slot + offset], isActive);
				_style.normal.background = null;
				if (textEffects != TextEffects.None)
				{
					AdvGame.DrawTextEffect(ZoomRect(GetSlotRectRelative(_slot), zoom), GetCount(_slot), _style, Color.black, _style.normal.textColor, 2f, textEffects);
				}
				else
				{
					GUI.Label(ZoomRect(GetSlotRectRelative(_slot), zoom), GetCount(_slot), _style);
				}
			}
			else if (displayType == ConversationDisplayType.TextOnly)
			{
				string text = items[_slot + offset].label;
				if (runtimeInventory != null)
				{
					text = runtimeInventory.GetLabel(items[_slot + offset]);
				}
				string count = GetCount(_slot);
				if (count != string.Empty)
				{
					text = text + " (" + count + ")";
				}
				if (textEffects != TextEffects.None)
				{
					AdvGame.DrawTextEffect(ZoomRect(GetSlotRectRelative(_slot), zoom), text, _style, Color.black, _style.normal.textColor, 2f, textEffects);
				}
				else
				{
					GUI.Label(ZoomRect(GetSlotRectRelative(_slot), zoom), text, _style);
				}
			}
		}

		public void HandleDefaultClick(MouseState _mouseState, int _slot, AC_InteractionMethod interactionMethod)
		{
			if (!(runtimeInventory != null))
			{
				return;
			}
			runtimeInventory.StopHighlighting();
			runtimeInventory.SetFont(font, GetFontSize(), fontColor, textEffects);
			if (items.Count <= _slot + offset || items[_slot + offset] == null)
			{
				runtimeInventory.MoveItemToIndex(runtimeInventory.selectedItem, runtimeInventory.localItems, _slot + offset);
				return;
			}
			switch (interactionMethod)
			{
			case AC_InteractionMethod.ChooseHotspotThenInteraction:
				runtimeInventory.ShowInteractions(items[_slot + offset]);
				return;
			case AC_InteractionMethod.ChooseInteractionThenHotspot:
			{
				if (_mouseState != MouseState.SingleClick)
				{
					return;
				}
				int selectedCursorID = GameObject.FindWithTag("GameEngine").GetComponent<PlayerCursor>().GetSelectedCursorID();
				switch (GameObject.FindWithTag("GameEngine").GetComponent<PlayerCursor>().GetSelectedCursor())
				{
				case -2:
					if (runtimeInventory.selectedItem != null && items[_slot + offset] == runtimeInventory.selectedItem)
					{
						runtimeInventory.SelectItem(items[_slot + offset]);
					}
					else
					{
						runtimeInventory.Combine(items[_slot + offset]);
					}
					break;
				case -1:
					runtimeInventory.SelectItem(items[_slot + offset]);
					break;
				default:
					if (selectedCursorID > -1)
					{
						runtimeInventory.RunInteraction(items[_slot + offset], selectedCursorID);
					}
					break;
				}
				return;
			}
			}
			switch (_mouseState)
			{
			case MouseState.SingleClick:
				if (runtimeInventory.selectedItem == null)
				{
					runtimeInventory.Use(items[_slot + offset]);
				}
				else
				{
					runtimeInventory.Combine(items[_slot + offset]);
				}
				break;
			case MouseState.RightClick:
				if (runtimeInventory.selectedItem == null)
				{
					runtimeInventory.Look(items[_slot + offset]);
				}
				else
				{
					runtimeInventory.SetNull();
				}
				break;
			}
		}

		public override void RecalculateSize()
		{
			PopulateList();
			if (inventoryBoxType == AC_InventoryBoxType.HostpotBased)
			{
				if (!Application.isPlaying)
				{
					if (numSlots < 0)
					{
						numSlots = 0;
					}
					if (numSlots > items.Count)
					{
						numSlots = items.Count;
					}
				}
				else
				{
					numSlots = items.Count;
				}
			}
			else
			{
				numSlots = items.Count;
				if (limitSlots)
				{
					numSlots = maxSlots;
				}
				LimitOffset(items.Count);
			}
			base.RecalculateSize();
		}

		private void PopulateList()
		{
			if (Application.isPlaying)
			{
				if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>())
				{
					runtimeInventory = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
					if (inventoryBoxType == AC_InventoryBoxType.HostpotBased)
					{
						items = runtimeInventory.MatchInteractions();
					}
					else if (inventoryBoxType == AC_InventoryBoxType.DisplaySelected)
					{
						items = runtimeInventory.GetSelected();
					}
					else if (inventoryBoxType == AC_InventoryBoxType.DisplayLastSelected)
					{
						if (runtimeInventory.selectedItem != null)
						{
							items = new List<InvItem>();
							items = runtimeInventory.GetSelected();
						}
						else if (items.Count == 1 && !runtimeInventory.IsItemCarried(items[0]))
						{
							items.Clear();
						}
					}
					else if (inventoryBoxType == AC_InventoryBoxType.Container)
					{
						PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
						if ((bool)component.activeContainer)
						{
							items.Clear();
							foreach (ContainerItem item in component.activeContainer.items)
							{
								InvItem invItem = new InvItem(AdvGame.GetReferences().inventoryManager.GetItem(item.linkedID));
								invItem.count = item.count;
								items.Add(invItem);
							}
						}
					}
					else
					{
						items = new List<InvItem>();
						foreach (InvItem localItem in runtimeInventory.localItems)
						{
							if (AdvGame.GetReferences().settingsManager.hideSelectedFromMenu && runtimeInventory.selectedItem == localItem)
							{
								items.Add(null);
							}
							else
							{
								items.Add(localItem);
							}
						}
					}
				}
			}
			else
			{
				items = new List<InvItem>();
				if ((bool)AdvGame.GetReferences().inventoryManager)
				{
					foreach (InvItem item2 in AdvGame.GetReferences().inventoryManager.items)
					{
						items.Add(item2);
						if (item2 != null)
						{
							item2.recipeSlot = -1;
						}
					}
				}
			}
			if (inventoryBoxType != AC_InventoryBoxType.Default && inventoryBoxType != AC_InventoryBoxType.CustomScript)
			{
				return;
			}
			if (limitToCategory && categoryID > -1)
			{
				while (AreAnyItemsInWrongCategory())
				{
					foreach (InvItem item3 in items)
					{
						if (item3 != null && item3.binID != categoryID)
						{
							items.Remove(item3);
							break;
						}
					}
				}
			}
			while (AreAnyItemsInRecipe())
			{
				foreach (InvItem item4 in items)
				{
					if (item4 != null && item4.recipeSlot > -1)
					{
						if (AdvGame.GetReferences().settingsManager.canReorderItems)
						{
							items[items.IndexOf(item4)] = null;
						}
						else
						{
							items.Remove(item4);
						}
						break;
					}
				}
			}
		}

		public override bool CanBeShifted(AC_ShiftInventory shiftType)
		{
			if (shiftType == AC_ShiftInventory.ShiftLeft)
			{
				if (offset == 0)
				{
					return false;
				}
			}
			else if (maxSlots + offset >= items.Count)
			{
				return false;
			}
			return true;
		}

		private bool AreAnyItemsInRecipe()
		{
			foreach (InvItem item in items)
			{
				if (item != null && item.recipeSlot >= 0)
				{
					return true;
				}
			}
			return false;
		}

		private bool AreAnyItemsInWrongCategory()
		{
			foreach (InvItem item in items)
			{
				if (item != null && item.binID != categoryID)
				{
					return true;
				}
			}
			return false;
		}

		public override void Shift(AC_ShiftInventory shiftType, int amount)
		{
			if (limitSlots)
			{
				if (numSlots >= maxSlots)
				{
					Shift(shiftType, maxSlots, items.Count, amount);
				}
			}
			else
			{
				Debug.Log("Cannot offset " + title + " as it does not limit the number of available slots.");
			}
		}

		private void DrawTexture(Rect rect, InvItem _item, bool isActive)
		{
			if (_item == null)
			{
				return;
			}
			Texture2D texture2D = null;
			if (Application.isPlaying && runtimeInventory != null && inventoryBoxType != AC_InventoryBoxType.DisplaySelected)
			{
				if (_item == runtimeInventory.highlightItem && _item.activeTex != null)
				{
					runtimeInventory.DrawHighlighted(rect);
					return;
				}
				if (_item.activeTex != null && ((isActive && AdvGame.GetReferences().settingsManager.activeWhenHover) || _item == runtimeInventory.selectedItem))
				{
					texture2D = _item.activeTex;
				}
				else if (_item.tex != null)
				{
					texture2D = _item.tex;
				}
			}
			else if (_item.tex != null)
			{
				texture2D = _item.tex;
			}
			if (texture2D != null)
			{
				GUI.DrawTexture(rect, texture2D, ScaleMode.StretchToFill, true, 0f);
			}
		}

		public override string GetLabel(int i)
		{
			if (items.Count <= i + offset || items[i + offset] == null)
			{
				return null;
			}
			return items[i + offset].GetLabel();
		}

		public InvItem GetItem(int i)
		{
			if (items.Count <= i + offset || items[i + offset] == null)
			{
				return null;
			}
			return items[i + offset];
		}

		private string GetCount(int i)
		{
			if (items.Count <= i + offset || items[i + offset] == null)
			{
				return string.Empty;
			}
			if (items[i + offset].count < 2)
			{
				return string.Empty;
			}
			return items[i + offset].count.ToString();
		}

		public void ResetOffset()
		{
			offset = 0;
		}

		protected override void AutoSize()
		{
			if (items.Count > 0)
			{
				foreach (InvItem item in items)
				{
					if (item != null)
					{
						if (displayType == ConversationDisplayType.IconOnly)
						{
							AutoSize(new GUIContent(item.tex));
						}
						else if (displayType == ConversationDisplayType.TextOnly)
						{
							AutoSize(new GUIContent(item.label));
						}
						return;
					}
				}
			}
			AutoSize(GUIContent.none);
		}

		public void ClickContainer(MouseState _mouseState, int _slot, Container container)
		{
			if (container == null || runtimeInventory == null)
			{
				return;
			}
			runtimeInventory.SetFont(font, GetFontSize(), fontColor, textEffects);
			switch (_mouseState)
			{
			case MouseState.SingleClick:
				if (runtimeInventory.selectedItem == null)
				{
					if (container.items[_slot + offset] != null)
					{
						ContainerItem containerItem = container.items[_slot + offset];
						runtimeInventory.Add(containerItem.linkedID, containerItem.count, selectItemsAfterTaking, -1);
						container.items.Remove(containerItem);
					}
				}
				else
				{
					container.InsertAt(runtimeInventory.selectedItem, _slot + offset);
					runtimeInventory.Remove(runtimeInventory.selectedItem);
				}
				break;
			case MouseState.RightClick:
				if (runtimeInventory.selectedItem != null)
				{
					runtimeInventory.SetNull();
				}
				break;
			}
		}
	}
}
