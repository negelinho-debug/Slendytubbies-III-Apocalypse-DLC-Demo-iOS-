using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionMenuCheck : ActionCheck
	{
		public enum MenuCheckType
		{
			MenuIsVisible,
			MenuIsLocked,
			ElementIsVisible
		}

		public MenuCheckType checkType;

		public string menuToCheck = string.Empty;

		public int menuToCheckParameterID = -1;

		public string elementToCheck = string.Empty;

		public int elementToCheckParameterID = -1;

		public ActionMenuCheck()
		{
			isDisplayed = true;
			title = "Menu: Check state";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			menuToCheck = AssignString(parameters, menuToCheckParameterID, menuToCheck);
			elementToCheck = AssignString(parameters, elementToCheckParameterID, elementToCheck);
		}

		public override bool CheckCondition()
		{
			Menu menuWithName = PlayerMenus.GetMenuWithName(menuToCheck);
			if (menuWithName != null)
			{
				if (checkType == MenuCheckType.MenuIsVisible)
				{
					return menuWithName.IsVisible();
				}
				if (checkType == MenuCheckType.MenuIsLocked)
				{
					return menuWithName.isLocked;
				}
				if (checkType == MenuCheckType.ElementIsVisible)
				{
					MenuElement elementWithName = PlayerMenus.GetElementWithName(menuToCheck, elementToCheck);
					if (elementWithName != null)
					{
						return elementWithName.isVisible;
					}
				}
			}
			return false;
		}
	}
}
