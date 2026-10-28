using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class Menu : ScriptableObject
	{
		public bool isEditing;

		public bool isLocked;

		public int id;

		public string title;

		public Vector2 manualSize = Vector2.zero;

		public AC_PositionType positionType;

		public Vector2 manualPosition = Vector2.zero;

		public TextAnchor alignment = TextAnchor.MiddleCenter;

		public string toggleKey = string.Empty;

		public bool ignoreMouseClicks;

		public bool pauseWhenEnabled;

		public bool enabledOnStart;

		public ActionListAsset actionListOnTurnOn;

		public ActionListAsset actionListOnTurnOff;

		public Texture2D backgroundTexture;

		public Texture2D sliderTexture;

		public List<MenuElement> visibleElements = new List<MenuElement>();

		public float transitionProgress;

		public AppearType appearType;

		public SpeechMenuType speechMenuType;

		public MenuElement selected_element;

		public int selected_slot;

		public List<MenuElement> elements = new List<MenuElement>();

		[SerializeField]
		private Vector2 biggestElementSize;

		public float spacing;

		private bool isEnabled;

		public AC_SizeType sizeType;

		public MenuOrientation orientation;

		[SerializeField]
		private Rect rect = default(Rect);

		public MenuTransition transitionType;

		public PanDirection panDirection;

		public PanMovement panMovement;

		public float panDistance = 0.5f;

		public float fadeSpeed = 0.5f;

		public TextAnchor zoomAnchor = TextAnchor.MiddleCenter;

		private bool isFading;

		private FadeType fadeType;

		private Vector2 panOffset = Vector2.zero;

		private Vector2 dragOffset = Vector2.zero;

		private float zoomAmount = 1f;

		public bool zoomElements;

		private Rect aspectCorrectedRect = default(Rect);

		private PlayerMenus playerMenus;

		public void Declare(int[] idArray)
		{
			spacing = 0.5f;
			orientation = MenuOrientation.Vertical;
			appearType = AppearType.Manual;
			elements = new List<MenuElement>();
			visibleElements = new List<MenuElement>();
			enabledOnStart = false;
			isEnabled = false;
			sizeType = AC_SizeType.Automatic;
			speechMenuType = SpeechMenuType.All;
			actionListOnTurnOn = null;
			actionListOnTurnOff = null;
			fadeSpeed = 0.5f;
			transitionType = MenuTransition.Fade;
			panDirection = PanDirection.Up;
			panMovement = PanMovement.Linear;
			panDistance = 0.5f;
			zoomAnchor = TextAnchor.MiddleCenter;
			zoomElements = false;
			ignoreMouseClicks = false;
			pauseWhenEnabled = false;
			id = 0;
			isLocked = false;
			foreach (int num in idArray)
			{
				if (id == num)
				{
					id++;
				}
			}
			title = "Menu " + (id + 1);
		}

		public void Copy(Menu _menu)
		{
			isEditing = false;
			id = _menu.id;
			isLocked = _menu.isLocked;
			title = _menu.title;
			manualSize = _menu.manualSize;
			positionType = _menu.positionType;
			manualPosition = _menu.manualPosition;
			alignment = _menu.alignment;
			toggleKey = _menu.toggleKey;
			backgroundTexture = _menu.backgroundTexture;
			sliderTexture = _menu.sliderTexture;
			visibleElements = new List<MenuElement>();
			transitionProgress = 0f;
			appearType = _menu.appearType;
			selected_element = null;
			selected_slot = 0;
			spacing = _menu.spacing;
			sizeType = _menu.sizeType;
			orientation = _menu.orientation;
			fadeSpeed = _menu.fadeSpeed;
			transitionType = _menu.transitionType;
			panDirection = _menu.panDirection;
			panMovement = _menu.panMovement;
			panDistance = _menu.panDistance;
			zoomAnchor = _menu.zoomAnchor;
			zoomElements = _menu.zoomElements;
			pauseWhenEnabled = _menu.pauseWhenEnabled;
			speechMenuType = _menu.speechMenuType;
			enabledOnStart = _menu.enabledOnStart;
			actionListOnTurnOn = _menu.actionListOnTurnOn;
			actionListOnTurnOff = _menu.actionListOnTurnOff;
			ignoreMouseClicks = _menu.ignoreMouseClicks;
			elements = new List<MenuElement>();
			foreach (MenuElement element in _menu.elements)
			{
				elements.Add(element.DuplicateSelf());
			}
			Recalculate();
			if (appearType == AppearType.Manual && _menu.enabledOnStart)
			{
				transitionProgress = 1f;
				TurnOn(false);
			}
			else
			{
				transitionProgress = 0f;
				TurnOff(false);
			}
			if (transitionType == MenuTransition.Zoom)
			{
				zoomAmount = 0f;
			}
			UpdateTransition();
		}

		public void DrawOutline(MenuElement _selectedElement)
		{
			DrawStraightLine.DrawBox(rect, Color.yellow, 1f, false, 1);
			foreach (MenuElement visibleElement in visibleElements)
			{
				if (visibleElement == _selectedElement)
				{
					visibleElement.DrawOutline(true, this);
				}
				visibleElement.DrawOutline(false, this);
			}
		}

		public void StartDisplay()
		{
			if (isFading)
			{
				GUI.BeginGroup(new Rect(dragOffset.x + panOffset.x + GetRect().x, dragOffset.y + panOffset.y + GetRect().y, GetRect().width * zoomAmount, GetRect().height * zoomAmount));
			}
			else
			{
				GUI.BeginGroup(new Rect(dragOffset.x + GetRect().x, dragOffset.y + GetRect().y, GetRect().width * zoomAmount, GetRect().height * zoomAmount));
			}
			if ((bool)backgroundTexture)
			{
				Rect position = new Rect(0f, 0f, rect.width, rect.height);
				GUI.DrawTexture(position, backgroundTexture, ScaleMode.StretchToFill, true, 0f);
			}
		}

		public void EndDisplay()
		{
			GUI.EndGroup();
		}

		public void SetPosition(Vector2 _position)
		{
			rect.x = _position.x * AdvGame.GetMainGameViewSize().x;
			rect.y = _position.y * AdvGame.GetMainGameViewSize().y;
			FitMenuInsideScreen();
			UpdateAspectRect();
		}

		public void SetCentre(Vector2 _position)
		{
			Vector2 vector = new Vector2(_position.x * AdvGame.GetMainGameViewSize().x, _position.y * AdvGame.GetMainGameViewSize().y);
			rect.x = vector.x - rect.width / 2f;
			rect.y = vector.y - rect.height / 2f;
			FitMenuInsideScreen();
			UpdateAspectRect();
		}

		private Vector2 GetCentre()
		{
			Vector2 zero = Vector2.zero;
			zero.x = (rect.x + rect.width / 2f) / AdvGame.GetMainGameViewSize().x * 100f;
			zero.y = (rect.y + rect.height / 2f) / AdvGame.GetMainGameViewSize().y * 100f;
			return zero;
		}

		private void FitMenuInsideScreen()
		{
			if (rect.x < 0f)
			{
				rect.x = 0f;
			}
			if (rect.y < 0f)
			{
				rect.y = 0f;
			}
			if (rect.x + rect.width > AdvGame.GetMainGameViewSize().x)
			{
				rect.x = AdvGame.GetMainGameViewSize().x - rect.width;
			}
			if (rect.y + rect.height > AdvGame.GetMainGameViewSize().y)
			{
				rect.y = AdvGame.GetMainGameViewSize().y - rect.height;
			}
		}

		public void Align(TextAnchor _anchor)
		{
			switch (_anchor)
			{
			case TextAnchor.UpperLeft:
			case TextAnchor.MiddleLeft:
			case TextAnchor.LowerLeft:
				rect.x = 0f;
				break;
			case TextAnchor.UpperCenter:
			case TextAnchor.MiddleCenter:
			case TextAnchor.LowerCenter:
				rect.x = (AdvGame.GetMainGameViewSize().x - rect.width) / 2f;
				break;
			default:
				rect.x = AdvGame.GetMainGameViewSize().x - rect.width;
				break;
			}
			switch (_anchor)
			{
			case TextAnchor.LowerLeft:
			case TextAnchor.LowerCenter:
			case TextAnchor.LowerRight:
				rect.y = AdvGame.GetMainGameViewSize().y - rect.height;
				break;
			case TextAnchor.MiddleLeft:
			case TextAnchor.MiddleCenter:
			case TextAnchor.MiddleRight:
				rect.y = (AdvGame.GetMainGameViewSize().y - rect.height) / 2f;
				break;
			default:
				rect.y = 0f;
				break;
			}
		}

		private void SetManualSize(Vector2 _size)
		{
			rect.width = _size.x * AdvGame.GetMainGameViewSize().x;
			rect.height = _size.y * AdvGame.GetMainGameViewSize().y;
		}

		public Rect GetRect()
		{
			if (!Application.isPlaying)
			{
				return MainCamera._LimitMenuToAspect(rect);
			}
			if (aspectCorrectedRect == default(Rect))
			{
				UpdateAspectRect();
			}
			return aspectCorrectedRect;
		}

		public void UpdateAspectRect()
		{
			aspectCorrectedRect = MainCamera._LimitMenuToAspect(rect);
		}

		public bool IsPointerOverSlot(MenuElement _element, int slot, Vector2 _pointer)
		{
			Rect slotRectRelative = _element.GetSlotRectRelative(slot);
			return GetRectAbsolute(slotRectRelative).Contains(_pointer);
		}

		public Rect GetRectAbsolute(Rect _rectRelative)
		{
			Rect result = new Rect(_rectRelative.x + dragOffset.x + GetRect().x, _rectRelative.y + dragOffset.y + GetRect().y, _rectRelative.width, _rectRelative.height);
			return result;
		}

		public void ResetVisibleElements()
		{
			visibleElements.Clear();
			foreach (MenuElement element in elements)
			{
				if (element.isVisible)
				{
					visibleElements.Add(element);
				}
			}
		}

		public void Recalculate()
		{
			ResetVisibleElements();
			PositionElements();
			if (sizeType == AC_SizeType.Automatic)
			{
				AutoResize();
			}
			else if (sizeType == AC_SizeType.Manual)
			{
				SetManualSize(new Vector2(manualSize.x / 100f, manualSize.y / 100f));
			}
			else if (sizeType == AC_SizeType.AbsolutePixels)
			{
				rect.width = manualSize.x;
				rect.height = manualSize.y;
			}
			if (positionType == AC_PositionType.Centred)
			{
				Centre();
				manualPosition = GetCentre();
			}
			else if (positionType == AC_PositionType.Aligned)
			{
				Align(alignment);
				manualPosition = GetCentre();
			}
			else if (positionType == AC_PositionType.Manual || !Application.isPlaying)
			{
				SetCentre(new Vector2(manualPosition.x / 100f, manualPosition.y / 100f));
			}
		}

		public void AutoResize()
		{
			visibleElements.Clear();
			biggestElementSize = default(Vector2);
			foreach (MenuElement element in elements)
			{
				if (!(element != null))
				{
					continue;
				}
				element.RecalculateSize();
				if (element.isVisible)
				{
					visibleElements.Add(element);
					if (element.GetSizeFromCorner().x > biggestElementSize.x)
					{
						biggestElementSize.x = element.GetSizeFromCorner().x;
					}
					if (element.GetSizeFromCorner().y > biggestElementSize.y)
					{
						biggestElementSize.y = element.GetSizeFromCorner().y;
					}
				}
			}
			rect.width = spacing / 100f * AdvGame.GetMainGameViewSize().x + biggestElementSize.x;
			rect.height = spacing / 100f * AdvGame.GetMainGameViewSize().x + biggestElementSize.y;
			manualSize = new Vector2(rect.width * 100f / AdvGame.GetMainGameViewSize().x, rect.height * 100f / AdvGame.GetMainGameViewSize().y);
		}

		private void PositionElements()
		{
			float num = 0f;
			foreach (MenuElement visibleElement in visibleElements)
			{
				if (visibleElement == null)
				{
					Debug.Log("Null element found");
					break;
				}
				if (visibleElement.positionType == AC_PositionType2.RelativeToMenuSize && sizeType == AC_SizeType.Automatic)
				{
					Debug.LogError("Menu " + title + " cannot display because it's size is Automatic, while it's Element " + visibleElement.title + "'s Position is set to Relative");
					break;
				}
				visibleElement.RecalculateSize();
				if (visibleElement.positionType == AC_PositionType2.RelativeToMenuSize)
				{
					visibleElement.SetRelativePosition(new Vector2(rect.width / 100f, rect.height / 100f));
				}
				else if (orientation == MenuOrientation.Horizontal)
				{
					if (visibleElement.positionType == AC_PositionType2.Aligned)
					{
						visibleElement.SetPosition(new Vector2(spacing / 100f * AdvGame.GetMainGameViewSize().x + num, spacing / 100f * AdvGame.GetMainGameViewSize().x));
					}
					num += visibleElement.GetSize().x + spacing / 100f * AdvGame.GetMainGameViewSize().x;
				}
				else
				{
					if (visibleElement.positionType == AC_PositionType2.Aligned)
					{
						visibleElement.SetPosition(new Vector2(spacing / 100f * AdvGame.GetMainGameViewSize().x, spacing / 100f * AdvGame.GetMainGameViewSize().x + num));
					}
					num += visibleElement.GetSize().y + spacing / 100f * AdvGame.GetMainGameViewSize().x;
				}
			}
		}

		public void Centre()
		{
			SetCentre(new Vector2(0.5f, 0.5f));
		}

		public bool IsEnabled()
		{
			if (isLocked)
			{
				if (isFading && fadeType == FadeType.fadeOut)
				{
					return isEnabled;
				}
				return false;
			}
			return isEnabled;
		}

		public bool IsVisible()
		{
			if (transitionProgress == 1f && isEnabled)
			{
				return true;
			}
			return false;
		}

		public void HandleTransition()
		{
			if (!isFading || !isEnabled)
			{
				return;
			}
			if (fadeType == FadeType.fadeIn)
			{
				transitionProgress += 0.2f * fadeSpeed;
				if (transitionProgress > 1f)
				{
					transitionProgress = 1f;
					UpdateTransition();
					EndTransitionOn();
				}
				else
				{
					UpdateTransition();
				}
			}
			else
			{
				transitionProgress -= 0.2f * fadeSpeed;
				if (transitionProgress < 0f)
				{
					transitionProgress = 0f;
					UpdateTransition();
					EndTransitionOff();
				}
				else
				{
					UpdateTransition();
				}
			}
		}

		private void EndTransitionOn()
		{
			transitionProgress = 1f;
			isEnabled = true;
			isFading = false;
		}

		private void EndTransitionOff()
		{
			transitionProgress = 0f;
			isFading = false;
			isEnabled = false;
			ReturnGameState();
			PlayerMenus component = GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>();
			component.CheckCrossfade(this);
		}

		public bool IsOn()
		{
			if (!isLocked && isEnabled && !isFading)
			{
				return true;
			}
			return false;
		}

		public bool IsOff()
		{
			if (isLocked)
			{
				return true;
			}
			if (!isEnabled)
			{
				return true;
			}
			return false;
		}

		public bool TurnOn(bool doFade)
		{
			if (IsOn())
			{
				return false;
			}
			if (transitionType == MenuTransition.None)
			{
				doFade = false;
			}
			if (playerMenus == null)
			{
				playerMenus = GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>();
			}
			playerMenus.UpdateMenuPosition(this, Vector2.zero);
			if (isLocked)
			{
				Debug.Log("Cannot turn on menu " + title + " as it is locked.");
			}
			else if (!isEnabled || (isFading && fadeType == FadeType.fadeOut && appearType == AppearType.OnHotspot))
			{
				if (positionType == AC_PositionType.AppearAtCursorAndFreeze && (bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>())
				{
					PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
					SetCentre(new Vector2(component.GetInvertedMouse().x / (float)Screen.width + (manualPosition.x - 50f) / 100f, component.GetInvertedMouse().y / (float)Screen.height + (manualPosition.y - 50f) / 100f));
				}
				selected_slot = -2;
				MenuSystem.OnMenuEnable(this);
				ChangeGameState();
				Recalculate();
				dragOffset = Vector2.zero;
				isEnabled = true;
				isFading = doFade;
				if (actionListOnTurnOn != null)
				{
					AdvGame.RunActionListAsset(actionListOnTurnOn);
				}
				if (doFade && fadeSpeed > 0f)
				{
					fadeType = FadeType.fadeIn;
				}
				else
				{
					transitionProgress = 1f;
					isEnabled = true;
					isFading = false;
				}
			}
			return true;
		}

		public bool IsFading()
		{
			return isFading;
		}

		public bool TurnOff(bool doFade)
		{
			if (IsOff())
			{
				return false;
			}
			if (actionListOnTurnOff != null)
			{
				AdvGame.RunActionListAsset(actionListOnTurnOff);
			}
			if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>())
			{
				PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
				if (appearType == AppearType.OnContainer)
				{
					component.activeContainer = null;
				}
			}
			if (transitionType == MenuTransition.None)
			{
				doFade = false;
			}
			if (isEnabled && (!isFading || (isFading && fadeType == FadeType.fadeIn && appearType == AppearType.OnHotspot)))
			{
				isFading = doFade;
				if (doFade && fadeSpeed > 0f)
				{
					fadeType = FadeType.fadeOut;
				}
				else
				{
					UpdateTransition();
					isFading = false;
					isEnabled = false;
					ReturnGameState();
				}
			}
			return true;
		}

		public void ForceOff()
		{
			if (isEnabled || isFading)
			{
				transitionProgress = 0f;
				UpdateTransition();
				isFading = false;
				isEnabled = false;
			}
		}

		public void UpdateTransition()
		{
			if (transitionType == MenuTransition.Fade)
			{
				return;
			}
			if (transitionType == MenuTransition.FadeAndPan || transitionType == MenuTransition.Pan)
			{
				float num = 0f;
				if (panMovement == PanMovement.Linear)
				{
					num = (1f - transitionProgress) * panDistance;
				}
				if (panMovement == PanMovement.Smooth)
				{
					num = (transitionProgress * transitionProgress - 2f * transitionProgress + 1f) * panDistance;
				}
				else if (panMovement == PanMovement.Overshoot)
				{
					num = (1.3333334f * transitionProgress * transitionProgress - 2.3333333f * transitionProgress + 1f) * panDistance;
				}
				if (panDirection == PanDirection.Down)
				{
					panOffset = new Vector2(0f, num);
				}
				else if (panDirection == PanDirection.Left)
				{
					panOffset = new Vector2(0f - num, 0f);
				}
				else if (panDirection == PanDirection.Up)
				{
					panOffset = new Vector2(0f, 0f - num);
				}
				else if (panDirection == PanDirection.Right)
				{
					panOffset = new Vector2(num, 0f);
				}
				panOffset = new Vector2(panOffset.x * AdvGame.GetMainGameViewSize().x, panOffset.y * AdvGame.GetMainGameViewSize().y);
			}
			else if (transitionType == MenuTransition.Zoom)
			{
				zoomAmount = transitionProgress;
				if (zoomAnchor == TextAnchor.UpperLeft)
				{
					panOffset = Vector2.zero;
				}
				else if (zoomAnchor == TextAnchor.UpperCenter)
				{
					panOffset = new Vector2((1f - zoomAmount) * rect.width / 2f, 0f);
				}
				else if (zoomAnchor == TextAnchor.UpperRight)
				{
					panOffset = new Vector2((1f - zoomAmount) * rect.width, 0f);
				}
				else if (zoomAnchor == TextAnchor.MiddleLeft)
				{
					panOffset = new Vector2(0f, (1f - zoomAmount) * rect.height / 2f);
				}
				else if (zoomAnchor == TextAnchor.MiddleCenter)
				{
					panOffset = new Vector2((1f - zoomAmount) * rect.width / 2f, (1f - zoomAmount) * rect.height / 2f);
				}
				else if (zoomAnchor == TextAnchor.MiddleRight)
				{
					panOffset = new Vector2((1f - zoomAmount) * rect.width, (1f - zoomAmount) * rect.height / 2f);
				}
				else if (zoomAnchor == TextAnchor.LowerLeft)
				{
					panOffset = new Vector2(0f, (1f - zoomAmount) * rect.height);
				}
				else if (zoomAnchor == TextAnchor.LowerCenter)
				{
					panOffset = new Vector2((1f - zoomAmount) * rect.width / 2f, (1f - zoomAmount) * rect.height);
				}
				else if (zoomAnchor == TextAnchor.LowerRight)
				{
					panOffset = new Vector2((1f - zoomAmount) * rect.width, (1f - zoomAmount) * rect.height);
				}
			}
		}

		private void ChangeGameState()
		{
			if (IsBlocking() && Application.isPlaying)
			{
				if (appearType != AppearType.OnInteraction)
				{
					GameObject.FindWithTag("GameEngine").GetComponent<PlayerInteraction>().DisableHotspot(true);
				}
				KickStarter.mainCamera.FadeIn(0f);
				GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>().gameState = GameState.Paused;
			}
		}

		private void ReturnGameState()
		{
			if (IsBlocking() && !playerMenus.ArePauseMenusOn(this) && Application.isPlaying)
			{
				GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>().RestoreLastNonPausedState();
			}
		}

		public bool IsBlocking()
		{
			if (pauseWhenEnabled && (appearType == AppearType.Manual || appearType == AppearType.OnInputKey || appearType == AppearType.OnContainer))
			{
				return true;
			}
			return false;
		}

		public void MatchInteractions(List<Button> buttons)
		{
			foreach (MenuElement element in elements)
			{
				if (element is MenuInteraction)
				{
					MenuInteraction menuInteraction = (MenuInteraction)element;
					menuInteraction.MatchInteractions(buttons);
				}
				else if (element is MenuInventoryBox)
				{
					Recalculate();
					element.AutoSetVisibility();
				}
			}
			Recalculate();
			Recalculate();
		}

		public void MatchInteractions(InvItem item)
		{
			foreach (MenuElement element in elements)
			{
				if (element is MenuInteraction)
				{
					MenuInteraction menuInteraction = (MenuInteraction)element;
					menuInteraction.MatchInteractions(item);
				}
				else if (element is MenuInventoryBox)
				{
					Recalculate();
					element.AutoSetVisibility();
				}
			}
			Recalculate();
			Recalculate();
		}

		public void HideInteractions()
		{
			foreach (MenuElement element in elements)
			{
				if (element is MenuInteraction)
				{
					element.isVisible = false;
					element.isClickable = false;
				}
			}
		}

		public void MatchLookInteraction(Button button)
		{
			CursorManager cursorManager = AdvGame.GetReferences().cursorManager;
			foreach (MenuElement element in elements)
			{
				if (element is MenuInteraction)
				{
					MenuInteraction menuInteraction = (MenuInteraction)element;
					menuInteraction.MatchLookInteraction(cursorManager.lookCursor_ID);
				}
			}
		}

		public void MatchUseInteraction(Button button)
		{
			foreach (MenuElement element in elements)
			{
				if (element is MenuInteraction)
				{
					MenuInteraction menuInteraction = (MenuInteraction)element;
					menuInteraction.MatchUseInteraction(button);
				}
			}
		}

		public void SetDragOffset(Vector2 pos, Rect dragRect)
		{
			if (pos.x < dragRect.x)
			{
				pos.x = dragRect.x;
			}
			else if (pos.x > dragRect.x + dragRect.width - GetRect().width)
			{
				pos.x = dragRect.x + dragRect.width - GetRect().width;
			}
			if (pos.y < dragRect.y)
			{
				pos.y = dragRect.y;
			}
			else if (pos.y > dragRect.y + dragRect.height - GetRect().height)
			{
				pos.y = dragRect.y + dragRect.height - GetRect().height;
			}
			dragOffset = pos;
			UpdateAspectRect();
		}

		public Vector2 GetDragStart()
		{
			return dragOffset;
		}

		public float GetZoom()
		{
			if (transitionType == MenuTransition.Zoom && zoomElements)
			{
				return zoomAmount;
			}
			return 1f;
		}

		public int ControlSelected(int selected_option)
		{
			if (selected_slot == -2)
			{
				selected_option = 0;
			}
			if (selected_option < 0)
			{
				selected_option = 0;
				selected_element = visibleElements[0];
				selected_slot = 0;
			}
			else
			{
				int num = 0;
				selected_slot = -1;
				int num2 = 0;
				int i = 0;
				for (num2 = 0; num2 < visibleElements.Count; num2++)
				{
					if (visibleElements[num2].isClickable)
					{
						for (i = 0; i < visibleElements[num2].GetNumSlots(); i++)
						{
							if (selected_option == num)
							{
								selected_slot = i;
								selected_element = visibleElements[num2];
								break;
							}
							num++;
						}
					}
					if (selected_slot != -1)
					{
						break;
					}
				}
				if (selected_slot == -1)
				{
					selected_slot = i - 1;
					selected_element = visibleElements[num2 - 1];
					selected_option = num - 1;
				}
			}
			return selected_option;
		}

		public MenuElement GetElementWithName(string menuElementName)
		{
			foreach (MenuElement element in elements)
			{
				if (element.title == menuElementName)
				{
					return element;
				}
			}
			return null;
		}

		public Vector2 GetSlotCentre(MenuElement _element, int slot)
		{
			foreach (MenuElement element in elements)
			{
				if (element == _element)
				{
					Rect slotRectRelative = _element.GetSlotRectRelative(slot);
					return new Vector2(GetRect().x + slotRectRelative.x + slotRectRelative.width / 2f, GetRect().y + slotRectRelative.y + slotRectRelative.height / 2f);
				}
			}
			return Vector2.zero;
		}
	}
}
