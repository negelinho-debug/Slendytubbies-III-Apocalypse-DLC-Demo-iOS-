using UnityEngine;

namespace AC
{
	public class MenuLabel : MenuElement
	{
		public string label = "Element";

		public TextAnchor anchor;

		public TextEffects textEffects;

		public AC_LabelType labelType;

		public int variableID;

		public int variableNumber;

		public bool useCharacterColour;

		public bool autoAdjustHeight = true;

		private string newLabel = string.Empty;

		private Dialog dialog;

		private PlayerMenus playerMenus;

		public override void Declare()
		{
			label = "Label";
			isVisible = true;
			isClickable = false;
			numSlots = 1;
			anchor = TextAnchor.MiddleCenter;
			SetSize(new Vector2(10f, 5f));
			labelType = AC_LabelType.Normal;
			variableID = 0;
			variableNumber = 0;
			useCharacterColour = false;
			autoAdjustHeight = true;
			textEffects = TextEffects.None;
			newLabel = string.Empty;
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuLabel menuLabel = ScriptableObject.CreateInstance<MenuLabel>();
			menuLabel.Declare();
			menuLabel.CopyLabel(this);
			return menuLabel;
		}

		public void CopyLabel(MenuLabel _element)
		{
			label = _element.label;
			anchor = _element.anchor;
			textEffects = _element.textEffects;
			labelType = _element.labelType;
			variableID = _element.variableID;
			variableNumber = _element.variableNumber;
			useCharacterColour = _element.useCharacterColour;
			autoAdjustHeight = _element.autoAdjustHeight;
			newLabel = string.Empty;
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
			if (Application.isPlaying)
			{
				if (labelType == AC_LabelType.Hotspot)
				{
					if (playerMenus.GetHotspotLabel() != string.Empty)
					{
						newLabel = playerMenus.GetHotspotLabel();
					}
				}
				else if (labelType == AC_LabelType.Normal)
				{
					newLabel = TranslateLabel(label);
				}
				else if (labelType == AC_LabelType.GlobalVariable)
				{
					newLabel = RuntimeVariables.GetVariable(variableID).GetValue();
				}
				else if (labelType == AC_LabelType.DialogueLine)
				{
					if (useCharacterColour)
					{
						_style.normal.textColor = dialog.GetColour();
						if (dialog.GetColour() == Color.white)
						{
							_style.normal.textColor = Color.grey;
						}
					}
					string line = dialog.GetLine();
					newLabel = line;
					if (sizeType == AC_SizeType.Manual && autoAdjustHeight)
					{
						GUIContent content = new GUIContent(newLabel);
						relativeRect.height = _style.CalcHeight(content, relativeRect.width) + 8f;
					}
				}
				else if (labelType == AC_LabelType.DialogueSpeaker)
				{
					_style.normal.textColor = dialog.GetColour();
					if (dialog.GetColour() == Color.white)
					{
						_style.normal.textColor = Color.grey;
					}
					newLabel = dialog.GetSpeaker();
				}
				else if (labelType == AC_LabelType.DialoguePortrait)
				{
					if (dialog.GetPortrait() == null)
					{
						newLabel = dialog.GetSpeaker();
					}
					else
					{
						newLabel = string.Empty;
						if (dialog.IsAnimating())
						{
							GUI.DrawTextureWithTexCoords(ZoomRect(relativeRect, zoom), dialog.GetPortrait(), dialog.GetAnimatedRect());
						}
						else
						{
							GUI.DrawTexture(ZoomRect(relativeRect, zoom), dialog.GetPortrait(), ScaleMode.StretchToFill, true, 0f);
						}
					}
				}
			}
			else
			{
				newLabel = label;
			}
			newLabel = AdvGame.ConvertTokens(newLabel);
			string text = string.Empty;
			if (labelType == AC_LabelType.DialogueLine)
			{
				text = dialog.GetSpeaker() + "\n" + newLabel;
			}
			if (textEffects != TextEffects.None)
			{
				AdvGame.DrawTextEffect(ZoomRect(relativeRect, zoom), text, _style, Color.black, _style.normal.textColor, 2f, textEffects);
				AdvGame.DrawTextEffect(ZoomRect(relativeRect, zoom), newLabel, _style, Color.black, Color.white, 2f, textEffects);
			}
			else
			{
				GUI.Label(ZoomRect(relativeRect, zoom), newLabel, _style);
			}
		}

		public override string GetLabel(int slot)
		{
			if (labelType == AC_LabelType.Normal)
			{
				return TranslateLabel(label);
			}
			if (labelType == AC_LabelType.DialogueSpeaker)
			{
				if (dialog == null)
				{
					dialog = GameObject.FindWithTag("GameEngine").GetComponent<Dialog>();
				}
				return dialog.GetSpeaker();
			}
			if (labelType == AC_LabelType.DialoguePortrait)
			{
				if (dialog == null)
				{
					dialog = GameObject.FindWithTag("GameEngine").GetComponent<Dialog>();
				}
				return dialog.GetSpeaker();
			}
			if (labelType == AC_LabelType.GlobalVariable)
			{
				return RuntimeVariables.GetVariable(variableID).GetValue();
			}
			if (labelType == AC_LabelType.Hotspot)
			{
				if (playerMenus == null)
				{
					playerMenus = GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>();
				}
				return playerMenus.GetHotspotLabel();
			}
			return string.Empty;
		}

		public override void RecalculateSize()
		{
			if (Application.isPlaying)
			{
				if ((bool)GameObject.FindWithTag("GameEngine"))
				{
					dialog = GameObject.FindWithTag("GameEngine").GetComponent<Dialog>();
				}
				if ((bool)GameObject.FindWithTag("PersistentEngine"))
				{
					playerMenus = GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>();
				}
			}
			base.RecalculateSize();
		}

		protected override void AutoSize()
		{
			if (labelType == AC_LabelType.DialogueLine)
			{
				GUIContent gUIContent = new GUIContent(TranslateLabel(label));
				GUIStyle gUIStyle = new GUIStyle();
				gUIStyle.font = font;
				gUIStyle.fontSize = (int)(AdvGame.GetMainGameViewSize().x * fontScaleFactor / 100f);
				Dialog component = GameObject.FindWithTag("GameEngine").GetComponent<Dialog>();
				string text = " " + component.GetLine() + " ";
				if (text.Length > 40)
				{
					text = text.Insert(text.Length / 2, " \n ");
				}
				gUIContent = new GUIContent(text);
				AutoSize(gUIContent);
			}
			else if (label == string.Empty && backgroundTexture != null)
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
	}
}
