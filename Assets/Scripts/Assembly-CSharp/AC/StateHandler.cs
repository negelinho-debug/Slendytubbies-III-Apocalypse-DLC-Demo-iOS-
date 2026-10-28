using UnityEngine;

namespace AC
{
	public class StateHandler : MonoBehaviour
	{
		public GameState gameState;

		private GameState previousUpdateState;

		private GameState lastNonPausedState;

		public bool cursorIsOff;

		public bool inputIsOff;

		public bool interactionIsOff;

		public bool menuIsOff;

		public bool movementIsOff;

		public bool cameraIsOff;

		public bool triggerIsOff;

		public bool playerIsOff;

		public bool playedGlobalOnStart;

		public Texture2D tempFadeTexture;

		private Dialog dialog;

		private ArrowPrompt[] arrowPrompts;

		private DragBase[] dragBases;

		private Parallax2D[] parallax2Ds;

		private Hotspot[] hotspots;

		private PlayerInput playerInput;

		private PlayerCursor playerCursor;

		private PlayerInteraction playerInteraction;

		private PlayerMovement playerMovement;

		private PlayerMenus playerMenus;

		private SettingsManager settingsManager;

		private ActionListManager actionListManager;

		private void Awake()
		{
			Time.timeScale = 1f;
			Object.DontDestroyOnLoad(this);
			GetReferences();
		}

		private void OnLevelWasLoaded()
		{
			GetReferences();
		}

		public bool PlayGlobalOnStart()
		{
			if (playedGlobalOnStart)
			{
				return false;
			}
			if ((bool)settingsManager.actionListOnStart)
			{
				AdvGame.RunActionListAsset(settingsManager.actionListOnStart);
				playedGlobalOnStart = true;
				return true;
			}
			return false;
		}

		private void GetReferences()
		{
			settingsManager = AdvGame.GetReferences().settingsManager;
			if (!(settingsManager != null) || !settingsManager.IsInLoadingScene())
			{
				playerCursor = GameObject.FindWithTag("GameEngine").GetComponent<PlayerCursor>();
				playerInput = playerCursor.GetComponent<PlayerInput>();
				playerInteraction = playerCursor.GetComponent<PlayerInteraction>();
				playerMovement = playerCursor.GetComponent<PlayerMovement>();
				dialog = playerCursor.GetComponent<Dialog>();
				playerMenus = GetComponent<PlayerMenus>();
				actionListManager = playerCursor.GetComponent<ActionListManager>();
				dragBases = Object.FindObjectsOfType(typeof(DragBase)) as DragBase[];
				hotspots = Object.FindObjectsOfType(typeof(Hotspot)) as Hotspot[];
				arrowPrompts = Object.FindObjectsOfType(typeof(ArrowPrompt)) as ArrowPrompt[];
				parallax2Ds = Object.FindObjectsOfType(typeof(Parallax2D)) as Parallax2D[];
			}
		}

		private void Update()
		{
			if (settingsManager != null && settingsManager.IsInLoadingScene())
			{
				return;
			}
			if (gameState != GameState.Paused)
			{
				lastNonPausedState = gameState;
			}
			if (!inputIsOff)
			{
				playerInput.UpdateInput();
				if (gameState == GameState.Normal)
				{
					playerInput.UpdateDirectInput();
				}
				if (gameState != GameState.Paused)
				{
					dialog.UpdateSkipDialogue();
				}
			}
			if (!cursorIsOff)
			{
				playerCursor.UpdateCursor();
			}
			if (!menuIsOff)
			{
				playerMenus.UpdateAllMenus();
			}
			if (!interactionIsOff)
			{
				playerInteraction.UpdateInteraction();
			}
			actionListManager.UpdateActionListManager();
			if (!movementIsOff)
			{
				DragBase[] array = dragBases;
				foreach (DragBase dragBase in array)
				{
					dragBase.UpdateMovement();
				}
				if (gameState == GameState.Normal && (bool)settingsManager && settingsManager.movementMethod != MovementMethod.None)
				{
					playerMovement.UpdatePlayerMovement();
				}
			}
			if (!interactionIsOff)
			{
				playerInteraction.UpdateInventory();
			}
			if (HasGameStateChanged())
			{
				if (settingsManager.movementMethod == MovementMethod.UltimateFPS)
				{
					UltimateFPSIntegration._Update(gameState);
				}
				if (gameState != GameState.Paused)
				{
					AudioListener.pause = false;
				}
			}
			previousUpdateState = gameState;
		}

		private void LateUpdate()
		{
			Parallax2D[] array = parallax2Ds;
			foreach (Parallax2D parallax2D in array)
			{
				parallax2D.UpdateOffset();
			}
			if (!cameraIsOff)
			{
				KickStarter.mainCamera._LateUpdate();
			}
		}

		private bool HasGameStateChanged()
		{
			if (previousUpdateState != gameState)
			{
				return true;
			}
			return false;
		}

		private void OnGUI()
		{
			if (settingsManager != null && settingsManager.IsInLoadingScene())
			{
				return;
			}
			if (!cursorIsOff && gameState == GameState.Normal && (bool)settingsManager)
			{
				if (settingsManager.hotspotIconDisplay != HotspotIconDisplay.Never)
				{
					Hotspot[] array = hotspots;
					foreach (Hotspot hotspot in array)
					{
						hotspot.DrawHotspotIcon();
					}
				}
				DragBase[] array2 = dragBases;
				foreach (DragBase dragBase in array2)
				{
					dragBase.DrawGrabIcon();
				}
			}
			if (!inputIsOff)
			{
				playerInput.DrawDragLine();
				ArrowPrompt[] array3 = arrowPrompts;
				foreach (ArrowPrompt arrowPrompt in array3)
				{
					arrowPrompt.DrawArrows();
				}
			}
			if (!menuIsOff)
			{
				playerMenus.DrawMenus();
			}
			if (!cursorIsOff)
			{
				playerCursor.DrawCursor();
			}
			if (!cameraIsOff)
			{
				KickStarter.mainCamera.DrawCameraFade();
			}
		}

		public GameState GetLastNonPausedState()
		{
			return lastNonPausedState;
		}

		public void RestoreLastNonPausedState()
		{
			if (actionListManager.IsGameplayBlocked())
			{
				gameState = GameState.Cutscene;
			}
			else if (playerInput.activeConversation != null)
			{
				gameState = GameState.DialogOptions;
			}
			else
			{
				gameState = GameState.Normal;
			}
		}

		public void TurnOnAC()
		{
			gameState = GameState.Normal;
		}

		public void TurnOffAC()
		{
			if ((bool)GameObject.FindWithTag("GameEngine"))
			{
				if ((bool)GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>())
				{
					GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>().KillAllLists();
				}
				if ((bool)GameObject.FindWithTag("GameEngine").GetComponent<Dialog>())
				{
					GameObject.FindWithTag("GameEngine").GetComponent<Dialog>().KillDialog(true);
				}
			}
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
			gameState = GameState.Cutscene;
		}

		private void OnDestroy()
		{
			playerInput = null;
			playerCursor = null;
			playerMenus = null;
			playerInteraction = null;
			playerMovement = null;
			settingsManager = null;
			arrowPrompts = null;
			hotspots = null;
			actionListManager = null;
			dialog = null;
		}
	}
}
