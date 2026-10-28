using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionEndGame : Action
	{
		public enum AC_EndGameType
		{
			QuitGame,
			LoadAutosave,
			ResetScene,
			RestartGame
		}

		public AC_EndGameType endGameType;

		public ChooseSceneBy chooseSceneBy;

		public int sceneNumber;

		public string sceneName;

		public ActionEndGame()
		{
			isDisplayed = true;
			title = "Engine: End game";
			numSockets = 0;
		}

		public override float Run()
		{
			if (endGameType == AC_EndGameType.QuitGame)
			{
				Application.Quit();
			}
			else if (endGameType == AC_EndGameType.LoadAutosave)
			{
				SaveSystem.LoadAutoSave();
			}
			else
			{
				LevelStorage component = GameObject.FindWithTag("PersistentEngine").GetComponent<LevelStorage>();
				SceneChanger component2 = component.GetComponent<SceneChanger>();
				component.GetComponent<RuntimeInventory>().SetNull();
				component.GetComponent<RuntimeInventory>().RemoveRecipes();
				UnityEngine.Object.DestroyImmediate(GameObject.FindWithTag("Player"));
				if (endGameType == AC_EndGameType.RestartGame)
				{
					component.GetComponent<SaveSystem>().ClearAllData();
					component.ClearAllLevelData();
					component.GetComponent<RuntimeInventory>().Awake();
					component.GetComponent<RuntimeVariables>().Awake();
					GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>().playedGlobalOnStart = false;
					component2.ChangeScene(AdvGame.GetSceneName(chooseSceneBy, sceneName), sceneNumber, false);
				}
				else if (endGameType == AC_EndGameType.ResetScene)
				{
					component.ClearCurrentLevelData();
					component2.ChangeScene(string.Empty, Application.loadedLevel, false);
				}
			}
			return 0f;
		}

		public override int End(List<Action> actions)
		{
			return -1;
		}
	}
}
