using UnityEngine;

namespace AC
{
	public class MenuTimer : MenuElement
	{
		public Texture2D timerTexture;

		public override void Declare()
		{
			isVisible = true;
			isClickable = false;
			numSlots = 1;
			SetSize(new Vector2(20f, 5f));
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuTimer menuTimer = ScriptableObject.CreateInstance<MenuTimer>();
			menuTimer.Declare();
			menuTimer.CopyTimer(this);
			return menuTimer;
		}

		public void CopyTimer(MenuTimer _element)
		{
			timerTexture = _element.timerTexture;
			base.Copy(_element);
		}

		public override void Display(GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			if ((bool)timerTexture)
			{
				if (Application.isPlaying)
				{
					if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>())
					{
						PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
						if ((bool)component.activeConversation && component.activeConversation.isTimed)
						{
							Rect rect = relativeRect;
							rect.width = slotSize.x / 100f * AdvGame.GetMainGameViewSize().x * component.activeConversation.GetTimeRemaining();
							GUI.DrawTexture(ZoomRect(rect, zoom), timerTexture, ScaleMode.StretchToFill, true, 0f);
						}
					}
				}
				else
				{
					GUI.DrawTexture(ZoomRect(relativeRect, zoom), timerTexture, ScaleMode.StretchToFill, true, 0f);
				}
			}
			base.Display(_style, _slot, zoom, isActive);
		}
	}
}
