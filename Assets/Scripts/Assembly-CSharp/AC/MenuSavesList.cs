using UnityEngine;

namespace AC
{
	public class MenuSavesList : MenuElement
	{
		public enum SaveDisplayType
		{
			LabelOnly,
			ScreenshotOnly,
			LabelAndScreenshot
		}

		public string newSaveText = "New save";

		public TextEffects textEffects;

		public TextAnchor anchor;

		public AC_SaveListType saveListType;

		public int maxSaves = 5;

		public ActionListAsset actionListOnSave;

		public SaveDisplayType displayType;

		public Texture2D blankSlotTexture;

		public bool fixedOption;

		public int optionToShow;

		private bool newSaveSlot;

		public override void Declare()
		{
			newSaveText = "New save";
			isVisible = true;
			isClickable = true;
			numSlots = 1;
			maxSaves = 5;
			SetSize(new Vector2(20f, 5f));
			anchor = TextAnchor.MiddleCenter;
			saveListType = AC_SaveListType.Save;
			actionListOnSave = null;
			newSaveSlot = false;
			textEffects = TextEffects.None;
			displayType = SaveDisplayType.LabelOnly;
			blankSlotTexture = null;
			fixedOption = false;
			optionToShow = 1;
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuSavesList menuSavesList = ScriptableObject.CreateInstance<MenuSavesList>();
			menuSavesList.Declare();
			menuSavesList.CopySavesList(this);
			return menuSavesList;
		}

		public void CopySavesList(MenuSavesList _element)
		{
			newSaveText = _element.newSaveText;
			textEffects = _element.textEffects;
			anchor = _element.anchor;
			saveListType = _element.saveListType;
			maxSaves = _element.maxSaves;
			actionListOnSave = _element.actionListOnSave;
			displayType = _element.displayType;
			blankSlotTexture = _element.blankSlotTexture;
			fixedOption = _element.fixedOption;
			optionToShow = _element.optionToShow;
			base.Copy(_element);
		}

		public override string GetLabel(int slot)
		{
			return SaveSystem.GetSaveSlotLabel(slot, optionToShow, fixedOption);
		}

		public bool ProcessClick(int _slot)
		{
			if (saveListType == AC_SaveListType.Save)
			{
				if (newSaveSlot && _slot == numSlots - 1)
				{
					SaveSystem.SaveNewGame();
				}
				else
				{
					SaveSystem.SaveGame(_slot, optionToShow, fixedOption);
				}
				if ((bool)actionListOnSave)
				{
					AdvGame.RunActionListAsset(actionListOnSave);
				}
			}
			else if (saveListType == AC_SaveListType.Load)
			{
				if (fixedOption && newSaveSlot)
				{
					return false;
				}
				return SaveSystem.LoadGame(_slot, optionToShow, fixedOption);
			}
			return true;
		}

		public override void Display(GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			base.Display(_style, _slot, zoom, isActive);
			if (displayType != SaveDisplayType.LabelOnly)
			{
				Texture2D saveSlotScreenshot = SaveSystem.GetSaveSlotScreenshot(_slot, optionToShow, fixedOption);
				if (saveSlotScreenshot != null)
				{
					GUI.DrawTexture(ZoomRect(GetSlotRectRelative(_slot), zoom), saveSlotScreenshot, ScaleMode.StretchToFill, true, 0f);
				}
				else if (blankSlotTexture != null)
				{
					GUI.DrawTexture(ZoomRect(GetSlotRectRelative(_slot), zoom), blankSlotTexture, ScaleMode.StretchToFill, true, 0f);
				}
			}
			if (displayType != SaveDisplayType.ScreenshotOnly)
			{
				_style.alignment = anchor;
				if (zoom < 1f)
				{
					_style.fontSize = (int)((float)_style.fontSize * zoom);
				}
				string empty = string.Empty;
				if (newSaveSlot)
				{
					if (!fixedOption && _slot == numSlots - 1)
					{
						empty = TranslateLabel(newSaveText);
					}
					else
					{
						empty = ((!fixedOption || saveListType != AC_SaveListType.Save) ? SaveSystem.GetSaveSlotLabel(_slot, optionToShow, fixedOption) : TranslateLabel(newSaveText));
					}
				}
				else
				{
					empty = SaveSystem.GetSaveSlotLabel(_slot, optionToShow, fixedOption);
				}
				if (textEffects != TextEffects.None)
				{
					AdvGame.DrawTextEffect(ZoomRect(GetSlotRectRelative(_slot), zoom), empty, _style, Color.black, _style.normal.textColor, 2f, textEffects);
				}
				else
				{
					GUI.Label(ZoomRect(GetSlotRectRelative(_slot), zoom), empty, _style);
				}
			}
		}

		public override void RecalculateSize()
		{
			newSaveSlot = false;
			if (Application.isPlaying)
			{
				if (fixedOption)
				{
					numSlots = 1;
					if (!SaveSystem.DoesSaveExist(optionToShow))
					{
						newSaveSlot = true;
					}
				}
				else
				{
					numSlots = SaveSystem.GetNumSlots();
					if (saveListType == AC_SaveListType.Save && numSlots < maxSaves)
					{
						newSaveSlot = true;
						numSlots++;
					}
				}
			}
			base.RecalculateSize();
		}

		protected override void AutoSize()
		{
			if (displayType == SaveDisplayType.ScreenshotOnly)
			{
				if (blankSlotTexture != null)
				{
					AutoSize(new GUIContent(blankSlotTexture));
				}
				else
				{
					AutoSize(GUIContent.none);
				}
			}
			else if (displayType == SaveDisplayType.LabelAndScreenshot)
			{
				if (blankSlotTexture != null)
				{
					AutoSize(new GUIContent(blankSlotTexture));
				}
				else
				{
					AutoSize(new GUIContent(SaveSystem.GetSaveSlotLabel(0, optionToShow, fixedOption)));
				}
			}
			else
			{
				AutoSize(new GUIContent(SaveSystem.GetSaveSlotLabel(0, optionToShow, fixedOption)));
			}
		}
	}
}
