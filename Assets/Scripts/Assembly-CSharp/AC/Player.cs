using UnityEngine;

namespace AC
{
	public class Player : Char
	{
		public AnimationClip jumpAnim;

		public string jumpParameter = "Jump";

		public int ID;

		public bool lockedPath;

		public DetectHotspots hotspotDetector;

		private bool isTilting;

		private float actualTilt;

		private float targetTilt;

		private float tiltSpeed;

		private float tiltStartTime;

		private Transform fpCam;

		private PlayerInput playerInput;

		public void Awake()
		{
			Object.DontDestroyOnLoad(this);
			GetReferences();
			if ((bool)soundChild && (bool)soundChild.gameObject.GetComponent<AudioSource>())
			{
				audioSource = soundChild.gameObject.GetComponent<AudioSource>();
			}
			if ((bool)GameObject.FindWithTag("FirstPersonCamera"))
			{
				fpCam = GameObject.FindWithTag("FirstPersonCamera").transform;
			}
			_Awake();
		}

		private void GetReferences()
		{
			if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>())
			{
				playerInput = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
			}
		}

		private void OnLevelWasLoaded()
		{
			GetReferences();
		}

		private void Update()
		{
			if (!stateHandler || !stateHandler.playerIsOff)
			{
				if ((bool)hotspotDetector)
				{
					hotspotDetector._Update();
				}
				_Update();
			}
		}

		public bool isCutscene()
		{
			if (stateHandler != null)
			{
				return stateHandler.gameState != GameState.Normal;
			}
			return false;
		}

		private void FixedUpdate()
		{
			if (settingsManager == null)
			{
				GetSettingsManager();
			}
			if ((bool)stateHandler && stateHandler.playerIsOff)
			{
				return;
			}
			if ((bool)activePath && !pausePath)
			{
				if (IsTurningBeforeWalking())
				{
					charState = CharState.Idle;
				}
				else if (((bool)stateHandler && stateHandler.gameState == GameState.Cutscene && !lockedPath) || ((bool)settingsManager && settingsManager.movementMethod == MovementMethod.PointAndClick) || ((bool)settingsManager && settingsManager.movementMethod == MovementMethod.StraightToCursor && settingsManager.singleTapStraight) || IsMovingToHotspot())
				{
					charState = CharState.Move;
				}
				if (!lockedPath)
				{
					CheckIfStuck();
				}
			}
			else if (activePath == null && stateHandler.gameState == GameState.Cutscene && charState == CharState.Move)
			{
				charState = CharState.Decelerate;
			}
			if (isJumping && IsGrounded())
			{
				isJumping = false;
			}
			if (isTilting)
			{
				actualTilt = Mathf.Lerp(actualTilt, targetTilt, AdvGame.Interpolate(tiltStartTime, tiltSpeed, MoveMethod.Smooth, null));
				if (Mathf.Abs(targetTilt - actualTilt) < 2f)
				{
					isTilting = false;
				}
			}
			_FixedUpdate();
			if (IsUFPSPlayer() && isTurning)
			{
				if (isTilting)
				{
					UltimateFPSIntegration.SetRotation(new Vector2(actualTilt, newRotation.eulerAngles.y));
				}
				else
				{
					UltimateFPSIntegration.SetPitch(newRotation.eulerAngles.y);
				}
			}
			else if (isTilting)
			{
				UpdateTilt();
			}
			if (IsUFPSPlayer() && activePath != null && charState == CharState.Move)
			{
				UltimateFPSIntegration.Teleport(transform.position);
			}
		}

		private bool IsGrounded()
		{
			if (_rigidbody != null && Mathf.Abs(_rigidbody.velocity.y) > 0.1f)
			{
				return false;
			}
			if (_collider != null)
			{
				return Physics.CheckCapsule(transform.position + new Vector3(0f, _collider.bounds.size.y, 0f), transform.position + new Vector3(0f, _collider.bounds.size.x / 4f, 0f), _collider.bounds.size.x / 2f);
			}
			Debug.Log("Player has no Collider component");
			return false;
		}

		public void Jump()
		{
			if (!isJumping && IsGrounded() && activePath == null)
			{
				if (_rigidbody != null)
				{
					_rigidbody.velocity = new Vector3(0f, settingsManager.jumpSpeed, 0f);
					isJumping = true;
				}
				else
				{
					Debug.Log("Player cannot jump as he has no Rigidbody component");
				}
			}
		}

		private bool IsMovingToHotspot()
		{
			if (playerInput != null && playerInput.hotspotMovingTo != null)
			{
				return true;
			}
			return false;
		}

		public new void EndPath()
		{
			lockedPath = false;
			base.EndPath();
		}

		public void SetLockedPath(Paths pathOb)
		{
			if (!settingsManager)
			{
				return;
			}
			if (settingsManager.movementMethod == MovementMethod.Direct)
			{
				lockedPath = true;
				if (pathOb.pathSpeed == PathSpeed.Run)
				{
					isRunning = true;
				}
				else
				{
					isRunning = false;
				}
				if (pathOb.affectY)
				{
					transform.position = pathOb.transform.position;
				}
				else
				{
					transform.position = new Vector3(pathOb.transform.position.x, transform.position.y, pathOb.transform.position.z);
				}
				activePath = pathOb;
				targetNode = 1;
				charState = CharState.Idle;
			}
			else
			{
				Debug.LogWarning("Path-constrained player movement is only available with Direct control.");
			}
		}

		protected override void Accelerate()
		{
			if (settingsManager.movementMethod == MovementMethod.UltimateFPS && activePath == null)
			{
				moveSpeed = 0f;
			}
			else
			{
				base.Accelerate();
			}
		}

		private void UpdateTilt()
		{
			if ((bool)fpCam && (bool)fpCam.GetComponent<FirstPersonCamera>())
			{
				fpCam.GetComponent<FirstPersonCamera>().SetRotationY(actualTilt);
			}
			else if (settingsManager.movementMethod == MovementMethod.UltimateFPS)
			{
				UltimateFPSIntegration.SetTilt(actualTilt);
			}
		}

		public void SetTilt(Vector3 lookAtPosition, bool isInstant)
		{
			if (fpCam == null)
			{
				return;
			}
			if (isInstant)
			{
				isTilting = false;
				transform.LookAt(lookAtPosition);
				float x = transform.localEulerAngles.x;
				if (targetTilt > 180f)
				{
					targetTilt -= 360f;
				}
				if ((bool)fpCam.GetComponent<FirstPersonCamera>())
				{
					fpCam.GetComponent<FirstPersonCamera>().SetRotationY(x);
				}
				else if (settingsManager.movementMethod == MovementMethod.UltimateFPS)
				{
					UltimateFPSIntegration.SetTilt(x);
				}
				return;
			}
			actualTilt = fpCam.eulerAngles.x;
			if (actualTilt > 180f)
			{
				actualTilt -= 360f;
			}
			Quaternion rotation = fpCam.rotation;
			fpCam.transform.LookAt(lookAtPosition);
			targetTilt = fpCam.localEulerAngles.x;
			fpCam.rotation = rotation;
			if (targetTilt > 180f)
			{
				targetTilt -= 360f;
			}
			Vector3 vector = lookAtPosition - transform.position;
			vector.y = 0f;
			tiltSpeed = Mathf.Abs(2f / Vector3.Dot(fpCam.forward.normalized, vector.normalized)) * turnSpeed / 100f;
			tiltSpeed = Mathf.Min(tiltSpeed, 2f);
			tiltStartTime = Time.time;
			isTilting = true;
		}
	}
}
