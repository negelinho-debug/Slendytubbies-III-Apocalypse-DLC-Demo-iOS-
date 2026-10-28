using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionSceneCheck : ActionCheck
	{
		public enum IntCondition
		{
			EqualTo,
			NotEqualTo
		}

		public enum SceneToCheck
		{
			Current,
			Previous
		}

		public ChooseSceneBy chooseSceneBy;

		public SceneToCheck sceneToCheck;

		public int sceneNumber;

		public string sceneName;

		public IntCondition intCondition;

		public ActionSceneCheck()
		{
			isDisplayed = true;
			title = "Engine: Check scene";
		}

		public override bool CheckCondition()
		{
			int num = 0;
			string empty = string.Empty;
			if (sceneToCheck == SceneToCheck.Previous)
			{
				SceneChanger component = GameObject.FindWithTag("PersistentEngine").GetComponent<SceneChanger>();
				num = component.previousScene;
				empty = component.previousSceneName;
			}
			else
			{
				num = Application.loadedLevel;
				empty = Application.loadedLevelName;
			}
			if (intCondition == IntCondition.EqualTo)
			{
				if (chooseSceneBy == ChooseSceneBy.Name && empty == sceneName)
				{
					return true;
				}
				if (chooseSceneBy == ChooseSceneBy.Number && num == sceneNumber)
				{
					return true;
				}
			}
			else if (intCondition == IntCondition.NotEqualTo)
			{
				if (chooseSceneBy == ChooseSceneBy.Name && empty != sceneName)
				{
					return true;
				}
				if (chooseSceneBy == ChooseSceneBy.Number && num != sceneNumber)
				{
					return true;
				}
			}
			return false;
		}
	}
}
