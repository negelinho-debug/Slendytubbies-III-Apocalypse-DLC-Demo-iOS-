using UnityEngine;

namespace AC
{
	public class PlayerCursor : MonoBehaviour
	{
		private int selectedCursor = -1;

		private bool showCursor;

		private bool canShowHardwareCursor;

		private float pulse;

		private int pulseDirection;

		private CursorIconBase activeIcon;

		private CursorIconBase activeLookIcon;

		private string lastCursorName;

		private SettingsManager settingsManager;

		private CursorManager cursorManager;

		private StateHandler stateHandler;

		private RuntimeInventory runtimeInventory;

		private PlayerInput playerInput;

		private PlayerMovement playerMovement;

		private PlayerInteraction playerInteraction;

		private PlayerMenus playerMenus;

		private void Awake()
		{
			playerInput = GetComponent<PlayerInput>();
			playerInteraction = GetComponent<PlayerInteraction>();
			playerMovement = GetComponent<PlayerMovement>();
			if (AdvGame.GetReferences() == null)
			{
				Debug.LogError("A References file is required - please use the Adventure Creator window to create one.");
				return;
			}
			if ((bool)AdvGame.GetReferences().settingsManager)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			if ((bool)AdvGame.GetReferences().cursorManager)
			{
				cursorManager = AdvGame.GetReferences().cursorManager;
			}
		}

		private void Start()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
			{
				stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
			}
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>())
			{
				runtimeInventory = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
			}
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>())
			{
				playerMenus = GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>();
			}
		}

		public void UpdateCursor()
		{
			if (cursorManager.cursorRendering == CursorRendering.Software)
			{
				if (!canShowHardwareCursor)
				{
					Cursor.visible = false;
				}
				else if (playerInput.dragState == DragState.Moveable)
				{
					Cursor.visible = false;
				}
				else if ((bool)settingsManager && (bool)cursorManager && (!cursorManager.allowMainCursor || cursorManager.pointerIcon.texture == null) && runtimeInventory.selectedItem == null && settingsManager.inputMethod == InputMethod.MouseAndKeyboard && stateHandler.gameState != GameState.Cutscene)
				{
					Cursor.visible = true;
				}
				else if (cursorManager == null)
				{
					Cursor.visible = true;
				}
				else
				{
					Cursor.visible = false;
				}
			}
			if ((bool)settingsManager && (bool)stateHandler)
			{
				if (stateHandler.gameState == GameState.Cutscene)
				{
					if (cursorManager.waitIcon.texture != null)
					{
						showCursor = true;
					}
					else
					{
						showCursor = false;
					}
				}
				else if (stateHandler.gameState != GameState.Normal && settingsManager.inputMethod == InputMethod.KeyboardOrController)
				{
					showCursor = false;
				}
				else if ((bool)cursorManager)
				{
					if (stateHandler.gameState == GameState.Paused && (cursorManager.cursorDisplay == CursorDisplay.OnlyWhenPaused || cursorManager.cursorDisplay == CursorDisplay.Always))
					{
						showCursor = true;
					}
					else if (playerInput.dragState == DragState.Moveable)
					{
						showCursor = false;
					}
					else if ((stateHandler.gameState == GameState.Normal || stateHandler.gameState == GameState.DialogOptions) && cursorManager.cursorDisplay == CursorDisplay.Always)
					{
						showCursor = true;
					}
					else
					{
						showCursor = false;
					}
				}
				else
				{
					showCursor = true;
				}
				if (stateHandler.gameState == GameState.Normal && settingsManager.interactionMethod == AC_InteractionMethod.ChooseInteractionThenHotspot && cursorManager != null && cursorManager.cycleCursors && (playerInput.mouseState == MouseState.RightClick || playerInput.InputGetButtonDown("CycleCursors")))
				{
					CycleCursors();
				}
				else if (stateHandler.gameState == GameState.Normal && settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot && (playerInput.mouseState == MouseState.RightClick || playerInput.InputGetButtonDown("CycleCursors")))
				{
					playerInteraction.SetNextInteraction();
				}
			}
			if (cursorManager.cursorRendering == CursorRendering.Hardware)
			{
				Cursor.visible = showCursor;
			}
		}

		public void DrawCursor()
		{
			if (!playerInput || !playerInteraction || !stateHandler || !settingsManager || !cursorManager || !runtimeInventory || !showCursor)
			{
				return;
			}
			if (playerInput.IsCursorLocked() && settingsManager.hideLockedCursor)
			{
				canShowHardwareCursor = false;
				return;
			}
			GUI.depth = -1;
			canShowHardwareCursor = true;
			if (runtimeInventory.selectedItem != null && cursorManager.inventoryHandling != InventoryHandling.ChangeHotspotLabel)
			{
				selectedCursor = -2;
				canShowHardwareCursor = false;
			}
			else if (settingsManager.interactionMethod != AC_InteractionMethod.ChooseInteractionThenHotspot)
			{
				if (settingsManager.interactionMethod == AC_InteractionMethod.ContextSensitive && cursorManager.allowInteractionCursorForInventory && runtimeInventory.hoverItem != null)
				{
					ShowContextIcons(runtimeInventory.hoverItem);
					return;
				}
				if (playerInteraction.GetActiveHotspot() != null && stateHandler.gameState == GameState.Normal && (playerInteraction.GetActiveHotspot().HasContextUse() || playerInteraction.GetActiveHotspot().HasContextLook()))
				{
					selectedCursor = 0;
					if (settingsManager.interactionMethod == AC_InteractionMethod.ContextSensitive)
					{
						if (cursorManager.allowInteractionCursor)
						{
							canShowHardwareCursor = false;
							ShowContextIcons();
						}
						else if (cursorManager.mouseOverIcon.texture != null)
						{
							DrawIcon(cursorManager.mouseOverIcon, false);
						}
					}
				}
				else
				{
					selectedCursor = -1;
				}
			}
			else if (settingsManager.interactionMethod == AC_InteractionMethod.ChooseInteractionThenHotspot && (stateHandler.gameState == GameState.DialogOptions || stateHandler.gameState == GameState.Paused))
			{
				selectedCursor = -1;
			}
			if (stateHandler.gameState == GameState.Cutscene && cursorManager.waitIcon.texture != null)
			{
				DrawIcon(cursorManager.waitIcon, false);
			}
			else if (selectedCursor == -2 && runtimeInventory.selectedItem != null && (settingsManager.interactionMethod != AC_InteractionMethod.ChooseHotspotThenInteraction || settingsManager.inventoryInteractions == InventoryInteractions.Single))
			{
				canShowHardwareCursor = false;
				if (settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot && settingsManager.cycleInventoryCursors && playerInteraction.GetActiveHotspot() == null && runtimeInventory.hoverItem == null && playerInteraction.GetInteractionIndex() >= 0)
				{
					playerInteraction.ResetInteractionIndex();
					runtimeInventory.SetNull();
					return;
				}
				if (settingsManager.inventoryActiveEffect != InventoryActiveEffect.None && (bool)runtimeInventory.selectedItem.activeTex && playerMenus.GetHotspotLabel() != string.Empty && (settingsManager.activeWhenUnhandled || playerInteraction.DoesHotspotHaveInventoryInteraction() || (runtimeInventory.hoverItem != null && runtimeInventory.hoverItem.DoesHaveInventoryInteraction(runtimeInventory.selectedItem))))
				{
					DrawActiveInventoryCursor();
				}
				else if ((bool)runtimeInventory.selectedItem.tex)
				{
					DrawIcon(AdvGame.GUIBox(playerInput.GetMousePosition(), cursorManager.inventoryCursorSize), runtimeInventory.selectedItem.tex);
					pulseDirection = 0;
				}
				else
				{
					selectedCursor = -1;
					runtimeInventory.SetNull();
					pulseDirection = 0;
				}
				if (runtimeInventory.selectedItem != null && runtimeInventory.selectedItem.canCarryMultiple)
				{
					runtimeInventory.DrawInventoryCount(playerInput.GetMousePosition(), cursorManager.inventoryCursorSize, runtimeInventory.selectedItem.count);
				}
			}
			else if (selectedCursor >= 0 && settingsManager.interactionMethod == AC_InteractionMethod.ChooseInteractionThenHotspot && cursorManager.allowInteractionCursor)
			{
				pulseDirection = 0;
				canShowHardwareCursor = false;
				DrawIcon(cursorManager.cursorIcons[selectedCursor], false);
			}
			else if (settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot)
			{
				ShowCycleCursor(playerInteraction.GetActiveUseButtonIconID(), playerInteraction.GetActiveInvButtonID());
			}
			else
			{
				if (!cursorManager.allowMainCursor && settingsManager.inputMethod != InputMethod.KeyboardOrController)
				{
					return;
				}
				pulseDirection = 0;
				if (settingsManager.interactionMethod == AC_InteractionMethod.ChooseHotspotThenInteraction && runtimeInventory.hoverItem == null && playerInteraction.GetActiveHotspot() != null && (!playerInput.interactionMenuIsOn || settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingMenuAndClickingHotspot))
				{
					if (playerInteraction.GetActiveHotspot().IsSingleInteraction())
					{
						ShowContextIcons();
					}
					else if (cursorManager.mouseOverIcon.texture != null)
					{
						DrawIcon(cursorManager.mouseOverIcon, false);
					}
					else
					{
						DrawMainCursor();
					}
				}
				else if (selectedCursor == -1 || settingsManager.interactionMethod == AC_InteractionMethod.ChooseHotspotThenInteraction)
				{
					DrawMainCursor();
				}
				else if (selectedCursor == -2 && runtimeInventory.selectedItem == null)
				{
					selectedCursor = -1;
				}
			}
		}

		private void DrawMainCursor()
		{
			bool flag = false;
			if ((bool)cursorManager.walkIcon.texture && (bool)playerInput && !playerInput.mouseOverMenu && !playerInput.interactionMenuIsOn && (bool)stateHandler && stateHandler.gameState == GameState.Normal)
			{
				if (cursorManager.onlyWalkWhenOverNavMesh)
				{
					if (playerMovement.ClickPoint(playerInput.GetMousePosition()) != Vector3.zero)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				DrawIcon(cursorManager.walkIcon, false);
			}
			else if ((bool)cursorManager.pointerIcon.texture)
			{
				DrawIcon(cursorManager.pointerIcon, false);
			}
			else
			{
				Debug.LogWarning("No 'main' texture defined - please set in SettingsManager.");
			}
		}

		private void ShowContextIcons()
		{
			Hotspot activeHotspot = playerInteraction.GetActiveHotspot();
			if (activeHotspot == null)
			{
				return;
			}
			if (activeHotspot.HasContextUse())
			{
				if (!activeHotspot.HasContextLook())
				{
					DrawIcon(cursorManager.GetCursorIconFromID(activeHotspot.GetFirstUseButton().iconID), false);
					return;
				}
				Button firstUseButton = activeHotspot.GetFirstUseButton();
				if (activeHotspot.HasContextUse() && activeHotspot.HasContextLook() && cursorManager.lookUseCursorAction == LookUseCursorAction.DisplayBothSideBySide)
				{
					CursorIcon cursorIconFromID = cursorManager.GetCursorIconFromID(firstUseButton.iconID);
					DrawIcon(new Vector2((0f - cursorIconFromID.size) * (float)Screen.width / 2f, 0f), cursorIconFromID, false);
				}
				else
				{
					DrawIcon(cursorManager.GetCursorIconFromID(firstUseButton.iconID), false);
				}
			}
			if (activeHotspot.HasContextLook() && (!activeHotspot.HasContextUse() || (activeHotspot.HasContextUse() && cursorManager.lookUseCursorAction == LookUseCursorAction.DisplayBothSideBySide)) && cursorManager.cursorIcons.Count > 0)
			{
				CursorIcon cursorIconFromID2 = cursorManager.GetCursorIconFromID(cursorManager.lookCursor_ID);
				if (activeHotspot.HasContextUse() && activeHotspot.HasContextLook() && cursorManager.lookUseCursorAction == LookUseCursorAction.DisplayBothSideBySide)
				{
					DrawIcon(new Vector2(cursorIconFromID2.size * (float)Screen.width / 2f, 0f), cursorIconFromID2, true);
				}
				else
				{
					DrawIcon(cursorIconFromID2, true);
				}
			}
		}

		private void ShowContextIcons(InvItem invItem)
		{
			if (cursorManager.cursorIcons.Count <= 0)
			{
				return;
			}
			if (invItem.lookActionList != null && cursorManager.lookUseCursorAction == LookUseCursorAction.DisplayBothSideBySide)
			{
				CursorIcon cursorIconFromID = cursorManager.GetCursorIconFromID(invItem.useIconID);
				DrawIcon(new Vector2((0f - cursorIconFromID.size) * (float)Screen.width / 2f, 0f), cursorIconFromID, false);
			}
			else
			{
				DrawIcon(cursorManager.GetCursorIconFromID(invItem.useIconID), false);
			}
			if (invItem.lookActionList != null)
			{
				CursorIcon cursorIconFromID2 = cursorManager.GetCursorIconFromID(cursorManager.lookCursor_ID);
				if (invItem.lookActionList != null && cursorManager.lookUseCursorAction == LookUseCursorAction.DisplayBothSideBySide)
				{
					DrawIcon(new Vector2(cursorIconFromID2.size * (float)Screen.width / 2f, 0f), cursorIconFromID2, true);
				}
				else
				{
					DrawIcon(cursorIconFromID2, true);
				}
			}
		}

		private void ShowCycleCursor(int useCursorID, int invCursorID)
		{
			if (invCursorID >= 0)
			{
				selectedCursor = -2;
				DrawActiveInventoryCursor();
			}
			else if (useCursorID >= 0)
			{
				selectedCursor = useCursorID;
				DrawIcon(cursorManager.GetCursorIconFromID(useCursorID), false);
			}
			else if (useCursorID == -1)
			{
				selectedCursor = -1;
				DrawMainCursor();
			}
		}

		private void DrawActiveInventoryCursor()
		{
			if (runtimeInventory.selectedItem == null)
			{
				return;
			}
			if (runtimeInventory.selectedItem.activeTex == null)
			{
				DrawIcon(AdvGame.GUIBox(playerInput.GetMousePosition(), cursorManager.inventoryCursorSize), runtimeInventory.selectedItem.tex);
				pulseDirection = 0;
			}
			if (settingsManager.inventoryActiveEffect == InventoryActiveEffect.Simple)
			{
				DrawIcon(AdvGame.GUIBox(playerInput.GetMousePosition(), cursorManager.inventoryCursorSize), runtimeInventory.selectedItem.activeTex);
			}
			else if (settingsManager.inventoryActiveEffect == InventoryActiveEffect.Pulse && (bool)runtimeInventory.selectedItem.tex)
			{
				if (pulseDirection == 0)
				{
					pulse = 0f;
					pulseDirection = 1;
				}
				else if (pulse > 1f)
				{
					pulse = 1f;
					pulseDirection = -1;
				}
				else if (pulse < 0f)
				{
					pulse = 0f;
					pulseDirection = 1;
				}
				else if (pulseDirection == 1)
				{
					pulse += settingsManager.inventoryPulseSpeed * Time.deltaTime;
				}
				else if (pulseDirection == -1)
				{
					pulse -= settingsManager.inventoryPulseSpeed * Time.deltaTime;
				}
				Color color = GUI.color;
				Color color2 = GUI.color;
				color2.a = pulse;
				GUI.color = color2;
				DrawIcon(AdvGame.GUIBox(playerInput.GetMousePosition(), cursorManager.inventoryCursorSize), runtimeInventory.selectedItem.activeTex);
				GUI.color = color;
				DrawIcon(AdvGame.GUIBox(playerInput.GetMousePosition(), cursorManager.inventoryCursorSize), runtimeInventory.selectedItem.tex);
			}
		}

		private void DrawIcon(Rect _rect, Texture2D _tex)
		{
			if (_tex != null)
			{
				if (cursorManager.cursorRendering == CursorRendering.Hardware)
				{
					Cursor.SetCursor(_tex, Vector2.zero, CursorMode.Auto);
				}
				else
				{
					GUI.DrawTexture(_rect, _tex, ScaleMode.ScaleToFit, true, 0f);
				}
			}
		}

		private void DrawIcon(Vector2 offset, CursorIconBase icon, bool isLook)
		{
			if (icon == null)
			{
				return;
			}
			bool flag = false;
			if (isLook && activeLookIcon != icon)
			{
				activeLookIcon = icon;
				icon.Reset();
			}
			else if (!isLook && activeIcon != icon)
			{
				activeIcon = icon;
				flag = true;
				icon.Reset();
			}
			if (cursorManager.cursorRendering == CursorRendering.Hardware)
			{
				if (icon.isAnimated)
				{
					Texture2D animatedTexture = icon.GetAnimatedTexture();
					if (icon.GetName() != lastCursorName)
					{
						lastCursorName = icon.GetName();
						Cursor.SetCursor(animatedTexture, icon.clickOffset, CursorMode.Auto);
					}
				}
				else if (flag)
				{
					Cursor.SetCursor(icon.texture, icon.clickOffset, CursorMode.Auto);
				}
			}
			else
			{
				icon.Draw(playerInput.GetMousePosition() + offset);
			}
		}

		private void DrawIcon(CursorIconBase icon, bool isLook)
		{
			if (icon != null)
			{
				DrawIcon(new Vector2(0f, 0f), icon, isLook);
			}
		}

		private void CycleCursors()
		{
			if (cursorManager.cursorIcons.Count > 0)
			{
				selectedCursor++;
				if (selectedCursor >= cursorManager.cursorIcons.Count)
				{
					selectedCursor = -1;
				}
			}
			else
			{
				selectedCursor = -1;
			}
		}

		public int GetSelectedCursor()
		{
			return selectedCursor;
		}

		public int GetSelectedCursorID()
		{
			if ((bool)cursorManager && cursorManager.cursorIcons.Count > 0 && selectedCursor > -1)
			{
				return cursorManager.cursorIcons[selectedCursor].id;
			}
			return -1;
		}

		public void ResetSelectedCursor()
		{
			selectedCursor = -1;
		}

		public void SetCursorFromID(int ID)
		{
			if (!cursorManager || cursorManager.cursorIcons.Count <= 0)
			{
				return;
			}
			foreach (CursorIcon cursorIcon in cursorManager.cursorIcons)
			{
				if (cursorIcon.id == ID)
				{
					selectedCursor = cursorManager.cursorIcons.IndexOf(cursorIcon);
				}
			}
		}

		private void OnDestroy()
		{
			stateHandler = null;
			runtimeInventory = null;
			playerInput = null;
			playerInteraction = null;
			playerMenus = null;
		}
	}
}
