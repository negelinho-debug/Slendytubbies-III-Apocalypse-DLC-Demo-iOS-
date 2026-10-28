using System;
using AC;
using UnityEngine;

public class FirstPersonCamera : _Camera
{
	public float rotationY;

	public Vector2 sensitivity = new Vector2(15f, 15f);

	public float minY = -60f;

	public float maxY = 60f;

	public bool allowMouseWheelZooming;

	public float minimumZoom = 13f;

	public float maximumZoom = 65f;

	public bool headBob = true;

	public float bobbingSpeed = 0.18f;

	public float bobbingAmount = 0.2f;

	private float bobTimer;

	private float height;

	private float deltaHeight;

	private PlayerInput playerInput;

	private StateHandler stateHandler;

	private void OnLevelWasLoaded()
	{
		Awake();
	}

	protected override void Awake()
	{
		if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>())
		{
			playerInput = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
		}
		height = transform.localPosition.y;
	}

	private void Start()
	{
		if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
		{
			stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
		}
	}

	public new void ResetTarget()
	{
	}

	private void Update()
	{
		if ((bool)playerInput && headBob)
		{
			deltaHeight = 0f;
			if ((playerInput.moveKeys.x == 0f && playerInput.moveKeys.y == 0f) || AdvGame.GetReferences().settingsManager.IsFirstPersonDragRotation())
			{
				bobTimer = 0f;
			}
			else
			{
				float num = Mathf.Sin(bobTimer);
				if (playerInput.isRunning)
				{
					bobTimer += 2f * bobbingSpeed;
				}
				else
				{
					bobTimer += bobbingSpeed;
				}
				if (bobTimer > (float)Math.PI * 2f)
				{
					bobTimer -= (float)Math.PI * 2f;
				}
				float value = Mathf.Abs(playerInput.moveKeys.x) + Mathf.Abs(playerInput.moveKeys.y);
				value = Mathf.Clamp(value, 0f, 1f);
				deltaHeight = value * num * bobbingAmount;
			}
			transform.localPosition = new Vector3(transform.localPosition.x, height + deltaHeight, transform.localPosition.z);
		}
		if (!allowMouseWheelZooming || !GetComponent<Camera>() || !stateHandler || stateHandler.gameState != GameState.Normal)
		{
			return;
		}
		try
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				GetComponent<Camera>().fieldOfView = Mathf.Max(GetComponent<Camera>().fieldOfView - 3f, minimumZoom);
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				GetComponent<Camera>().fieldOfView = Mathf.Min(GetComponent<Camera>().fieldOfView + 3f, maximumZoom);
			}
		}
		catch
		{
		}
	}

	private void FixedUpdate()
	{
		rotationY = Mathf.Clamp(rotationY, minY, maxY);
		transform.localEulerAngles = new Vector3(rotationY, 0f, 0f);
	}

	public void SetRotationY(float tilt)
	{
		rotationY = tilt;
	}
}
