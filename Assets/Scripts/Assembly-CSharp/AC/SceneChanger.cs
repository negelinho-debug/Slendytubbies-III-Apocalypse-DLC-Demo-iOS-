using UnityEngine;

namespace AC
{
	public class SceneChanger : MonoBehaviour
	{
		public int previousScene = -1;

		public string previousSceneName = string.Empty;

		private SettingsManager settingsManager;

		private void Awake()
		{
			settingsManager = AdvGame.GetReferences().settingsManager;
		}

		public void ChangeScene(string sceneName, int sceneNumber, bool saveRoomData)
		{
			bool useLoadingScreen = false;
			if (settingsManager != null && settingsManager.useLoadingScreen)
			{
				useLoadingScreen = true;
			}
			KickStarter.mainCamera.FadeOut(0f);
			if ((bool)KickStarter.player)
			{
				KickStarter.player.Halt();
			}
			Sound[] array = Object.FindObjectsOfType(typeof(Sound)) as Sound[];
			Sound[] array2 = array;
			foreach (Sound sound in array2)
			{
				if (sound.canDestroy)
				{
					Object.Destroy(sound);
				}
			}
			Dialog component = GameObject.FindWithTag("GameEngine").GetComponent<Dialog>();
			component.KillDialog(true);
			LevelStorage component2 = GetComponent<LevelStorage>();
			if (saveRoomData)
			{
				component2.StoreCurrentLevelData();
				previousScene = Application.loadedLevel;
				previousSceneName = Application.loadedLevelName;
			}
			StateHandler component3 = GetComponent<StateHandler>();
			component3.gameState = GameState.Normal;
			LoadLevel(sceneName, sceneNumber, useLoadingScreen);
		}

		private void LoadLevel(string sceneName, int sceneNumber)
		{
			if (settingsManager != null && settingsManager.useLoadingScreen)
			{
				LoadLevel(sceneName, sceneNumber, true);
			}
			else
			{
				LoadLevel(sceneName, sceneNumber, false);
			}
		}

		private void LoadLevel(string sceneName, int sceneNumber, bool useLoadingScreen)
		{
			if (useLoadingScreen)
			{
				GameObject gameObject = new GameObject("LevelManager");
				LoadingScreen loadingScreen = gameObject.AddComponent<LoadingScreen>();
				loadingScreen.StartCoroutine(loadingScreen.InnerLoad(sceneName, sceneNumber, AdvGame.GetSceneName(settingsManager.loadingSceneIs, settingsManager.loadingSceneName), settingsManager.loadingScene));
			}
			else if (sceneName != string.Empty)
			{
				Application.LoadLevel(sceneName);
			}
			else
			{
				Application.LoadLevel(sceneNumber);
			}
		}
	}
}
