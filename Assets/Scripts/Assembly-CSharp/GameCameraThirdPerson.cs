using AC;
using UnityEngine;

public class GameCameraThirdPerson : _Camera
{
	public enum RotationLock
	{
		Free,
		Locked,
		Limited
	}

	public RotationLock spinLock;

	public RotationLock pitchLock = RotationLock.Locked;

	public float horizontalOffset;

	public float verticalOffset = 2f;

	public float distance = 2f;

	public bool allowMouseWheelZooming;

	public bool detectCollisions = true;

	public float minDistance = 1f;

	public float maxDistance = 3f;

	public bool toggleCursor;

	public float spinSpeed = 5f;

	public float spinAccleration = 5f;

	public float spinDeceleration = 5f;

	public string spinAxis = string.Empty;

	public bool invertSpin;

	public bool alwaysBehind;

	public bool resetSpinWhenSwitch;

	public float spinOffset;

	public float maxSpin = 40f;

	public float pitchSpeed = 3f;

	public float pitchAccleration = 20f;

	public float pitchDeceleration = 20f;

	public float maxPitch = 40f;

	public string pitchAxis = string.Empty;

	public bool invertPitch;

	public bool resetPitchWhenSwitch;

	private float collisionOffset;

	private float deltaDistance;

	private float deltaSpin;

	private float deltaPitch;

	private float roll;

	private float spin;

	private float pitch;

	private float initialPitch;

	private float initialSpin;

	private StateHandler stateHandler;

	private PlayerInput playerInput;

	protected override void Awake()
	{
		base.Awake();
		if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>())
		{
			playerInput = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
		}
		initialPitch = transform.eulerAngles.x;
		initialSpin = transform.eulerAngles.y;
	}

	private void Start()
	{
		ResetTarget();
		Vector3 eulerAngles = transform.eulerAngles;
		roll = eulerAngles.x;
		spin = eulerAngles.y;
		if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
		{
			stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
		}
		Update();
	}

	public void ResetRotation()
	{
		if (pitchLock != RotationLock.Locked && resetPitchWhenSwitch)
		{
			pitch = initialPitch;
		}
		if (spinLock != RotationLock.Locked && resetSpinWhenSwitch)
		{
			spin = initialSpin;
		}
	}

	private void LateUpdate()
	{
		if (detectCollisions && target != null)
		{
			Vector3 end = target.position - (transform.rotation * Vector3.forward * distance + new Vector3(0f, 0f - verticalOffset, 0f)) + transform.rotation * Vector3.right * horizontalOffset;
			RaycastHit hitInfo;
			if (Physics.Linecast(target.position + new Vector3(0f, verticalOffset, 0f), end, out hitInfo))
			{
				collisionOffset = distance - hitInfo.distance;
			}
			else
			{
				collisionOffset = 0f;
			}
		}
	}

	private void Update()
	{
		if (!target)
		{
			return;
		}
		if (stateHandler == null || stateHandler.gameState == GameState.Normal)
		{
			if (allowMouseWheelZooming && minDistance < maxDistance)
			{
				if (Input.GetAxis("Mouse ScrollWheel") < 0f)
				{
					deltaDistance = Mathf.Lerp(deltaDistance, Mathf.Min(spinSpeed, maxDistance - distance), spinAccleration / 5f * Time.deltaTime);
				}
				else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
				{
					deltaDistance = Mathf.Lerp(deltaDistance, 0f - Mathf.Min(spinSpeed, distance - minDistance), spinAccleration / 5f * Time.deltaTime);
				}
				else
				{
					deltaDistance = Mathf.Lerp(deltaDistance, 0f, spinAccleration * Time.deltaTime);
				}
				distance += deltaDistance;
				distance = Mathf.Clamp(distance, minDistance, maxDistance);
			}
			if (playerInput.IsCursorLocked() || !toggleCursor)
			{
				if (!isDragControlled)
				{
					inputMovement = new Vector2(Input.GetAxis(spinAxis), Input.GetAxis(pitchAxis));
				}
				else if (playerInput.dragState == DragState._Camera)
				{
					inputMovement = playerInput.GetDragVector();
				}
				else
				{
					inputMovement = Vector2.zero;
				}
				if (spinLock != RotationLock.Locked)
				{
					if (inputMovement.x > 0f)
					{
						deltaSpin = Mathf.Lerp(deltaSpin, spinSpeed, spinAccleration * Time.deltaTime);
					}
					else if (inputMovement.x < 0f)
					{
						deltaSpin = Mathf.Lerp(deltaSpin, 0f - spinSpeed, spinAccleration * Time.deltaTime);
					}
					else
					{
						deltaSpin = Mathf.Lerp(deltaSpin, 0f, spinDeceleration * Time.deltaTime);
					}
					if (spinLock == RotationLock.Limited)
					{
						if ((invertSpin && deltaSpin > 0f) || (!invertSpin && deltaSpin < 0f))
						{
							if (maxSpin - spin < 5f)
							{
								deltaSpin *= (maxSpin - spin) / 5f;
							}
						}
						else if (((invertSpin && deltaSpin < 0f) || (!invertSpin && deltaSpin > 0f)) && maxSpin + spin < 5f)
						{
							deltaSpin *= (maxSpin + spin) / 5f;
						}
					}
					if (invertSpin)
					{
						spin += deltaSpin;
					}
					else
					{
						spin -= deltaSpin;
					}
					if (spinLock == RotationLock.Limited)
					{
						spin = Mathf.Clamp(spin, 0f - maxSpin, maxSpin);
					}
				}
				else if (alwaysBehind)
				{
					spin = Mathf.LerpAngle(spin, target.eulerAngles.y + spinOffset, spinAccleration * Time.deltaTime);
				}
				if (pitchLock != RotationLock.Locked)
				{
					if (inputMovement.y > 0f)
					{
						deltaPitch = Mathf.Lerp(deltaPitch, 0f - pitchSpeed, pitchAccleration * Time.deltaTime);
					}
					else if (inputMovement.y < 0f)
					{
						deltaPitch = Mathf.Lerp(deltaPitch, pitchSpeed, pitchAccleration * Time.deltaTime);
					}
					else
					{
						deltaPitch = Mathf.Lerp(deltaPitch, 0f, pitchDeceleration * Time.deltaTime);
					}
					if (pitchLock == RotationLock.Limited)
					{
						if ((invertPitch && deltaPitch > 0f) || (!invertPitch && deltaPitch < 0f))
						{
							if (maxPitch - pitch < 5f)
							{
								deltaPitch *= (maxPitch - pitch) / 5f;
							}
						}
						else if (((invertPitch && deltaPitch < 0f) || (!invertPitch && deltaPitch > 0f)) && maxPitch + pitch < 5f)
						{
							deltaPitch *= (maxPitch + pitch) / 5f;
						}
					}
					if (invertPitch)
					{
						pitch += deltaPitch;
					}
					else
					{
						pitch -= deltaPitch;
					}
					if (pitchLock == RotationLock.Limited)
					{
						pitch = Mathf.Clamp(pitch, 0f - maxPitch, maxPitch);
					}
				}
			}
			if (pitchLock == RotationLock.Locked)
			{
				pitch = maxPitch;
			}
		}
		float num = spin;
		float num2 = pitch;
		if (!targetIsPlayer)
		{
			if (spinLock != RotationLock.Locked)
			{
				num += target.eulerAngles.y;
			}
			if (pitchLock != RotationLock.Locked)
			{
				num2 += target.eulerAngles.x;
			}
		}
		Quaternion quaternion = Quaternion.Euler(num2, num, roll);
		transform.rotation = quaternion;
		transform.position = target.position - (quaternion * Vector3.forward * (distance - Mathf.Min(collisionOffset, distance - minDistance)) + new Vector3(0f, 0f - verticalOffset, 0f)) + quaternion * Vector3.right * horizontalOffset;
	}

	private void OnDestroy()
	{
		stateHandler = null;
		playerInput = null;
	}
}
