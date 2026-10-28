using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class SceneSettings : MonoBehaviour
	{
		public Cutscene cutsceneOnStart;

		public Cutscene cutsceneOnLoad;

		public Cutscene cutsceneOnVarChange;

		public PlayerStart defaultPlayerStart;

		public AC_NavigationMethod navigationMethod = AC_NavigationMethod.meshCollider;

		public NavigationMesh navMesh;

		public SortingMap sortingMap;

		public Sound defaultSound;

		private void Awake()
		{
			NavigationMesh[] array = Object.FindObjectsOfType(typeof(NavigationMesh)) as NavigationMesh[];
			NavigationMesh[] array2 = array;
			foreach (NavigationMesh navigationMesh in array2)
			{
				if (navMesh != navigationMesh)
				{
					navigationMesh.TurnOff();
				}
			}
			if ((bool)navMesh && ((bool)navMesh.GetComponent<Collider>() || (bool)navMesh.GetComponent<Collider2D>()))
			{
				navMesh.TurnOn();
			}
		}

		private void Start()
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if (!settingsManager.IsInLoadingScene() && (bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>())
			{
				SaveSystem component = GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>();
				LevelStorage component2 = component.GetComponent<LevelStorage>();
				if ((bool)component2)
				{
					component2.ReturnCurrentLevelData();
				}
				if (component.loadingGame == LoadingGame.No)
				{
					FindPlayerStart();
				}
				else
				{
					component.loadingGame = LoadingGame.No;
				}
			}
		}

		public void ResetPlayerReference()
		{
			if ((bool)sortingMap)
			{
				sortingMap.GetAllFollowers();
			}
		}

		private void FindPlayerStart()
		{
			if (GetPlayerStart() != null)
			{
				GetPlayerStart().SetPlayerStart();
			}
			StateHandler component = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
			bool flag = component.PlayGlobalOnStart();
			if (cutsceneOnStart != null)
			{
				if (!flag)
				{
					component.gameState = GameState.Cutscene;
				}
				Invoke("RunCutsceneOnStart", 0.01f);
			}
		}

		private void RunCutsceneOnStart()
		{
			GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>().gameState = GameState.Normal;
			cutsceneOnStart.Interact();
		}

		public PlayerStart GetPlayerStart()
		{
			SceneChanger component = GameObject.FindWithTag("PersistentEngine").GetComponent<SceneChanger>();
			PlayerStart[] array = Object.FindObjectsOfType(typeof(PlayerStart)) as PlayerStart[];
			PlayerStart[] array2 = array;
			foreach (PlayerStart playerStart in array2)
			{
				if (playerStart.chooseSceneBy == ChooseSceneBy.Name && playerStart.previousSceneName != string.Empty && playerStart.previousSceneName == component.previousSceneName)
				{
					return playerStart;
				}
				if (playerStart.chooseSceneBy == ChooseSceneBy.Number && playerStart.previousScene > -1 && playerStart.previousScene == component.previousScene)
				{
					return playerStart;
				}
			}
			if ((bool)defaultPlayerStart)
			{
				return defaultPlayerStart;
			}
			return null;
		}

		public void OnLoad()
		{
			if (cutsceneOnLoad != null)
			{
				cutsceneOnLoad.Interact();
			}
		}

		public void PlayDefaultSound(AudioClip audioClip, bool doLoop)
		{
			if (defaultSound == null)
			{
				Debug.Log("Cannot play sound since no Default Sound Prefab is defined - please set one in the Scene Manager.");
			}
			else if ((bool)audioClip && (bool)defaultSound.GetComponent<AudioSource>())
			{
				defaultSound.GetComponent<AudioSource>().clip = audioClip;
				defaultSound.Play(doLoop);
			}
		}

		public void PauseGame()
		{
			Sound[] array = Object.FindObjectsOfType(typeof(Sound)) as Sound[];
			List<Sound> list = new List<Sound>();
			Sound[] array2 = array;
			foreach (Sound sound in array2)
			{
				if (sound.playWhilePaused && sound.IsPlaying())
				{
					list.Add(sound);
				}
			}
			Time.timeScale = 0f;
			AudioListener.pause = true;
			foreach (Sound item in list)
			{
				item.Play();
			}
		}

		public void UnpauseGame(float newScale)
		{
			Time.timeScale = newScale;
		}
	}
}
