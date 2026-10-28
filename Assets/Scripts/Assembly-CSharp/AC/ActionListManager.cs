using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class ActionListManager : MonoBehaviour
	{
		public bool showActiveActionLists;

		private bool playCutsceneOnVarChange;

		private bool saveAfterCutscene;

		private Conversation conversationOnEnd;

		private List<ActionList> activeLists = new List<ActionList>();

		private PlayerMenus playerMenus;

		private StateHandler stateHandler;

		private void Awake()
		{
			activeLists.Clear();
		}

		private void Start()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
			{
				stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
				playerMenus = stateHandler.GetComponent<PlayerMenus>();
			}
		}

		public void UpdateActionListManager()
		{
			if (saveAfterCutscene && !IsGameplayBlocked())
			{
				saveAfterCutscene = false;
				SaveSystem.SaveAutoSave();
			}
			if (playCutsceneOnVarChange && (bool)stateHandler && (stateHandler.gameState == GameState.Normal || stateHandler.gameState == GameState.DialogOptions))
			{
				playCutsceneOnVarChange = false;
				if (GetComponent<SceneSettings>().cutsceneOnVarChange != null)
				{
					GetComponent<SceneSettings>().cutsceneOnVarChange.Interact();
				}
			}
		}

		public void EndCutscene()
		{
			if (!IsGameplayBlocked())
			{
				return;
			}
			if (AdvGame.GetReferences().settingsManager.blackOutWhenSkipping)
			{
				KickStarter.mainCamera.HideScene();
			}
			Sound[] array = Object.FindObjectsOfType(typeof(Sound)) as Sound[];
			Sound[] array2 = array;
			foreach (Sound sound in array2)
			{
				if ((bool)sound.GetComponent<AudioSource>() && sound.soundType != SoundType.Music && !sound.GetComponent<AudioSource>().loop)
				{
					sound.Stop();
				}
			}
			int num = 0;
			while (IsGameplayBlocked() && num < 20)
			{
				for (int j = 0; j < activeLists.Count; j++)
				{
					if (activeLists[j].isSkippable && activeLists[j].actionListType == ActionListType.PauseGameplay)
					{
						activeLists[j].Skip();
					}
				}
				num++;
			}
		}

		public bool AreActionListsRunning()
		{
			if (activeLists.Count > 0)
			{
				return true;
			}
			return false;
		}

		public void AddToList(ActionList _list)
		{
			if (!IsListRunning(_list))
			{
				activeLists.Add(_list);
			}
			if ((bool)_list.conversation)
			{
				conversationOnEnd = _list.conversation;
			}
			if (!(_list is RuntimeActionList) || _list.actionListType != ActionListType.PauseGameplay || _list.unfreezePauseMenus || !playerMenus.ArePauseMenusOn(null))
			{
				SetCorrectGameState();
			}
		}

		public void EndList(ActionList _list)
		{
			if (IsListRunning(_list))
			{
				activeLists.Remove(_list);
			}
			_list.Reset();
			if (_list.conversation == conversationOnEnd && _list.conversation != null)
			{
				if ((bool)stateHandler)
				{
					stateHandler.gameState = GameState.Cutscene;
				}
				else
				{
					Debug.LogWarning("Could not set correct GameState!");
				}
				conversationOnEnd.Interact();
				conversationOnEnd = null;
			}
			else if (!(_list is RuntimeActionList) || _list.actionListType != ActionListType.PauseGameplay || _list.unfreezePauseMenus || !playerMenus.ArePauseMenusOn(null))
			{
				SetCorrectGameStateEnd();
			}
			if (_list.autosaveAfter)
			{
				if (!IsGameplayBlocked())
				{
					SaveSystem.SaveAutoSave();
				}
				else
				{
					saveAfterCutscene = true;
				}
			}
			if (_list is RuntimeActionList)
			{
				RuntimeActionList runtimeActionList = (RuntimeActionList)_list;
				runtimeActionList.DestroySelf();
			}
		}

		public void EndAssetList(string assetName)
		{
			RuntimeActionList[] array = Object.FindObjectsOfType(typeof(RuntimeActionList)) as RuntimeActionList[];
			RuntimeActionList[] array2 = array;
			foreach (RuntimeActionList runtimeActionList in array2)
			{
				if (runtimeActionList.name == assetName)
				{
					EndList(runtimeActionList);
				}
			}
		}

		public void VariableChanged()
		{
			playCutsceneOnVarChange = true;
		}

		public bool IsListRunning(ActionList _list)
		{
			foreach (ActionList activeList in activeLists)
			{
				if (activeList == _list)
				{
					return true;
				}
			}
			return false;
		}

		public void KillAllLists()
		{
			foreach (ActionList activeList in activeLists)
			{
				activeList.Reset();
				if (activeList is RuntimeActionList)
				{
					RuntimeActionList runtimeActionList = (RuntimeActionList)activeList;
					runtimeActionList.DestroySelf();
				}
			}
			activeLists.Clear();
		}

		public static void KillAll()
		{
			GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>().KillAllLists();
		}

		private void SetCorrectGameStateEnd()
		{
			if (stateHandler == null)
			{
				Start();
			}
			if (stateHandler != null)
			{
				if (playerMenus.ArePauseMenusOn(null))
				{
					stateHandler.gameState = GameState.Paused;
				}
				else
				{
					stateHandler.RestoreLastNonPausedState();
				}
			}
			else
			{
				Debug.LogWarning("Could not set correct GameState!");
			}
		}

		private void SetCorrectGameState()
		{
			if (stateHandler == null)
			{
				Start();
			}
			if (stateHandler != null)
			{
				if (IsGameplayBlocked())
				{
					stateHandler.gameState = GameState.Cutscene;
				}
				else if (playerMenus.ArePauseMenusOn(null))
				{
					stateHandler.gameState = GameState.Paused;
				}
				else if (GetComponent<PlayerInput>().activeConversation != null)
				{
					stateHandler.gameState = GameState.DialogOptions;
				}
				else
				{
					stateHandler.gameState = GameState.Normal;
				}
			}
			else
			{
				Debug.LogWarning("Could not set correct GameState!");
			}
		}

		public bool IsGameplayBlocked()
		{
			foreach (ActionList activeList in activeLists)
			{
				if (activeList.actionListType == ActionListType.PauseGameplay)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsInSkippableCutscene()
		{
			foreach (ActionList activeList in activeLists)
			{
				if (activeList.actionListType == ActionListType.PauseGameplay && activeList.isSkippable)
				{
					return true;
				}
			}
			return false;
		}

		private void OnDestroy()
		{
			activeLists.Clear();
			stateHandler = null;
		}
	}
}
