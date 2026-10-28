using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class MenuJournal : MenuElement
	{
		public List<JournalPage> pages = new List<JournalPage>();

		public int numPages = 1;

		public int showPage = 1;

		public TextAnchor anchor;

		public TextEffects textEffects;

		public override void Declare()
		{
			pages = new List<JournalPage>();
			pages.Add(new JournalPage());
			numPages = 1;
			showPage = 1;
			isVisible = true;
			isClickable = false;
			numSlots = 1;
			anchor = TextAnchor.MiddleCenter;
			SetSize(new Vector2(10f, 5f));
			textEffects = TextEffects.None;
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuJournal menuJournal = ScriptableObject.CreateInstance<MenuJournal>();
			menuJournal.Declare();
			menuJournal.CopyJournal(this);
			return menuJournal;
		}

		public void CopyJournal(MenuJournal _element)
		{
			pages = new List<JournalPage>();
			foreach (JournalPage page in _element.pages)
			{
				pages.Add(page);
			}
			numPages = _element.numPages;
			showPage = 1;
			anchor = _element.anchor;
			textEffects = _element.textEffects;
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
			if (pages.Count >= showPage)
			{
				string text = TranslatePage(pages[showPage - 1]);
				if (textEffects != TextEffects.None)
				{
					AdvGame.DrawTextEffect(ZoomRect(relativeRect, zoom), text, _style, Color.black, _style.normal.textColor, 2f, textEffects);
				}
				else
				{
					GUI.Label(ZoomRect(relativeRect, zoom), text, _style);
				}
			}
		}

		public override string GetLabel(int slot)
		{
			return TranslatePage(pages[showPage - 1]);
		}

		public void Shift(AC_ShiftInventory shiftType, bool doLoop)
		{
			switch (shiftType)
			{
			case AC_ShiftInventory.ShiftRight:
				if (pages.Count > showPage)
				{
					showPage++;
				}
				else if (doLoop)
				{
					showPage = 1;
				}
				break;
			case AC_ShiftInventory.ShiftLeft:
				if (showPage > 1)
				{
					showPage--;
				}
				else if (doLoop)
				{
					showPage = pages.Count;
				}
				break;
			}
		}

		private string TranslatePage(JournalPage page)
		{
			if (Options.GetLanguage() > 0 && page.lineID > -1)
			{
				return SpeechManager.GetTranslation(page.lineID, Options.GetLanguage());
			}
			return page.text;
		}

		protected override void AutoSize()
		{
			if (showPage > 0 && pages.Count >= showPage - 1)
			{
				if (pages[showPage - 1].text == string.Empty && backgroundTexture != null)
				{
					GUIContent content = new GUIContent(backgroundTexture);
					AutoSize(content);
				}
				else
				{
					GUIContent content2 = new GUIContent(pages[showPage - 1].text);
					AutoSize(content2);
				}
			}
		}
	}
}
