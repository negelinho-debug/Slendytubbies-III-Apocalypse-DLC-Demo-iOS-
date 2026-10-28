using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class MenuCrafting : MenuElement
	{
		public TextEffects textEffects;

		public CraftingElementType craftingType;

		public int categoryID;

		public List<InvItem> items = new List<InvItem>();

		private Recipe activeRecipe;

		public override void Declare()
		{
			isVisible = true;
			isClickable = true;
			numSlots = 4;
			SetSize(new Vector2(6f, 10f));
			textEffects = TextEffects.None;
			craftingType = CraftingElementType.Ingredients;
			items = new List<InvItem>();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuCrafting menuCrafting = ScriptableObject.CreateInstance<MenuCrafting>();
			menuCrafting.Declare();
			menuCrafting.CopyCrafting(this);
			return menuCrafting;
		}

		public void CopyCrafting(MenuCrafting _element)
		{
			isClickable = _element.isClickable;
			textEffects = _element.textEffects;
			numSlots = _element.numSlots;
			craftingType = _element.craftingType;
			PopulateList();
			base.Copy(_element);
		}

		public override void Display(GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			base.Display(_style, _slot, zoom, isActive);
			if (craftingType == CraftingElementType.Ingredients)
			{
				bool flag = false;
				foreach (InvItem item in items)
				{
					if (item.recipeSlot == _slot)
					{
						flag = true;
						break;
					}
				}
				GUI.Label(GetSlotRectRelative(_slot), string.Empty, _style);
				if (flag)
				{
					DrawTexture(ZoomRect(GetSlotRectRelative(_slot), zoom), _slot);
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
			}
			else
			{
				if (craftingType != CraftingElementType.Output)
				{
					return;
				}
				GUI.Label(GetSlotRectRelative(_slot), string.Empty, _style);
				if (items.Count > 0)
				{
					DrawTexture(ZoomRect(GetSlotRectRelative(_slot), zoom), _slot);
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
			}
		}

		public void HandleDefaultClick(MouseState _mouseState, int _slot)
		{
			if (!GameObject.FindWithTag("PersistentEngine") || !GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>())
			{
				return;
			}
			RuntimeInventory component = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
			if (craftingType != CraftingElementType.Ingredients)
			{
				return;
			}
			switch (_mouseState)
			{
			case MouseState.SingleClick:
				if (component.selectedItem == null)
				{
					if (GetItem(_slot) != null)
					{
						component.TransferCraftingToLocal(GetItem(_slot).recipeSlot, true);
					}
					break;
				}
				if (GetItem(_slot) != null)
				{
					component.TransferCraftingToLocal(GetItem(_slot).recipeSlot, false);
				}
				component.TransferLocalToCrafting(component.selectedItem, _slot);
				break;
			case MouseState.RightClick:
				if (component.selectedItem != null)
				{
					component.SetNull();
				}
				break;
			}
			PlayerMenus.ResetInventoryBoxes();
		}

		public void ClickOutput(Menu _menu, MouseState _mouseState)
		{
			if (!GameObject.FindWithTag("PersistentEngine") || !GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>())
			{
				return;
			}
			RuntimeInventory component = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
			if (items.Count <= 0)
			{
				return;
			}
			if (_mouseState == MouseState.SingleClick && component.selectedItem == null)
			{
				if (activeRecipe.onCreateRecipe == OnCreateRecipe.SelectItem)
				{
					component.PerformCrafting(activeRecipe, true);
				}
				else if (activeRecipe.onCreateRecipe == OnCreateRecipe.RunActionList)
				{
					component.PerformCrafting(activeRecipe, false);
					if (activeRecipe.invActionList != null && (bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<RuntimeActionList>())
					{
						AdvGame.RunActionListAsset(activeRecipe.invActionList);
					}
				}
				else
				{
					component.PerformCrafting(activeRecipe, false);
				}
			}
			PlayerMenus.ResetInventoryBoxes();
		}

		public override void RecalculateSize()
		{
			PopulateList();
			base.RecalculateSize();
		}

		private void PopulateList()
		{
			if (Application.isPlaying)
			{
				if (!GameObject.FindWithTag("PersistentEngine") || !GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>())
				{
					return;
				}
				RuntimeInventory component = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
				if (craftingType == CraftingElementType.Ingredients)
				{
					items = new List<InvItem>();
					{
						foreach (InvItem craftingItem in component.craftingItems)
						{
							items.Add(craftingItem);
						}
						return;
					}
				}
				if (craftingType == CraftingElementType.Output)
				{
					SetOutput(true, component);
				}
			}
			else
			{
				items = new List<InvItem>();
			}
		}

		public void SetOutput(bool autoCreate, RuntimeInventory runtimeInventory)
		{
			items = new List<InvItem>();
			activeRecipe = runtimeInventory.CalculateRecipe(autoCreate);
			if (activeRecipe != null)
			{
				foreach (InvItem item in AdvGame.GetReferences().inventoryManager.items)
				{
					if (item.id == activeRecipe.resultID)
					{
						InvItem invItem = new InvItem(item);
						invItem.count = 1;
						items.Add(invItem);
					}
				}
			}
			if (!autoCreate)
			{
				base.RecalculateSize();
			}
		}

		private bool AreAnyItemsInWrongCategory()
		{
			foreach (InvItem item in items)
			{
				if (item.binID != categoryID)
				{
					return true;
				}
			}
			return false;
		}

		private void DrawTexture(Rect rect, int i)
		{
			Texture2D texture2D = null;
			if (Application.isPlaying)
			{
				texture2D = GetItem(i).tex;
			}
			else if (items[i].tex != null)
			{
				texture2D = items[i].tex;
			}
			if (texture2D != null)
			{
				GUI.DrawTexture(rect, texture2D, ScaleMode.StretchToFill, true, 0f);
			}
		}

		public override string GetLabel(int i)
		{
			if (GetItem(i).altLabel != string.Empty)
			{
				return GetItem(i).altLabel;
			}
			return GetItem(i).label;
		}

		public InvItem GetItem(int i)
		{
			if (craftingType == CraftingElementType.Output)
			{
				if (items.Count > i)
				{
					return items[i];
				}
			}
			else if (craftingType == CraftingElementType.Ingredients)
			{
				foreach (InvItem item in items)
				{
					if (item.recipeSlot == i)
					{
						return item;
					}
				}
			}
			return null;
		}

		private string GetCount(int i)
		{
			if (GetItem(i).count < 2)
			{
				return string.Empty;
			}
			return GetItem(i).count.ToString();
		}

		protected override void AutoSize()
		{
			if (items.Count > 0)
			{
				AutoSize(new GUIContent(items[0].tex));
			}
			else
			{
				AutoSize(GUIContent.none);
			}
		}
	}
}
