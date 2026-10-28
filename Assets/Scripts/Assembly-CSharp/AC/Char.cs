using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class Char : MonoBehaviour
	{
		public AnimationEngine animationEngine = AnimationEngine.SpritesUnity;

		public AnimEngine animEngine;

		public TalkingAnimation talkingAnimation;

		public string speechLabel = string.Empty;

		public CursorIconBase portraitIcon = new CursorIconBase();

		public Color speechColor = Color.white;

		protected Quaternion newRotation;

		private float prevHeight;

		private float prevHeight2;

		public float heightChange;

		private Shapeable shapeable;

		private List<LipSyncShape> lipSyncShapes = new List<LipSyncShape>();

		public bool isLipSyncing;

		public string phonemeParameter = string.Empty;

		public int lipSyncGroupID;

		public Transform leftHandBone;

		public Transform rightHandBone;

		public AnimationClip idleAnim;

		public AnimationClip walkAnim;

		public AnimationClip runAnim;

		public AnimationClip talkAnim;

		public AnimationClip turnLeftAnim;

		public AnimationClip turnRightAnim;

		public AnimationClip headLookLeftAnim;

		public AnimationClip headLookRightAnim;

		public AnimationClip headLookUpAnim;

		public AnimationClip headLookDownAnim;

		public Animation _animation;

		public Transform upperBodyBone;

		public Transform leftArmBone;

		public Transform rightArmBone;

		public Transform neckBone;

		public float animCrossfadeSpeed = 0.2f;

		public string moveSpeedParameter = "Speed";

		public string verticalMovementParameter = string.Empty;

		public string turnParameter = string.Empty;

		public string talkParameter = "IsTalking";

		public string directionParameter = "Direction";

		public string angleParameter = "Angle";

		public string headYawParameter = string.Empty;

		public string headPitchParameter = string.Empty;

		public bool relyOnRootMotion;

		public string lastPlayedAnim = string.Empty;

		public Animator animator;

		public Transform spriteChild;

		public string idleAnimSprite = "idle";

		public string walkAnimSprite = "walk";

		public string runAnimSprite = "run";

		public string talkAnimSprite = "talk";

		public bool lockScale;

		public float spriteScale = 1f;

		public bool lockDirection;

		public string spriteDirection = "D";

		private float spriteAngle;

		public bool doDirections = true;

		public bool crossfadeAnims;

		public bool doDiagonals;

		public bool isTalking;

		public AC_2DFrameFlipping frameFlipping;

		public bool flipCustomAnims;

		private Vector3 originalScale;

		private bool flipFrames;

		public float walkSpeedScale = 2f;

		public float runSpeedScale = 6f;

		public float turnSpeed = 7f;

		public float acceleration = 6f;

		public float deceleration;

		public float sortingMapScale = 1f;

		public bool isReversing;

		public bool turnBeforeWalking;

		public bool isJumping;

		public bool ignoreGravity;

		public bool freezeRigidbodyWhenIdle;

		protected Rigidbody _rigidbody;

		private Rigidbody2D _rigidbody2D;

		public Collider _collider;

		public AudioClip walkSound;

		public AudioClip runSound;

		public Sound soundChild;

		protected AudioSource audioSource;

		public Paths activePath;

		public CharState charState;

		protected float moveSpeed;

		protected Vector3 moveDirection;

		protected int targetNode;

		protected bool pausePath;

		private Vector3 lookDirection;

		private float pausePathTime;

		private ActionList nodeActionList;

		private int prevNode;

		private int lastPathPrevNode;

		private int lastPathTargetNode;

		private Paths lastPathActivePath;

		private bool tankTurning;

		protected bool isTurning;

		protected bool isTurningLeft;

		private Vector2 targetHeadAngles;

		private Vector2 actualHeadAngles;

		public Vector3 headTurnTarget;

		public HeadFacing headFacing;

		protected StateHandler stateHandler;

		protected SettingsManager settingsManager;

		private ActionListManager actionListManager;

		private SpeechManager speechManager;

		public bool isRunning { get; set; }

		protected void _Awake()
		{
			originalScale = transform.localScale;
			charState = CharState.Idle;
			shapeable = GetShapeable();
			ResetAnimationEngine();
			ResetBaseClips();
			GetSettingsManager();
			if ((bool)spriteChild && (bool)spriteChild.GetComponent<Animator>())
			{
				animator = spriteChild.GetComponent<Animator>();
			}
			if ((bool)soundChild && (bool)soundChild.gameObject.GetComponent<AudioSource>())
			{
				audioSource = soundChild.gameObject.GetComponent<AudioSource>();
			}
			if ((bool)GetComponent<Animator>())
			{
				animator = GetComponent<Animator>();
			}
			if ((bool)GetComponent<Animation>())
			{
				_animation = GetComponent<Animation>();
			}
			if ((bool)GetComponent<Rigidbody>())
			{
				_rigidbody = GetComponent<Rigidbody>();
			}
			else if ((bool)GetComponent<Rigidbody2D>())
			{
				_rigidbody2D = GetComponent<Rigidbody2D>();
			}
			if ((bool)GetComponent<Collider>())
			{
				_collider = GetComponent<Collider>();
			}
		}

		private void Start()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
			{
				stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
			}
		}

		protected void GetSettingsManager()
		{
			if ((bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().settingsManager)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			if ((bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().speechManager)
			{
				speechManager = AdvGame.GetReferences().speechManager;
			}
		}

		protected void _Update()
		{
			UpdateHeadTurnAngle();
			CalcHeightChange();
			if (settingsManager == null)
			{
				GetSettingsManager();
			}
			if (settingsManager != null && (bool)spriteChild)
			{
				UpdateSpriteChild(settingsManager.IsTopDown(), settingsManager.IsUnity2D());
			}
			AnimUpdate();
			SpeedUpdate();
		}

		protected void _FixedUpdate()
		{
			if (settingsManager == null)
			{
				GetSettingsManager();
			}
			PathUpdate();
			PhysicsUpdate();
			MoveUpdate();
		}

		protected void PathUpdate()
		{
			if (!activePath || activePath.nodes.Count <= 0)
			{
				return;
			}
			if (pausePath)
			{
				if (nodeActionList != null)
				{
					if (actionListManager == null)
					{
						actionListManager = GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>();
					}
					if (!actionListManager.IsListRunning(nodeActionList))
					{
						SetNextNodes();
					}
				}
				else if (Time.time > pausePathTime)
				{
					SetNextNodes();
				}
				return;
			}
			Vector3 vector = activePath.nodes[targetNode] - transform.position;
			Vector3 direction = new Vector3(vector.x, 0f, vector.z);
			if ((bool)settingsManager && settingsManager.IsUnity2D())
			{
				vector.z = 0f;
				SetMoveDirection(vector);
				direction = new Vector3(vector.x, 0f, vector.y);
				SetLookDirection(direction, false);
			}
			else if (activePath.affectY)
			{
				SetMoveDirection(vector);
				SetLookDirection(direction, false);
			}
			else
			{
				SetLookDirection(direction, false);
				SetMoveDirectionAsForward();
			}
			float num = 0.1f;
			if ((bool)settingsManager)
			{
				num = 1.05f - settingsManager.destinationAccuracy;
			}
			if (isRunning)
			{
				num *= runSpeedScale / walkSpeedScale;
			}
			if ((!settingsManager.IsUnity2D() || !(vector.magnitude < num)) && (!activePath.affectY || !(vector.magnitude < num)) && (activePath.affectY || !(direction.magnitude < num)))
			{
				return;
			}
			if (targetNode == 0 && prevNode == 0)
			{
				SetNextNodes();
			}
			else if (activePath.nodePause > 0f)
			{
				PausePath(activePath.nodePause);
			}
			else if (activePath.nodeCommands.Count > targetNode)
			{
				if (activePath.commandSource == ActionListSource.InScene && activePath.nodeCommands[targetNode].cutscene != null)
				{
					PausePath(activePath.nodeCommands[targetNode].cutscene, activePath.nodeCommands[targetNode].parameterID);
				}
				else if (activePath.commandSource == ActionListSource.AssetFile && activePath.nodeCommands[targetNode].actionListAsset != null)
				{
					PausePath(activePath.nodeCommands[targetNode].actionListAsset, activePath.nodeCommands[targetNode].parameterID);
				}
				else
				{
					SetNextNodes();
				}
			}
			else
			{
				SetNextNodes();
			}
		}

		private void SpeedUpdate()
		{
			if (charState == CharState.Move)
			{
				Accelerate();
			}
			else if (charState == CharState.Decelerate || charState == CharState.Custom)
			{
				Decelerate();
			}
			else if (charState == CharState.Idle && moveSpeed > 0f)
			{
				moveSpeed = 0f;
			}
		}

		private void PhysicsUpdate()
		{
			if ((bool)_rigidbody)
			{
				if (ignoreGravity)
				{
					_rigidbody.useGravity = false;
				}
				else if (charState == CharState.Custom && moveSpeed < 0.01f)
				{
					_rigidbody.useGravity = false;
				}
				else if ((bool)activePath && activePath.affectY)
				{
					_rigidbody.useGravity = false;
				}
				else
				{
					_rigidbody.useGravity = true;
				}
			}
			else if ((bool)_rigidbody2D)
			{
				if (ignoreGravity)
				{
					_rigidbody2D.gravityScale = 0f;
				}
				else if (charState == CharState.Custom && moveSpeed < 0.01f)
				{
					_rigidbody2D.gravityScale = 0f;
				}
				else if ((bool)activePath && activePath.affectY)
				{
					_rigidbody2D.gravityScale = 0f;
				}
				else
				{
					_rigidbody2D.gravityScale = 1f;
				}
			}
		}

		private void AnimUpdate()
		{
			if (isTalking)
			{
				ProcessLipSync();
			}
			if (isJumping)
			{
				animEngine.PlayJump();
				StopStandardAudio();
			}
			else if (charState == CharState.Idle || charState == CharState.Decelerate)
			{
				if (isTurning)
				{
					if (isTurningLeft)
					{
						animEngine.PlayTurnLeft();
					}
					else if (!isTurningLeft)
					{
						animEngine.PlayTurnRight();
					}
					else
					{
						animEngine.PlayIdle();
					}
				}
				else if (isTalking && talkingAnimation == TalkingAnimation.Standard)
				{
					animEngine.PlayTalk();
				}
				else
				{
					animEngine.PlayIdle();
				}
				StopStandardAudio();
			}
			else if (charState == CharState.Move)
			{
				if (isRunning)
				{
					animEngine.PlayRun();
				}
				else
				{
					animEngine.PlayWalk();
				}
				PlayStandardAudio();
			}
			else
			{
				StopStandardAudio();
			}
			animEngine.PlayVertical();
		}

		private void MoveUpdate()
		{
			if ((bool)animEngine)
			{
				if (moveSpeed > 0.01f && (!animEngine.rootMotion || !relyOnRootMotion))
				{
					Vector3 vector = moveDirection * moveSpeed * walkSpeedScale * sortingMapScale;
					if ((bool)settingsManager)
					{
						if (settingsManager.IsTopDown())
						{
							vector.z *= settingsManager.verticalReductionFactor;
						}
						else if (settingsManager.IsUnity2D())
						{
							vector.y *= settingsManager.verticalReductionFactor;
							vector.z = 0f;
						}
					}
					if (DoRigidbodyMovement())
					{
						_rigidbody.MovePosition(_rigidbody.position + vector * Time.deltaTime);
					}
					else
					{
						transform.position += vector * Time.deltaTime;
					}
				}
				if (isTurning)
				{
					if ((bool)animEngine && animEngine.turningIsLinear && moveSpeed > 0.01f && !IsUFPSPlayer())
					{
						Turn(true);
					}
					else
					{
						Turn(false);
					}
				}
			}
			if ((bool)_rigidbody)
			{
				if (freezeRigidbodyWhenIdle && !isJumping && (charState == CharState.Custom || charState == CharState.Idle))
				{
					_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
				}
				else
				{
					_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
				}
			}
		}

		protected virtual void Accelerate()
		{
			moveSpeed = Mathf.Lerp(b: GetComponent<Animator>() ? ((!isRunning) ? walkSpeedScale : runSpeedScale) : ((!isRunning) ? moveDirection.magnitude : (moveDirection.magnitude * runSpeedScale / walkSpeedScale)), a: moveSpeed, t: Time.deltaTime * acceleration);
		}

		public void SetMoveSpeed(float newSpeed)
		{
			moveSpeed = newSpeed;
		}

		private void Decelerate()
		{
			if (deceleration <= 0f)
			{
				moveSpeed = Mathf.Lerp(moveSpeed, 0f, Time.deltaTime * acceleration);
			}
			else
			{
				moveSpeed = Mathf.Lerp(moveSpeed, 0f, Time.deltaTime * deceleration);
			}
			if (moveSpeed < 0.01f)
			{
				moveSpeed = 0f;
				if (charState != CharState.Custom)
				{
					charState = CharState.Idle;
				}
			}
		}

		public bool IsTurning()
		{
			return isTurning;
		}

		public void TankTurnLeft()
		{
			lookDirection = -transform.right;
			tankTurning = true;
			isTurning = true;
		}

		public void TankTurnRight()
		{
			lookDirection = transform.right;
			tankTurning = true;
			isTurning = true;
		}

		public void StopTurning()
		{
			lookDirection = transform.forward;
			tankTurning = false;
			isTurning = false;
		}

		protected bool IsUFPSPlayer()
		{
			if (this is Player && settingsManager.movementMethod == MovementMethod.UltimateFPS)
			{
				return true;
			}
			return false;
		}

		public void Teleport(Vector3 _position)
		{
			if (IsUFPSPlayer())
			{
				UltimateFPSIntegration.Teleport(_position);
			}
			else
			{
				transform.position = _position;
			}
		}

		public void SetRotation(Quaternion _rotation)
		{
			transform.rotation = _rotation;
			SetLookDirection(transform.forward, true);
		}

		public void SetRotation(float angle)
		{
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
			SetLookDirection(transform.forward, true);
		}

		public void Turn(bool isInstant)
		{
			if (!(lookDirection != Vector3.zero))
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(lookDirection, Vector3.up);
			if (isInstant)
			{
				isTurning = false;
				if (IsUFPSPlayer())
				{
					UltimateFPSIntegration.SetRotation(quaternion.eulerAngles);
				}
				else if (DoRigidbodyMovement())
				{
					_rigidbody.rotation = quaternion;
				}
				else
				{
					transform.rotation = quaternion;
				}
				if (settingsManager != null && (bool)spriteChild)
				{
					UpdateSpriteChild(settingsManager.IsTopDown(), settingsManager.IsUnity2D());
				}
				return;
			}
			if ((bool)animEngine && animEngine.turningIsLinear)
			{
				if (DoRigidbodyMovement())
				{
					newRotation = Quaternion.RotateTowards(_rigidbody.rotation, quaternion, 5f);
				}
				else
				{
					newRotation = Quaternion.RotateTowards(transform.rotation, quaternion, 5f);
				}
			}
			else
			{
				float num = turnSpeed * Time.deltaTime;
				if (tankTurning || moveSpeed == 0f)
				{
					num /= 2f;
				}
				if (DoRigidbodyMovement())
				{
					newRotation = Quaternion.Lerp(_rigidbody.rotation, quaternion, num);
				}
				else
				{
					newRotation = Quaternion.Lerp(transform.rotation, quaternion, num);
				}
			}
			isTurning = true;
			if (!IsUFPSPlayer())
			{
				if (DoRigidbodyMovement())
				{
					_rigidbody.MoveRotation(newRotation);
				}
				else
				{
					transform.rotation = newRotation;
				}
			}
			Vector3 vector = quaternion * Vector3.forward;
			Vector3 vector2 = transform.rotation * Vector3.forward;
			float current = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
			float target = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
			float num2 = Mathf.DeltaAngle(current, target);
			if (num2 < 0f)
			{
				isTurningLeft = false;
			}
			else
			{
				isTurningLeft = true;
			}
			if (isTurning && Quaternion.Angle(Quaternion.LookRotation(lookDirection), transform.rotation) < 3f)
			{
				isTurning = false;
			}
		}

		public void SetLookDirection(Vector3 _direction, bool isInstant)
		{
			lookDirection = new Vector3(_direction.x, 0f, _direction.z);
			Turn(isInstant);
		}

		public void SetMoveDirection(Vector3 _direction)
		{
			if (_direction != Vector3.zero)
			{
				Quaternion quaternion = Quaternion.LookRotation(_direction, Vector3.up);
				moveDirection = quaternion * Vector3.forward;
				moveDirection.Normalize();
			}
		}

		public void SetMoveDirectionAsForward()
		{
			isReversing = false;
			moveDirection = transform.forward;
			if ((bool)settingsManager && settingsManager.IsUnity2D())
			{
				moveDirection = new Vector3(moveDirection.x, moveDirection.z, 0f);
			}
			moveDirection.Normalize();
		}

		public void SetMoveDirectionAsBackward()
		{
			isReversing = true;
			moveDirection = -transform.forward;
			if ((bool)settingsManager && settingsManager.IsUnity2D())
			{
				moveDirection = new Vector3(moveDirection.x, moveDirection.z, 0f);
			}
			moveDirection.Normalize();
		}

		public Vector3 GetMoveDirection()
		{
			return moveDirection;
		}

		private void SetNextNodes()
		{
			pausePath = false;
			nodeActionList = null;
			int num = targetNode;
			if ((bool)GetComponent<Player>() && stateHandler.gameState == GameState.Normal)
			{
				targetNode = activePath.GetNextNode(targetNode, prevNode, true);
			}
			else
			{
				targetNode = activePath.GetNextNode(targetNode, prevNode, false);
			}
			prevNode = num;
			if (targetNode == 0 && activePath.pathType == AC_PathType.Loop && activePath.teleportToStart)
			{
				Teleport(activePath.transform.position);
				if (activePath.nodes.Count > 1)
				{
					SetLookDirection(activePath.nodes[1] - activePath.nodes[0], true);
				}
				SetNextNodes();
			}
			else if (targetNode == -1)
			{
				EndPath();
			}
		}

		public void EndPath()
		{
			if ((bool)GetComponent<Paths>() && activePath == GetComponent<Paths>())
			{
				activePath.nodes.Clear();
			}
			else
			{
				lastPathPrevNode = prevNode;
				lastPathTargetNode = targetNode;
				lastPathActivePath = activePath;
			}
			activePath = null;
			targetNode = 0;
			if (charState == CharState.Move)
			{
				charState = CharState.Decelerate;
			}
		}

		public void ResumeLastPath()
		{
			if (lastPathActivePath != null)
			{
				SetPath(lastPathActivePath, lastPathTargetNode, lastPathPrevNode);
			}
		}

		public void SetLastPath(Paths _lastPathActivePath, int _lastPathTargetNode, int _lastPathPrevNode)
		{
			lastPathActivePath = _lastPathActivePath;
			lastPathTargetNode = _lastPathTargetNode;
			lastPathPrevNode = _lastPathPrevNode;
		}

		public void Halt()
		{
			if (!GetComponent<Paths>() || !(activePath == GetComponent<Paths>()))
			{
				lastPathPrevNode = prevNode;
				lastPathTargetNode = targetNode;
				lastPathActivePath = activePath;
			}
			activePath = null;
			targetNode = 0;
			moveSpeed = 0f;
			if (charState == CharState.Move || charState == CharState.Decelerate)
			{
				charState = CharState.Idle;
			}
		}

		protected void ReverseDirection()
		{
			int num = targetNode;
			targetNode = prevNode;
			prevNode = num;
		}

		private void PausePath(float pauseTime)
		{
			charState = CharState.Decelerate;
			pausePath = true;
			pausePathTime = Time.time + pauseTime;
			nodeActionList = null;
		}

		private void PausePath(Cutscene pauseCutscene, int parameterID)
		{
			charState = CharState.Decelerate;
			pausePath = true;
			pausePathTime = 0f;
			if (pauseCutscene.useParameters && parameterID >= 0 && pauseCutscene.parameters.Count > parameterID)
			{
				pauseCutscene.parameters[parameterID].SetValue(gameObject);
			}
			pauseCutscene.Interact();
			nodeActionList = pauseCutscene;
		}

		private void PausePath(ActionListAsset pauseAsset, int parameterID)
		{
			charState = CharState.Decelerate;
			pausePath = true;
			pausePathTime = 0f;
			if (pauseAsset.useParameters && parameterID >= 0 && pauseAsset.parameters.Count > parameterID)
			{
				int value = 0;
				if ((bool)gameObject.GetComponent<ConstantID>())
				{
					value = gameObject.GetComponent<ConstantID>().constantID;
				}
				else
				{
					Debug.LogWarning(gameObject.name + " requires a ConstantID script component!");
				}
				pauseAsset.parameters[parameterID].SetValue(value);
			}
			nodeActionList = AdvGame.RunActionListAsset(pauseAsset);
		}

		private void TurnBeforeWalking()
		{
			Vector3 vector = activePath.nodes[1] - transform.position;
			if ((bool)settingsManager && settingsManager.IsUnity2D())
			{
				SetLookDirection(new Vector3(vector.x, 0f, vector.y), false);
			}
			else
			{
				SetLookDirection(new Vector3(vector.x, 0f, vector.z), false);
			}
		}

		protected bool IsTurningBeforeWalking()
		{
			if (isTurning && CanTurnBeforeMoving())
			{
				return true;
			}
			return false;
		}

		private bool CanTurnBeforeMoving()
		{
			if (turnBeforeWalking && activePath == GetComponent<Paths>() && targetNode <= 1 && activePath.nodes.Count > 1)
			{
				return true;
			}
			return false;
		}

		private void SetPath(Paths pathOb, PathSpeed _speed, int _targetNode, int _prevNode)
		{
			activePath = pathOb;
			targetNode = _targetNode;
			prevNode = _prevNode;
			if (CanTurnBeforeMoving())
			{
				TurnBeforeWalking();
			}
			if ((bool)pathOb)
			{
				if (_speed == PathSpeed.Run)
				{
					isRunning = true;
				}
				else
				{
					isRunning = false;
				}
			}
			charState = CharState.Idle;
		}

		public void SetPath(Paths pathOb, PathSpeed _speed)
		{
			SetPath(pathOb, _speed, 0, 0);
		}

		public void SetPath(Paths pathOb)
		{
			SetPath(pathOb, pathOb.pathSpeed, 0, 0);
		}

		public void SetPath(Paths pathOb, int _targetNode, int _prevNode)
		{
			SetPath(pathOb, pathOb.pathSpeed, _targetNode, _prevNode);
		}

		protected void CheckIfStuck()
		{
		}

		public Paths GetPath()
		{
			return activePath;
		}

		public Paths GetPath(bool isCurrent)
		{
			if (isCurrent)
			{
				return activePath;
			}
			return lastPathActivePath;
		}

		public int GetTargetNode(bool isCurrent)
		{
			if (isCurrent)
			{
				return targetNode;
			}
			return lastPathTargetNode;
		}

		public int GetPrevNode(bool isCurrent)
		{
			if (isCurrent)
			{
				return prevNode;
			}
			return lastPathPrevNode;
		}

		public void MoveToPoint(Vector3 point, bool run)
		{
			List<Vector3> list = new List<Vector3>();
			list.Add(point);
			MoveAlongPoints(list.ToArray(), run);
		}

		protected void ChangePathfindSpeed(bool run)
		{
			Paths component = GetComponent<Paths>();
			if ((bool)component)
			{
				if (run && !isRunning)
				{
					SetPath(component, PathSpeed.Run);
				}
				else if (!run && isRunning)
				{
					SetPath(component, PathSpeed.Walk);
				}
			}
		}

		public void MoveAlongPoints(Vector3[] pointData, bool run)
		{
			Paths component = GetComponent<Paths>();
			if ((bool)component)
			{
				component.BuildNavPath(pointData);
				if (run)
				{
					SetPath(component, PathSpeed.Run);
				}
				else
				{
					SetPath(component, PathSpeed.Walk);
				}
			}
			else
			{
				Debug.LogWarning(name + " cannot pathfind without a Paths component");
			}
		}

		public void ResetBaseClips()
		{
			if ((bool)spriteChild && (bool)spriteChild.GetComponent<Animation>())
			{
				List<string> list = new List<string>();
				foreach (AnimationState item in spriteChild.GetComponent<Animation>())
				{
					if ((idleAnim == null || item.name != idleAnim.name) && (walkAnim == null || item.name != walkAnim.name) && (runAnim == null || item.name != runAnim.name))
					{
						list.Add(item.name);
					}
				}
				foreach (string item2 in list)
				{
					spriteChild.GetComponent<Animation>().RemoveClip(item2);
				}
			}
			if (!_animation)
			{
				return;
			}
			List<string> list2 = new List<string>();
			foreach (AnimationState item3 in _animation)
			{
				if ((idleAnim == null || item3.name != idleAnim.name) && (walkAnim == null || item3.name != walkAnim.name) && (runAnim == null || item3.name != runAnim.name))
				{
					list2.Add(item3.name);
				}
			}
			foreach (string item4 in list2)
			{
				_animation.RemoveClip(item4);
			}
		}

		public float GetSpriteAngle()
		{
			return spriteAngle;
		}

		public string GetSpriteDirection()
		{
			return "_" + spriteDirection;
		}

		public int GetSpriteDirectionInt()
		{
			if (spriteDirection == "D")
			{
				return 0;
			}
			if (spriteDirection == "L")
			{
				return 1;
			}
			if (spriteDirection == "R")
			{
				return 2;
			}
			if (spriteDirection == "U")
			{
				return 3;
			}
			if (spriteDirection == "DL")
			{
				return 4;
			}
			if (spriteDirection == "DR")
			{
				return 5;
			}
			if (spriteDirection == "UL")
			{
				return 6;
			}
			if (spriteDirection == "UR")
			{
				return 7;
			}
			return 0;
		}

		public void SetSpriteDirection(CharDirection direction)
		{
			switch (direction)
			{
			case CharDirection.Down:
				spriteDirection = "D";
				break;
			case CharDirection.Left:
				spriteDirection = "L";
				break;
			case CharDirection.Right:
				spriteDirection = "R";
				break;
			case CharDirection.Up:
				spriteDirection = "U";
				break;
			case CharDirection.DownLeft:
				spriteDirection = "DL";
				break;
			case CharDirection.DownRight:
				spriteDirection = "DR";
				break;
			case CharDirection.UpLeft:
				spriteDirection = "UL";
				break;
			case CharDirection.UpRight:
				spriteDirection = "UR";
				break;
			}
		}

		private string SetSpriteDirection(float rightAmount, float forwardAmount)
		{
			float num = Vector2.Angle(new Vector2(1f, 0f), new Vector2(rightAmount, forwardAmount));
			if (doDiagonals)
			{
				if (forwardAmount > 0f)
				{
					if (num > 22.5f && num < 67.5f)
					{
						return "UR";
					}
					if (num > 112.5f && num < 157.5f)
					{
						return "UL";
					}
				}
				else
				{
					if (num > 22.5f && num < 67.55f)
					{
						return "DR";
					}
					if (num > 112.5f && num < 157.5f)
					{
						return "DL";
					}
				}
			}
			if (forwardAmount > 0f)
			{
				if (num > 45f && num < 135f)
				{
					return "U";
				}
			}
			else if (num > 45f && num < 135f)
			{
				return "D";
			}
			if (rightAmount > 0f)
			{
				return "R";
			}
			return "L";
		}

		private void CalcHeightChange()
		{
			float y = transform.position.y;
			if (y != prevHeight && y != prevHeight2 && prevHeight != prevHeight2)
			{
				heightChange = y - prevHeight;
			}
			else
			{
				heightChange = 0f;
			}
			prevHeight2 = prevHeight;
			prevHeight = y;
		}

		protected void StopStandardAudio()
		{
			if ((bool)audioSource && audioSource.isPlaying && (((bool)runSound && audioSource.clip == runSound) || ((bool)walkSound && audioSource.clip == walkSound)))
			{
				audioSource.Stop();
			}
		}

		protected void PlayStandardAudio()
		{
			if (!audioSource)
			{
				return;
			}
			if (isRunning && (bool)runSound)
			{
				if (!audioSource.isPlaying || !(audioSource.clip == runSound))
				{
					audioSource.loop = false;
					audioSource.clip = runSound;
					audioSource.Play();
				}
			}
			else if ((bool)walkSound && (!audioSource.isPlaying || !(audioSource.clip == walkSound)))
			{
				audioSource.loop = false;
				audioSource.clip = walkSound;
				audioSource.Play();
			}
		}

		public void ResetAnimationEngine()
		{
			string text = "AnimEngine_" + animationEngine;
			if (animEngine == null || animEngine.ToString() != text)
			{
				animEngine = (AnimEngine)ScriptableObject.CreateInstance(text);
				animEngine.Declare(this);
			}
		}

		public void UpdateSpriteChild(bool isTopDown, bool isUnity2D)
		{
			float num = 0f;
			float num2 = 0f;
			if (isTopDown || isUnity2D)
			{
				num = Vector3.Dot(Vector3.forward, transform.forward.normalized);
				num2 = Vector3.Dot(Vector3.right, transform.forward.normalized);
			}
			else
			{
				num = Vector3.Dot(KickStarter.mainCamera.ForwardVector().normalized, transform.forward.normalized);
				num2 = Vector3.Dot(KickStarter.mainCamera.RightVector().normalized, transform.forward.normalized);
			}
			spriteAngle = Mathf.Atan(num2 / num) * 57.29578f;
			if (num > 0f)
			{
				spriteAngle += 180f;
			}
			else if (num2 > 0f)
			{
				spriteAngle += 360f;
			}
			if (charState == CharState.Custom && !flipCustomAnims)
			{
				flipFrames = false;
			}
			else
			{
				if (!lockDirection)
				{
					spriteDirection = SetSpriteDirection(num2, num);
				}
				if (!doDirections)
				{
					flipFrames = false;
				}
				else if (frameFlipping == AC_2DFrameFlipping.LeftMirrorsRight && spriteDirection.Contains("L"))
				{
					spriteDirection = spriteDirection.Replace("L", "R");
					flipFrames = true;
				}
				else if (frameFlipping == AC_2DFrameFlipping.RightMirrorsLeft && spriteDirection.Contains("R"))
				{
					spriteDirection = spriteDirection.Replace("R", "L");
					flipFrames = true;
				}
				else
				{
					flipFrames = false;
				}
			}
			if (frameFlipping != AC_2DFrameFlipping.None && ((flipFrames && spriteChild.localScale.x > 0f) || (!flipFrames && spriteChild.localScale.x < 0f)))
			{
				spriteChild.localScale = new Vector3(0f - spriteChild.localScale.x, spriteChild.localScale.y, spriteChild.localScale.z);
			}
			if (isTopDown)
			{
				if ((bool)animEngine && !animEngine.isSpriteBased)
				{
					spriteChild.rotation = transform.rotation;
					spriteChild.RotateAround(transform.position, Vector3.right, 90f);
				}
				else
				{
					spriteChild.rotation = Quaternion.Euler(90f, 0f, 0f);
				}
			}
			else if (isUnity2D)
			{
				spriteChild.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
			else
			{
				spriteChild.rotation = Quaternion.Euler(spriteChild.rotation.eulerAngles.x, KickStarter.mainCamera.transform.rotation.eulerAngles.y, spriteChild.rotation.eulerAngles.z);
			}
			if (!spriteChild.GetComponent<FollowSortingMap>())
			{
				return;
			}
			if (!lockScale)
			{
				spriteScale = spriteChild.GetComponent<FollowSortingMap>().GetLocalScale();
			}
			if (spriteScale != 0f)
			{
				transform.localScale = originalScale * spriteScale;
				if (lockScale)
				{
					sortingMapScale = spriteScale;
				}
				else
				{
					sortingMapScale = spriteChild.GetComponent<FollowSortingMap>().GetLocalSpeed();
				}
			}
		}

		public void SetSorting(int order)
		{
			if ((bool)spriteChild)
			{
				if ((bool)spriteChild.GetComponent<FollowSortingMap>())
				{
					spriteChild.GetComponent<FollowSortingMap>().LockSortingOrder(order);
				}
				else
				{
					spriteChild.GetComponent<Renderer>().sortingOrder = order;
				}
			}
			if ((bool)GetComponent<Renderer>())
			{
				if ((bool)GetComponent<FollowSortingMap>())
				{
					GetComponent<FollowSortingMap>().LockSortingOrder(order);
				}
				else
				{
					GetComponent<Renderer>().sortingOrder = order;
				}
			}
		}

		public void SetSorting(string layer)
		{
			if ((bool)spriteChild)
			{
				if ((bool)spriteChild.GetComponent<FollowSortingMap>())
				{
					spriteChild.GetComponent<FollowSortingMap>().LockSortingLayer(layer);
				}
				else
				{
					spriteChild.GetComponent<Renderer>().sortingLayerName = layer;
				}
			}
			if ((bool)GetComponent<Renderer>())
			{
				if ((bool)GetComponent<FollowSortingMap>())
				{
					GetComponent<FollowSortingMap>().LockSortingLayer(layer);
				}
				else
				{
					GetComponent<Renderer>().sortingLayerName = layer;
				}
			}
		}

		public void ReleaseSorting()
		{
			if ((bool)spriteChild && (bool)spriteChild.GetComponent<Renderer>() && (bool)spriteChild.GetComponent<FollowSortingMap>())
			{
				spriteChild.GetComponent<FollowSortingMap>().lockSorting = false;
			}
			if ((bool)GetComponent<Renderer>() && (bool)GetComponent<FollowSortingMap>())
			{
				GetComponent<FollowSortingMap>().lockSorting = false;
			}
		}

		public float GetMoveSpeed()
		{
			return moveSpeed;
		}

		public void SetHeadTurnTarget(Vector3 position, bool isInstant)
		{
			SetHeadTurnTarget(position, isInstant, HeadFacing.Manual);
		}

		public void SetHeadTurnTarget(Vector3 position, bool isInstant, HeadFacing _headFacing)
		{
			if (_headFacing != HeadFacing.Hotspot || headFacing != HeadFacing.Manual)
			{
				headTurnTarget = position;
				headFacing = _headFacing;
				if (isInstant)
				{
					CalculateHeadTurn();
					SnapHeadMovement();
				}
			}
		}

		public void ClearHeadTurnTarget(HeadFacing _headFacing, bool isInstant)
		{
			if (headFacing == _headFacing)
			{
				ClearHeadTurnTarget(isInstant);
			}
		}

		public void ClearHeadTurnTarget(bool isInstant)
		{
			headFacing = HeadFacing.None;
			if (isInstant)
			{
				targetHeadAngles = Vector2.zero;
				SnapHeadMovement();
			}
		}

		public void SnapHeadMovement()
		{
			actualHeadAngles = targetHeadAngles;
			AnimateHeadTurn();
		}

		public bool IsMovingHead()
		{
			if (actualHeadAngles != targetHeadAngles)
			{
				return true;
			}
			return false;
		}

		public Shapeable GetShapeable()
		{
			Shapeable shapeable = GetComponent<Shapeable>();
			if (shapeable == null)
			{
				shapeable = GetComponentInChildren<Shapeable>();
			}
			return shapeable;
		}

		private void UpdateHeadTurnAngle()
		{
			CalculateHeadTurn();
			if (IsMovingHead())
			{
				AnimateHeadTurn();
			}
		}

		private void CalculateHeadTurn()
		{
			if (headFacing == HeadFacing.None)
			{
				targetHeadAngles = Vector2.zero;
				return;
			}
			Vector3 vector = headTurnTarget - transform.position;
			vector.y = 0f;
			targetHeadAngles.x = Vector3.Angle(transform.forward, vector);
			targetHeadAngles.x = Mathf.Min(targetHeadAngles.x, 60f);
			targetHeadAngles.x /= 60f;
			Vector3 lhs = Vector3.Cross(transform.forward, vector);
			float num = Vector3.Dot(lhs, Vector2.up);
			if (num < 0f)
			{
				targetHeadAngles.x *= -1f;
			}
			Vector3 vector2 = headTurnTarget - transform.position;
			if (_collider is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = (CapsuleCollider)_collider;
				vector2 -= new Vector3(0f, capsuleCollider.height * transform.localScale.y * 0.8f, 0f);
			}
			targetHeadAngles.y = Vector3.Angle(vector2, vector);
			targetHeadAngles.y = Mathf.Min(targetHeadAngles.y, 30f);
			targetHeadAngles.y /= 30f;
			if (vector2.y < vector.y)
			{
				targetHeadAngles.y *= -1f;
			}
		}

		private void AnimateHeadTurn()
		{
			if (targetHeadAngles.x == 0f && stateHandler != null && stateHandler.gameState == GameState.Normal)
			{
				actualHeadAngles = Vector2.Lerp(actualHeadAngles, targetHeadAngles, Time.deltaTime * 4f);
			}
			else
			{
				actualHeadAngles = Vector2.Lerp(actualHeadAngles, targetHeadAngles, Time.deltaTime * 10f);
			}
			animEngine.TurnHead(actualHeadAngles);
		}

		public Vector2 GetScreenCentre()
		{
			Vector3 position = transform.position;
			if ((bool)_collider && _collider is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = (CapsuleCollider)_collider;
				float num = capsuleCollider.height * transform.localScale.y;
				if (spriteChild != null && (bool)spriteChild.GetComponent<SpriteRenderer>())
				{
					num *= spriteChild.localScale.y;
				}
				if ((bool)settingsManager && settingsManager.IsTopDown())
				{
					position.z += num;
				}
				else
				{
					position.y += num;
				}
			}
			else if (spriteChild != null)
			{
				if ((bool)spriteChild.GetComponent<SpriteRenderer>())
				{
					position.y = spriteChild.GetComponent<SpriteRenderer>().bounds.extents.y + spriteChild.GetComponent<SpriteRenderer>().bounds.center.y;
				}
				else if ((bool)spriteChild.GetComponent<Renderer>())
				{
					position.y = spriteChild.GetComponent<Renderer>().bounds.extents.y + spriteChild.GetComponent<Renderer>().bounds.center.y;
				}
			}
			Vector3 vector = Camera.main.WorldToViewportPoint(position);
			return new Vector2(vector.x, 1f - vector.y);
		}

		private bool DoRigidbodyMovement()
		{
			if ((bool)_rigidbody && (spriteChild == null || spriteChild.GetComponent<FollowSortingMap>() == null))
			{
				return true;
			}
			return false;
		}

		private void OnLevelWasLoaded()
		{
			headFacing = HeadFacing.None;
			lockDirection = false;
			lockScale = false;
			isLipSyncing = false;
			lipSyncShapes.Clear();
			ReleaseSorting();
		}

		public void StartLipSync(List<LipSyncShape> _lipSyncShapes)
		{
			lipSyncShapes = _lipSyncShapes;
			isLipSyncing = true;
		}

		public int GetLipSyncFrame()
		{
			if (lipSyncShapes.Count > 0)
			{
				return lipSyncShapes[0].frame;
			}
			return 0;
		}

		public float GetLipSyncNormalised()
		{
			if (lipSyncShapes.Count > 0)
			{
				return (float)lipSyncShapes[0].frame / (float)(speechManager.phonemes.Count - 1);
			}
			return 0f;
		}

		public bool LipSyncGameObject()
		{
			if (isLipSyncing && speechManager.lipSyncOutput == LipSyncOutput.PortraitAndGameObject)
			{
				return true;
			}
			return false;
		}

		private void ProcessLipSync()
		{
			if (lipSyncShapes.Count <= 0 || !(Time.time > lipSyncShapes[0].timeIndex))
			{
				return;
			}
			if (speechManager.lipSyncOutput == LipSyncOutput.PortraitAndGameObject)
			{
				if ((bool)shapeable && lipSyncShapes.Count > 1)
				{
					float deltaTime = lipSyncShapes[1].timeIndex - lipSyncShapes[0].timeIndex;
					shapeable.SetActiveKey(lipSyncGroupID, lipSyncShapes[1].frame, 100f, deltaTime, MoveMethod.Smooth, null);
				}
				else
				{
					shapeable.SetActiveKey(lipSyncGroupID, 0, 100f, 0.2f, MoveMethod.Smooth, null);
				}
			}
			lipSyncShapes.RemoveAt(0);
		}

		public void StopSpeaking()
		{
			isTalking = false;
			if ((bool)_animation && !isLipSyncing)
			{
				foreach (AnimationState item in _animation)
				{
					if (item.layer == 7)
					{
						item.normalizedTime = 1f;
						item.weight = 0f;
					}
				}
			}
			if (shapeable != null && speechManager.lipSyncOutput == LipSyncOutput.PortraitAndGameObject)
			{
				shapeable.DisableAllKeys(lipSyncGroupID, 0.1f, MoveMethod.Curved, null);
			}
			lipSyncShapes.Clear();
			if ((bool)GetComponent<AudioSource>())
			{
				GetComponent<AudioSource>().Stop();
			}
		}
	}
}
