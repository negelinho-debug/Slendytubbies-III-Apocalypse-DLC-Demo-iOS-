using System.Collections;
using UnityEngine;

namespace AC
{
	public class PlayerInteraction : MonoBehaviour
	{
		private Hotspot hotspot;

		private Button button;

		private int interactionIndex = -1;

		private PlayerInput playerInput;

		private PlayerMenus playerMenus;

		private PlayerCursor playerCursor;

		private StateHandler stateHandler;

		private RuntimeInventory runtimeInventory;

		private SettingsManager settingsManager;

		private CursorManager cursorManager;

		private void Awake()
		{
			playerInput = GetComponent<PlayerInput>();
			playerCursor = GetComponent<PlayerCursor>();
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
			if ((bool)GameObject.FindWithTag("PersistentEngine"))
			{
				if ((bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
				{
					stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
				}
				if ((bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
				{
					runtimeInventory = stateHandler.GetComponent<RuntimeInventory>();
				}
				if ((bool)GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>())
				{
					playerMenus = GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>();
				}
			}
		}

		public void UpdateInteraction()
		{
			if (!stateHandler || !playerInput || !settingsManager || !runtimeInventory || stateHandler.gameState != GameState.Normal)
			{
				return;
			}
			if (playerInput.dragState == DragState.Moveable)
			{
				DisableHotspot(true);
				return;
			}
			if (playerInput.mouseState == MouseState.RightClick && runtimeInventory.selectedItem != null && !playerInput.mouseOverMenu && (settingsManager.interactionMethod == AC_InteractionMethod.ContextSensitive || settingsManager.inventoryInteractions == InventoryInteractions.Single) && (settingsManager.interactionMethod != AC_InteractionMethod.ChooseHotspotThenInteraction || settingsManager.selectInteractions != SelectInteractions.CyclingCursorAndClickingHotspot || !settingsManager.cycleInventoryCursors))
			{
				if (settingsManager.rightClickInventory == RightClickInventory.ExaminesItem)
				{
					playerInput.mouseState = MouseState.Normal;
					runtimeInventory.Look(runtimeInventory.selectedItem);
				}
				else if (settingsManager.rightClickInventory == RightClickInventory.DeselectsItem)
				{
					playerInput.mouseState = MouseState.Normal;
					runtimeInventory.SetNull();
				}
			}
			if (playerInput.IsCursorLocked() && settingsManager.hideLockedCursor)
			{
				DisableHotspot(true);
			}
			else
			{
				if (!playerInput.IsCursorReadable())
				{
					return;
				}
				if (!playerInput.mouseOverMenu && (bool)Camera.main && !playerInput.ActiveArrowsDisablingHotspots())
				{
					if (settingsManager.interactionMethod == AC_InteractionMethod.ChooseHotspotThenInteraction)
					{
						if (settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot)
						{
							ContextSensitiveClick();
						}
						else if (!playerInput.interactionMenuIsOn || settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingMenuAndClickingHotspot)
						{
							ChooseHotspotThenInteractionClick();
						}
					}
					else
					{
						ContextSensitiveClick();
					}
				}
				else
				{
					DisableHotspot(false);
				}
				if (settingsManager.playerFacesHotspots && KickStarter.player != null)
				{
					if ((bool)hotspot)
					{
						KickStarter.player.SetHeadTurnTarget(hotspot.GetIconPosition(), false, HeadFacing.Hotspot);
					}
					else if (button == null)
					{
						KickStarter.player.ClearHeadTurnTarget(HeadFacing.Hotspot, false);
					}
				}
			}
		}

		public void UpdateInventory()
		{
			if (hotspot == null && button == null && IsDroppingInventory())
			{
				runtimeInventory.SetNull();
			}
		}

		private Hotspot CheckForHotspots()
		{
			if ((bool)settingsManager)
			{
				if ((bool)playerInput && settingsManager.inventoryDragDrop && playerInput.GetMousePosition() == Vector2.zero)
				{
					return null;
				}
				if (settingsManager.hotspotDetection == HotspotDetection.PlayerVicinity && (bool)KickStarter.player.hotspotDetector)
				{
					if (settingsManager.movementMethod == MovementMethod.Direct || settingsManager.IsInFirstPerson())
					{
						if (settingsManager.hotspotsInVicinity != HotspotsInVicinity.ShowAll)
						{
							return KickStarter.player.hotspotDetector.GetSelected();
						}
						KickStarter.player.hotspotDetector.HighlightAll();
					}
					else
					{
						KickStarter.player.hotspotDetector.HighlightAll();
					}
				}
				if ((bool)settingsManager && settingsManager.IsUnity2D())
				{
					RaycastHit2D raycastHit2D;
					if (Camera.main.orthographic)
					{
						raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(playerInput.GetMousePosition()), Vector2.zero, settingsManager.navMeshRaycastLength, 1 << LayerMask.NameToLayer(settingsManager.hotspotLayer));
					}
					else
					{
						Vector3 position = playerInput.GetMousePosition();
						position.z = 0f - Camera.main.transform.position.z;
						raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero, settingsManager.navMeshRaycastLength, 1 << LayerMask.NameToLayer(settingsManager.hotspotLayer));
					}
					if (raycastHit2D.collider != null && (bool)raycastHit2D.collider.gameObject.GetComponent<Hotspot>())
					{
						Hotspot component = raycastHit2D.collider.gameObject.GetComponent<Hotspot>();
						if (settingsManager.hotspotDetection == HotspotDetection.MouseOver)
						{
							return component;
						}
						if (settingsManager.hotspotDetection == HotspotDetection.PlayerVicinity && (bool)KickStarter.player.hotspotDetector && KickStarter.player.hotspotDetector.IsHotspotInTrigger(component))
						{
							return component;
						}
					}
				}
				else
				{
					Ray ray = Camera.main.ScreenPointToRay(playerInput.GetMousePosition());
					RaycastHit hitInfo;
					if (Physics.Raycast(ray, out hitInfo, settingsManager.hotspotRaycastLength, 1 << LayerMask.NameToLayer(settingsManager.hotspotLayer)) && (bool)hitInfo.collider.gameObject.GetComponent<Hotspot>())
					{
						Hotspot component2 = hitInfo.collider.gameObject.GetComponent<Hotspot>();
						if (settingsManager.hotspotDetection == HotspotDetection.MouseOver)
						{
							return component2;
						}
						if (settingsManager.hotspotDetection == HotspotDetection.PlayerVicinity && (bool)KickStarter.player.hotspotDetector && KickStarter.player.hotspotDetector.IsHotspotInTrigger(component2))
						{
							return component2;
						}
					}
				}
			}
			return null;
		}

		private bool CanDoDoubleTap()
		{
			if (runtimeInventory.selectedItem != null && settingsManager.inventoryDragDrop)
			{
				return false;
			}
			if (settingsManager.inputMethod == InputMethod.TouchScreen && settingsManager.doubleTapHotspots)
			{
				return true;
			}
			return false;
		}

		private void ChooseHotspotThenInteractionClick()
		{
			if (CanDoDoubleTap())
			{
				if (playerInput.mouseState == MouseState.SingleClick)
				{
					ChooseHotspotThenInteractionClick_Process(true);
				}
			}
			else
			{
				ChooseHotspotThenInteractionClick_Process(false);
			}
		}

		private void ChooseHotspotThenInteractionClick_Process(bool doubleTap)
		{
			Hotspot hotspot = CheckForHotspots();
			if (this.hotspot != null && hotspot == null)
			{
				DisableHotspot(false);
			}
			else
			{
				if (!(hotspot != null))
				{
					return;
				}
				if (hotspot.IsSingleInteraction())
				{
					ContextSensitiveClick();
					return;
				}
				if (playerInput.mouseState == MouseState.HeldDown && playerInput.dragState == DragState.Player)
				{
					DisableHotspot(false);
					return;
				}
				bool flag = false;
				if (hotspot != this.hotspot)
				{
					flag = true;
					if ((bool)this.hotspot)
					{
						this.hotspot.Deselect();
						playerMenus.DisableHotspotMenus();
					}
					this.hotspot = hotspot;
					this.hotspot.Select();
					playerMenus.SetInteractionMenus(false);
				}
				if (!this.hotspot)
				{
					return;
				}
				if (playerInput.mouseState == MouseState.SingleClick || (settingsManager.inventoryDragDrop && IsDroppingInventory()) || (settingsManager.MouseOverForInteractionMenu() && runtimeInventory.hoverItem == null && flag && !IsDroppingInventory()))
				{
					if (runtimeInventory.selectedItem != null && settingsManager.inventoryInteractions == InventoryInteractions.Single)
					{
						if (settingsManager.inventoryDragDrop || !flag || !doubleTap)
						{
							HandleInteraction();
						}
					}
					else
					{
						if (!playerMenus)
						{
							return;
						}
						if (playerInput.interactionMenuIsOn && settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingMenuAndClickingHotspot)
						{
							ClickHotspotToInteract();
						}
						else if (!flag || !doubleTap)
						{
							playerMenus.SetInteractionMenus(true);
							if (settingsManager.seeInteractions == SeeInteractions.ClickOnHotspot)
							{
								playerInput.hotspotMovingTo = null;
								StopInteraction();
								runtimeInventory.SetNull();
							}
						}
					}
				}
				else if (playerInput.mouseState == MouseState.RightClick)
				{
					this.hotspot.Deselect();
				}
			}
		}

		private void ContextSensitiveClick()
		{
			if (CanDoDoubleTap())
			{
				if (playerInput.mouseState == MouseState.SingleClick)
				{
					ContextSensitiveClick_Process(true, CheckForHotspots());
				}
				else if (playerInput.mouseState == MouseState.RightClick)
				{
					HandleInteraction();
				}
				return;
			}
			ContextSensitiveClick_Process(false, CheckForHotspots());
			if (playerInput.mouseOverMenu || !hotspot || (playerInput.mouseState != MouseState.SingleClick && playerInput.mouseState != MouseState.DoubleClick && playerInput.mouseState != MouseState.RightClick && !IsDroppingInventory()))
			{
				return;
			}
			if (settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot && (runtimeInventory.selectedItem == null || (runtimeInventory.selectedItem != null && settingsManager.cycleInventoryCursors)))
			{
				if (playerInput.mouseState != MouseState.RightClick)
				{
					ClickHotspotToInteract();
				}
			}
			else
			{
				HandleInteraction();
			}
		}

		private void ContextSensitiveClick_Process(bool doubleTap, Hotspot newHotspot)
		{
			if (hotspot != null && newHotspot == null)
			{
				DisableHotspot(false);
			}
			else
			{
				if (!(newHotspot != null))
				{
					return;
				}
				if (playerInput.mouseState == MouseState.HeldDown && playerInput.dragState == DragState.Player)
				{
					DisableHotspot(false);
				}
				else if (newHotspot != hotspot)
				{
					DisableHotspot(false);
					hotspot = newHotspot;
					hotspot.Select();
					if (settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot)
					{
						runtimeInventory.MatchInteractions();
						if (settingsManager.autoCycleWhenInteract)
						{
							RestoreHotspotInteraction();
						}
						else
						{
							ResetInteractionIndex();
						}
					}
				}
				else if (hotspot != null && doubleTap)
				{
					HandleInteraction();
				}
			}
		}

		public void DisableHotspot(bool isInstant)
		{
			if ((bool)hotspot)
			{
				if (isInstant)
				{
					hotspot.DeselectInstant();
				}
				else
				{
					hotspot.Deselect();
				}
				hotspot = null;
			}
		}

		public bool DoesHotspotHaveInventoryInteraction()
		{
			if ((bool)hotspot && (bool)runtimeInventory && runtimeInventory.selectedItem != null)
			{
				foreach (Button invButton in hotspot.invButtons)
				{
					if (invButton.invID == runtimeInventory.selectedItem.id && !invButton.isDisabled)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void HandleInteraction()
		{
			if (!hotspot)
			{
				return;
			}
			if (settingsManager == null || settingsManager.interactionMethod == AC_InteractionMethod.ContextSensitive)
			{
				if (playerInput.mouseState == MouseState.SingleClick || playerInput.mouseState == MouseState.DoubleClick)
				{
					if (runtimeInventory.selectedItem == null && hotspot.HasContextUse())
					{
						ClickButton(InteractionType.Use, -1, -1);
					}
					else if (runtimeInventory.selectedItem != null)
					{
						ClickButton(InteractionType.Inventory, -1, runtimeInventory.selectedItem.id);
						if (settingsManager.inventoryDisableLeft)
						{
							runtimeInventory.SetNull();
						}
					}
					else if (hotspot.HasContextLook() && cursorManager.leftClickExamine)
					{
						ClickButton(InteractionType.Examine, -1, -1);
					}
				}
				else if (playerInput.mouseState == MouseState.RightClick)
				{
					if (hotspot.HasContextLook() && runtimeInventory.selectedItem == null)
					{
						ClickButton(InteractionType.Examine, -1, -1);
					}
				}
				else if (settingsManager.inventoryDragDrop && IsDroppingInventory())
				{
					ClickButton(InteractionType.Inventory, -1, runtimeInventory.selectedItem.id);
				}
			}
			else if (settingsManager.interactionMethod == AC_InteractionMethod.ChooseInteractionThenHotspot && (bool)playerCursor && (bool)cursorManager)
			{
				if (playerInput.mouseState == MouseState.SingleClick)
				{
					if (runtimeInventory.selectedItem == null && hotspot.provideUseInteraction && playerCursor.GetSelectedCursor() >= 0)
					{
						ClickButton(InteractionType.Use, cursorManager.cursorIcons[playerCursor.GetSelectedCursor()].id, -1);
					}
					else if (runtimeInventory.selectedItem != null && playerCursor.GetSelectedCursor() == -2)
					{
						playerCursor.ResetSelectedCursor();
						ClickButton(InteractionType.Inventory, -1, runtimeInventory.selectedItem.id);
						if (settingsManager.inventoryDisableLeft)
						{
							runtimeInventory.SetNull();
						}
					}
				}
				else if (settingsManager.inventoryDragDrop && IsDroppingInventory())
				{
					ClickButton(InteractionType.Inventory, -1, runtimeInventory.selectedItem.id);
				}
			}
			else
			{
				if (settingsManager.interactionMethod != AC_InteractionMethod.ChooseHotspotThenInteraction)
				{
					return;
				}
				if (runtimeInventory.selectedItem != null && hotspot.provideUseInteraction && settingsManager.inventoryInteractions == InventoryInteractions.Single)
				{
					ClickButton(InteractionType.Inventory, -1, runtimeInventory.selectedItem.id);
				}
				else if (runtimeInventory.selectedItem == null && hotspot.IsSingleInteraction())
				{
					ClickButton(InteractionType.Use, -1, -1);
					if (settingsManager.inventoryDisableLeft)
					{
						runtimeInventory.SetNull();
					}
				}
			}
		}

		public void ClickButton(InteractionType _interactionType, int selectedCursorID, int selectedItemID)
		{
			StopCoroutine("UseObject");
			if ((bool)KickStarter.player)
			{
				KickStarter.player.EndPath();
			}
			if (settingsManager.interactionMethod == AC_InteractionMethod.ChooseHotspotThenInteraction && settingsManager.selectInteractions == SelectInteractions.CyclingCursorAndClickingHotspot)
			{
				if (settingsManager.autoCycleWhenInteract)
				{
					SetNextInteraction();
				}
				else
				{
					ResetInteractionIndex();
				}
				hotspot.lastInteractionIndex = interactionIndex;
			}
			else if (settingsManager.interactionMethod == AC_InteractionMethod.ChooseInteractionThenHotspot && settingsManager.autoCycleWhenInteract)
			{
				playerCursor.ResetSelectedCursor();
			}
			playerInput.mouseState = MouseState.Normal;
			playerInput.ResetClick();
			button = null;
			switch (_interactionType)
			{
			case InteractionType.Use:
				if (selectedCursorID == -1)
				{
					button = hotspot.GetFirstUseButton();
					break;
				}
				foreach (Button useButton in hotspot.useButtons)
				{
					if (useButton.iconID == selectedCursorID)
					{
						button = useButton;
						break;
					}
				}
				if (button != null || settingsManager.interactionMethod != AC_InteractionMethod.ChooseInteractionThenHotspot)
				{
					break;
				}
				AdvGame.RunActionListAsset(cursorManager.GetUnhandledInteraction(selectedCursorID));
				runtimeInventory.SetNull();
				KickStarter.player.ClearHeadTurnTarget(HeadFacing.Hotspot, false);
				return;
			case InteractionType.Examine:
				button = hotspot.lookButton;
				break;
			case InteractionType.Inventory:
				if (selectedItemID < 0)
				{
					break;
				}
				foreach (Button invButton in hotspot.invButtons)
				{
					if (invButton.invID == selectedItemID)
					{
						button = invButton;
						break;
					}
				}
				break;
			}
			if (button != null && button.isDisabled)
			{
				button = null;
				if (_interactionType != InteractionType.Inventory)
				{
					KickStarter.player.ClearHeadTurnTarget(HeadFacing.Hotspot, false);
					return;
				}
			}
			StartCoroutine("UseObject", selectedItemID);
		}

		private IEnumerator UseObject(int selectedItemID)
		{
			bool doRun = false;
			if (playerInput.hotspotMovingTo == hotspot)
			{
				doRun = true;
			}
			if (playerInput != null && playerInput.runLock == PlayerMoveLock.AlwaysWalk)
			{
				doRun = false;
			}
			if ((bool)KickStarter.player)
			{
				if (button != null && !button.isBlocking && (button.playerAction == PlayerAction.WalkToMarker || button.playerAction == PlayerAction.WalkTo) && settingsManager.movementMethod != MovementMethod.UltimateFPS)
				{
					stateHandler.gameState = GameState.Normal;
					playerInput.hotspotMovingTo = hotspot;
				}
				else
				{
					stateHandler.gameState = GameState.Cutscene;
					playerInput.hotspotMovingTo = null;
				}
			}
			Hotspot _hotspot = hotspot;
			hotspot.Deselect();
			hotspot = null;
			if ((bool)KickStarter.player)
			{
				if (button != null && button.playerAction != PlayerAction.DoNothing)
				{
					Vector3 lookVector = Vector3.zero;
					Vector3 targetPos = _hotspot.transform.position;
					if (settingsManager.ActInScreenSpace())
					{
						lookVector = AdvGame.GetScreenDirection(KickStarter.player.transform.position, _hotspot.transform.position);
					}
					else
					{
						lookVector = targetPos - KickStarter.player.transform.position;
						lookVector.y = 0f;
					}
					KickStarter.player.SetLookDirection(lookVector, false);
					if (button.playerAction == PlayerAction.TurnToFace)
					{
						while (KickStarter.player.IsTurning())
						{
							yield return new WaitForFixedUpdate();
						}
					}
					if (button.playerAction == PlayerAction.WalkToMarker && (bool)_hotspot.walkToMarker)
					{
						if (Vector3.Distance(KickStarter.player.transform.position, _hotspot.walkToMarker.transform.position) > 1.05f - settingsManager.destinationAccuracy)
						{
							if ((bool)GetComponent<NavigationManager>())
							{
								Vector3 vector = _hotspot.walkToMarker.transform.position;
								if (settingsManager.ActInScreenSpace())
								{
									vector = AdvGame.GetScreenNavMesh(vector);
								}
								Vector3[] pointsArray = GetComponent<NavigationManager>().navigationEngine.GetPointsArray(KickStarter.player.transform.position, vector);
								KickStarter.player.MoveAlongPoints(pointsArray, doRun);
								targetPos = pointsArray[pointsArray.Length - 1];
							}
							while ((bool)KickStarter.player.activePath)
							{
								yield return new WaitForFixedUpdate();
							}
						}
						if (button.faceAfter)
						{
							lookVector = _hotspot.walkToMarker.transform.forward;
							lookVector.y = 0f;
							KickStarter.player.Halt();
							KickStarter.player.SetLookDirection(lookVector, false);
							while (KickStarter.player.IsTurning())
							{
								yield return new WaitForFixedUpdate();
							}
						}
					}
					else if (lookVector.magnitude > 2f && button.playerAction == PlayerAction.WalkTo)
					{
						if ((bool)GetComponent<NavigationManager>())
						{
							Vector3 vector2 = _hotspot.transform.position;
							if ((bool)_hotspot.walkToMarker)
							{
								vector2 = _hotspot.walkToMarker.transform.position;
							}
							if (settingsManager.ActInScreenSpace())
							{
								vector2 = AdvGame.GetScreenNavMesh(vector2);
							}
							Vector3[] pointsArray2 = GetComponent<NavigationManager>().navigationEngine.GetPointsArray(KickStarter.player.transform.position, vector2);
							KickStarter.player.MoveAlongPoints(pointsArray2, doRun);
							targetPos = pointsArray2[pointsArray2.Length - 1];
						}
						if (button.setProximity)
						{
							button.proximity = Mathf.Max(button.proximity, 1f);
							targetPos.y = KickStarter.player.transform.position.y;
							while (Vector3.Distance(KickStarter.player.transform.position, targetPos) > button.proximity && (bool)KickStarter.player.activePath)
							{
								yield return new WaitForFixedUpdate();
							}
						}
						else
						{
							yield return new WaitForSeconds(0.6f);
						}
						if (button.faceAfter)
						{
							if (settingsManager.ActInScreenSpace())
							{
								lookVector = AdvGame.GetScreenDirection(KickStarter.player.transform.position, _hotspot.transform.position);
							}
							else
							{
								lookVector = _hotspot.transform.position - KickStarter.player.transform.position;
								lookVector.y = 0f;
							}
							KickStarter.player.Halt();
							KickStarter.player.SetLookDirection(lookVector, false);
							while (KickStarter.player.IsTurning())
							{
								yield return new WaitForFixedUpdate();
							}
						}
					}
				}
				else
				{
					KickStarter.player.charState = CharState.Decelerate;
				}
				KickStarter.player.EndPath();
				playerInput.hotspotMovingTo = null;
				yield return new WaitForSeconds(0.1f);
				KickStarter.player.EndPath();
				playerInput.hotspotMovingTo = null;
			}
			_hotspot.Deselect();
			hotspot = null;
			if ((bool)KickStarter.player)
			{
				KickStarter.player.ClearHeadTurnTarget(HeadFacing.Hotspot, false);
			}
			if (button == null)
			{
				if (selectedItemID >= 0 && runtimeInventory.GetItem(selectedItemID) != null && (bool)runtimeInventory.GetItem(selectedItemID).unhandledActionList)
				{
					ActionListAsset unhandledActionList = runtimeInventory.GetItem(selectedItemID).unhandledActionList;
					runtimeInventory.SetNull();
					AdvGame.RunActionListAsset(unhandledActionList);
				}
				else if (selectedItemID >= 0 && (bool)runtimeInventory.unhandledHotspot)
				{
					runtimeInventory.SetNull();
					AdvGame.RunActionListAsset(runtimeInventory.unhandledHotspot);
				}
				else
				{
					stateHandler.gameState = GameState.Normal;
					if (settingsManager.inventoryDragDrop)
					{
						runtimeInventory.SetNull();
					}
				}
			}
			else
			{
				runtimeInventory.SetNull();
				if (_hotspot.interactionSource == InteractionSource.AssetFile)
				{
					AdvGame.RunActionListAsset(button.assetFile);
				}
				else if (_hotspot.interactionSource == InteractionSource.CustomScript)
				{
					if (button.customScriptObject != null && button.customScriptFunction != string.Empty)
					{
						button.customScriptObject.SendMessage(button.customScriptFunction);
					}
				}
				else if (_hotspot.interactionSource == InteractionSource.InScene)
				{
					if ((bool)button.interaction)
					{
						button.interaction.Interact();
					}
					else
					{
						stateHandler.gameState = GameState.Normal;
					}
				}
			}
			button = null;
		}

		public string GetLabel()
		{
			string text = string.Empty;
			if ((bool)cursorManager && runtimeInventory.selectedItem != null && cursorManager.inventoryHandling != InventoryHandling.ChangeCursor && (settingsManager.interactionMethod != AC_InteractionMethod.ChooseHotspotThenInteraction || settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot))
			{
				text = ((Options.GetLanguage() <= 0) ? (cursorManager.hotspotPrefix1.label + " " + runtimeInventory.selectedItem.label + " " + cursorManager.hotspotPrefix2.label + " ") : (SpeechManager.GetTranslation(cursorManager.hotspotPrefix1.lineID, Options.GetLanguage()) + " " + runtimeInventory.selectedItem.label + " " + SpeechManager.GetTranslation(cursorManager.hotspotPrefix1.lineID, Options.GetLanguage()) + " "));
			}
			else if ((bool)cursorManager && cursorManager.addHotspotPrefix)
			{
				if (settingsManager.interactionMethod == AC_InteractionMethod.ContextSensitive)
				{
					if ((bool)hotspot && hotspot.provideUseInteraction)
					{
						text = cursorManager.GetLabelFromID(hotspot.GetFirstUseButton().iconID);
					}
				}
				else if (settingsManager.interactionMethod == AC_InteractionMethod.ChooseInteractionThenHotspot)
				{
					text = cursorManager.GetLabelFromID(playerCursor.GetSelectedCursorID());
				}
				else if (settingsManager.interactionMethod == AC_InteractionMethod.ChooseHotspotThenInteraction && settingsManager.selectInteractions == SelectInteractions.CyclingCursorAndClickingHotspot)
				{
					text = cursorManager.GetLabelFromID(playerCursor.GetSelectedCursorID());
				}
			}
			if ((bool)hotspot)
			{
				if (Options.GetLanguage() > 0)
				{
					text += SpeechManager.GetTranslation(hotspot.lineID, Options.GetLanguage());
				}
				else
				{
					text = ((!(hotspot.hotspotName != string.Empty)) ? (text + hotspot.name) : (text + hotspot.hotspotName));
				}
			}
			return text;
		}

		public void StopInteraction()
		{
			button = null;
			StopCoroutine("UseObject");
		}

		public Vector2 GetHotspotScreenCentre()
		{
			if ((bool)hotspot)
			{
				Vector2 iconScreenPosition = hotspot.GetIconScreenPosition();
				return new Vector2(iconScreenPosition.x / (float)Screen.width, 1f - iconScreenPosition.y / (float)Screen.height);
			}
			return Vector2.zero;
		}

		public bool IsMouseOverHotspot()
		{
			if ((bool)settingsManager && settingsManager.interactionMethod == AC_InteractionMethod.ChooseInteractionThenHotspot && (bool)playerCursor && playerCursor.GetSelectedCursor() == -1)
			{
				return false;
			}
			if ((bool)settingsManager && settingsManager.IsUnity2D())
			{
				RaycastHit2D raycastHit2D = default(RaycastHit2D);
				if (Camera.main.orthographic)
				{
					raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(playerInput.GetMousePosition()), Vector2.zero, settingsManager.navMeshRaycastLength);
				}
				else
				{
					Vector3 position = playerInput.GetMousePosition();
					position.z = 0f - Camera.main.transform.position.z;
					raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero, settingsManager.navMeshRaycastLength, 1 << LayerMask.NameToLayer(settingsManager.hotspotLayer));
				}
				if (raycastHit2D.collider != null && (bool)raycastHit2D.collider.gameObject.GetComponent<Hotspot>())
				{
					return true;
				}
			}
			else
			{
				Ray ray = Camera.main.ScreenPointToRay(playerInput.GetMousePosition());
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, settingsManager.hotspotRaycastLength, 1 << LayerMask.NameToLayer(settingsManager.hotspotLayer)) && (bool)hitInfo.collider.gameObject.GetComponent<Hotspot>())
				{
					return true;
				}
				if (Physics.Raycast(ray, out hitInfo, settingsManager.moveableRaycastLength, 1 << LayerMask.NameToLayer(settingsManager.hotspotLayer)) && (bool)hitInfo.collider.gameObject.GetComponent<DragBase>())
				{
					return true;
				}
			}
			return false;
		}

		public bool IsDroppingInventory()
		{
			if (stateHandler.gameState == GameState.Cutscene || stateHandler.gameState == GameState.DialogOptions)
			{
				return false;
			}
			if (runtimeInventory.selectedItem == null || !runtimeInventory.localItems.Contains(runtimeInventory.selectedItem))
			{
				return false;
			}
			if (settingsManager.inventoryDragDrop && playerInput.mouseState == MouseState.Normal && playerInput.dragState == DragState.Inventory)
			{
				return true;
			}
			if (settingsManager.inventoryDragDrop && playerInput.CanClick() && playerInput.mouseState == MouseState.Normal && playerInput.dragState == DragState.None)
			{
				return true;
			}
			if (playerInput.mouseState == MouseState.SingleClick && settingsManager.inventoryDisableLeft)
			{
				return true;
			}
			if (playerInput.mouseState == MouseState.RightClick && settingsManager.rightClickInventory == RightClickInventory.DeselectsItem && (settingsManager.interactionMethod == AC_InteractionMethod.ContextSensitive || settingsManager.inventoryInteractions == InventoryInteractions.Single))
			{
				return true;
			}
			return false;
		}

		public Hotspot GetActiveHotspot()
		{
			return hotspot;
		}

		public int GetActiveUseButtonIconID()
		{
			if (settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot)
			{
				if (runtimeInventory.hoverItem != null && settingsManager.inventoryInteractions == InventoryInteractions.Multiple)
				{
					if (interactionIndex == -1)
					{
						if (runtimeInventory.hoverItem.interactions.Count == 0)
						{
							return -1;
						}
						interactionIndex = 0;
						return 0;
					}
					if (interactionIndex < runtimeInventory.hoverItem.interactions.Count)
					{
						return runtimeInventory.hoverItem.interactions[interactionIndex].icon.id;
					}
				}
				else if ((bool)GetActiveHotspot())
				{
					if (interactionIndex == -1)
					{
						if (GetActiveHotspot().GetFirstUseButton() == null)
						{
							return -1;
						}
						interactionIndex = GetActiveHotspot().FindFirstEnabledInteraction();
						return interactionIndex;
					}
					if (interactionIndex < GetActiveHotspot().useButtons.Count)
					{
						return GetActiveHotspot().useButtons[interactionIndex].iconID;
					}
				}
			}
			else if (runtimeInventory.selectedItem != null && settingsManager.inventoryInteractions == InventoryInteractions.Multiple)
			{
				if (interactionIndex == -1)
				{
					return -1;
				}
				if (interactionIndex < runtimeInventory.selectedItem.interactions.Count)
				{
					return runtimeInventory.selectedItem.interactions[interactionIndex].icon.id;
				}
			}
			else if ((bool)GetActiveHotspot())
			{
				if (interactionIndex == -1)
				{
					if (GetActiveHotspot().GetFirstUseButton() == null)
					{
						return -1;
					}
					interactionIndex = 0;
					return 0;
				}
				if (interactionIndex < GetActiveHotspot().useButtons.Count)
				{
					return GetActiveHotspot().useButtons[interactionIndex].iconID;
				}
			}
			return -1;
		}

		public int GetActiveInvButtonID()
		{
			if (settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot)
			{
				if (runtimeInventory.hoverItem != null && settingsManager.inventoryInteractions == InventoryInteractions.Multiple)
				{
					if (interactionIndex >= runtimeInventory.hoverItem.interactions.Count && runtimeInventory.matchingInvInteractions.Count > 0)
					{
						return runtimeInventory.hoverItem.combineID[runtimeInventory.matchingInvInteractions[interactionIndex - runtimeInventory.hoverItem.interactions.Count]];
					}
				}
				else if ((bool)GetActiveHotspot() && interactionIndex >= GetActiveHotspot().useButtons.Count)
				{
					return GetActiveHotspot().invButtons[runtimeInventory.matchingInvInteractions[interactionIndex - GetActiveHotspot().useButtons.Count]].invID;
				}
			}
			else if (runtimeInventory.selectedItem != null && settingsManager.inventoryInteractions == InventoryInteractions.Multiple)
			{
				if (interactionIndex >= runtimeInventory.selectedItem.interactions.Count && runtimeInventory.matchingInvInteractions.Count > 0)
				{
					return runtimeInventory.selectedItem.combineID[runtimeInventory.matchingInvInteractions[interactionIndex - runtimeInventory.selectedItem.interactions.Count]];
				}
			}
			else if ((bool)GetActiveHotspot() && interactionIndex >= GetActiveHotspot().useButtons.Count)
			{
				return GetActiveHotspot().invButtons[runtimeInventory.matchingInvInteractions[interactionIndex - GetActiveHotspot().useButtons.Count]].invID;
			}
			return -1;
		}

		public void SetNextInteraction()
		{
			if (settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot)
			{
				if (runtimeInventory.selectedItem == null || settingsManager.cycleInventoryCursors)
				{
					if (runtimeInventory.hoverItem != null)
					{
						interactionIndex = runtimeInventory.hoverItem.GetNextInteraction(interactionIndex, runtimeInventory.matchingInvInteractions.Count);
					}
					else if (GetActiveHotspot() != null)
					{
						interactionIndex = GetActiveHotspot().GetNextInteraction(interactionIndex, runtimeInventory.matchingInvInteractions.Count);
					}
					if (!settingsManager.cycleInventoryCursors && GetActiveInvButtonID() >= 0)
					{
						interactionIndex = -1;
					}
					else
					{
						runtimeInventory.SelectItemByID(GetActiveInvButtonID());
					}
				}
			}
			else if (runtimeInventory.selectedItem != null)
			{
				interactionIndex = runtimeInventory.selectedItem.GetNextInteraction(interactionIndex, runtimeInventory.matchingInvInteractions.Count);
			}
			else if (GetActiveHotspot() != null)
			{
				interactionIndex = GetActiveHotspot().GetNextInteraction(interactionIndex, runtimeInventory.matchingInvInteractions.Count);
			}
		}

		private void RestoreHotspotInteraction()
		{
			if (hotspot != null)
			{
				interactionIndex = hotspot.lastInteractionIndex;
				if (!settingsManager.cycleInventoryCursors && GetActiveInvButtonID() >= 0)
				{
					interactionIndex = -1;
				}
				else
				{
					runtimeInventory.SelectItemByID(GetActiveInvButtonID());
				}
			}
		}

		public void SetPreviousInteraction()
		{
			if (runtimeInventory.selectedItem == null || settingsManager.cycleInventoryCursors)
			{
				if (runtimeInventory.selectedItem != null)
				{
					interactionIndex = runtimeInventory.selectedItem.GetPreviousInteraction(interactionIndex, runtimeInventory.matchingInvInteractions.Count);
				}
				else if (GetActiveHotspot() != null)
				{
					interactionIndex = GetActiveHotspot().GetPreviousInteraction(interactionIndex, runtimeInventory.matchingInvInteractions.Count);
				}
			}
		}

		public void ResetInteractionIndex()
		{
			interactionIndex = -1;
		}

		public int GetInteractionIndex()
		{
			return interactionIndex;
		}

		private void ClickHotspotToInteract()
		{
			int activeInvButtonID = GetActiveInvButtonID();
			if (activeInvButtonID == -1)
			{
				ClickButton(InteractionType.Use, GetActiveUseButtonIconID(), -1);
			}
			else
			{
				ClickButton(InteractionType.Inventory, -1, activeInvButtonID);
			}
		}

		public void ClickInvItemToInteract()
		{
			int activeInvButtonID = GetActiveInvButtonID();
			if (activeInvButtonID == -1)
			{
				if (settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingCursorAndClickingHotspot)
				{
					runtimeInventory.RunInteraction(true, GetActiveUseButtonIconID());
				}
				else
				{
					runtimeInventory.RunInteraction(false, GetActiveUseButtonIconID());
				}
			}
			else
			{
				runtimeInventory.SelectItem(runtimeInventory.hoverItem);
				runtimeInventory.Combine(activeInvButtonID);
			}
		}

		private void OnDestroy()
		{
			playerInput = null;
			stateHandler = null;
			runtimeInventory = null;
		}
	}
}
