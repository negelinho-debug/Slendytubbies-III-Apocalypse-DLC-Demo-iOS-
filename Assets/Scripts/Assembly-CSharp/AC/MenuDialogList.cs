using UnityEngine;

namespace AC
{
	public class MenuDialogList : MenuElement
	{
		public TextEffects textEffects;

		public ConversationDisplayType displayType;

		public Texture2D testIcon;

		public TextAnchor anchor;

		public bool fixedOption;

		public int optionToShow;

		public int maxSlots = 10;

		private int numOptions;

		private string[] labels;

		private Texture2D[] icons;

		public override void Declare()
		{
			isVisible = true;
			isClickable = true;
			fixedOption = false;
			displayType = ConversationDisplayType.TextOnly;
			testIcon = null;
			optionToShow = 1;
			numSlots = 0;
			SetSize(new Vector2(20f, 5f));
			maxSlots = 10;
			anchor = TextAnchor.MiddleLeft;
			textEffects = TextEffects.None;
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuDialogList menuDialogList = ScriptableObject.CreateInstance<MenuDialogList>();
			menuDialogList.Declare();
			menuDialogList.CopyDialogList(this);
			return menuDialogList;
		}

		public void CopyDialogList(MenuDialogList _element)
		{
			textEffects = _element.textEffects;
			displayType = _element.displayType;
			testIcon = _element.testIcon;
			anchor = _element.anchor;
			labels = _element.labels;
			fixedOption = _element.fixedOption;
			optionToShow = _element.optionToShow;
			maxSlots = _element.maxSlots;
			base.Copy(_element);
		}

		public override void Display(GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			base.Display(_style, _slot, zoom, isActive);
			_style.wordWrap = true;
			_style.alignment = anchor;
			if (zoom < 1f)
			{
				_style.fontSize = (int)((float)_style.fontSize * zoom);
			}
			string text = string.Empty;
			if (fixedOption)
			{
				_slot = 0;
				if (!Application.isPlaying)
				{
					text = "Dialogue option " + optionToShow;
				}
			}
			else if (!Application.isPlaying)
			{
				text = "Dialogue option " + _slot;
			}
			if (Application.isPlaying)
			{
				text = labels[_slot];
			}
			if (displayType == ConversationDisplayType.TextOnly)
			{
				if (textEffects != TextEffects.None)
				{
					AdvGame.DrawTextEffect(ZoomRect(GetSlotRectRelative(_slot), zoom), text, _style, Color.black, _style.normal.textColor, 2f, textEffects);
				}
				else
				{
					GUI.Label(ZoomRect(GetSlotRectRelative(_slot), zoom), text, _style);
				}
				return;
			}
			if (Application.isPlaying && icons[_slot] != null)
			{
				GUI.DrawTexture(ZoomRect(GetSlotRectRelative(_slot), zoom), icons[_slot], ScaleMode.StretchToFill, true, 0f);
			}
			else if (testIcon != null)
			{
				GUI.DrawTexture(ZoomRect(GetSlotRectRelative(_slot), zoom), testIcon, ScaleMode.StretchToFill, true, 0f);
			}
			GUI.Label(ZoomRect(GetSlotRectRelative(_slot), zoom), string.Empty, _style);
		}

		public override void RecalculateSize()
		{
			if (Application.isPlaying && (bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>())
			{
				PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
				if ((bool)component && (bool)component.activeConversation)
				{
					numOptions = component.activeConversation.GetCount();
					if (fixedOption)
					{
						if (numOptions < optionToShow)
						{
							numSlots = 0;
						}
						else
						{
							numSlots = 1;
							labels = new string[numSlots];
							labels[0] = component.activeConversation.GetOptionName(optionToShow - 1);
							icons = new Texture2D[numSlots];
							icons[0] = component.activeConversation.GetOptionIcon(optionToShow - 1);
						}
					}
					else
					{
						numSlots = numOptions;
						labels = new string[numSlots];
						icons = new Texture2D[numSlots];
						for (int i = 0; i < numSlots; i++)
						{
							labels[i] = component.activeConversation.GetOptionName(i + offset);
							icons[i] = component.activeConversation.GetOptionIcon(i + offset);
						}
						if (numSlots > maxSlots)
						{
							numSlots = maxSlots;
						}
						LimitOffset(numOptions);
					}
				}
				else
				{
					numSlots = 0;
				}
			}
			else if (fixedOption)
			{
				numSlots = 1;
				offset = 0;
				labels = new string[numSlots];
				icons = new Texture2D[numSlots];
			}
			base.RecalculateSize();
		}

		public override void Shift(AC_ShiftInventory shiftType, int amount)
		{
			if (isVisible && numSlots >= maxSlots)
			{
				Shift(shiftType, maxSlots, numOptions, amount);
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
			else if (maxSlots + offset >= numOptions)
			{
				return false;
			}
			return true;
		}

		public override string GetLabel(int slot)
		{
			if (labels.Length > slot)
			{
				return labels[slot];
			}
			return string.Empty;
		}

		public void RunOption(int slot)
		{
			PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
			if ((bool)component && (bool)component.activeConversation)
			{
				if (fixedOption)
				{
					component.activeConversation.RunOption(optionToShow - 1);
				}
				else
				{
					component.activeConversation.RunOption(slot + offset);
				}
			}
		}

		protected override void AutoSize()
		{
			if (displayType == ConversationDisplayType.IconOnly)
			{
				AutoSize(new GUIContent(testIcon));
			}
			else
			{
				AutoSize(new GUIContent("Dialogue option 0"));
			}
		}
	}
}
