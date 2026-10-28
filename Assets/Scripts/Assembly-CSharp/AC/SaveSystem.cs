using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace AC
{
	public class SaveSystem : MonoBehaviour
	{
		public LoadingGame loadingGame;

		public List<SaveFile> foundSaveFiles;

		private string saveDirectory = string.Empty;

		private string saveExtention = ".save";

		private SaveData saveData;

		private LevelStorage levelStorage;

		private void Awake()
		{
			saveDirectory = Application.persistentDataPath;
			levelStorage = GetComponent<LevelStorage>();
			GatherSaveFiles();
		}

		private void GatherSaveFiles()
		{
			foundSaveFiles = new List<SaveFile>();
			for (int i = 0; i < 50; i++)
			{
				if (!File.Exists(saveDirectory + Path.DirectorySeparatorChar + GetProjectName() + "_" + i + saveExtention))
				{
					continue;
				}
				SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
				string text = "Save " + i;
				if (i == 0)
				{
					text = "Autosave";
				}
				if (settingsManager.saveTimeDisplay != SaveTimeDisplay.None)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(saveDirectory);
					FileInfo[] files = directoryInfo.GetFiles(GetProjectName() + "_" + i + saveExtention);
					string text2 = files[0].LastWriteTime.ToString();
					if (settingsManager.saveTimeDisplay == SaveTimeDisplay.DateOnly)
					{
						text2 = text2.Substring(0, text2.IndexOf(" "));
					}
					text = text + " (" + text2 + ")";
				}
				Texture2D screenShot = null;
				if (settingsManager.takeSaveScreenshots)
				{
					screenShot = Serializer.LoadScreenshot(GetSaveScreenshotName(i));
				}
				foundSaveFiles.Add(new SaveFile(i, text, screenShot));
			}
		}

		private IEnumerator TakeScreenshot(string fileName)
		{
			StateHandler stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
			bool originalMenuState = stateHandler.menuIsOff;
			bool originalCursorState = stateHandler.cursorIsOff;
			stateHandler.menuIsOff = true;
			stateHandler.cursorIsOff = true;
			yield return new WaitForEndOfFrame();
			Texture2D screenshotTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			screenshotTex.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
			screenshotTex.Apply();
			Serializer.SaveScreenshot(screenshotTex, fileName);
			Object.Destroy(screenshotTex);
			stateHandler.menuIsOff = originalMenuState;
			stateHandler.cursorIsOff = originalCursorState;
			GatherSaveFiles();
		}

		public static SaveMethod GetSaveMethod()
		{
			return SaveMethod.Binary;
		}

		public static bool DoesSaveExist(int saveID)
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>())
			{
				SaveSystem component = GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>();
				foreach (SaveFile foundSaveFile in component.foundSaveFiles)
				{
					if (foundSaveFile.ID == saveID)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static void LoadAutoSave()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>())
			{
				SaveSystem component = GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>();
				if (File.Exists(component.GetSaveFileName(0)))
				{
					component.LoadSaveGame(0);
				}
				else
				{
					Debug.LogWarning("Could not load game: file " + component.GetSaveFileName(0) + " does not exist.");
				}
			}
		}

		public static bool LoadGame(int elementSlot, int saveID, bool useSaveID)
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>())
			{
				SaveSystem component = GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>();
				if (!useSaveID)
				{
					saveID = component.foundSaveFiles[elementSlot].ID;
				}
				if (saveID != -1)
				{
					component.LoadSaveGame(saveID);
					return true;
				}
				Debug.LogWarning("Could not load game: file " + component.GetSaveFileName(saveID) + " does not exist.");
			}
			return false;
		}

		public void ClearAllData()
		{
			saveData = new SaveData();
		}

		public void LoadSaveGame(int saveID)
		{
			string text = Serializer.LoadSaveFile(GetSaveFileName(saveID));
			if (!(text.ToString() != string.Empty))
			{
				return;
			}
			int num = text.IndexOf("||");
			string text2 = text.Substring(0, num);
			string text3 = text.Substring(num + 2);
			if (GetSaveMethod() == SaveMethod.XML)
			{
				saveData = (SaveData)Serializer.DeserializeObjectXML<SaveData>(text2);
				levelStorage.allLevelData = (List<SingleLevelData>)Serializer.DeserializeObjectXML<List<SingleLevelData>>(text3);
			}
			else
			{
				saveData = Serializer.DeserializeObjectBinary<SaveData>(text2);
				levelStorage.allLevelData = Serializer.DeserializeRoom(text3);
			}
			KillActionLists();
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if (settingsManager.playerSwitching == PlayerSwitching.Allow && KickStarter.player.ID != saveData.mainData.currentPlayerID)
			{
				KickStarter.ResetPlayer(GetPlayerByID(saveData.mainData.currentPlayerID), saveData.mainData.currentPlayerID, true, Quaternion.identity);
			}
			int playerScene = GetPlayerScene(saveData.mainData.currentPlayerID, saveData.playerData);
			if (playerScene != Application.loadedLevel)
			{
				loadingGame = LoadingGame.InNewScene;
				if ((bool)GetComponent<SceneChanger>())
				{
					SceneChanger component = GetComponent<SceneChanger>();
					component.ChangeScene(string.Empty, playerScene, false);
				}
				return;
			}
			loadingGame = LoadingGame.InSameScene;
			Sound[] array = Object.FindObjectsOfType(typeof(Sound)) as Sound[];
			Sound[] array2 = array;
			foreach (Sound sound in array2)
			{
				if ((bool)sound.GetComponent<AudioSource>() && sound.soundType != SoundType.Music && !sound.GetComponent<AudioSource>().loop)
				{
					sound.Stop();
				}
			}
			OnLevelWasLoaded();
		}

		private Player GetPlayerByID(int id)
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			foreach (PlayerPrefab player in settingsManager.players)
			{
				if (player.ID == id)
				{
					if ((bool)player.playerOb)
					{
						return player.playerOb;
					}
					return null;
				}
			}
			return null;
		}

		private int GetPlayerScene(int playerID, List<PlayerData> _playerData)
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if (settingsManager.playerSwitching == PlayerSwitching.DoNotAllow)
			{
				if (_playerData.Count > 0)
				{
					return _playerData[0].currentScene;
				}
			}
			else
			{
				foreach (PlayerData _playerDatum in _playerData)
				{
					if (_playerDatum.playerID == playerID)
					{
						return _playerDatum.currentScene;
					}
				}
			}
			return Application.loadedLevel;
		}

		private string GetPlayerSceneName(int playerID, List<PlayerData> _playerData)
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if (settingsManager.playerSwitching == PlayerSwitching.DoNotAllow)
			{
				if (_playerData.Count > 0)
				{
					return _playerData[0].currentSceneName;
				}
			}
			else
			{
				foreach (PlayerData _playerDatum in _playerData)
				{
					if (_playerDatum.playerID == playerID)
					{
						return _playerDatum.currentSceneName;
					}
				}
			}
			return Application.loadedLevelName;
		}

		private void OnLevelWasLoaded()
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if (settingsManager.IsInLoadingScene())
			{
				return;
			}
			if (loadingGame == LoadingGame.InNewScene || loadingGame == LoadingGame.InSameScene)
			{
				if ((bool)GameObject.FindWithTag("GameEngine"))
				{
					if ((bool)GameObject.FindWithTag("GameEngine").GetComponent<Dialog>())
					{
						Dialog component = GameObject.FindWithTag("GameEngine").GetComponent<Dialog>();
						component.KillDialog(true);
					}
					if ((bool)GameObject.FindWithTag("GameEngine").GetComponent<PlayerInteraction>())
					{
						PlayerInteraction component2 = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInteraction>();
						component2.StopInteraction();
					}
				}
				ReturnMainData();
				levelStorage.ReturnCurrentLevelData();
				if (loadingGame == LoadingGame.InSameScene)
				{
					loadingGame = LoadingGame.No;
				}
			}
			if ((bool)GetComponent<RuntimeInventory>())
			{
				GetComponent<RuntimeInventory>().RemoveRecipes();
			}
			if (loadingGame == LoadingGame.JustSwitchingPlayer)
			{
				foreach (PlayerData playerDatum in saveData.playerData)
				{
					if (playerDatum.playerID == KickStarter.player.ID)
					{
						ReturnCameraData(playerDatum);
						break;
					}
				}
				GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>().gameState = GameState.Cutscene;
				KickStarter.mainCamera.FadeIn(0.5f);
				Invoke("ReturnToGameplay", 0.01f);
			}
			AssetLoader.UnloadAssets();
		}

		public static void SaveNewGame()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>())
			{
				GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>().SaveNewSaveGame();
			}
		}

		public void SaveNewSaveGame()
		{
			if (foundSaveFiles != null && foundSaveFiles.Count > 0)
			{
				int num = -1;
				for (int i = 0; i < foundSaveFiles.Count; i++)
				{
					if (num != -1 && num != foundSaveFiles[i].ID)
					{
						SaveSaveGame(num);
						return;
					}
					num = foundSaveFiles[i].ID + 1;
				}
				int saveID = foundSaveFiles[foundSaveFiles.Count - 1].ID + 1;
				SaveSaveGame(saveID);
			}
			else
			{
				SaveSaveGame(1);
			}
		}

		public static void SaveAutoSave()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>())
			{
				SaveSystem component = GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>();
				component.SaveSaveGame(0);
			}
		}

		public static void SaveGame(int elementSlot, int saveID, bool useSaveID)
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>())
			{
				SaveSystem component = GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>();
				if (!useSaveID)
				{
					saveID = component.foundSaveFiles[elementSlot].ID;
				}
				component.SaveSaveGame(saveID);
			}
		}

		public void SaveSaveGame(int saveID)
		{
			levelStorage.StoreCurrentLevelData();
			Player player = KickStarter.player;
			PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
			RuntimeInventory component2 = GetComponent<RuntimeInventory>();
			SceneChanger component3 = GetComponent<SceneChanger>();
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			StateHandler component4 = GetComponent<StateHandler>();
			if ((bool)component && (bool)component2 && (bool)component3 && (bool)settingsManager && (bool)component4)
			{
				if (saveData != null && saveData.playerData != null && saveData.playerData.Count > 0)
				{
					foreach (PlayerData playerDatum in saveData.playerData)
					{
						if (playerDatum.playerID == player.ID)
						{
							saveData.playerData.Remove(playerDatum);
							break;
						}
					}
				}
				else
				{
					saveData = new SaveData();
					saveData.mainData = default(MainData);
					saveData.playerData = new List<PlayerData>();
				}
				PlayerData item = SavePlayerData(player);
				saveData.playerData.Add(item);
				saveData.mainData.cursorIsOff = component4.cursorIsOff;
				saveData.mainData.inputIsOff = component4.inputIsOff;
				saveData.mainData.interactionIsOff = component4.interactionIsOff;
				saveData.mainData.menuIsOff = component4.menuIsOff;
				saveData.mainData.movementIsOff = component4.movementIsOff;
				saveData.mainData.cameraIsOff = component4.cameraIsOff;
				saveData.mainData.triggerIsOff = component4.triggerIsOff;
				saveData.mainData.playerIsOff = component4.playerIsOff;
				if (player != null)
				{
					saveData.mainData.currentPlayerID = player.ID;
				}
				else
				{
					saveData.mainData.currentPlayerID = 0;
				}
				saveData.mainData.timeScale = component.timeScale;
				if ((bool)component.activeArrows)
				{
					saveData.mainData.activeArrows = Serializer.GetConstantID(component.activeArrows.gameObject);
				}
				if ((bool)component.activeConversation)
				{
					saveData.mainData.activeConversation = Serializer.GetConstantID(component.activeConversation.gameObject);
				}
				if (component2.selectedItem != null)
				{
					saveData.mainData.selectedInventoryID = component2.selectedItem.id;
				}
				else
				{
					saveData.mainData.selectedInventoryID = -1;
				}
				RuntimeVariables.DownloadAll();
				saveData.mainData.runtimeVariablesData = CreateVariablesData(RuntimeVariables.GetAllVars(), false, VariableLocation.Global);
				saveData.mainData.menuLockData = CreateMenuLockData(PlayerMenus.GetMenus());
				saveData.mainData.menuElementVisibilityData = CreateMenuElementVisibilityData(PlayerMenus.GetMenus());
				saveData.mainData.menuJournalData = CreateMenuJournalData(PlayerMenus.GetMenus());
				string empty = string.Empty;
				string empty2 = string.Empty;
				if (GetSaveMethod() == SaveMethod.XML)
				{
					empty = Serializer.SerializeObjectXML<SaveData>(saveData);
					empty2 = Serializer.SerializeObjectXML<List<SingleLevelData>>(levelStorage.allLevelData);
				}
				else
				{
					empty = Serializer.SerializeObjectBinary(saveData);
					empty2 = Serializer.SerializeObjectBinary(levelStorage.allLevelData);
				}
				string data = empty + "||" + empty2;
				Serializer.CreateSaveFile(GetSaveFileName(saveID), data);
				if (settingsManager.takeSaveScreenshots)
				{
					StartCoroutine("TakeScreenshot", GetSaveScreenshotName(saveID));
				}
				else
				{
					GatherSaveFiles();
				}
			}
			else
			{
				if (component == null)
				{
					Debug.LogWarning("Save failed - no PlayerInput found.");
				}
				if (component2 == null)
				{
					Debug.LogWarning("Save failed - no RuntimeInventory found.");
				}
				if (component3 == null)
				{
					Debug.LogWarning("Save failed - no SceneChanger found.");
				}
				if (settingsManager == null)
				{
					Debug.LogWarning("Save failed - no Settings Manager found.");
				}
			}
		}

		public void SaveCurrentPlayerData()
		{
			if (saveData != null && saveData.playerData != null && saveData.playerData.Count > 0)
			{
				foreach (PlayerData playerDatum in saveData.playerData)
				{
					if (playerDatum.playerID == KickStarter.player.ID)
					{
						saveData.playerData.Remove(playerDatum);
						break;
					}
				}
			}
			else
			{
				saveData = new SaveData();
				saveData.mainData = default(MainData);
				saveData.playerData = new List<PlayerData>();
			}
			PlayerData item = SavePlayerData(KickStarter.player);
			saveData.playerData.Add(item);
		}

		private PlayerData SavePlayerData(Player player)
		{
			PlayerData result = new PlayerData
			{
				currentScene = Application.loadedLevel,
				currentSceneName = Application.loadedLevelName
			};
			SceneChanger component = GetComponent<SceneChanger>();
			result.previousScene = component.previousScene;
			result.previousSceneName = component.previousSceneName;
			PlayerInput component2 = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
			result.playerUpLock = component2.isUpLocked;
			result.playerDownLock = component2.isDownLocked;
			result.playerLeftlock = component2.isLeftLocked;
			result.playerRightLock = component2.isRightLocked;
			result.playerRunLock = (int)component2.runLock;
			result.playerFreeAimLock = component2.freeAimLock;
			RuntimeInventory component3 = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
			component3.RemoveRecipes();
			result.inventoryData = CreateInventoryData(component3.localItems);
			if (player == null)
			{
				result.playerPortraitGraphic = 0;
				return result;
			}
			result.playerID = player.ID;
			result.playerLocX = player.transform.position.x;
			result.playerLocY = player.transform.position.y;
			result.playerLocZ = player.transform.position.z;
			result.playerRotY = player.transform.eulerAngles.y;
			result.playerWalkSpeed = player.walkSpeedScale;
			result.playerRunSpeed = player.runSpeedScale;
			if (player.animationEngine == AnimationEngine.Sprites2DToolkit || player.animationEngine == AnimationEngine.SpritesUnity)
			{
				result.playerIdleAnim = player.idleAnimSprite;
				result.playerWalkAnim = player.walkAnimSprite;
				result.playerRunAnim = player.runAnimSprite;
				result.playerTalkAnim = player.talkAnimSprite;
			}
			else if (player.animationEngine == AnimationEngine.Legacy)
			{
				result.playerIdleAnim = AssetLoader.GetAssetInstanceID(player.idleAnim).ToString();
				result.playerWalkAnim = AssetLoader.GetAssetInstanceID(player.walkAnim).ToString();
				result.playerRunAnim = AssetLoader.GetAssetInstanceID(player.runAnim).ToString();
				result.playerTalkAnim = AssetLoader.GetAssetInstanceID(player.talkAnim).ToString();
			}
			else if (player.animationEngine == AnimationEngine.Mecanim)
			{
				result.playerWalkAnim = player.moveSpeedParameter;
				result.playerTalkAnim = player.talkParameter;
				result.playerRunAnim = player.turnParameter;
			}
			result.playerWalkSound = AssetLoader.GetAssetInstanceID(player.walkSound);
			result.playerRunSound = AssetLoader.GetAssetInstanceID(player.runSound);
			result.playerPortraitGraphic = AssetLoader.GetAssetInstanceID(player.portraitIcon.texture);
			result.playerSpeechLabel = player.speechLabel;
			result.playerLockDirection = player.lockDirection;
			result.playerLockScale = player.lockScale;
			if ((bool)player.spriteChild && (bool)player.spriteChild.GetComponent<FollowSortingMap>())
			{
				result.playerLockSorting = player.spriteChild.GetComponent<FollowSortingMap>().lockSorting;
			}
			else if ((bool)player.GetComponent<FollowSortingMap>())
			{
				result.playerLockSorting = player.GetComponent<FollowSortingMap>().lockSorting;
			}
			else
			{
				result.playerLockSorting = false;
			}
			result.playerSpriteDirection = player.spriteDirection;
			result.playerSpriteScale = player.spriteScale;
			if ((bool)player.spriteChild && (bool)player.spriteChild.GetComponent<Renderer>())
			{
				result.playerSortingOrder = player.spriteChild.GetComponent<Renderer>().sortingOrder;
				result.playerSortingLayer = player.spriteChild.GetComponent<Renderer>().sortingLayerName;
			}
			else if ((bool)player.GetComponent<Renderer>())
			{
				result.playerSortingOrder = player.GetComponent<Renderer>().sortingOrder;
				result.playerSortingLayer = player.GetComponent<Renderer>().sortingLayerName;
			}
			result.playerActivePath = 0;
			result.lastPlayerActivePath = 0;
			if ((bool)player.GetPath(true))
			{
				result.playerTargetNode = player.GetTargetNode(true);
				result.playerPrevNode = player.GetPrevNode(true);
				result.playerIsRunning = player.isRunning;
				if ((bool)player.GetComponent<Paths>() && player.GetPath(true) == player.GetComponent<Paths>())
				{
					result.playerPathData = Serializer.CreatePathData(player.GetComponent<Paths>());
					result.playerLockedPath = false;
				}
				else
				{
					result.playerPathData = string.Empty;
					result.playerActivePath = Serializer.GetConstantID(player.GetPath(true).gameObject);
					result.playerLockedPath = player.lockedPath;
				}
			}
			if ((bool)player.GetPath(false))
			{
				result.lastPlayerTargetNode = player.GetTargetNode(false);
				result.lastPlayerPrevNode = player.GetPrevNode(false);
				result.lastPlayerActivePath = Serializer.GetConstantID(player.GetPath(false).gameObject);
			}
			result.playerIgnoreGravity = player.ignoreGravity;
			if (player.headFacing == HeadFacing.Manual)
			{
				result.isHeadTurning = true;
				result.headTargetX = player.headTurnTarget.x;
				result.headTargetY = player.headTurnTarget.y;
				result.headTargetZ = player.headTurnTarget.z;
			}
			else
			{
				result.isHeadTurning = false;
				result.headTargetX = 0f;
				result.headTargetY = 0f;
				result.headTargetZ = 0f;
			}
			MainCamera mainCamera = KickStarter.mainCamera;
			if ((bool)mainCamera.attachedCamera)
			{
				result.gameCamera = Serializer.GetConstantID(mainCamera.attachedCamera.gameObject);
			}
			if ((bool)mainCamera.lastNavCamera)
			{
				result.lastNavCamera = Serializer.GetConstantID(mainCamera.lastNavCamera.gameObject);
			}
			if ((bool)mainCamera.lastNavCamera2)
			{
				result.lastNavCamera2 = Serializer.GetConstantID(mainCamera.lastNavCamera2.gameObject);
			}
			mainCamera.StopShaking();
			result.mainCameraLocX = mainCamera.transform.position.x;
			result.mainCameraLocY = mainCamera.transform.position.y;
			result.mainCameraLocZ = mainCamera.transform.position.z;
			result.mainCameraRotX = mainCamera.transform.eulerAngles.x;
			result.mainCameraRotY = mainCamera.transform.eulerAngles.y;
			result.mainCameraRotZ = mainCamera.transform.eulerAngles.z;
			result.isSplitScreen = mainCamera.isSplitScreen;
			if (mainCamera.isSplitScreen)
			{
				result.isTopLeftSplit = mainCamera.isTopLeftSplit;
				result.splitAmountMain = mainCamera.splitAmountMain;
				result.splitAmountOther = mainCamera.splitAmountOther;
				if (mainCamera.splitOrientation == MenuOrientation.Vertical)
				{
					result.splitIsVertical = true;
				}
				else
				{
					result.splitIsVertical = false;
				}
				if ((bool)mainCamera.splitCamera && (bool)mainCamera.splitCamera.GetComponent<ConstantID>())
				{
					result.splitCameraID = mainCamera.splitCamera.GetComponent<ConstantID>().constantID;
				}
				else
				{
					result.splitCameraID = 0;
				}
			}
			return result;
		}

		public static int GetNumSlots()
		{
			return GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>().foundSaveFiles.Count;
		}

		private string GetProjectName()
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if ((bool)settingsManager)
			{
				if (settingsManager.saveFileName == string.Empty)
				{
					settingsManager.saveFileName = SetProjectName();
				}
				if (settingsManager.saveFileName != string.Empty)
				{
					return settingsManager.saveFileName;
				}
			}
			return SetProjectName();
		}

		public static string SetProjectName()
		{
			string[] array = Application.dataPath.Split('/');
			return array[array.Length - 2];
		}

		private string GetSaveFileName(int saveID)
		{
			string empty = string.Empty;
			return saveDirectory + Path.DirectorySeparatorChar + GetProjectName() + "_" + saveID + saveExtention;
		}

		private string GetSaveScreenshotName(int saveID)
		{
			string empty = string.Empty;
			return saveDirectory + Path.DirectorySeparatorChar + GetProjectName() + "_" + saveID + ".jpg";
		}

		private void KillActionLists()
		{
			ActionListManager component = GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>();
			component.KillAllLists();
			Moveable[] array = Object.FindObjectsOfType(typeof(Moveable)) as Moveable[];
			Moveable[] array2 = array;
			foreach (Moveable moveable in array2)
			{
				moveable.Kill();
			}
		}

		public static string GetSaveSlotLabel(int elementSlot, int saveID, bool useSaveID)
		{
			if (Application.isPlaying)
			{
				SaveSystem component = GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>();
				if (useSaveID)
				{
					foreach (SaveFile foundSaveFile in component.foundSaveFiles)
					{
						if (foundSaveFile.ID == saveID)
						{
							return foundSaveFile.label;
						}
					}
				}
				else if (elementSlot >= 0 && elementSlot < component.foundSaveFiles.Count)
				{
					return component.foundSaveFiles[elementSlot].label;
				}
				return string.Empty;
			}
			return "Save test (01/01/2001 12:00:00)";
		}

		public static Texture2D GetSaveSlotScreenshot(int elementSlot, int saveID, bool useSaveID)
		{
			if (Application.isPlaying)
			{
				SaveSystem component = GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>();
				if (useSaveID)
				{
					foreach (SaveFile foundSaveFile in component.foundSaveFiles)
					{
						if (foundSaveFile.ID == saveID)
						{
							return foundSaveFile.screenShot;
						}
					}
				}
				else if (elementSlot >= 0 && elementSlot < component.foundSaveFiles.Count)
				{
					return component.foundSaveFiles[elementSlot].screenShot;
				}
			}
			return null;
		}

		private void ReturnMainData()
		{
			PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
			RuntimeInventory component2 = GetComponent<RuntimeInventory>();
			SceneChanger component3 = GetComponent<SceneChanger>();
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			StateHandler component4 = GetComponent<StateHandler>();
			if ((bool)component && (bool)component2 && (bool)settingsManager && (bool)component4)
			{
				PlayerData playerData = default(PlayerData);
				if (settingsManager.playerSwitching == PlayerSwitching.DoNotAllow)
				{
					if (saveData.playerData.Count > 0)
					{
						playerData = saveData.playerData[0];
					}
				}
				else
				{
					foreach (PlayerData playerDatum in saveData.playerData)
					{
						if (playerDatum.playerID == saveData.mainData.currentPlayerID)
						{
							playerData = playerDatum;
						}
					}
				}
				ReturnPlayerData(playerData, KickStarter.player);
				ReturnCameraData(playerData);
				component4.cursorIsOff = saveData.mainData.cursorIsOff;
				component4.inputIsOff = saveData.mainData.inputIsOff;
				component4.interactionIsOff = saveData.mainData.interactionIsOff;
				component4.menuIsOff = saveData.mainData.menuIsOff;
				component4.movementIsOff = saveData.mainData.movementIsOff;
				component4.cameraIsOff = saveData.mainData.cameraIsOff;
				component4.triggerIsOff = saveData.mainData.triggerIsOff;
				component4.playerIsOff = saveData.mainData.playerIsOff;
				component3.previousScene = playerData.previousScene;
				component.SetUpLock(playerData.playerUpLock);
				component.isDownLocked = playerData.playerDownLock;
				component.isLeftLocked = playerData.playerLeftlock;
				component.isRightLocked = playerData.playerRightLock;
				component.runLock = (PlayerMoveLock)playerData.playerRunLock;
				component.freeAimLock = playerData.playerFreeAimLock;
				component2.RemoveRecipes();
				component2.localItems = AssignInventory(component2, playerData.inventoryData);
				if (saveData.mainData.selectedInventoryID > -1)
				{
					component2.SelectItemByID(saveData.mainData.selectedInventoryID);
				}
				else
				{
					component2.SetNull();
				}
				component2.RemoveRecipes();
				component.RemoveActiveArrows();
				ArrowPrompt arrowPrompt = Serializer.returnComponent<ArrowPrompt>(saveData.mainData.activeArrows);
				if ((bool)arrowPrompt)
				{
					arrowPrompt.TurnOn();
				}
				component.activeConversation = Serializer.returnComponent<Conversation>(saveData.mainData.activeConversation);
				component.timeScale = saveData.mainData.timeScale;
				AssignVariables(saveData.mainData.runtimeVariablesData);
				foreach (Menu menu in PlayerMenus.GetMenus())
				{
					foreach (MenuElement element in menu.elements)
					{
						if (element is MenuInventoryBox)
						{
							MenuInventoryBox menuInventoryBox = (MenuInventoryBox)element;
							menuInventoryBox.ResetOffset();
						}
					}
				}
				AssignMenuLocks(PlayerMenus.GetMenus(), saveData.mainData.menuLockData);
				AssignMenuElementVisibility(PlayerMenus.GetMenus(), saveData.mainData.menuElementVisibilityData);
				AssignMenuJournals(PlayerMenus.GetMenus(), saveData.mainData.menuJournalData);
				component4.gameState = GameState.Cutscene;
				KickStarter.mainCamera.FadeIn(0.5f);
				Invoke("ReturnToGameplay", 0.01f);
			}
			else
			{
				if (component == null)
				{
					Debug.LogWarning("Load failed - no PlayerInput found.");
				}
				if (component2 == null)
				{
					Debug.LogWarning("Load failed - no RuntimeInventory found.");
				}
				if (component3 == null)
				{
					Debug.LogWarning("Load failed - no SceneChanger found.");
				}
				if (settingsManager == null)
				{
					Debug.LogWarning("Load failed - no Settings Manager found.");
				}
			}
		}

		public bool DoesPlayerDataExist(int ID, bool doSceneCheck)
		{
			if (saveData != null && saveData.playerData.Count > 0)
			{
				foreach (PlayerData playerDatum in saveData.playerData)
				{
					if (playerDatum.playerID == ID)
					{
						if (doSceneCheck && playerDatum.currentScene == -1)
						{
							return false;
						}
						return true;
					}
				}
			}
			return false;
		}

		public int GetPlayerScene(int ID)
		{
			if ((bool)KickStarter.player && saveData.playerData.Count > 0)
			{
				foreach (PlayerData playerDatum in saveData.playerData)
				{
					if (playerDatum.playerID == ID)
					{
						return playerDatum.currentScene;
					}
				}
			}
			return Application.loadedLevel;
		}

		public string GetPlayerSceneName(int ID)
		{
			if ((bool)KickStarter.player && saveData.playerData.Count > 0)
			{
				foreach (PlayerData playerDatum in saveData.playerData)
				{
					if (playerDatum.playerID == ID)
					{
						return playerDatum.currentSceneName;
					}
				}
			}
			return Application.loadedLevelName;
		}

		public int AssignPlayerData(int ID, bool doInventory)
		{
			if ((bool)KickStarter.player && saveData.playerData.Count > 0)
			{
				foreach (PlayerData playerDatum in saveData.playerData)
				{
					if (playerDatum.playerID == ID)
					{
						if (playerDatum.currentScene != -1)
						{
							ReturnPlayerData(playerDatum, KickStarter.player);
							ReturnCameraData(playerDatum);
							PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
							component.SetUpLock(playerDatum.playerUpLock);
							component.isDownLocked = playerDatum.playerDownLock;
							component.isLeftLocked = playerDatum.playerLeftlock;
							component.isRightLocked = playerDatum.playerRightLock;
							component.runLock = (PlayerMoveLock)playerDatum.playerRunLock;
							component.freeAimLock = playerDatum.playerFreeAimLock;
						}
						RuntimeInventory component2 = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
						component2.SetNull();
						component2.RemoveRecipes();
						if (doInventory)
						{
							component2.localItems = AssignInventory(component2, playerDatum.inventoryData);
						}
						return playerDatum.currentScene;
					}
				}
			}
			AssetLoader.UnloadAssets();
			return Application.loadedLevel;
		}

		private void ReturnPlayerData(PlayerData playerData, Player player)
		{
			if (player == null)
			{
				return;
			}
			player.Teleport(new Vector3(playerData.playerLocX, playerData.playerLocY, playerData.playerLocZ));
			player.SetRotation(playerData.playerRotY);
			player.walkSpeedScale = playerData.playerWalkSpeed;
			player.runSpeedScale = playerData.playerRunSpeed;
			if (player.animationEngine == AnimationEngine.Sprites2DToolkit || player.animationEngine == AnimationEngine.SpritesUnity)
			{
				player.idleAnimSprite = playerData.playerIdleAnim;
				player.walkAnimSprite = playerData.playerWalkAnim;
				player.talkAnimSprite = playerData.playerTalkAnim;
				player.runAnimSprite = playerData.playerRunAnim;
			}
			else if (player.animationEngine == AnimationEngine.Legacy)
			{
				int result = 0;
				int.TryParse(playerData.playerIdleAnim, out result);
				player.idleAnim = AssetLoader.RetrieveAsset(player.idleAnim, result);
				int.TryParse(playerData.playerWalkAnim, out result);
				player.walkAnim = AssetLoader.RetrieveAsset(player.walkAnim, result);
				int.TryParse(playerData.playerTalkAnim, out result);
				player.talkAnim = AssetLoader.RetrieveAsset(player.talkAnim, result);
				int.TryParse(playerData.playerRunAnim, out result);
				player.runAnim = AssetLoader.RetrieveAsset(player.runAnim, result);
			}
			else if (player.animationEngine == AnimationEngine.Mecanim)
			{
				player.moveSpeedParameter = playerData.playerWalkAnim;
				player.talkParameter = playerData.playerTalkAnim;
				player.turnParameter = playerData.playerRunAnim;
			}
			player.walkSound = AssetLoader.RetrieveAsset(player.walkSound, playerData.playerWalkSound);
			player.runSound = AssetLoader.RetrieveAsset(player.runSound, playerData.playerRunSound);
			player.portraitIcon.texture = AssetLoader.RetrieveAsset(player.portraitIcon.texture, playerData.playerPortraitGraphic);
			player.speechLabel = playerData.playerSpeechLabel;
			player.lockDirection = playerData.playerLockDirection;
			player.lockScale = playerData.playerLockScale;
			if ((bool)player.spriteChild && (bool)player.spriteChild.GetComponent<FollowSortingMap>())
			{
				player.spriteChild.GetComponent<FollowSortingMap>().lockSorting = playerData.playerLockSorting;
			}
			else if ((bool)player.GetComponent<FollowSortingMap>())
			{
				player.GetComponent<FollowSortingMap>().lockSorting = playerData.playerLockSorting;
			}
			else
			{
				player.ReleaseSorting();
			}
			if (playerData.playerLockDirection)
			{
				player.spriteDirection = playerData.playerSpriteDirection;
			}
			if (playerData.playerLockScale)
			{
				player.spriteScale = playerData.playerSpriteScale;
			}
			if (playerData.playerLockSorting)
			{
				if ((bool)player.spriteChild && (bool)player.spriteChild.GetComponent<Renderer>())
				{
					player.spriteChild.GetComponent<Renderer>().sortingOrder = playerData.playerSortingOrder;
					player.spriteChild.GetComponent<Renderer>().sortingLayerName = playerData.playerSortingLayer;
				}
				else if ((bool)player.GetComponent<Renderer>())
				{
					player.GetComponent<Renderer>().sortingOrder = playerData.playerSortingOrder;
					player.GetComponent<Renderer>().sortingLayerName = playerData.playerSortingLayer;
				}
			}
			player.Halt();
			if (playerData.playerPathData != null && playerData.playerPathData != string.Empty && (bool)player.GetComponent<Paths>())
			{
				Paths component = player.GetComponent<Paths>();
				component = Serializer.RestorePathData(component, playerData.playerPathData);
				player.SetPath(component, playerData.playerTargetNode, playerData.playerPrevNode);
				player.isRunning = playerData.playerIsRunning;
				player.lockedPath = false;
			}
			else if (playerData.playerActivePath != 0)
			{
				Paths paths = Serializer.returnComponent<Paths>(playerData.playerActivePath);
				if ((bool)paths)
				{
					player.lockedPath = playerData.playerLockedPath;
					if (player.lockedPath)
					{
						player.SetLockedPath(paths);
					}
					else
					{
						player.SetPath(paths, playerData.playerTargetNode, playerData.playerPrevNode);
					}
				}
			}
			if (playerData.lastPlayerActivePath != 0)
			{
				Paths paths2 = Serializer.returnComponent<Paths>(playerData.lastPlayerActivePath);
				if ((bool)paths2)
				{
					player.SetLastPath(paths2, playerData.lastPlayerTargetNode, playerData.lastPlayerPrevNode);
				}
			}
			if (playerData.isHeadTurning)
			{
				player.SetHeadTurnTarget(new Vector3(playerData.headTargetX, playerData.headTargetY, playerData.headTargetZ), true);
			}
			else
			{
				player.ClearHeadTurnTarget(true);
			}
			player.ignoreGravity = playerData.playerIgnoreGravity;
		}

		private void ReturnCameraData(PlayerData playerData)
		{
			MainCamera mainCamera = KickStarter.mainCamera;
			if (mainCamera.isSplitScreen)
			{
				mainCamera.RemoveSplitScreen();
			}
			mainCamera.StopShaking();
			mainCamera.SetGameCamera(Serializer.returnComponent<_Camera>(playerData.gameCamera));
			mainCamera.lastNavCamera = Serializer.returnComponent<_Camera>(playerData.lastNavCamera);
			mainCamera.lastNavCamera2 = Serializer.returnComponent<_Camera>(playerData.lastNavCamera2);
			mainCamera.ResetMoving();
			mainCamera.transform.position = new Vector3(playerData.mainCameraLocX, playerData.mainCameraLocY, playerData.mainCameraLocZ);
			mainCamera.transform.eulerAngles = new Vector3(playerData.mainCameraRotX, playerData.mainCameraRotY, playerData.mainCameraRotZ);
			mainCamera.ResetProjection();
			if ((bool)mainCamera.attachedCamera)
			{
				mainCamera.attachedCamera.MoveCameraInstant();
			}
			mainCamera.SnapToAttached();
			mainCamera.isSplitScreen = playerData.isSplitScreen;
			if (!mainCamera.isSplitScreen)
			{
				return;
			}
			mainCamera.isTopLeftSplit = playerData.isTopLeftSplit;
			if (playerData.splitIsVertical)
			{
				mainCamera.splitOrientation = MenuOrientation.Vertical;
			}
			else
			{
				mainCamera.splitOrientation = MenuOrientation.Horizontal;
			}
			if (playerData.splitCameraID != 0)
			{
				_Camera camera = Serializer.returnComponent<_Camera>(playerData.splitCameraID);
				if ((bool)camera)
				{
					mainCamera.splitCamera = camera;
				}
			}
			mainCamera.StartSplitScreen(playerData.splitAmountMain, playerData.splitAmountOther);
		}

		private void ReturnToGameplay()
		{
			PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
			StateHandler component2 = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
			if ((bool)component.activeConversation)
			{
				component2.gameState = GameState.DialogOptions;
			}
			else
			{
				component2.gameState = GameState.Normal;
			}
			component.mouseState = MouseState.Normal;
			if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>())
			{
				SceneSettings component3 = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
				component3.OnLoad();
			}
		}

		public static void AssignVariables(string runtimeVariablesData)
		{
			if (runtimeVariablesData == null)
			{
				return;
			}
			if (runtimeVariablesData.Length > 0)
			{
				string[] array = runtimeVariablesData.Split("|"[0]);
				string[] array2 = array;
				foreach (string text in array2)
				{
					string[] array3 = text.Split(":"[0]);
					int result = 0;
					int.TryParse(array3[0], out result);
					GVar variable = RuntimeVariables.GetVariable(result);
					if (variable != null)
					{
						if (variable.type == VariableType.String)
						{
							string value = array3[1];
							variable.SetValue(value);
						}
						else if (variable.type == VariableType.Float)
						{
							float result2 = 0f;
							float.TryParse(array3[1], out result2);
							variable.SetValue(result2, SetVarMethod.SetValue);
						}
						else
						{
							int result3 = 0;
							int.TryParse(array3[1], out result3);
							variable.SetValue(result3, SetVarMethod.SetValue);
						}
					}
				}
			}
			RuntimeVariables.UploadAll();
		}

		private List<InvItem> AssignInventory(RuntimeInventory runtimeInventory, string inventoryData)
		{
			List<InvItem> list = new List<InvItem>();
			if (inventoryData != null && inventoryData.Length > 0)
			{
				string[] array = inventoryData.Split("|"[0]);
				string[] array2 = array;
				foreach (string text in array2)
				{
					string[] array3 = text.Split(":"[0]);
					int result = 0;
					int.TryParse(array3[0], out result);
					int result2 = 0;
					int.TryParse(array3[1], out result2);
					list = runtimeInventory.Add(result, result2, list, false);
				}
			}
			return list;
		}

		private void AssignMenuLocks(List<Menu> menus, string menuLockData)
		{
			if (menuLockData.Length <= 0)
			{
				return;
			}
			string[] array = menuLockData.Split("|"[0]);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(":"[0]);
				int result = 0;
				int.TryParse(array3[0], out result);
				bool result2 = false;
				bool.TryParse(array3[1], out result2);
				foreach (Menu menu in menus)
				{
					if (menu.id == result)
					{
						menu.isLocked = result2;
						break;
					}
				}
			}
		}

		private void AssignMenuElementVisibility(List<Menu> menus, string menuElementVisibilityData)
		{
			if (menuElementVisibilityData.Length <= 0)
			{
				return;
			}
			string[] array = menuElementVisibilityData.Split("|"[0]);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(":"[0]);
				int result = 0;
				int.TryParse(array3[0], out result);
				foreach (Menu menu in menus)
				{
					if (menu.id != result)
					{
						continue;
					}
					string[] array4 = array3[1].Split("+"[0]);
					string[] array5 = array4;
					foreach (string text2 in array5)
					{
						string[] array6 = text2.Split("="[0]);
						int result2 = 0;
						int.TryParse(array6[0], out result2);
						bool result3 = false;
						bool.TryParse(array6[1], out result3);
						foreach (MenuElement element in menu.elements)
						{
							if (element.ID == result2 && element.isVisible != result3)
							{
								element.isVisible = result3;
								break;
							}
						}
					}
					menu.ResetVisibleElements();
					menu.Recalculate();
					break;
				}
			}
		}

		private void AssignMenuJournals(List<Menu> menus, string menuJournalData)
		{
			if (menuJournalData.Length <= 0)
			{
				return;
			}
			string[] array = menuJournalData.Split("|"[0]);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(":"[0]);
				int result = 0;
				int.TryParse(array3[0], out result);
				int result2 = 0;
				int.TryParse(array3[1], out result2);
				foreach (Menu menu in menus)
				{
					if (menu.id != result)
					{
						continue;
					}
					foreach (MenuElement element in menu.elements)
					{
						if (element.ID == result2 && element is MenuJournal)
						{
							MenuJournal menuJournal = (MenuJournal)element;
							menuJournal.pages = new List<JournalPage>();
							menuJournal.showPage = 1;
							string[] array4 = array3[2].Split("~"[0]);
							string[] array5 = array4;
							foreach (string text2 in array5)
							{
								string[] array6 = text2.Split("*"[0]);
								int result3 = -1;
								int.TryParse(array6[0], out result3);
								menuJournal.pages.Add(new JournalPage(result3, array6[1]));
							}
							break;
						}
					}
				}
			}
		}

		private string CreateInventoryData(List<InvItem> invItems)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (InvItem invItem in invItems)
			{
				stringBuilder.Append(invItem.id.ToString());
				stringBuilder.Append(":");
				stringBuilder.Append(invItem.count.ToString());
				stringBuilder.Append("|");
			}
			if (invItems != null && invItems.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		public static string CreateVariablesData(List<GVar> vars, bool isOptionsData, VariableLocation location)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GVar var in vars)
			{
				if ((!isOptionsData || var.link != VarLink.OptionsData) && (isOptionsData || var.link == VarLink.OptionsData) && location != VariableLocation.Local)
				{
					continue;
				}
				stringBuilder.Append(var.id.ToString());
				stringBuilder.Append(":");
				if (var.type == VariableType.String)
				{
					string text = var.textVal;
					if (text.Contains("|"))
					{
						text = text.Replace("|", string.Empty);
						Debug.LogWarning("Removed pipe delimeter from variable " + var.label);
					}
					stringBuilder.Append(text);
				}
				else if (var.type == VariableType.Float)
				{
					stringBuilder.Append(var.floatVal.ToString());
				}
				else
				{
					stringBuilder.Append(var.val.ToString());
				}
				stringBuilder.Append("|");
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		private string CreateMenuLockData(List<Menu> menus)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Menu menu in menus)
			{
				stringBuilder.Append(menu.id.ToString());
				stringBuilder.Append(":");
				stringBuilder.Append(menu.isLocked.ToString());
				stringBuilder.Append("|");
			}
			if (menus.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		private string CreateMenuElementVisibilityData(List<Menu> menus)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Menu menu in menus)
			{
				if (menu.elements.Count <= 0)
				{
					continue;
				}
				stringBuilder.Append(menu.id.ToString());
				stringBuilder.Append(":");
				foreach (MenuElement element in menu.elements)
				{
					stringBuilder.Append(element.ID.ToString());
					stringBuilder.Append("=");
					stringBuilder.Append(element.isVisible.ToString());
					stringBuilder.Append("+");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("|");
			}
			if (menus.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		private string CreateMenuJournalData(List<Menu> menus)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Menu menu in menus)
			{
				foreach (MenuElement element in menu.elements)
				{
					if (!(element is MenuJournal))
					{
						continue;
					}
					MenuJournal menuJournal = (MenuJournal)element;
					stringBuilder.Append(menu.id.ToString());
					stringBuilder.Append(":");
					stringBuilder.Append(menuJournal.ID);
					stringBuilder.Append(":");
					foreach (JournalPage page in menuJournal.pages)
					{
						stringBuilder.Append(page.lineID);
						stringBuilder.Append("*");
						stringBuilder.Append(page.text);
						stringBuilder.Append("~");
					}
					if (menuJournal.pages.Count > 0)
					{
						stringBuilder.Remove(stringBuilder.Length - 1, 1);
					}
					stringBuilder.Append("|");
				}
			}
			if (stringBuilder.ToString() != string.Empty)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		public List<InvItem> GetItemsFromPlayer(int _playerID)
		{
			if (KickStarter.player.ID == _playerID)
			{
				return GetComponent<RuntimeInventory>().localItems;
			}
			if (saveData != null && saveData.playerData != null)
			{
				foreach (PlayerData playerDatum in saveData.playerData)
				{
					if (playerDatum.playerID == _playerID)
					{
						return AssignInventory(GetComponent<RuntimeInventory>(), playerDatum.inventoryData);
					}
				}
			}
			return new List<InvItem>();
		}

		public void AssignItemsToPlayer(List<InvItem> invItems, int _playerID)
		{
			string inventoryData = CreateInventoryData(invItems);
			if (saveData != null && saveData.playerData != null)
			{
				foreach (PlayerData playerDatum in saveData.playerData)
				{
					if (playerDatum.playerID == _playerID)
					{
						PlayerData item = default(PlayerData);
						item.CopyData(playerDatum);
						item.inventoryData = inventoryData;
						saveData.playerData.Remove(playerDatum);
						saveData.playerData.Add(item);
						return;
					}
				}
			}
			PlayerData item2 = new PlayerData
			{
				playerID = _playerID,
				inventoryData = inventoryData,
				currentScene = -1
			};
			if (saveData == null)
			{
				ClearAllData();
			}
			saveData.playerData.Add(item2);
		}
	}
}
