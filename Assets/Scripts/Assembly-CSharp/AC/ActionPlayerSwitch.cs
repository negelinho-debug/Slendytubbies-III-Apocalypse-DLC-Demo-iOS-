using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionPlayerSwitch : Action
	{
		public int playerID;

		public int playerNumber;

		public NewPlayerPosition newPlayerPosition = NewPlayerPosition.ReplaceNPC;

		public OldPlayer oldPlayer;

		public bool restorePreviousData;

		public ChooseSceneBy chooseNewSceneBy;

		public int newPlayerScene;

		public string newPlayerSceneName;

		public int oldPlayerNPC_ID;

		public NPC oldPlayerNPC;

		public int newPlayerNPC_ID;

		public NPC newPlayerNPC;

		public int newPlayerMarker_ID;

		public Marker newPlayerMarker;

		private SettingsManager settingsManager;

		public ActionPlayerSwitch()
		{
			isDisplayed = true;
			title = "Player: Switch";
		}

		public override void AssignValues()
		{
			oldPlayerNPC = AssignFile(oldPlayerNPC_ID, oldPlayerNPC);
			newPlayerNPC = AssignFile(newPlayerNPC_ID, newPlayerNPC);
			newPlayerMarker = AssignFile(newPlayerMarker_ID, newPlayerMarker);
		}

		public override float Run()
		{
			if (!settingsManager)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			if ((bool)settingsManager && settingsManager.playerSwitching == PlayerSwitching.Allow && settingsManager.players.Count > 0 && settingsManager.players.Count > playerNumber && playerNumber > -1)
			{
				if (KickStarter.player.ID == playerID)
				{
					Debug.Log("Cannot switch player - already controlling the desired prefab.");
					return 0f;
				}
				if (settingsManager.players[playerNumber].playerOb != null)
				{
					SaveSystem component = GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>();
					component.SaveCurrentPlayerData();
					Vector3 position = KickStarter.player.transform.position;
					Quaternion rotation = KickStarter.player.transform.rotation;
					Vector3 localScale = KickStarter.player.transform.localScale;
					if (oldPlayer == OldPlayer.ReplaceWithNPC && oldPlayerNPC != null && (newPlayerPosition == NewPlayerPosition.ReplaceNPC || newPlayerPosition == NewPlayerPosition.AppearAtMarker))
					{
						oldPlayerNPC.transform.position = position;
						oldPlayerNPC.transform.rotation = rotation;
						oldPlayerNPC.transform.localScale = localScale;
					}
					Quaternion rotation2 = Quaternion.identity;
					if (newPlayerPosition == NewPlayerPosition.ReplaceCurrentPlayer)
					{
						rotation2 = rotation;
					}
					else if (newPlayerPosition == NewPlayerPosition.ReplaceNPC && (bool)newPlayerNPC)
					{
						rotation2 = newPlayerNPC.transform.rotation;
					}
					else if (newPlayerPosition == NewPlayerPosition.AppearAtMarker && (bool)newPlayerMarker)
					{
						rotation2 = newPlayerMarker.transform.rotation;
					}
					KickStarter.ResetPlayer(settingsManager.players[playerNumber].playerOb, playerID, true, rotation2);
					Player player = KickStarter.player;
					int loadedLevel = Application.loadedLevel;
					if (restorePreviousData && component.DoesPlayerDataExist(playerID, true))
					{
						loadedLevel = GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>().GetPlayerScene(playerID);
						if (loadedLevel != Application.loadedLevel)
						{
							component.loadingGame = LoadingGame.JustSwitchingPlayer;
							GameObject.FindWithTag("PersistentEngine").GetComponent<SceneChanger>().ChangeScene(string.Empty, loadedLevel, true);
						}
					}
					else if (newPlayerPosition == NewPlayerPosition.ReplaceCurrentPlayer)
					{
						player.Teleport(position);
						player.SetRotation(rotation);
						player.transform.localScale = localScale;
					}
					else if (newPlayerPosition == NewPlayerPosition.ReplaceNPC)
					{
						if ((bool)newPlayerNPC)
						{
							player.Teleport(newPlayerNPC.transform.position);
							player.SetRotation(newPlayerNPC.transform.rotation);
							player.transform.localScale = newPlayerNPC.transform.localScale;
							newPlayerNPC.transform.position += new Vector3(100f, -100f, 100f);
						}
					}
					else if (newPlayerPosition == NewPlayerPosition.AppearAtMarker)
					{
						if ((bool)newPlayerMarker)
						{
							player.Teleport(newPlayerMarker.transform.position);
							player.SetRotation(newPlayerMarker.transform.rotation);
							player.transform.localScale = newPlayerMarker.transform.localScale;
						}
					}
					else if (newPlayerPosition == NewPlayerPosition.AppearInOtherScene && (chooseNewSceneBy != ChooseSceneBy.Name || !(newPlayerSceneName == Application.loadedLevelName)) && (chooseNewSceneBy != ChooseSceneBy.Number || newPlayerScene != Application.loadedLevel))
					{
						GameObject.FindWithTag("PersistentEngine").GetComponent<SceneChanger>().ChangeScene(AdvGame.GetSceneName(chooseNewSceneBy, newPlayerSceneName), newPlayerScene, true);
					}
					MainCamera mainCamera = KickStarter.mainCamera;
					if ((bool)mainCamera.attachedCamera)
					{
						mainCamera.attachedCamera.MoveCameraInstant();
					}
					AssetLoader.UnloadAssets();
				}
				else
				{
					Debug.LogWarning("Cannot switch player - no player prefabs is defined.");
				}
			}
			return 0f;
		}
	}
}
