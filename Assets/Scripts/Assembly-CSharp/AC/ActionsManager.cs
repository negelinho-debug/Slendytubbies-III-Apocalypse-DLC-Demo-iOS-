using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionsManager : ScriptableObject
	{
		public int defaultClass;

		public List<ActionType> AllActions = new List<ActionType>();

		public List<ActionType> EnabledActions = new List<ActionType>();

		private string[] categories = new string[15]
		{
			"Camera", "Character", "Container", "Dialogue", "Engine", "Hotspot", "Input", "Inventory", "Menu", "Moveable",
			"Object", "Player", "Third-Party", "Variable", "Custom"
		};

		public string GetDefaultAction()
		{
			if (EnabledActions.Count > 0 && EnabledActions.Count > defaultClass)
			{
				return EnabledActions[defaultClass].fileName;
			}
			return string.Empty;
		}

		public void SetEnabled()
		{
			EnabledActions.Clear();
			foreach (ActionType allAction in AllActions)
			{
				if (allAction.isEnabled)
				{
					EnabledActions.Add(allAction);
				}
			}
		}

		private string[] GetDefaultPopUp()
		{
			List<string> list = new List<string>();
			foreach (ActionType enabledAction in EnabledActions)
			{
				list.Add(enabledAction.title);
			}
			return list.ToArray();
		}

		public string GetActionName(int i)
		{
			return EnabledActions[i].fileName;
		}

		public bool DoesActionExist(string _name)
		{
			foreach (ActionType enabledAction in EnabledActions)
			{
				if (_name == enabledAction.fileName || _name == "AC." + enabledAction.fileName)
				{
					return true;
				}
			}
			return false;
		}

		public int GetActionsSize()
		{
			return EnabledActions.Count;
		}

		public string[] GetActionTitles()
		{
			List<string> list = new List<string>();
			foreach (ActionType enabledAction in EnabledActions)
			{
				list.Add(enabledAction.title);
			}
			return list.ToArray();
		}

		public string[] GetActionCategories()
		{
			return categories;
		}

		public string[] GetActionSubCategories(int categoryNumber)
		{
			List<string> list = new List<string>();
			string text = categories[categoryNumber];
			bool flag = false;
			foreach (ActionType enabledAction in EnabledActions)
			{
				if (enabledAction.title.Contains(text + ": "))
				{
					flag = true;
					string item = enabledAction.title.Replace(text + ": ", string.Empty);
					list.Add(item);
				}
			}
			if (flag)
			{
				return list.ToArray();
			}
			foreach (ActionType enabledAction2 in EnabledActions)
			{
				flag = false;
				string[] array = categories;
				foreach (string text2 in array)
				{
					if (enabledAction2.title.Contains(text2 + ": "))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					string item2 = enabledAction2.title.Replace(text + ": ", string.Empty);
					list.Add(item2);
				}
			}
			return list.ToArray();
		}

		public int GetActionCategory(int number)
		{
			if (EnabledActions.Count == 0)
			{
				return 0;
			}
			int length = EnabledActions[number].title.IndexOf(":");
			string text = EnabledActions[number].title.Substring(0, length);
			for (int i = 0; i < categories.Length; i++)
			{
				if (categories[i] == text)
				{
					return i;
				}
			}
			for (int j = 0; j < categories.Length; j++)
			{
				if (categories[j] == "Custom")
				{
					return j;
				}
			}
			return 0;
		}

		public int GetActionSubCategory(string title, int categoryNumber)
		{
			int num = 0;
			string text = categories[categoryNumber];
			foreach (ActionType enabledAction in EnabledActions)
			{
				if (enabledAction.title.Contains(text + ": "))
				{
					if (enabledAction.title == title)
					{
						return num;
					}
					num++;
				}
			}
			foreach (ActionType enabledAction2 in EnabledActions)
			{
				bool flag = false;
				string[] array = categories;
				foreach (string text2 in array)
				{
					if (enabledAction2.title.Contains(text2 + ": "))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					if (enabledAction2.title == title)
					{
						return num;
					}
					num++;
				}
			}
			return 0;
		}

		public int GetTypeNumber(int categoryNumber, int subCategoryNumber)
		{
			if (GetActionSubCategories(categoryNumber).Length == 0)
			{
				Debug.LogWarning("No Actions found in selected category");
				return defaultClass;
			}
			if (GetActionSubCategories(categoryNumber).Length <= subCategoryNumber)
			{
				subCategoryNumber = 0;
			}
			string value = GetActionSubCategories(categoryNumber)[subCategoryNumber];
			foreach (ActionType enabledAction in EnabledActions)
			{
				if (enabledAction.title.Contains(categories[categoryNumber] + ": ") && enabledAction.title.Contains(value))
				{
					return EnabledActions.IndexOf(enabledAction);
				}
			}
			foreach (ActionType enabledAction2 in EnabledActions)
			{
				if (enabledAction2.title.Contains(value))
				{
					return EnabledActions.IndexOf(enabledAction2);
				}
			}
			return 0;
		}
	}
}
