using UnityEngine;

namespace AC
{
	public class PlayerInput : MonoBehaviour
	{
		[HideInInspector]
		public MouseState mouseState;

		[HideInInspector]
		public DragState dragState;

		[HideInInspector]
		public Hotspot hotspotMovingTo;

		[HideInInspector]
		public bool mouseOverMenu;

		[HideInInspector]
		public bool interactionMenuIsOn;

		[HideInInspector]
		public Vector2 moveKeys = new Vector2(0f, 0f);

		[HideInInspector]
		public bool isRunning;

		[HideInInspector]
		public float timeScale = 1f;

		[HideInInspector]
		public bool isUpLocked;

		[HideInInspector]
		public bool isDownLocked;

		[HideInInspector]
		public bool isLeftLocked;

		[HideInInspector]
		public bool isRightLocked;

		[HideInInspector]
		public PlayerMoveLock runLock;

		[HideInInspector]
		public bool freeAimLock;

		[HideInInspector]
		public int selected_option;

		public float clickDelay = 0.3f;

		public float doubleClickDelay = 1f;

		private float clickTime;

		private float doubleClickTime;

		[HideInInspector]
		public MenuDrag activeDragElement;

		[HideInInspector]
		public bool hasUnclickedSinceClick;

		[HideInInspector]
		public string menuButtonInput;

		[HideInInspector]
		public float menuButtonValue;

		[HideInInspector]
		public SimulateInputType menuInput;

		private Vector2 xboxCursor;

		public float cursorMoveSpeed = 4f;

		private Vector2 mousePosition;

		private bool scrollingLocked;

		private Vector2 dragStartPosition = Vector2.zero;

		[HideInInspector]
		public float dragSpeed;

		private Vector2 dragVector;

		private float touchTime;

		private float touchThreshold = 0.2f;

		[HideInInspector]
		public Vector2 freeAim;

		[HideInInspector]
		public bool cursorIsLocked;

		private bool toggleRun;

		private bool canDragMoveable;

		private float cameraInfluence = 100000f;

		private DragBase dragObject;

		private Vector2 lastMousePosition;

		private Vector3 lastCameraPosition;

		private Vector3 dragForce;

		private Vector2 deltaDragMouse;

		[HideInInspector]
		public Conversation activeConversation;

		[HideInInspector]
		public ArrowPrompt activeArrows;

		[HideInInspector]
		public Container activeContainer;

		private PlayerInteraction playerInteraction;

		private ActionListManager actionListManager;

		private StateHandler stateHandler;

		private RuntimeInventory runtimeInventory;

		private SettingsManager settingsManager;

		private void Awake()
		{
			if ((bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().settingsManager)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
				cursorIsLocked = settingsManager.lockCursorOnStart;
			}
			actionListManager = GetComponent<ActionListManager>();
			playerInteraction = GetComponent<PlayerInteraction>();
			ResetClick();
			xboxCursor.x = Screen.width / 2;
			xboxCursor.y = Screen.height / 2;
		}

		private void Start()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine"))
			{
				if ((bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
				{
					stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
				}
				if ((bool)GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>())
				{
					runtimeInventory = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
				}
			}
			if (settingsManager.movementMethod == MovementMethod.UltimateFPS)
			{
				UltimateFPSIntegration.SetCameraState(cursorIsLocked);
			}
			if (settingsManager.offsetTouchCursor)
			{
				mousePosition = xboxCursor;
			}
		}

		public void UpdateInput()
		{
			if (clickTime > 0f)
			{
				clickTime -= 4f * GetDeltaTime();
			}
			if (clickTime < 0f)
			{
				clickTime = 0f;
			}
			if (doubleClickTime > 0f)
			{
				doubleClickTime -= 4f * GetDeltaTime();
			}
			if (doubleClickTime < 0f)
			{
				doubleClickTime = 0f;
			}
			if (!stateHandler || !settingsManager)
			{
				return;
			}
			try
			{
				if (InputGetButtonDown("ToggleCursor") && stateHandler.gameState == GameState.Normal)
				{
					ToggleCursor();
				}
			}
			catch
			{
				cursorIsLocked = false;
			}
			if (stateHandler.gameState == GameState.Cutscene && InputGetButtonDown("EndCutscene"))
			{
				actionListManager.EndCutscene();
			}
			if (settingsManager.inputMethod == InputMethod.MouseAndKeyboard)
			{
				if (!cursorIsLocked || stateHandler.gameState == GameState.Paused || stateHandler.gameState == GameState.DialogOptions || (freeAimLock && settingsManager.IsInFirstPerson()))
				{
					mousePosition = InputMousePosition();
					freeAim = Vector2.zero;
				}
				else if (dragObject != null && settingsManager.IsInFirstPerson() && settingsManager.disableFreeAimWhenDragging)
				{
					mousePosition = InputMousePosition();
					freeAim = Vector2.zero;
				}
				else if (cursorIsLocked && stateHandler.gameState == GameState.Normal)
				{
					mousePosition = new Vector2(Screen.width / 2, Screen.height / 2);
					freeAim = new Vector2(InputGetAxis("CursorHorizontal"), InputGetAxis("CursorVertical"));
				}
				if (mouseState == MouseState.Normal)
				{
					dragState = DragState.None;
				}
				if (InputGetMouseButtonDown(0))
				{
					if (mouseState == MouseState.Normal)
					{
						if (CanDoubleClick())
						{
							mouseState = MouseState.DoubleClick;
							ResetClick();
						}
						else if (CanClick())
						{
							dragStartPosition = GetInvertedMouse();
							mouseState = MouseState.SingleClick;
							ResetClick();
							ResetDoubleClick();
						}
					}
				}
				else if (InputGetMouseButtonDown(1))
				{
					mouseState = MouseState.RightClick;
				}
				else if (InputGetMouseButton(0))
				{
					mouseState = MouseState.HeldDown;
					SetDragState();
				}
				else if (mouseState == MouseState.HeldDown && dragState == DragState.None && CanClick())
				{
					mouseState = MouseState.LetGo;
				}
				else
				{
					mouseState = MouseState.Normal;
				}
				if (settingsManager.inputMethod == InputMethod.TouchScreen)
				{
					if (dragState == DragState.Player)
					{
						if (settingsManager.IsFirstPersonDragMovement())
						{
							freeAim = new Vector2(dragVector.x * settingsManager.freeAimTouchSpeed, 0f);
						}
						else
						{
							freeAim = new Vector2(dragVector.x * settingsManager.freeAimTouchSpeed, (0f - dragVector.y) * settingsManager.freeAimTouchSpeed);
						}
					}
					else
					{
						freeAim = Vector2.zero;
					}
				}
			}
			else if (settingsManager.inputMethod == InputMethod.TouchScreen)
			{
				int touchCount = Input.touchCount;
				if (touchCount > 0)
				{
					if (settingsManager.offsetTouchCursor)
					{
						if (touchTime > touchThreshold)
						{
							Touch touch = Input.GetTouch(0);
							if (touch.phase == TouchPhase.Moved && touchCount == 1)
							{
								if (stateHandler.gameState == GameState.Paused)
								{
									mousePosition += touch.deltaPosition * 1.7f;
								}
								else
								{
									mousePosition += touch.deltaPosition * Time.deltaTime / touch.deltaTime;
								}
								if (mousePosition.x < 0f)
								{
									mousePosition.x = 0f;
								}
								else if (mousePosition.x > (float)Screen.width)
								{
									mousePosition.x = Screen.width;
								}
								if (mousePosition.y < 0f)
								{
									mousePosition.y = 0f;
								}
								else if (mousePosition.y > (float)Screen.height)
								{
									mousePosition.y = Screen.height;
								}
							}
						}
					}
					else
					{
						mousePosition = Input.GetTouch(0).position;
					}
				}
				if (mouseState == MouseState.Normal)
				{
					dragState = DragState.None;
				}
				if (touchTime > 0f && touchTime < touchThreshold)
				{
					dragStartPosition = GetInvertedMouse();
				}
				if ((touchCount == 1 && stateHandler.gameState == GameState.Cutscene && Input.GetTouch(0).phase == TouchPhase.Began) || (touchCount == 1 && !settingsManager.offsetTouchCursor && Input.GetTouch(0).phase == TouchPhase.Began) || touchTime == -1f)
				{
					if (mouseState == MouseState.Normal)
					{
						if (CanDoubleClick())
						{
							mouseState = MouseState.DoubleClick;
							ResetClick();
						}
						else if (CanClick())
						{
							dragStartPosition = GetInvertedMouse();
							mouseState = MouseState.SingleClick;
							ResetClick();
							ResetDoubleClick();
						}
					}
				}
				else if (touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began)
				{
					mouseState = MouseState.RightClick;
				}
				else if (touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved))
				{
					mouseState = MouseState.HeldDown;
					SetDragState();
				}
				else if (mouseState == MouseState.HeldDown && dragState == DragState.None && CanClick())
				{
					mouseState = MouseState.LetGo;
				}
				else
				{
					mouseState = MouseState.Normal;
				}
				if (settingsManager.offsetTouchCursor)
				{
					if (touchCount > 0)
					{
						touchTime += GetDeltaTime();
					}
					else if (touchTime > 0f && touchTime < touchThreshold)
					{
						touchTime = -1f;
					}
					else
					{
						touchTime = 0f;
					}
				}
				if (dragState == DragState.Player)
				{
					if (settingsManager.IsFirstPersonDragMovement())
					{
						freeAim = new Vector2(dragVector.x * settingsManager.freeAimTouchSpeed, 0f);
					}
					else
					{
						freeAim = new Vector2(dragVector.x * settingsManager.freeAimTouchSpeed, (0f - dragVector.y) * settingsManager.freeAimTouchSpeed);
					}
				}
				else
				{
					freeAim = Vector2.zero;
				}
			}
			else if (settingsManager.inputMethod == InputMethod.KeyboardOrController)
			{
				if (cursorIsLocked && stateHandler.gameState == GameState.Normal)
				{
					mousePosition = new Vector2(Screen.width / 2, Screen.height / 2);
					freeAim = new Vector2(InputGetAxis("CursorHorizontal") * 50f, InputGetAxis("CursorVertical") * 50f);
				}
				else
				{
					xboxCursor.x += InputGetAxis("CursorHorizontal") * cursorMoveSpeed * (float)Screen.width;
					xboxCursor.y += InputGetAxis("CursorVertical") * cursorMoveSpeed * (float)Screen.height;
					xboxCursor.x = Mathf.Clamp(xboxCursor.x, 0f, Screen.width);
					xboxCursor.y = Mathf.Clamp(xboxCursor.y, 0f, Screen.height);
					mousePosition = xboxCursor;
					freeAim = Vector2.zero;
				}
				if (mouseState == MouseState.Normal)
				{
					dragState = DragState.None;
				}
				if (InputGetButtonDown("InteractionA"))
				{
					if (mouseState == MouseState.Normal)
					{
						if (CanDoubleClick())
						{
							mouseState = MouseState.DoubleClick;
							ResetClick();
						}
						else if (CanClick())
						{
							dragStartPosition = GetInvertedMouse();
							mouseState = MouseState.SingleClick;
							ResetClick();
							ResetDoubleClick();
						}
					}
				}
				else if (InputGetButtonDown("InteractionB"))
				{
					mouseState = MouseState.RightClick;
				}
				else if (InputGetButton("InteractionA"))
				{
					mouseState = MouseState.HeldDown;
					SetDragState();
				}
				else
				{
					mouseState = MouseState.Normal;
				}
				if (stateHandler.gameState == GameState.DialogOptions || stateHandler.gameState == GameState.Paused)
				{
					if (!scrollingLocked)
					{
						if ((double)InputGetAxisRaw("Vertical") > 0.1 || (double)InputGetAxisRaw("Horizontal") < -0.1)
						{
							scrollingLocked = true;
							selected_option--;
						}
						else if ((double)InputGetAxisRaw("Vertical") < -0.1 || (double)InputGetAxisRaw("Horizontal") > 0.1)
						{
							scrollingLocked = true;
							selected_option++;
						}
					}
					else if ((double)InputGetAxisRaw("Vertical") < 0.05 && (double)InputGetAxisRaw("Vertical") > -0.05 && (double)InputGetAxisRaw("Horizontal") < 0.05 && (double)InputGetAxisRaw("Horizontal") > -0.05)
					{
						scrollingLocked = false;
					}
				}
			}
			if (hotspotMovingTo != null)
			{
				freeAim = Vector2.zero;
			}
			if (settingsManager.SelectInteractionMethod() == SelectInteractions.CyclingMenuAndClickingHotspot && interactionMenuIsOn)
			{
				try
				{
					if (InputGetButtonDown("CycleInteractionsRight"))
					{
						playerInteraction.SetNextInteraction();
					}
					else if (InputGetButtonDown("CycleInteractionsLeft"))
					{
						playerInteraction.SetPreviousInteraction();
					}
					else if (InputGetAxis("CycleInteractions") > 0.1f)
					{
						playerInteraction.SetNextInteraction();
					}
					else if (InputGetAxis("CycleInteractions") < -0.1f)
					{
						playerInteraction.SetPreviousInteraction();
					}
				}
				catch
				{
				}
			}
			mousePosition = KickStarter.mainCamera.LimitMouseToAspect(mousePosition);
			if (mouseState == MouseState.Normal && !hasUnclickedSinceClick)
			{
				hasUnclickedSinceClick = true;
			}
			if (mouseState == MouseState.Normal)
			{
				canDragMoveable = true;
			}
			UpdateDrag();
			if (dragState != DragState.None)
			{
				dragVector = GetInvertedMouse() - dragStartPosition;
				dragSpeed = dragVector.magnitude;
			}
			else
			{
				dragSpeed = 0f;
			}
		}

		public Vector2 GetMousePosition()
		{
			return mousePosition;
		}

		public Vector2 GetInvertedMouse()
		{
			return new Vector2(GetMousePosition().x, (float)Screen.height - GetMousePosition().y);
		}

		public bool IsCursorReadable()
		{
			if (settingsManager.inputMethod == InputMethod.TouchScreen && mouseState == MouseState.Normal)
			{
				if (runtimeInventory.selectedItem != null && settingsManager.inventoryDragDrop)
				{
					return true;
				}
				return settingsManager.offsetTouchCursor;
			}
			return true;
		}

		public void DrawDragLine()
		{
			if (dragState == DragState.Player && settingsManager.movementMethod != MovementMethod.StraightToCursor && settingsManager.drawDragLine)
			{
				Vector2 pointA = dragStartPosition;
				Vector2 invertedMouse = GetInvertedMouse();
				if (invertedMouse.x >= 0f)
				{
					DrawStraightLine.Draw(pointA, invertedMouse, settingsManager.dragLineColor, settingsManager.dragLineWidth, true);
				}
			}
			if (!(activeDragElement != null))
			{
				return;
			}
			if (mouseState == MouseState.HeldDown)
			{
				if (!activeDragElement.DoDrag(GetDragVector()))
				{
					activeDragElement = null;
				}
			}
			else if (mouseState == MouseState.Normal && activeDragElement.CheckStop(GetInvertedMouse()))
			{
				activeDragElement = null;
			}
		}

		public void UpdateDirectInput()
		{
			if (!(settingsManager != null))
			{
				return;
			}
			if (activeArrows != null)
			{
				if (activeArrows.arrowPromptType == ArrowPromptType.KeyOnly || activeArrows.arrowPromptType == ArrowPromptType.KeyAndClick)
				{
					Vector2 vector = new Vector2(InputGetAxis("Horizontal"), InputGetAxis("Vertical"));
					if (vector.magnitude > 0f)
					{
						if (settingsManager.inputMethod == InputMethod.TouchScreen && dragState == DragState.ScreenArrows)
						{
							vector = GetDragVector() / settingsManager.dragRunThreshold / settingsManager.dragWalkThreshold;
						}
						float num = 0.95f;
						if (settingsManager.inputMethod == InputMethod.MouseAndKeyboard)
						{
							num = 0.05f;
						}
						if (vector.x > num)
						{
							activeArrows.DoRight();
						}
						else if (vector.x < 0f - num)
						{
							activeArrows.DoLeft();
						}
						else if (vector.y < 0f - num)
						{
							activeArrows.DoDown();
						}
						else if (vector.y > num)
						{
							activeArrows.DoUp();
						}
					}
				}
				if (activeArrows != null && (activeArrows.arrowPromptType == ArrowPromptType.ClickOnly || activeArrows.arrowPromptType == ArrowPromptType.KeyAndClick))
				{
					Vector2 invertedMouse = GetInvertedMouse();
					if (mouseState == MouseState.SingleClick)
					{
						if (activeArrows.upArrow.rect.Contains(invertedMouse))
						{
							activeArrows.DoUp();
						}
						else if (activeArrows.downArrow.rect.Contains(invertedMouse))
						{
							activeArrows.DoDown();
						}
						else if (activeArrows.leftArrow.rect.Contains(invertedMouse))
						{
							activeArrows.DoLeft();
						}
						else if (activeArrows.rightArrow.rect.Contains(invertedMouse))
						{
							activeArrows.DoRight();
						}
					}
				}
			}
			if (activeArrows == null && settingsManager.movementMethod != MovementMethod.PointAndClick)
			{
				float num2 = 0f;
				float num3 = 0f;
				if (settingsManager.inputMethod == InputMethod.TouchScreen || settingsManager.movementMethod == MovementMethod.Drag)
				{
					if (dragState != DragState.None)
					{
						num2 = dragVector.x;
						num3 = 0f - dragVector.y;
					}
				}
				else
				{
					num2 = InputGetAxis("Horizontal");
					num3 = InputGetAxis("Vertical");
				}
				try
				{
					if (InputGetButtonDown("Jump") && stateHandler.gameState == GameState.Normal)
					{
						KickStarter.player.Jump();
					}
				}
				catch
				{
				}
				if ((isUpLocked && num3 > 0f) || (isDownLocked && num3 < 0f))
				{
					num3 = 0f;
				}
				if ((isLeftLocked && num2 > 0f) || (isRightLocked && num2 < 0f))
				{
					num2 = 0f;
				}
				bool flag;
				if (runLock != PlayerMoveLock.Free)
				{
					flag = ((runLock != PlayerMoveLock.AlwaysWalk) ? true : false);
				}
				else if (settingsManager.inputMethod == InputMethod.TouchScreen || settingsManager.movementMethod == MovementMethod.Drag)
				{
					flag = ((dragStartPosition != Vector2.zero && dragSpeed > settingsManager.dragRunThreshold * 10f) ? true : false);
				}
				else
				{
					try
					{
						flag = InputGetButton("Run");
					}
					catch
					{
						flag = false;
					}
					try
					{
						if (InputGetButtonDown("ToggleRun"))
						{
							toggleRun = !toggleRun;
						}
					}
					catch
					{
						toggleRun = false;
					}
				}
				if (settingsManager.inputMethod != InputMethod.TouchScreen && (settingsManager.movementMethod == MovementMethod.FirstPerson || settingsManager.movementMethod == MovementMethod.Direct) && runLock == PlayerMoveLock.Free && toggleRun)
				{
					isRunning = !flag;
				}
				else
				{
					isRunning = flag;
				}
				moveKeys = CreateMoveKeys(num2, num3);
			}
			if (InputGetButtonDown("FlashHotspots"))
			{
				FlashHotspots();
			}
		}

		private Vector2 CreateMoveKeys(float h, float v)
		{
			if (settingsManager.movementMethod == MovementMethod.Direct && settingsManager.inputMethod != InputMethod.TouchScreen && settingsManager.directMovementType == DirectMovementType.RelativeToCamera)
			{
				if (settingsManager.limitDirectMovement == LimitDirectMovement.FourDirections)
				{
					if (Mathf.Abs(h) > Mathf.Abs(v))
					{
						v = 0f;
					}
					else
					{
						h = 0f;
					}
				}
				else if (settingsManager.limitDirectMovement == LimitDirectMovement.EightDirections)
				{
					if (!(Mathf.Abs(h) > Mathf.Abs(v)))
					{
						h = ((Mathf.Abs(h) < Mathf.Abs(v)) ? 0f : ((!(Mathf.Abs(h) > 0.4f) || !(Mathf.Abs(v) > 0.4f)) ? (v = 0f) : ((!(h * v > 0f)) ? (0f - v) : v)));
					}
					else
					{
						v = 0f;
					}
				}
			}
			return new Vector2(h, v);
		}

		private void FlashHotspots()
		{
			Hotspot[] array = Object.FindObjectsOfType(typeof(Hotspot)) as Hotspot[];
			Hotspot[] array2 = array;
			foreach (Hotspot hotspot in array2)
			{
				if (hotspot.IsOn() && (bool)hotspot.highlight && hotspot != playerInteraction.GetActiveHotspot())
				{
					hotspot.highlight.Flash();
				}
			}
		}

		public void RemoveActiveArrows()
		{
			if ((bool)activeArrows)
			{
				activeArrows.TurnOff();
			}
		}

		public void ResetClick()
		{
			clickTime = clickDelay;
			hasUnclickedSinceClick = false;
		}

		private void ResetDoubleClick()
		{
			doubleClickTime = doubleClickDelay;
		}

		public bool CanClick()
		{
			if (clickTime == 0f)
			{
				return true;
			}
			return false;
		}

		private bool CanDoubleClick()
		{
			if (doubleClickTime > 0f && clickTime == 0f)
			{
				return true;
			}
			return false;
		}

		public void SimulateInput(SimulateInputType input, string axis, float value)
		{
			if (axis != string.Empty)
			{
				menuInput = input;
				menuButtonInput = axis;
				if (input == SimulateInputType.Button)
				{
					menuButtonValue = 1f;
				}
				else
				{
					menuButtonValue = value;
				}
				CancelInvoke();
				Invoke("StopSimulatingInput", 0.1f);
			}
		}

		public bool IsCursorLocked()
		{
			return cursorIsLocked;
		}

		private void StopSimulatingInput()
		{
			menuButtonInput = string.Empty;
		}

		private float InputGetAxisRaw(string axis)
		{
			try
			{
				if (settingsManager.useOuya)
				{
					if (OuyaIntegration.GetAxisRaw(axis) != 0f)
					{
						return OuyaIntegration.GetAxisRaw(axis);
					}
				}
				else if (Input.GetAxisRaw(axis) != 0f)
				{
					return Input.GetAxisRaw(axis);
				}
			}
			catch
			{
			}
			if (menuButtonInput != string.Empty && menuButtonInput == axis && menuInput == SimulateInputType.Axis)
			{
				return menuButtonValue;
			}
			return 0f;
		}

		private float InputGetAxis(string axis)
		{
			try
			{
				if (settingsManager.useOuya)
				{
					if (OuyaIntegration.GetAxis(axis) != 0f)
					{
						return OuyaIntegration.GetAxis(axis);
					}
				}
				else if (Input.GetAxis(axis) != 0f)
				{
					return Input.GetAxis(axis);
				}
			}
			catch
			{
			}
			if (menuButtonInput != string.Empty && menuButtonInput == axis && menuInput == SimulateInputType.Axis)
			{
				return menuButtonValue;
			}
			return 0f;
		}

		private bool InputGetMouseButton(int button)
		{
			if (settingsManager.useOuya)
			{
				return OuyaIntegration.GetMouseButton(button);
			}
			return Input.GetMouseButton(button);
		}

		private Vector2 InputMousePosition()
		{
			if (settingsManager.useOuya)
			{
				return OuyaIntegration.mousePosition;
			}
			return Input.mousePosition;
		}

		private bool InputGetMouseButtonDown(int button)
		{
			if (settingsManager.useOuya)
			{
				return OuyaIntegration.GetMouseButtonDown(button);
			}
			return Input.GetMouseButtonDown(button);
		}

		private bool InputGetButton(string axis)
		{
			try
			{
				if (settingsManager.useOuya)
				{
					if (OuyaIntegration.GetButton(axis))
					{
						return true;
					}
				}
				else if (Input.GetButton(axis))
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		public bool InputGetButtonDown(string axis)
		{
			try
			{
				if (settingsManager.useOuya)
				{
					if (OuyaIntegration.GetButtonDown(axis))
					{
						return true;
					}
				}
				else if (Input.GetButtonDown(axis))
				{
					return true;
				}
			}
			catch
			{
			}
			if (menuButtonInput != string.Empty && menuButtonInput == axis && menuInput == SimulateInputType.Button)
			{
				if (menuButtonValue > 0f)
				{
					ResetClick();
					StopSimulatingInput();
					return true;
				}
				StopSimulatingInput();
			}
			return false;
		}

		private void SetDragState()
		{
			if (runtimeInventory.selectedItem != null && settingsManager.inventoryDragDrop && (stateHandler.gameState == GameState.Normal || stateHandler.gameState == GameState.Paused))
			{
				dragState = DragState.Inventory;
			}
			else if (activeDragElement != null && (stateHandler.gameState == GameState.Normal || stateHandler.gameState == GameState.Paused))
			{
				dragState = DragState.Menu;
			}
			else if (activeArrows != null && settingsManager.inputMethod == InputMethod.TouchScreen)
			{
				dragState = DragState.ScreenArrows;
			}
			else if (dragObject != null)
			{
				dragState = DragState.Moveable;
			}
			else if ((bool)KickStarter.mainCamera.attachedCamera && KickStarter.mainCamera.attachedCamera.isDragControlled)
			{
				if (!GetComponent<PlayerInteraction>().IsMouseOverHotspot())
				{
					dragState = DragState._Camera;
					if (deltaDragMouse.magnitude * Time.deltaTime <= 1f && (GetInvertedMouse() - dragStartPosition).magnitude < 10f)
					{
						dragState = DragState.None;
					}
				}
			}
			else if ((settingsManager.movementMethod == MovementMethod.Drag || settingsManager.movementMethod == MovementMethod.StraightToCursor || (settingsManager.movementMethod != MovementMethod.PointAndClick && settingsManager.inputMethod == InputMethod.TouchScreen)) && stateHandler.gameState == GameState.Normal)
			{
				if (!mouseOverMenu && !interactionMenuIsOn && !GetComponent<PlayerInteraction>().IsMouseOverHotspot())
				{
					dragState = DragState.Player;
				}
			}
			else
			{
				dragState = DragState.None;
			}
		}

		private void UpdateDrag()
		{
			if (dragState != DragState.None)
			{
				if (freeAim.magnitude != 0f)
				{
					deltaDragMouse = freeAim * 500f / Time.deltaTime;
				}
				else
				{
					deltaDragMouse = (mousePosition - lastMousePosition) / Time.deltaTime;
				}
			}
			if ((bool)dragObject && stateHandler.gameState != GameState.Normal)
			{
				LetGo(false);
			}
			if (mouseState == MouseState.HeldDown && dragState == DragState.None)
			{
				Grab();
			}
			else if (dragState == DragState.Moveable)
			{
				if ((bool)dragObject)
				{
					if (dragObject.isHeld && dragObject.IsOnScreen() && dragObject.IsCloseToCamera(settingsManager.moveableRaycastLength))
					{
						Drag();
					}
					else
					{
						LetGo(true);
					}
				}
			}
			else if ((bool)dragObject)
			{
				LetGo(true);
			}
			if (dragState != DragState.None)
			{
				lastMousePosition = mousePosition;
			}
		}

		public void SetFreeAimLock(bool _state)
		{
			freeAimLock = _state;
			if (settingsManager.movementMethod == MovementMethod.UltimateFPS)
			{
				UltimateFPSIntegration.SetCameraState(!_state);
			}
		}

		private void LetGo(bool unlockFPSCamera)
		{
			dragObject.LetGo();
			dragObject = null;
			if (unlockFPSCamera && settingsManager.movementMethod == MovementMethod.UltimateFPS && settingsManager.disableFreeAimWhenDragging)
			{
				UltimateFPSIntegration.SetCameraState(true);
			}
		}

		private void Grab()
		{
			if ((bool)dragObject)
			{
				dragObject.LetGo();
				dragObject = null;
			}
			else if (canDragMoveable)
			{
				canDragMoveable = false;
				RaycastHit hitInfo = default(RaycastHit);
				Ray ray = Camera.main.ScreenPointToRay(mousePosition);
				if (Physics.Raycast(ray, out hitInfo, settingsManager.moveableRaycastLength) && (bool)hitInfo.transform.GetComponent<DragBase>())
				{
					dragObject = hitInfo.transform.GetComponent<DragBase>();
					dragObject.Grab(hitInfo.point);
					lastMousePosition = mousePosition;
					lastCameraPosition = KickStarter.mainCamera.transform.position;
				}
				if (settingsManager.movementMethod == MovementMethod.UltimateFPS && settingsManager.disableFreeAimWhenDragging)
				{
					UltimateFPSIntegration.SetCameraState(false);
				}
			}
		}

		private void Drag()
		{
			if (dragObject.invertInput)
			{
				dragForce = -KickStarter.mainCamera.transform.right * deltaDragMouse.x + -KickStarter.mainCamera.transform.up * deltaDragMouse.y;
			}
			else
			{
				dragForce = KickStarter.mainCamera.transform.right * deltaDragMouse.x + KickStarter.mainCamera.transform.up * deltaDragMouse.y;
			}
			float magnitude = (KickStarter.mainCamera.transform.position - dragObject.transform.position).magnitude;
			Vector3 vector = KickStarter.mainCamera.transform.position - lastCameraPosition;
			dragForce += vector * cameraInfluence;
			dragObject.ApplyDragForce(dragForce, mousePosition, magnitude);
			lastCameraPosition = KickStarter.mainCamera.transform.position;
		}

		public Vector2 GetDragVector()
		{
			if (dragState == DragState._Camera)
			{
				return deltaDragMouse;
			}
			return dragVector;
		}

		public void SetUpLock(bool state)
		{
			isUpLocked = state;
			if (settingsManager == null)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			if (settingsManager.movementMethod == MovementMethod.UltimateFPS)
			{
				UltimateFPSIntegration.SetMovementState(state);
			}
		}

		public bool ActiveArrowsDisablingHotspots()
		{
			if (activeArrows != null && activeArrows.disableHotspots)
			{
				return true;
			}
			return false;
		}

		public void StartRotatingObject()
		{
			if (cursorIsLocked)
			{
				ToggleCursor();
			}
		}

		private void ToggleCursor()
		{
			if (!cursorIsLocked)
			{
				cursorIsLocked = true;
			}
			else
			{
				cursorIsLocked = false;
			}
			if (settingsManager.movementMethod == MovementMethod.UltimateFPS)
			{
				UltimateFPSIntegration.SetCameraState(cursorIsLocked);
			}
		}

		public bool IsDragObjectHeld(DragBase _dragBase)
		{
			if (_dragBase == null || dragObject == null)
			{
				return false;
			}
			if (_dragBase == dragObject)
			{
				return true;
			}
			return false;
		}

		private float GetDeltaTime()
		{
			if (Time.deltaTime == 0f)
			{
				return 0.02f;
			}
			return Time.deltaTime;
		}

		private void OnDestroy()
		{
			stateHandler = null;
			runtimeInventory = null;
			settingsManager = null;
			actionListManager = null;
			playerInteraction = null;
		}

		public bool InputAnyKey()
		{
			if (menuButtonInput != null && menuButtonInput != string.Empty)
			{
				return true;
			}
			return Input.anyKey;
		}

		public bool InputGetButtonDown(string axis, bool showError = false)
		{
			if (axis == string.Empty)
			{
				return false;
			}
			try
			{
				if (Input.GetButtonDown(axis))
				{
					return true;
				}
			}
			catch
			{
				if (showError)
				{
					Debug.Log("Cannot find Input button '" + axis + "' - please define it in Unity's Input Manager (Edit -> Project settings -> Input).");
				}
			}
			if (menuButtonInput != string.Empty && menuButtonInput == axis && menuInput == SimulateInputType.Button)
			{
				if (menuButtonValue > 0f)
				{
					StopSimulatingInput();
					return true;
				}
				StopSimulatingInput();
			}
			return false;
		}
	}
}
