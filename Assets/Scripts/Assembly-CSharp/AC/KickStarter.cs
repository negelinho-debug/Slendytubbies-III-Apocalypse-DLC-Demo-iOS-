using UnityEngine;

namespace AC
{
	public class KickStarter : MonoBehaviour
	{
		private static Player playerPrefab;

		private static MainCamera mainCameraPrefab;

		public static Player player
		{
			get
			{
				if (playerPrefab != null)
				{
					return playerPrefab;
				}
				if ((bool)GameObject.FindWithTag("Player") && (bool)GameObject.FindWithTag("Player").GetComponent<Player>())
				{
					playerPrefab = GameObject.FindWithTag("Player").GetComponent<Player>();
					return playerPrefab;
				}
				return null;
			}
		}

		public static MainCamera mainCamera
		{
			get
			{
				if (mainCameraPrefab != null)
				{
					return mainCameraPrefab;
				}
				if ((bool)GameObject.FindWithTag("MainCamera") && (bool)GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>())
				{
					mainCameraPrefab = GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>();
					return mainCameraPrefab;
				}
				return null;
			}
		}

		public static void ResetPlayer(Player ref_player, int ID, bool resetReferences, Quaternion _rotation)
		{
			if ((bool)GameObject.FindWithTag("Player"))
			{
				Object.DestroyImmediate(GameObject.FindWithTag("Player"));
			}
			if ((bool)ref_player)
			{
				SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
				Player player = Object.Instantiate(ref_player, Vector3.zero, _rotation);
				player.ID = ID;
				player.name = ref_player.name;
				playerPrefab = player;
				if ((bool)GameObject.FindWithTag("PersistentEngine"))
				{
					GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>().SetNull();
					GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>().RemoveRecipes();
					if (!settingsManager.shareInventory)
					{
						GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>().localItems.Clear();
					}
					SaveSystem component = GameObject.FindWithTag("PersistentEngine").GetComponent<SaveSystem>();
					if (component.DoesPlayerDataExist(ID, false))
					{
						component.AssignPlayerData(ID, !settingsManager.shareInventory);
					}
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
				}
				if ((bool)player.GetComponent<Animation>())
				{
					AdvGame.PlayAnimClip(player.GetComponent<Animation>(), AdvGame.GetAnimLayerInt(AnimLayer.Base), player.idleAnim, AnimationBlendMode.Blend, WrapMode.Loop, 0f, null, false);
				}
				else if ((bool)player.spriteChild)
				{
					if ((bool)player.spriteChild.GetComponent<FollowSortingMap>())
					{
						player.spriteChild.GetComponent<FollowSortingMap>().UpdateSortingMap();
					}
					player.UpdateSpriteChild(settingsManager.IsTopDown(), settingsManager.IsUnity2D());
				}
				player.animEngine.PlayIdle();
			}
			if (resetReferences)
			{
				GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>().ResetPlayerReference();
				GameObject.FindWithTag("GameEngine").GetComponent<PlayerMovement>().AssignFPCamera();
				_Camera[] array = Object.FindObjectsOfType(typeof(_Camera)) as _Camera[];
				_Camera[] array2 = array;
				foreach (_Camera camera in array2)
				{
					camera.ResetTarget();
				}
			}
		}

		private void Awake()
		{
			References references = (References)Resources.Load("References");
			if ((bool)references)
			{
				SceneManager sceneManager = AdvGame.GetReferences().sceneManager;
				SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
				ActionsManager actionsManager = AdvGame.GetReferences().actionsManager;
				InventoryManager inventoryManager = AdvGame.GetReferences().inventoryManager;
				VariablesManager variablesManager = AdvGame.GetReferences().variablesManager;
				SpeechManager speechManager = AdvGame.GetReferences().speechManager;
				CursorManager cursorManager = AdvGame.GetReferences().cursorManager;
				MenuManager menuManager = AdvGame.GetReferences().menuManager;
				if (sceneManager == null)
				{
					Debug.LogError("No Scene Manager found - please set one using the Adventure Creator Kit wizard");
				}
				if (settingsManager == null)
				{
					Debug.LogError("No Settings Manager found - please set one using the Adventure Creator Kit wizard");
				}
				else
				{
					if (settingsManager.IsInLoadingScene())
					{
						Debug.Log("Bypassing regular AC startup because the current scene is the 'Loading' scene.");
						return;
					}
					if (!GameObject.FindGameObjectWithTag("Player"))
					{
						ResetPlayer(settingsManager.GetDefaultPlayer(), settingsManager.GetDefaultPlayerID(), false, Quaternion.identity);
					}
					else
					{
						playerPrefab = GameObject.FindWithTag("Player").GetComponent<Player>();
					}
					if (LayerMask.NameToLayer(settingsManager.navMeshLayer) > -1)
					{
						if (LayerMask.NameToLayer(settingsManager.hotspotLayer) > -1)
						{
							Physics.IgnoreLayerCollision(LayerMask.NameToLayer(settingsManager.navMeshLayer), LayerMask.NameToLayer(settingsManager.hotspotLayer));
							Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(settingsManager.navMeshLayer), LayerMask.NameToLayer(settingsManager.hotspotLayer));
						}
						if (LayerMask.NameToLayer(settingsManager.deactivatedLayer) > -1)
						{
							Physics.IgnoreLayerCollision(LayerMask.NameToLayer(settingsManager.navMeshLayer), LayerMask.NameToLayer(settingsManager.deactivatedLayer));
							Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(settingsManager.navMeshLayer), LayerMask.NameToLayer(settingsManager.deactivatedLayer));
						}
						if (LayerMask.NameToLayer(settingsManager.backgroundImageLayer) > -1)
						{
							Physics.IgnoreLayerCollision(LayerMask.NameToLayer(settingsManager.navMeshLayer), LayerMask.NameToLayer(settingsManager.backgroundImageLayer));
							Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(settingsManager.navMeshLayer), LayerMask.NameToLayer(settingsManager.backgroundImageLayer));
						}
					}
				}
				if (actionsManager == null)
				{
					Debug.LogError("No Actions Manager found - please set one using the main Adventure Creator window");
				}
				if (inventoryManager == null)
				{
					Debug.LogError("No Inventory Manager found - please set one using the main Adventure Creator window");
				}
				if (variablesManager == null)
				{
					Debug.LogError("No Variables Manager found - please set one using the main Adventure Creator window");
				}
				if (speechManager == null)
				{
					Debug.LogError("No Speech Manager found - please set one using the main Adventure Creator window");
				}
				if (cursorManager == null)
				{
					Debug.LogError("No Cursor Manager found - please set one using the main Adventure Creator window");
				}
				if (menuManager == null)
				{
					Debug.LogError("No Menu Manager found - please set one using the main Adventure Creator window");
				}
				if (GameObject.FindWithTag("Player") == null)
				{
					Debug.LogWarning("No Player found - please set one using the Settings Manager, tagging it as Player and placing it in a Resources folder");
				}
			}
			else
			{
				Debug.LogError("No References object found. Please set one using the main Adventure Creator window");
			}
			if (!GameObject.FindGameObjectWithTag("PersistentEngine"))
			{
				try
				{
					GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("PersistentEngine"));
					gameObject.name = AdvGame.GetName("PersistentEngine");
				}
				catch
				{
				}
			}
			if (GameObject.FindWithTag("PersistentEngine") == null)
			{
				Debug.LogError("No PersistentEngine prefab found - please place one in the Resources directory, and tag it as PersistentEngine");
			}
			else
			{
				GameObject gameObject2 = GameObject.FindWithTag("PersistentEngine");
				if (gameObject2.GetComponent<Options>() == null)
				{
					Debug.LogError(gameObject2.name + " has no Options component attached.");
				}
				if (gameObject2.GetComponent<RuntimeInventory>() == null)
				{
					Debug.LogError(gameObject2.name + " has no RuntimeInventory component attached.");
				}
				if (gameObject2.GetComponent<RuntimeVariables>() == null)
				{
					Debug.LogError(gameObject2.name + " has no RuntimeVariables component attached.");
				}
				if (gameObject2.GetComponent<PlayerMenus>() == null)
				{
					Debug.LogError(gameObject2.name + " has no PlayerMenus component attached.");
				}
				if (gameObject2.GetComponent<StateHandler>() == null)
				{
					Debug.LogError(gameObject2.name + " has no StateHandler component attached.");
				}
				if (gameObject2.GetComponent<SceneChanger>() == null)
				{
					Debug.LogError(gameObject2.name + " has no SceneChanger component attached.");
				}
				if (gameObject2.GetComponent<SaveSystem>() == null)
				{
					Debug.LogError(gameObject2.name + " has no SaveSystem component attached.");
				}
				if (gameObject2.GetComponent<LevelStorage>() == null)
				{
					Debug.LogError(gameObject2.name + " has no LevelStorage component attached.");
				}
			}
			if (GameObject.FindWithTag("MainCamera") == null)
			{
				Debug.LogError("No MainCamera found - please click 'Organise room objects' in the Scene Manager to create one.");
			}
			else if (GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>() == null)
			{
				Debug.LogError("MainCamera has no MainCamera component.");
			}
			if (!(tag == "GameEngine"))
			{
				return;
			}
			if (GetComponent<MenuSystem>() == null)
			{
				Debug.LogError(name + " has no MenuSystem component attached.");
			}
			if (GetComponent<Dialog>() == null)
			{
				Debug.LogError(name + " has no Dialog component attached.");
			}
			if (GetComponent<PlayerInput>() == null)
			{
				Debug.LogError(name + " has no PlayerInput component attached.");
			}
			if (GetComponent<PlayerInteraction>() == null)
			{
				Debug.LogError(name + " has no PlayerInteraction component attached.");
			}
			if (GetComponent<PlayerMovement>() == null)
			{
				Debug.LogError(name + " has no PlayerMovement component attached.");
			}
			if (GetComponent<PlayerCursor>() == null)
			{
				Debug.LogError(name + " has no PlayerCursor component attached.");
			}
			if (GetComponent<SceneSettings>() == null)
			{
				Debug.LogError(name + " has no SceneSettings component attached.");
			}
			else
			{
				if (GetComponent<SceneSettings>().navigationMethod == AC_NavigationMethod.meshCollider && GetComponent<SceneSettings>().navMesh == null)
				{
					Debug.LogWarning("No NavMesh set.  Characters will not be able to PathFind until one is defined - please choose one using the Scene Manager.");
				}
				if (GetComponent<SceneSettings>().defaultPlayerStart == null)
				{
					Debug.LogWarning("No default PlayerStart set.  The game may not be able to begin if one is not defined - please choose one using the Scene Manager.");
				}
			}
			if (GetComponent<NavigationManager>() == null)
			{
				Debug.LogError(name + " has no NavigationManager component attached.");
			}
			if (GetComponent<ActionListManager>() == null)
			{
				Debug.LogError(name + " has no ActionListManager component attached.");
			}
		}

		private void OnLevelWasLoaded()
		{
			if ((bool)GameObject.FindWithTag("Player") && (bool)GameObject.FindWithTag("Player").GetComponent<Player>())
			{
				playerPrefab = GameObject.FindWithTag("Player").GetComponent<Player>();
			}
		}

		public void TurnOnAC()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
			{
				GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>().gameState = GameState.Normal;
			}
		}

		public void TurnOffAC()
		{
			GetComponent<ActionListManager>().KillAllLists();
			GetComponent<Dialog>().KillDialog(true);
			Moveable[] array = Object.FindObjectsOfType(typeof(Moveable)) as Moveable[];
			Moveable[] array2 = array;
			foreach (Moveable moveable in array2)
			{
				moveable.Kill();
			}
			Char[] array3 = Object.FindObjectsOfType(typeof(Char)) as Char[];
			Char[] array4 = array3;
			foreach (Char obj in array4)
			{
				obj.EndPath();
			}
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
			{
				GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>().gameState = GameState.Cutscene;
			}
		}
	}
}
