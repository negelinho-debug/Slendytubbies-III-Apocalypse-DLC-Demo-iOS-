using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class MenuButton : MenuElement
	{
		public string label = "Element";

		public string hotspotLabel = string.Empty;

		public int hotspotLabelID = -1;

		public TextAnchor anchor;

		public TextEffects textEffects;

		public AC_ButtonClickType buttonClickType;

		public SimulateInputType simulateInput;

		public float simulateValue = 1f;

		public bool doFade;

		public string switchMenuTitle;

		public string inventoryBoxTitle;

		public AC_ShiftInventory shiftInventory;

		public int shiftAmount = 1;

		public bool loopJournal;

		public ActionListAsset actionList;

		public string inputAxis;

		public Texture2D clickTexture;

		public bool onlyShowWhenEffective;

		public bool allowContinuousClick;

		private MenuElement elementToShift;

		private float clickAlpha;

		public override void Declare()
		{
			label = "Button";
			hotspotLabel = string.Empty;
			hotspotLabelID = -1;
			isVisible = true;
			isClickable = true;
			textEffects = TextEffects.None;
			buttonClickType = AC_ButtonClickType.RunActionList;
			simulateInput = SimulateInputType.Button;
			simulateValue = 1f;
			numSlots = 1;
			anchor = TextAnchor.MiddleCenter;
			SetSize(new Vector2(10f, 5f));
			doFade = false;
			switchMenuTitle = string.Empty;
			inventoryBoxTitle = string.Empty;
			shiftInventory = AC_ShiftInventory.ShiftLeft;
			loopJournal = false;
			actionList = null;
			inputAxis = string.Empty;
			clickTexture = null;
			clickAlpha = 0f;
			shiftAmount = 1;
			onlyShowWhenEffective = false;
			allowContinuousClick = false;
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuButton menuButton = ScriptableObject.CreateInstance<MenuButton>();
			menuButton.Declare();
			menuButton.CopyButton(this);
			return menuButton;
		}

		public void CopyButton(MenuButton _element)
		{
			label = _element.label;
			hotspotLabel = _element.hotspotLabel;
			hotspotLabelID = _element.hotspotLabelID;
			anchor = _element.anchor;
			textEffects = _element.textEffects;
			buttonClickType = _element.buttonClickType;
			simulateInput = _element.simulateInput;
			simulateValue = _element.simulateValue;
			doFade = _element.doFade;
			switchMenuTitle = _element.switchMenuTitle;
			inventoryBoxTitle = _element.inventoryBoxTitle;
			shiftInventory = _element.shiftInventory;
			loopJournal = _element.loopJournal;
			actionList = _element.actionList;
			inputAxis = _element.inputAxis;
			clickTexture = _element.clickTexture;
			clickAlpha = _element.clickAlpha;
			shiftAmount = _element.shiftAmount;
			onlyShowWhenEffective = _element.onlyShowWhenEffective;
			allowContinuousClick = _element.allowContinuousClick;
			base.Copy(_element);
		}

		public void ShowClick()
		{
			if (isClickable)
			{
				clickAlpha = 1f;
			}
		}

		public override void Display(GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			if (buttonClickType == AC_ButtonClickType.OffsetInventoryOrDialogue && onlyShowWhenEffective && inventoryBoxTitle != string.Empty && Application.isPlaying)
			{
				if (elementToShift == null)
				{
					foreach (Menu menu in PlayerMenus.GetMenus())
					{
						if (!(menu != null) || !menu.elements.Contains(this))
						{
							continue;
						}
						foreach (MenuElement element in menu.elements)
						{
							if (element != null && element.title == inventoryBoxTitle)
							{
								elementToShift = element;
								break;
							}
						}
					}
				}
				if (elementToShift != null && !elementToShift.CanBeShifted(shiftInventory))
				{
					return;
				}
			}
			if (clickAlpha > 0f)
			{
				if ((bool)clickTexture)
				{
					Color color = GUI.color;
					color.a = clickAlpha;
					GUI.color = color;
					GUI.DrawTexture(ZoomRect(GetSlotRectRelative(_slot), zoom), clickTexture, ScaleMode.StretchToFill, true, 0f);
					color.a = 1f;
					GUI.color = color;
				}
				clickAlpha -= Time.deltaTime;
				if (clickAlpha < 0f)
				{
					clickAlpha = 0f;
				}
			}
			base.Display(_style, _slot, zoom, isActive);
			_style.alignment = anchor;
			if (zoom < 1f)
			{
				_style.fontSize = (int)((float)_style.fontSize * zoom);
			}
			if (textEffects != TextEffects.None)
			{
				AdvGame.DrawTextEffect(ZoomRect(relativeRect, zoom), TranslateLabel(label), _style, Color.black, _style.normal.textColor, 2f, textEffects);
			}
			else
			{
				GUI.Label(ZoomRect(relativeRect, zoom), TranslateLabel(label), _style);
			}
		}

		public override string GetLabel(int slot)
		{
			return TranslateLabel(label);
		}

		protected override void AutoSize()
		{
			if (label == string.Empty && backgroundTexture != null)
			{
				GUIContent content = new GUIContent(backgroundTexture);
				AutoSize(content);
			}
			else
			{
				GUIContent content2 = new GUIContent(TranslateLabel(label));
				AutoSize(content2);
			}
		}

		public override void RecalculateSize()
		{
			clickAlpha = 0f;
			base.RecalculateSize();
		}

		public string GetHotspotLabel()
		{
			if (Options.GetLanguage() > 0 && hotspotLabelID > -1)
			{
				return SpeechManager.GetTranslation(hotspotLabelID, Options.GetLanguage());
			}
			return hotspotLabel;
		}
	}
}
