using UnityEngine;

namespace AC
{
	public class MenuToggle : MenuElement
	{
		public string label;

		public bool isOn;

		public TextEffects textEffects;

		public TextAnchor anchor;

		public int varID;

		public AC_ToggleType toggleType;

		public bool appendState = true;

		public Texture2D onTexture;

		public Texture2D offTexture;

		public override void Declare()
		{
			label = "Toggle";
			isOn = false;
			isVisible = true;
			isClickable = true;
			toggleType = AC_ToggleType.CustomScript;
			numSlots = 1;
			varID = 0;
			SetSize(new Vector2(15f, 5f));
			anchor = TextAnchor.MiddleLeft;
			appendState = true;
			onTexture = null;
			offTexture = null;
			textEffects = TextEffects.None;
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuToggle menuToggle = ScriptableObject.CreateInstance<MenuToggle>();
			menuToggle.Declare();
			menuToggle.CopyToggle(this);
			return menuToggle;
		}

		public void CopyToggle(MenuToggle _element)
		{
			label = _element.label;
			isOn = _element.isOn;
			textEffects = _element.textEffects;
			anchor = _element.anchor;
			toggleType = _element.toggleType;
			varID = _element.varID;
			appendState = _element.appendState;
			onTexture = _element.onTexture;
			offTexture = _element.offTexture;
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
			Rect rect = ZoomRect(relativeRect, zoom);
			if (isOn && onTexture != null)
			{
				GUI.DrawTexture(rect, onTexture, ScaleMode.StretchToFill, true, 0f);
			}
			else if (!isOn && offTexture != null)
			{
				GUI.DrawTexture(rect, offTexture, ScaleMode.StretchToFill, true, 0f);
			}
			string text = TranslateLabel(label);
			if (appendState)
			{
				text = ((!isOn) ? (text + " : Off") : (text + " : On"));
			}
			if (textEffects != TextEffects.None)
			{
				AdvGame.DrawTextEffect(rect, text, _style, Color.black, _style.normal.textColor, 2f, textEffects);
			}
			else
			{
				GUI.Label(rect, text, _style);
			}
		}

		public override string GetLabel(int slot)
		{
			if (isOn)
			{
				return TranslateLabel(label) + " : On";
			}
			return TranslateLabel(label) + " : Off";
		}

		public void Toggle()
		{
			if (isOn)
			{
				isOn = false;
			}
			else
			{
				isOn = true;
			}
			if (toggleType == AC_ToggleType.Subtitles)
			{
				if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<Options>())
				{
					Options component = GameObject.FindWithTag("PersistentEngine").GetComponent<Options>();
					component.optionsData.showSubtitles = isOn;
					component.SavePrefs();
				}
			}
			else
			{
				if (toggleType != AC_ToggleType.Variable || varID < 0)
				{
					return;
				}
				GVar variable = RuntimeVariables.GetVariable(varID);
				if (variable.type == VariableType.Boolean)
				{
					if (isOn)
					{
						variable.val = 1;
					}
					else
					{
						variable.val = 0;
					}
					variable.Upload();
				}
			}
		}

		public override void RecalculateSize()
		{
			if (Application.isPlaying)
			{
				if (toggleType == AC_ToggleType.Subtitles)
				{
					if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<Options>() && GameObject.FindWithTag("PersistentEngine").GetComponent<Options>().optionsData != null)
					{
						isOn = GameObject.FindWithTag("PersistentEngine").GetComponent<Options>().optionsData.showSubtitles;
					}
				}
				else if (toggleType == AC_ToggleType.Variable && varID >= 0)
				{
					if (RuntimeVariables.GetVariable(varID).type != VariableType.Boolean)
					{
						Debug.LogWarning("Cannot link MenuToggle " + title + " to Variable " + varID + " as it is not a Boolean.");
					}
					else
					{
						isOn = RuntimeVariables.GetBooleanValue(varID);
					}
				}
			}
			base.RecalculateSize();
		}

		protected override void AutoSize()
		{
			if (appendState)
			{
				AutoSize(new GUIContent(TranslateLabel(label) + " : Off"));
			}
			else
			{
				AutoSize(new GUIContent(TranslateLabel(label)));
			}
		}
	}
}
