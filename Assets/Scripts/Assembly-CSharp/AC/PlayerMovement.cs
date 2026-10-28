using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class PlayerMovement : MonoBehaviour
	{
		private FirstPersonCamera firstPersonCamera;

		private PlayerInput playerInput;

		private PlayerMenus playerMenus;

		private PlayerCursor playerCursor;

		private PlayerInteraction playerInteraction;

		private SettingsManager settingsManager;

		private SceneSettings sceneSettings;

		private NavigationManager navigationManager;

		private RuntimeInventory runtimeInventory;

		private void Awake()
		{
			if ((bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().settingsManager)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			playerInput = GetComponent<PlayerInput>();
			playerInteraction = GetComponent<PlayerInteraction>();
			playerCursor = GetComponent<PlayerCursor>();
			sceneSettings = GetComponent<SceneSettings>();
			navigationManager = GetComponent<NavigationManager>();
		}

		private void Start()
		{
			AssignFPCamera();
			playerMenus = GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>();
			runtimeInventory = playerMenus.GetComponent<RuntimeInventory>();
		}

		public void AssignFPCamera()
		{
			if ((bool)GameObject.FindWithTag("FirstPersonCamera") && (bool)GameObject.FindWithTag("FirstPersonCamera").GetComponent<FirstPersonCamera>())
			{
				firstPersonCamera = GameObject.FindWithTag("FirstPersonCamera").GetComponent<FirstPersonCamera>();
			}
		}

		public void UpdatePlayerMovement()
		{
			if (!settingsManager || !KickStarter.player || !playerInput || !playerInteraction)
			{
				return;
			}
			if (settingsManager.movementMethod == MovementMethod.UltimateFPS)
			{
				UFPSControlPlayer();
			}
			else
			{
				if (!(playerInput.activeArrows == null))
				{
					return;
				}
				if (playerInput.mouseState == MouseState.SingleClick && !playerInput.interactionMenuIsOn && !playerInput.mouseOverMenu && !playerInteraction.IsMouseOverHotspot())
				{
					if (playerInput.hotspotMovingTo != null)
					{
						StopMovingToHotspot();
					}
					playerInteraction.DisableHotspot(false);
				}
				if (playerInput.hotspotMovingTo != null && settingsManager.movementMethod != MovementMethod.PointAndClick && playerInput.moveKeys != Vector2.zero)
				{
					StopMovingToHotspot();
				}
				if (settingsManager.movementMethod == MovementMethod.Direct)
				{
					if (settingsManager.inputMethod == InputMethod.TouchScreen)
					{
						DragPlayer(true);
					}
					else if (KickStarter.player.GetPath() == null || !KickStarter.player.lockedPath)
					{
						DirectControlPlayer(true);
					}
					else
					{
						DirectControlPlayerPath();
					}
				}
				else if (settingsManager.movementMethod == MovementMethod.Drag)
				{
					DragPlayer(true);
				}
				else if (settingsManager.movementMethod == MovementMethod.StraightToCursor)
				{
					MoveStraightToCursor();
				}
				else if (settingsManager.movementMethod == MovementMethod.PointAndClick)
				{
					PointControlPlayer();
				}
				else
				{
					if (settingsManager.movementMethod != MovementMethod.FirstPerson)
					{
						return;
					}
					if (settingsManager.inputMethod == InputMethod.TouchScreen)
					{
						FirstPersonControlPlayer();
						if (settingsManager.dragAffects == DragAffects.Movement)
						{
							DragPlayer(false);
						}
						else
						{
							DragPlayerLook();
						}
					}
					else
					{
						FirstPersonControlPlayer();
						DirectControlPlayer(false);
					}
				}
			}
		}

		private void MoveStraightToCursor()
		{
			if (playerInput.isDownLocked && playerInput.isUpLocked && playerInput.isLeftLocked && playerInput.isRightLocked)
			{
				if (KickStarter.player.charState == CharState.Move)
				{
					KickStarter.player.charState = CharState.Decelerate;
				}
				return;
			}
			if (playerInput.dragState == DragState.None)
			{
				playerInput.dragSpeed = 0f;
				if (KickStarter.player.charState == CharState.Move && KickStarter.player.GetPath() == null)
				{
					KickStarter.player.charState = CharState.Decelerate;
				}
			}
			if (playerInput.mouseState == MouseState.SingleClick && settingsManager.singleTapStraight)
			{
				Vector3 vector = ClickPoint(playerInput.GetMousePosition());
				Vector3 vector2 = vector - KickStarter.player.transform.position;
				if (!(vector != Vector3.zero))
				{
					return;
				}
				if (vector2.magnitude < settingsManager.destinationAccuracy / 2f)
				{
					if (KickStarter.player.charState == CharState.Move)
					{
						KickStarter.player.charState = CharState.Decelerate;
					}
				}
				else if (vector2.magnitude > settingsManager.destinationAccuracy)
				{
					if (settingsManager.IsUnity2D())
					{
						vector2 = new Vector3(vector2.x, 0f, vector2.y);
					}
					bool run = false;
					if (vector2.magnitude > settingsManager.dragRunThreshold)
					{
						run = true;
					}
					if (playerInput.runLock == PlayerMoveLock.AlwaysRun)
					{
						run = true;
					}
					else if (playerInput.runLock == PlayerMoveLock.AlwaysWalk)
					{
						run = false;
					}
					List<Vector3> list = new List<Vector3>();
					list.Add(vector);
					KickStarter.player.MoveAlongPoints(list.ToArray(), run);
				}
			}
			else
			{
				if (playerInput.dragState != DragState.Player || (settingsManager.singleTapStraight && !playerInput.CanClick()))
				{
					return;
				}
				Vector3 vector3 = ClickPoint(playerInput.GetMousePosition());
				Vector3 direction = vector3 - KickStarter.player.transform.position;
				if (vector3 != Vector3.zero)
				{
					if (direction.magnitude < settingsManager.destinationAccuracy / 2f)
					{
						if (KickStarter.player.charState == CharState.Move)
						{
							KickStarter.player.charState = CharState.Decelerate;
						}
						if ((bool)KickStarter.player.activePath)
						{
							KickStarter.player.EndPath();
						}
					}
					else if (direction.magnitude > settingsManager.destinationAccuracy)
					{
						if (settingsManager.IsUnity2D())
						{
							direction = new Vector3(direction.x, 0f, direction.y);
						}
						bool isRunning = false;
						if (direction.magnitude > settingsManager.dragRunThreshold)
						{
							isRunning = true;
						}
						if (playerInput.runLock == PlayerMoveLock.AlwaysRun)
						{
							isRunning = true;
						}
						else if (playerInput.runLock == PlayerMoveLock.AlwaysWalk)
						{
							isRunning = false;
						}
						KickStarter.player.isRunning = isRunning;
						KickStarter.player.charState = CharState.Move;
						KickStarter.player.SetLookDirection(direction, false);
						KickStarter.player.SetMoveDirectionAsForward();
						if ((bool)KickStarter.player.activePath)
						{
							KickStarter.player.EndPath();
						}
					}
				}
				else
				{
					if (KickStarter.player.charState == CharState.Move)
					{
						KickStarter.player.charState = CharState.Decelerate;
					}
					if ((bool)KickStarter.player.activePath)
					{
						KickStarter.player.EndPath();
					}
				}
			}
		}

		public Vector3 ClickPoint(Vector2 mousePosition)
		{
			if (sceneSettings.navigationMethod == AC_NavigationMethod.PolygonCollider)
			{
				RaycastHit2D raycastHit2D;
				if (Camera.main.orthographic)
				{
					raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y)), Vector2.zero, settingsManager.navMeshRaycastLength);
				}
				else
				{
					Vector3 position = mousePosition;
					position.z = KickStarter.player.transform.position.z - Camera.main.transform.position.z;
					raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero);
				}
				if (raycastHit2D.collider != null)
				{
					return raycastHit2D.point;
				}
			}
			else
			{
				Ray ray = Camera.main.ScreenPointToRay(mousePosition);
				RaycastHit hitInfo = default(RaycastHit);
				if ((bool)settingsManager && (bool)sceneSettings && Physics.Raycast(ray, out hitInfo, settingsManager.navMeshRaycastLength))
				{
					return hitInfo.point;
				}
			}
			return Vector3.zero;
		}

		private void DragPlayer(bool doRotation)
		{
			if (playerInput.dragState == DragState.None)
			{
				playerInput.dragSpeed = 0f;
				if (KickStarter.player.charState == CharState.Move)
				{
					KickStarter.player.charState = CharState.Decelerate;
				}
			}
			if (playerInput.dragState != DragState.Player)
			{
				return;
			}
			Vector3 zero = Vector3.zero;
			zero = ((!settingsManager.IsTopDown()) ? (playerInput.moveKeys.y * KickStarter.mainCamera.ForwardVector() + playerInput.moveKeys.x * KickStarter.mainCamera.RightVector()) : (playerInput.moveKeys.y * Vector3.forward + playerInput.moveKeys.x * Vector3.right));
			if (playerInput.dragSpeed > settingsManager.dragWalkThreshold * 10f)
			{
				KickStarter.player.isRunning = playerInput.isRunning;
				KickStarter.player.charState = CharState.Move;
				if (doRotation)
				{
					KickStarter.player.SetLookDirection(zero, false);
					KickStarter.player.SetMoveDirectionAsForward();
				}
				else if (playerInput.GetDragVector().y < 0f)
				{
					KickStarter.player.SetMoveDirectionAsForward();
				}
				else
				{
					KickStarter.player.SetMoveDirectionAsBackward();
				}
			}
			else if (KickStarter.player.charState == CharState.Move && playerInput.hotspotMovingTo == null)
			{
				KickStarter.player.charState = CharState.Decelerate;
			}
		}

		private void DirectControlPlayer(bool doRotation)
		{
			if (settingsManager.directMovementType == DirectMovementType.RelativeToCamera)
			{
				if (playerInput.moveKeys != Vector2.zero)
				{
					Vector3 zero = Vector3.zero;
					zero = ((!settingsManager.IsTopDown()) ? (playerInput.moveKeys.y * KickStarter.mainCamera.ForwardVector() + playerInput.moveKeys.x * KickStarter.mainCamera.RightVector()) : (playerInput.moveKeys.y * Vector3.forward + playerInput.moveKeys.x * Vector3.right));
					KickStarter.player.isRunning = playerInput.isRunning;
					KickStarter.player.charState = CharState.Move;
					if (doRotation)
					{
						KickStarter.player.SetLookDirection(zero, false);
						KickStarter.player.SetMoveDirectionAsForward();
					}
					else
					{
						KickStarter.player.SetMoveDirection(zero);
					}
				}
				else if (KickStarter.player.charState == CharState.Move && playerInput.hotspotMovingTo == null)
				{
					KickStarter.player.charState = CharState.Decelerate;
				}
			}
			else if (settingsManager.directMovementType == DirectMovementType.TankControls)
			{
				if (playerInput.moveKeys.x < -0.5f)
				{
					KickStarter.player.TankTurnLeft();
				}
				else if (playerInput.moveKeys.x > 0.5f)
				{
					KickStarter.player.TankTurnRight();
				}
				else
				{
					KickStarter.player.StopTurning();
				}
				if (playerInput.moveKeys.y > 0f)
				{
					KickStarter.player.isRunning = playerInput.isRunning;
					KickStarter.player.charState = CharState.Move;
					KickStarter.player.SetMoveDirectionAsForward();
				}
				else if (playerInput.moveKeys.y < 0f)
				{
					KickStarter.player.isRunning = playerInput.isRunning;
					KickStarter.player.charState = CharState.Move;
					KickStarter.player.SetMoveDirectionAsBackward();
				}
				else if (KickStarter.player.charState == CharState.Move)
				{
					KickStarter.player.charState = CharState.Decelerate;
					KickStarter.player.SetMoveDirectionAsForward();
				}
			}
		}

		private void UFPSControlPlayer()
		{
			if (playerInput.moveKeys != Vector2.zero)
			{
				KickStarter.player.isRunning = playerInput.isRunning;
				KickStarter.player.charState = CharState.Move;
			}
			else if (KickStarter.player.charState == CharState.Move)
			{
				KickStarter.player.charState = CharState.Decelerate;
			}
		}

		private void DirectControlPlayerPath()
		{
			if (playerInput.moveKeys != Vector2.zero)
			{
				Vector3 zero = Vector3.zero;
				zero = ((!settingsManager.IsTopDown()) ? (playerInput.moveKeys.y * KickStarter.mainCamera.ForwardVector() + playerInput.moveKeys.x * KickStarter.mainCamera.RightVector()) : (playerInput.moveKeys.y * Vector3.forward + playerInput.moveKeys.x * Vector3.right));
				if (Vector3.Dot(zero, KickStarter.player.GetMoveDirection()) > 0f)
				{
					KickStarter.player.isRunning = playerInput.isRunning;
					KickStarter.player.charState = CharState.Move;
				}
			}
			else if (KickStarter.player.charState == CharState.Move)
			{
				KickStarter.player.charState = CharState.Decelerate;
			}
		}

		private void PointControlPlayer()
		{
			if (playerInput.IsCursorLocked())
			{
				return;
			}
			if (playerInput.isDownLocked && playerInput.isUpLocked && playerInput.isLeftLocked && playerInput.isRightLocked)
			{
				if (KickStarter.player.GetPath() == null && KickStarter.player.charState == CharState.Move)
				{
					KickStarter.player.charState = CharState.Decelerate;
				}
			}
			else if ((playerInput.mouseState == MouseState.SingleClick || playerInput.mouseState == MouseState.DoubleClick) && !playerInput.interactionMenuIsOn && !playerInput.mouseOverMenu && !playerInteraction.IsMouseOverHotspot() && (bool)playerCursor)
			{
				if (playerCursor.GetSelectedCursor() < 0)
				{
					if ((settingsManager.doubleClickMovement && playerInput.mouseState == MouseState.SingleClick) || (runtimeInventory.selectedItem != null && !settingsManager.canMoveWhenActive))
					{
						return;
					}
					bool run = false;
					if (playerInput.mouseState == MouseState.DoubleClick && !settingsManager.doubleClickMovement)
					{
						run = true;
					}
					if (settingsManager.interactionMethod == AC_InteractionMethod.ChooseHotspotThenInteraction && playerMenus != null)
					{
						playerMenus.SetInteractionMenus(false);
					}
					if (RaycastNavMesh(playerInput.GetMousePosition(), run))
					{
						return;
					}
					Vector3 vector = playerInput.GetMousePosition();
					if ((float)Screen.height * settingsManager.walkableClickRange > 1f)
					{
						for (int i = 1; (float)i < (float)Screen.height * settingsManager.walkableClickRange && !RaycastNavMesh(new Vector2(vector.x, vector.y - (float)i), run) && !RaycastNavMesh(new Vector2(vector.x, vector.y + (float)i), run) && !RaycastNavMesh(new Vector2(vector.x - (float)i, vector.y), run) && !RaycastNavMesh(new Vector2(vector.x + (float)i, vector.y), run) && !RaycastNavMesh(new Vector2(vector.x - (float)i, vector.y - (float)i), run) && !RaycastNavMesh(new Vector2(vector.x + (float)i, vector.y - (float)i), run) && !RaycastNavMesh(new Vector2(vector.x - (float)i, vector.y + (float)i), run) && !RaycastNavMesh(new Vector2(vector.x + (float)i, vector.y + (float)i), run); i += 4)
						{
						}
					}
				}
				else if (settingsManager.interactionMethod == AC_InteractionMethod.ChooseInteractionThenHotspot && settingsManager.autoCycleWhenInteract)
				{
					playerCursor.ResetSelectedCursor();
				}
			}
			else if (KickStarter.player.GetPath() == null && KickStarter.player.charState == CharState.Move)
			{
				KickStarter.player.charState = CharState.Decelerate;
			}
		}

		private bool ProcessHit(Vector3 hitPoint, GameObject hitObject, bool run)
		{
			if (hitObject.layer != LayerMask.NameToLayer(settingsManager.navMeshLayer))
			{
				return false;
			}
			if (!run)
			{
				ShowClick(hitPoint);
			}
			if (playerInput.runLock == PlayerMoveLock.AlwaysRun)
			{
				run = true;
			}
			else if (playerInput.runLock == PlayerMoveLock.AlwaysWalk)
			{
				run = false;
			}
			Vector3[] pointsArray = navigationManager.navigationEngine.GetPointsArray(KickStarter.player.transform.position, hitPoint);
			KickStarter.player.MoveAlongPoints(pointsArray, run);
			return true;
		}

		private bool RaycastNavMesh(Vector3 mousePosition, bool run)
		{
			if (sceneSettings.navigationMethod == AC_NavigationMethod.PolygonCollider)
			{
				RaycastHit2D raycastHit2D;
				if (Camera.main.orthographic)
				{
					raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y)), Vector2.zero, settingsManager.navMeshRaycastLength);
				}
				else
				{
					Vector3 position = mousePosition;
					position.z = KickStarter.player.transform.position.z - Camera.main.transform.position.z;
					raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero);
				}
				if (raycastHit2D.collider != null)
				{
					return ProcessHit(raycastHit2D.point, raycastHit2D.collider.gameObject, run);
				}
			}
			else
			{
				Ray ray = Camera.main.ScreenPointToRay(mousePosition);
				RaycastHit hitInfo = default(RaycastHit);
				if ((bool)settingsManager && (bool)sceneSettings && Physics.Raycast(ray, out hitInfo, settingsManager.navMeshRaycastLength))
				{
					return ProcessHit(hitInfo.point, hitInfo.collider.gameObject, run);
				}
			}
			return false;
		}

		private void ShowClick(Vector3 clickPoint)
		{
			if ((bool)settingsManager && (bool)settingsManager.clickPrefab)
			{
				Object.Destroy(GameObject.Find(settingsManager.clickPrefab.name + "(Clone)"));
				Object.Instantiate(settingsManager.clickPrefab, clickPoint, Quaternion.identity);
			}
		}

		private void FirstPersonControlPlayer()
		{
			if ((bool)firstPersonCamera)
			{
				Vector2 freeAim = playerInput.freeAim;
				if (freeAim.magnitude > settingsManager.dragWalkThreshold / 10f)
				{
					freeAim.Normalize();
					freeAim *= settingsManager.dragWalkThreshold / 10f;
				}
				float y = KickStarter.player.transform.localEulerAngles.y + freeAim.x * firstPersonCamera.sensitivity.x;
				firstPersonCamera.rotationY -= freeAim.y * firstPersonCamera.sensitivity.y;
				KickStarter.player.transform.localEulerAngles = new Vector3(0f, y, 0f);
			}
			else
			{
				Debug.LogWarning("Could not find first person camera");
			}
		}

		private void DragPlayerLook()
		{
			if ((!playerInput.isDownLocked || !playerInput.isUpLocked || !playerInput.isLeftLocked || !playerInput.isRightLocked) && playerInput.mouseState != MouseState.Normal && !playerInput.mouseOverMenu && !playerInput.interactionMenuIsOn && (playerInput.mouseState == MouseState.RightClick || !playerInteraction.IsMouseOverHotspot()) && playerInput.mouseState == MouseState.SingleClick)
			{
				playerInteraction.DisableHotspot(false);
			}
		}

		private void StopMovingToHotspot()
		{
			playerInput.hotspotMovingTo = null;
			KickStarter.player.EndPath();
			KickStarter.player.ClearHeadTurnTarget(HeadFacing.Hotspot, false);
			playerInteraction.StopInteraction();
		}

		private void OnDestroy()
		{
			firstPersonCamera = null;
			playerInput = null;
			settingsManager = null;
			sceneSettings = null;
			playerMenus = null;
			navigationManager = null;
		}
	}
}
