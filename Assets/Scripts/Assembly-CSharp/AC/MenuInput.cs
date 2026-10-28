using UnityEngine;

namespace AC
{
	public class MenuInput : MenuElement
	{
		public string label = "Element";

		public TextAnchor anchor;

		public TextEffects textEffects;

		public AC_InputType inputType;

		public int characterLimit = 10;

		public string linkedButton = string.Empty;

		public bool allowSpaces;

		private bool isSelected;

		public override void Declare()
		{
			label = "Input";
			isVisible = true;
			isClickable = true;
			numSlots = 1;
			anchor = TextAnchor.MiddleCenter;
			SetSize(new Vector2(10f, 5f));
			inputType = AC_InputType.AlphaNumeric;
			characterLimit = 10;
			linkedButton = string.Empty;
			textEffects = TextEffects.None;
			allowSpaces = false;
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuInput menuInput = ScriptableObject.CreateInstance<MenuInput>();
			menuInput.Declare();
			menuInput.CopyInput(this);
			return menuInput;
		}

		public void CopyInput(MenuInput _element)
		{
			label = _element.label;
			anchor = _element.anchor;
			textEffects = _element.textEffects;
			inputType = _element.inputType;
			characterLimit = _element.characterLimit;
			linkedButton = _element.linkedButton;
			allowSpaces = _element.allowSpaces;
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
			string text = label;
			if (isSelected || isActive)
			{
				text = ((!(Options.GetLanguageName() == "Arabic") && !(Options.GetLanguageName() == "Hebrew")) ? (text + "|") : ("|" + text));
			}
			if (textEffects != TextEffects.None)
			{
				AdvGame.DrawTextEffect(ZoomRect(relativeRect, zoom), text, _style, Color.black, _style.normal.textColor, 2f, textEffects);
			}
			else
			{
				GUI.Label(ZoomRect(relativeRect, zoom), text, _style);
			}
		}

		public override string GetLabel(int slot)
		{
			return TranslateLabel(label);
		}

		public void CheckForInput(string input, bool shift, string menuName)
		{
			bool flag = false;
			if (Options.GetLanguageName() == "Arabic" || Options.GetLanguageName() == "Hebrew")
			{
				flag = true;
			}
			isSelected = true;
			switch (input)
			{
			case "Backspace":
				if (label.Length > 1)
				{
					if (flag)
					{
						label = label.Substring(1, label.Length - 1);
					}
					else
					{
						label = label.Substring(0, label.Length - 1);
					}
				}
				else if (label.Length == 1)
				{
					label = string.Empty;
				}
				return;
			case "KeypadEnter":
			case "Return":
			case "Enter":
				if (linkedButton != string.Empty && menuName != string.Empty)
				{
					PlayerMenus.SimulateClick(menuName, PlayerMenus.GetElementWithName(menuName, linkedButton), 1);
				}
				return;
			}
			if ((inputType != AC_InputType.AlphaNumeric || (input.Length != 1 && !input.Contains("Alpha"))) && (inputType != AC_InputType.NumbericOnly || !input.Contains("Alpha")) && (inputType != AC_InputType.AlphaNumeric || !allowSpaces || !(input == "Space")))
			{
				return;
			}
			input = input.Replace("Alpha", string.Empty);
			input = input.Replace("Space", " ");
			input = ((!shift) ? input.ToLower() : input.ToUpper());
			if (characterLimit == 1)
			{
				label = input;
			}
			else if (label.Length < characterLimit)
			{
				if (flag)
				{
					label = input + label;
				}
				else
				{
					label += input;
				}
			}
		}

		public override void RecalculateSize()
		{
			Deselect();
			base.RecalculateSize();
		}

		public void Deselect()
		{
			isSelected = false;
		}

		protected override void AutoSize()
		{
			GUIContent content = new GUIContent(TranslateLabel(label));
			AutoSize(content);
		}
	}
}
