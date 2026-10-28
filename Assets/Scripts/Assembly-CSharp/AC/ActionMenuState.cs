using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionMenuState : Action
	{
		public enum MenuChangeType
		{
			TurnOnMenu,
			TurnOffMenu,
			HideMenuElement,
			ShowMenuElement,
			LockMenu,
			UnlockMenu,
			AddJournalPage
		}

		public MenuChangeType changeType;

		public string menuToChange = string.Empty;

		public int menuToChangeParameterID = -1;

		public string elementToChange = string.Empty;

		public int elementToChangeParameterID = -1;

		public string journalText = string.Empty;

		public bool doFade;

		public int lineID = -1;

		public ActionMenuState()
		{
			isDisplayed = true;
			lineID = -1;
			title = "Menu: Change state";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			menuToChange = AssignString(parameters, menuToChangeParameterID, menuToChange);
			elementToChange = AssignString(parameters, elementToChangeParameterID, elementToChange);
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				Menu menuWithName = PlayerMenus.GetMenuWithName(menuToChange);
				if (menuWithName != null)
				{
					if (changeType == MenuChangeType.TurnOnMenu)
					{
						if (menuWithName.appearType == AppearType.Manual || menuWithName.appearType == AppearType.OnInputKey)
						{
							if (!menuWithName.TurnOn(doFade))
							{
								isRunning = false;
								return 0f;
							}
							if (doFade && willWait)
							{
								return defaultPauseTime;
							}
						}
						else
						{
							Debug.LogWarning("Can only turn on menus with an Appear Type of Manual and OnInputKey.");
						}
					}
					else if (changeType == MenuChangeType.TurnOffMenu)
					{
						if (menuWithName.appearType == AppearType.Manual || menuWithName.appearType == AppearType.OnInputKey || menuWithName.appearType == AppearType.OnContainer)
						{
							if (!menuWithName.TurnOff(doFade))
							{
								isRunning = false;
								return 0f;
							}
							if (doFade && willWait)
							{
								return defaultPauseTime;
							}
						}
						else
						{
							Debug.LogWarning("Can only turn off menus with an Appear Type of Manual, OnContainer, and OnInputKey.");
						}
					}
					else if (changeType == MenuChangeType.HideMenuElement || changeType == MenuChangeType.ShowMenuElement)
					{
						MenuElement elementWithName = PlayerMenus.GetElementWithName(menuToChange, elementToChange);
						if (elementWithName != null)
						{
							if (changeType == MenuChangeType.HideMenuElement)
							{
								elementWithName.isVisible = false;
							}
							else
							{
								elementWithName.isVisible = true;
							}
							menuWithName.ResetVisibleElements();
							menuWithName.Recalculate();
						}
						else
						{
							Debug.LogWarning("Could not find menu element of name '" + elementToChange + "'");
						}
					}
					else if (changeType == MenuChangeType.LockMenu)
					{
						if (doFade)
						{
							menuWithName.TurnOff(true);
						}
						else
						{
							menuWithName.ForceOff();
						}
						menuWithName.isLocked = true;
						if (doFade && willWait)
						{
							return defaultPauseTime;
						}
					}
					else if (changeType == MenuChangeType.UnlockMenu)
					{
						menuWithName.isLocked = false;
					}
					else if (changeType == MenuChangeType.AddJournalPage)
					{
						MenuElement elementWithName2 = PlayerMenus.GetElementWithName(menuToChange, elementToChange);
						if (elementWithName2 != null)
						{
							if (journalText != string.Empty)
							{
								if (elementWithName2 is MenuJournal)
								{
									MenuJournal menuJournal = (MenuJournal)elementWithName2;
									JournalPage item = new JournalPage(lineID, journalText);
									menuJournal.pages.Add(item);
								}
								else
								{
									Debug.LogWarning(elementWithName2.title + " is not a journal!");
								}
							}
							else
							{
								Debug.LogWarning("No journal text to add!");
							}
						}
						else
						{
							Debug.LogWarning("Could not find menu element of name '" + elementToChange + "' inside '" + menuToChange + "'");
						}
					}
				}
				else if (menuToChange != string.Empty)
				{
					Debug.LogWarning("Could not find menu of name '" + menuToChange + "'");
				}
			}
			else
			{
				Menu menuWithName2 = PlayerMenus.GetMenuWithName(menuToChange);
				if (menuWithName2 != null)
				{
					if (menuWithName2.IsFading())
					{
						return defaultPauseTime;
					}
					isRunning = false;
					return 0f;
				}
			}
			return 0f;
		}

		public override void Skip()
		{
			Menu menuWithName = PlayerMenus.GetMenuWithName(menuToChange);
			if (!(menuWithName != null))
			{
				return;
			}
			if (changeType == MenuChangeType.TurnOnMenu)
			{
				if (menuWithName.appearType == AppearType.Manual || menuWithName.appearType == AppearType.OnInputKey)
				{
					menuWithName.TurnOn(false);
				}
			}
			else if (changeType == MenuChangeType.TurnOffMenu)
			{
				if (menuWithName.appearType == AppearType.Manual || menuWithName.appearType == AppearType.OnInputKey || menuWithName.appearType == AppearType.OnContainer)
				{
					menuWithName.ForceOff();
				}
			}
			else if (changeType == MenuChangeType.HideMenuElement || changeType == MenuChangeType.ShowMenuElement)
			{
				MenuElement elementWithName = PlayerMenus.GetElementWithName(menuToChange, elementToChange);
				if (elementWithName != null)
				{
					if (changeType == MenuChangeType.HideMenuElement)
					{
						elementWithName.isVisible = false;
					}
					else
					{
						elementWithName.isVisible = true;
					}
					menuWithName.ResetVisibleElements();
					menuWithName.Recalculate();
				}
			}
			else if (changeType == MenuChangeType.LockMenu)
			{
				menuWithName.isLocked = true;
				menuWithName.ForceOff();
			}
			else if (changeType == MenuChangeType.UnlockMenu)
			{
				menuWithName.isLocked = false;
			}
			else if (changeType == MenuChangeType.AddJournalPage)
			{
				MenuElement elementWithName2 = PlayerMenus.GetElementWithName(menuToChange, elementToChange);
				if (elementWithName2 != null && journalText != string.Empty && elementWithName2 is MenuJournal)
				{
					MenuJournal menuJournal = (MenuJournal)elementWithName2;
					JournalPage item = new JournalPage(lineID, journalText);
					menuJournal.pages.Add(item);
				}
			}
		}
	}
}
