using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class MenuCycle : MenuElement
	{
		public string label = "Element";

		public TextEffects textEffects;

		public TextAnchor anchor;

		public int selected;

		public List<string> optionsArray = new List<string>();

		public AC_CycleType cycleType;

		public int varID;

		public override void Declare()
		{
			label = "Cycle";
			selected = 0;
			isVisible = true;
			isClickable = true;
			numSlots = 1;
			textEffects = TextEffects.None;
			SetSize(new Vector2(15f, 5f));
			anchor = TextAnchor.MiddleLeft;
			cycleType = AC_CycleType.CustomScript;
			varID = 0;
			optionsArray = new List<string>();
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuCycle menuCycle = ScriptableObject.CreateInstance<MenuCycle>();
			menuCycle.Declare();
			menuCycle.CopyCycle(this);
			return menuCycle;
		}

		public void CopyCycle(MenuCycle _element)
		{
			label = _element.label;
			textEffects = _element.textEffects;
			anchor = _element.anchor;
			selected = _element.selected;
			optionsArray = _element.optionsArray;
			cycleType = _element.cycleType;
			varID = _element.varID;
			base.Copy(_element);
		}

		public override void Display(GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			base.Display(_style, _slot, zoom, isActive);
			_style.alignment = anchor;
			if (zoom < 1f)
			{
				_style.fontSize = (int)((float)_style.fontSize * zoom);
			}
			string text = TranslateLabel(label) + " : ";
			if (!Application.isPlaying)
			{
				if (optionsArray.Count <= 0)
				{
					text += "Default option";
				}
				else
				{
					text = ((selected < 0 || selected >= optionsArray.Count) ? (text + optionsArray[0]) : (text + optionsArray[selected]));
				}
			}
			else if (optionsArray.Count > selected && selected > -1)
			{
				text += optionsArray[selected];
			}
			else
			{
				Debug.Log("Could not gather options options for MenuCycle " + label);
				selected = 0;
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
			if (optionsArray.Count > selected && selected > -1)
			{
				return TranslateLabel(label) + " : " + optionsArray[selected];
			}
			return TranslateLabel(label);
		}

		public void Cycle()
		{
			selected++;
			if (selected > optionsArray.Count - 1)
			{
				selected = 0;
			}
			if (cycleType == AC_CycleType.Language)
			{
				if (selected == 0 && (bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().speechManager && AdvGame.GetReferences().speechManager.ignoreOriginalText && AdvGame.GetReferences().speechManager.languages.Count > 1)
				{
					selected = 1;
				}
				Options.SetLanguage(selected);
			}
			else if (cycleType == AC_CycleType.Variable && varID >= 0)
			{
				GVar variable = RuntimeVariables.GetVariable(varID);
				if (variable.type == VariableType.Integer)
				{
					variable.val = selected;
					variable.Upload();
				}
			}
		}

		public override void RecalculateSize()
		{
			if (Application.isPlaying)
			{
				if (cycleType == AC_CycleType.Language)
				{
					if ((bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().speechManager && (bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<Options>())
					{
						SpeechManager speechManager = AdvGame.GetReferences().speechManager;
						Options component = GameObject.FindWithTag("PersistentEngine").GetComponent<Options>();
						optionsArray = speechManager.languages;
						if (component.optionsData != null)
						{
							selected = component.optionsData.language;
						}
					}
				}
				else if (cycleType == AC_CycleType.Variable && varID >= 0)
				{
					if (RuntimeVariables.GetVariable(varID) == null || RuntimeVariables.GetVariable(varID).type != VariableType.Integer)
					{
						Debug.LogWarning("Cannot link MenuToggle " + title + " to Variable " + varID + " as it is not an Integer.");
					}
					else if (optionsArray.Count > 0)
					{
						selected = Mathf.Clamp(RuntimeVariables.GetIntegerValue(varID), 0, optionsArray.Count - 1);
					}
					else
					{
						selected = 0;
					}
				}
			}
			base.RecalculateSize();
		}

		protected override void AutoSize()
		{
			AutoSize(new GUIContent(TranslateLabel(label) + " : Default option"));
		}
	}
}
