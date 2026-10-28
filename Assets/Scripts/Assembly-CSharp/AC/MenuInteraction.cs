using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class MenuInteraction : MenuElement
	{
		public enum AC_DisplayType
		{
			IconOnly,
			TextOnly,
			IconAndText
		}

		public AC_DisplayType displayType;

		public TextAnchor anchor;

		public TextEffects textEffects;

		public int iconID;

		private CursorIcon icon;

		private string label = string.Empty;

		private CursorManager cursorManager;

		public override void Declare()
		{
			isVisible = true;
			isClickable = true;
			numSlots = 1;
			anchor = TextAnchor.MiddleCenter;
			SetSize(new Vector2(5f, 5f));
			iconID = -1;
			textEffects = TextEffects.None;
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuInteraction menuInteraction = ScriptableObject.CreateInstance<MenuInteraction>();
			menuInteraction.Declare();
			menuInteraction.CopyInteraction(this);
			return menuInteraction;
		}

		public void CopyInteraction(MenuInteraction _element)
		{
			displayType = _element.displayType;
			anchor = _element.anchor;
			textEffects = _element.textEffects;
			iconID = _element.iconID;
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
			if (displayType != AC_DisplayType.IconOnly)
			{
				if (textEffects != TextEffects.None)
				{
					AdvGame.DrawTextEffect(ZoomRect(relativeRect, zoom), label, _style, Color.black, _style.normal.textColor, 2f, textEffects);
				}
				else
				{
					GUI.Label(ZoomRect(relativeRect, zoom), label, _style);
				}
			}
			else
			{
				GUI.Label(ZoomRect(relativeRect, zoom), string.Empty, _style);
			}
			if (displayType != AC_DisplayType.TextOnly && icon != null)
			{
				icon.DrawAsInteraction(ZoomRect(relativeRect, zoom), isActive);
			}
		}

		public void MatchInteractions(InvItem item)
		{
			bool flag = false;
			foreach (InvInteraction interaction in item.interactions)
			{
				if (interaction.icon.id == iconID)
				{
					flag = true;
					break;
				}
			}
			isVisible = flag;
		}

		public void MatchInteractions(List<Button> buttons)
		{
			bool flag = false;
			foreach (Button button in buttons)
			{
				if (button.iconID == iconID && !button.isDisabled)
				{
					flag = true;
					break;
				}
			}
			isVisible = flag;
		}

		public void MatchUseInteraction(Button button)
		{
			if (button.iconID == iconID)
			{
				isVisible = true;
			}
		}

		public void MatchLookInteraction(int lookIconID)
		{
			if (lookIconID == iconID)
			{
				isVisible = true;
			}
		}

		public override void RecalculateSize()
		{
			if ((bool)AdvGame.GetReferences().cursorManager)
			{
				CursorManager cursorManager = AdvGame.GetReferences().cursorManager;
				CursorIcon cursorIconFromID = cursorManager.GetCursorIconFromID(iconID);
				if (cursorIconFromID != null)
				{
					icon = cursorIconFromID;
					label = cursorIconFromID.label;
					icon.Reset();
				}
			}
			base.RecalculateSize();
		}

		protected override void AutoSize()
		{
			if (displayType == AC_DisplayType.IconOnly && icon != null && icon.texture != null)
			{
				GUIContent content = new GUIContent(icon.texture);
				AutoSize(content);
			}
			else
			{
				GUIContent content2 = new GUIContent(label);
				AutoSize(content2);
			}
		}
	}
}
