using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class SettingsManager : ScriptableObject
	{
		public string saveFileName = string.Empty;

		public SaveTimeDisplay saveTimeDisplay;

		public bool takeSaveScreenshots;

		public ActionListAsset actionListOnStart;

		public bool blackOutWhenSkipping;

		public PlayerSwitching playerSwitching = PlayerSwitching.DoNotAllow;

		public Player player;

		public List<PlayerPrefab> players = new List<PlayerPrefab>();

		public bool shareInventory;

		public MovementMethod movementMethod;

		public InputMethod inputMethod;

		public AC_InteractionMethod interactionMethod;

		public CancelInteractions cancelInteractions;

		public SeeInteractions seeInteractions;

		public SelectInteractions selectInteractions;

		public bool cycleInventoryCursors = true;

		public bool autoCycleWhenInteract;

		public bool hideLockedCursor;

		public bool lockCursorOnStart;

		public bool disableFreeAimWhenDragging;

		public bool inventoryDragDrop;

		public bool inventoryDropLook;

		public bool useOuya;

		public InventoryInteractions inventoryInteractions = InventoryInteractions.Single;

		public InventoryActiveEffect inventoryActiveEffect = InventoryActiveEffect.Simple;

		public bool activeWhenHover;

		public bool inventoryDisableLeft = true;

		public float inventoryPulseSpeed = 1f;

		public bool activeWhenUnhandled = true;

		public bool canReorderItems;

		public bool hideSelectedFromMenu;

		public RightClickInventory rightClickInventory;

		public bool reverseInventoryCombinations;

		public bool canMoveWhenActive = true;

		public Transform clickPrefab;

		public DirectMovementType directMovementType;

		public LimitDirectMovement limitDirectMovement;

		public float destinationAccuracy = 0.9f;

		public float walkableClickRange = 0.5f;

		public DragAffects dragAffects;

		public float verticalReductionFactor = 0.7f;

		public bool doubleClickMovement;

		public float jumpSpeed = 4f;

		public bool singleTapStraight;

		public float freeAimTouchSpeed = 0.01f;

		public float dragWalkThreshold = 5f;

		public float dragRunThreshold = 20f;

		public bool drawDragLine;

		public float dragLineWidth = 3f;

		public Color dragLineColor = Color.white;

		public bool offsetTouchCursor;

		public bool doubleTapHotspots = true;

		public bool forceAspectRatio;

		public float wantedAspectRatio = 1.5f;

		public bool landscapeModeOnly = true;

		public CameraPerspective cameraPerspective = CameraPerspective.ThreeD;

		private int cameraPerspective_int;

		public MovingTurning movingTurning = MovingTurning.Unity2D;

		public HotspotDetection hotspotDetection;

		public HotspotsInVicinity hotspotsInVicinity;

		public HotspotIconDisplay hotspotIconDisplay;

		public HotspotIcon hotspotIcon;

		public Texture2D hotspotIconTexture;

		public float hotspotIconSize = 0.04f;

		public bool playerFacesHotspots;

		public float navMeshRaycastLength = 100f;

		public float hotspotRaycastLength = 100f;

		public float moveableRaycastLength = 30f;

		public string hotspotLayer = "Default";

		public string navMeshLayer = "NavMesh";

		public string backgroundImageLayer = "BackgroundImage";

		public string deactivatedLayer = "Ignore Raycast";

		public bool useLoadingScreen;

		public ChooseSceneBy loadingSceneIs;

		public string loadingSceneName = string.Empty;

		public int loadingScene;

		private string GetInputList()
		{
			string text = string.Empty;
			if (inputMethod == InputMethod.KeyboardOrController)
			{
				text += "\n";
				text += "- InteractionA (Button)";
				text += "\n";
				text += "- InteractionB (Button)";
				text += "\n";
				text += "- CursorHorizontal (Axis)";
				text += "\n";
				text += "- CursorVertical (Axis)";
			}
			if (movementMethod != MovementMethod.PointAndClick && movementMethod != MovementMethod.StraightToCursor)
			{
				text += "\n";
				text += "- ToggleCursor (Button)";
			}
			if (movementMethod == MovementMethod.Direct || movementMethod == MovementMethod.FirstPerson || inputMethod == InputMethod.KeyboardOrController)
			{
				if (inputMethod != InputMethod.TouchScreen)
				{
					text += "\n";
					text += "- Horizontal (Axis)";
					text += "\n";
					text += "- Vertical (Axis)";
					if (movementMethod == MovementMethod.Direct || movementMethod == MovementMethod.FirstPerson)
					{
						text += "\n";
						text += "- Run (Button)";
						text += "\n";
						text += "- ToggleRun (Button)";
						text += "\n";
						text += "- Jump (Button)";
					}
				}
				if (movementMethod == MovementMethod.FirstPerson && inputMethod == InputMethod.MouseAndKeyboard)
				{
					text += "\n";
					text += "- MouseScrollWheel (Axis)";
					text += "\n";
					text += "- CursorHorizontal (Axis)";
					text += "\n";
					text += "- CursorVertical (Axis)";
				}
				if ((movementMethod == MovementMethod.Direct || movementMethod == MovementMethod.FirstPerson) && hotspotDetection == HotspotDetection.PlayerVicinity && hotspotsInVicinity == HotspotsInVicinity.CycleMultiple)
				{
					text += "\n";
					text += "- CycleHotspotsLeft (Button)";
					text += "\n";
					text += "- CycleHotspotsRight (Button)";
					text += "\n";
					text += "- CycleHotspots (Axis)";
				}
			}
			if (SelectInteractionMethod() == SelectInteractions.CyclingMenuAndClickingHotspot)
			{
				text += "\n";
				text += "- CycleInteractionsLeft (Button)";
				text += "\n";
				text += "- CycleInteractionsRight (Button)";
				text += "\n";
				text += "- CycleInteractions (Axis)";
			}
			text += "\n";
			text += "- FlashHotspots (Button)";
			text += "\n";
			text += "- Menu (Button)";
			text += "\n";
			text += "- EndCutscene (Button)";
			text += "\n";
			text += "- ThrowMoveable (Button)";
			text += "\n";
			text += "- RotateMoveable (Button)";
			text += "\n";
			text += "- RotateMoveableToggle (Button)";
			text += "\n";
			return text + "- ZoomMoveable (Axis)";
		}

		public bool ActInScreenSpace()
		{
			if ((movingTurning == MovingTurning.ScreenSpace || movingTurning == MovingTurning.Unity2D) && cameraPerspective == CameraPerspective.TwoD)
			{
				return true;
			}
			return false;
		}

		public bool IsUnity2D()
		{
			if (movingTurning == MovingTurning.Unity2D && cameraPerspective == CameraPerspective.TwoD)
			{
				return true;
			}
			return false;
		}

		public bool IsTopDown()
		{
			if (movingTurning == MovingTurning.TopDown && cameraPerspective == CameraPerspective.TwoD)
			{
				return true;
			}
			return false;
		}

		public bool IsFirstPersonDragRotation()
		{
			if (movementMethod == MovementMethod.FirstPerson && inputMethod == InputMethod.TouchScreen && dragAffects == DragAffects.Rotation)
			{
				return true;
			}
			return false;
		}

		public bool IsFirstPersonDragMovement()
		{
			if (movementMethod == MovementMethod.FirstPerson && inputMethod == InputMethod.TouchScreen && dragAffects == DragAffects.Movement)
			{
				return true;
			}
			return false;
		}

		private int[] GetPlayerIDArray()
		{
			List<int> list = new List<int>();
			foreach (PlayerPrefab player in players)
			{
				list.Add(player.ID);
			}
			list.Sort();
			return list.ToArray();
		}

		public int GetDefaultPlayerID()
		{
			if (playerSwitching == PlayerSwitching.DoNotAllow)
			{
				return 0;
			}
			foreach (PlayerPrefab player in players)
			{
				if (player.isDefault)
				{
					return player.ID;
				}
			}
			return 0;
		}

		public bool CanClickOffInteractionMenu()
		{
			if (cancelInteractions == CancelInteractions.ClickOffMenu || inputMethod == InputMethod.TouchScreen)
			{
				return true;
			}
			return false;
		}

		public bool MouseOverForInteractionMenu()
		{
			if (seeInteractions == SeeInteractions.CursorOverHotspot && inputMethod != InputMethod.TouchScreen)
			{
				return true;
			}
			return false;
		}

		public Player GetDefaultPlayer()
		{
			if (playerSwitching == PlayerSwitching.DoNotAllow)
			{
				return player;
			}
			foreach (PlayerPrefab player in players)
			{
				if (player.isDefault)
				{
					if (player.playerOb != null)
					{
						return player.playerOb;
					}
					Debug.LogWarning("Default Player has no prefab!");
					return null;
				}
			}
			Debug.LogWarning("Cannot find default player!");
			return null;
		}

		private void SetDefaultPlayer(PlayerPrefab defaultPlayer)
		{
			foreach (PlayerPrefab player in players)
			{
				if (player == defaultPlayer)
				{
					player.isDefault = true;
				}
				else
				{
					player.isDefault = false;
				}
			}
		}

		private bool DoPlayerAnimEnginesMatch()
		{
			AnimationEngine animationEngine = AnimationEngine.Legacy;
			bool flag = false;
			foreach (PlayerPrefab player in players)
			{
				if (player.playerOb != null)
				{
					if (!flag)
					{
						flag = true;
						animationEngine = player.playerOb.animationEngine;
					}
					else if (player.playerOb.animationEngine != animationEngine)
					{
						return false;
					}
				}
			}
			return true;
		}

		public SelectInteractions SelectInteractionMethod()
		{
			if (inputMethod != InputMethod.TouchScreen && interactionMethod == AC_InteractionMethod.ChooseHotspotThenInteraction)
			{
				return selectInteractions;
			}
			return SelectInteractions.ClickingMenu;
		}

		public bool IsInLoadingScene()
		{
			if (useLoadingScreen && Application.loadedLevel == loadingScene && Application.loadedLevelName != string.Empty)
			{
				return true;
			}
			return false;
		}

		private bool IsOnOuya()
		{
			if (inputMethod != InputMethod.TouchScreen && useOuya)
			{
				return true;
			}
			return false;
		}

		public bool IsInFirstPerson()
		{
			if (movementMethod == MovementMethod.FirstPerson || movementMethod == MovementMethod.UltimateFPS)
			{
				return true;
			}
			return false;
		}
	}
}
