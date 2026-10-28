using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionScene : Action
	{
		public ChooseSceneBy chooseSceneBy;

		public int sceneNumber;

		public string sceneName;

		public bool assignScreenOverlay;

		public ActionScene()
		{
			isDisplayed = true;
			title = "Engine: Change scene";
			numSockets = 0;
		}

		public override float Run()
		{
			if (!assignScreenOverlay)
			{
				ChangeScene();
				return 0f;
			}
			if (!isRunning)
			{
				isRunning = true;
				KickStarter.mainCamera._ExitSceneWithOverlay();
				return defaultPauseTime;
			}
			ChangeScene();
			isRunning = false;
			return 0f;
		}

		private void ChangeScene()
		{
			if (sceneNumber > -1 || chooseSceneBy == ChooseSceneBy.Name)
			{
				SceneChanger component = GameObject.FindWithTag("PersistentEngine").GetComponent<SceneChanger>();
				component.ChangeScene(AdvGame.GetSceneName(chooseSceneBy, sceneName), sceneNumber, true);
			}
		}

		public override int End(List<Action> actions)
		{
			return -1;
		}
	}
}
