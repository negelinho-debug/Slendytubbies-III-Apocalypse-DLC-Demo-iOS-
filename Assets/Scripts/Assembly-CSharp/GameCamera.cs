using System.Collections;
using AC;
using UnityEngine;

public class GameCamera : _Camera
{
	public bool followCursor;

	public Vector2 cursorInfluence = new Vector2(0.3f, 0.1f);

	public bool actFromDefaultPlayerStart = true;

	public bool lockXLocAxis = true;

	public bool lockYLocAxis = true;

	public bool lockZLocAxis = true;

	public bool lockXRotAxis = true;

	public bool lockYRotAxis = true;

	public bool lockFOV = true;

	public CameraLocConstrainType xLocConstrainType;

	public CameraRotConstrainType yRotConstrainType;

	public CameraLocConstrainType zLocConstrainType;

	public float xGradient = 1f;

	public float yGradientLoc = 1f;

	public float zGradient = 1f;

	public float xGradientRot = 2f;

	public float yGradient = 2f;

	public float FOVGradient = 2f;

	public float xOffset;

	public float yOffsetLoc;

	public float zOffset;

	public float xOffsetRot;

	public float yOffset;

	public float FOVOffset;

	public float xFreedom = 2f;

	public float yFreedom = 2f;

	public float zFreedom = 2f;

	public bool limitX;

	public bool limitYLoc;

	public bool limitZ;

	public bool limitXRot;

	public bool limitY;

	public bool limitFOV;

	public float targetHeight;

	public float targetXOffset;

	public float targetZOffset;

	public Vector2 constrainX;

	public Vector2 constrainYLoc;

	public Vector2 constrainZ;

	public Vector2 constrainXRot;

	public Vector2 constrainY;

	public Vector2 constrainFOV;

	public float dampSpeed = 0.9f;

	public bool focalPointIsTarget;

	private Vector3 desiredPosition;

	private float desiredSpin;

	private float desiredPitch;

	private float desiredFOV;

	private Vector3 originalTargetPosition;

	private Vector3 originalPosition;

	private float originalSpin;

	private float originalPitch;

	private float originalFOV;

	protected override void Awake()
	{
		base.Awake();
		originalPosition = transform.position;
		originalSpin = transform.eulerAngles.y;
		originalPitch = transform.eulerAngles.x;
		originalFOV = _camera.fieldOfView;
		desiredPosition = originalPosition;
		desiredPitch = originalPitch;
		desiredSpin = originalSpin;
		desiredFOV = originalFOV;
		if (!lockXLocAxis && limitX)
		{
			desiredPosition.x = ConstrainAxis(desiredPosition.x, constrainX);
		}
		if (!lockYLocAxis && limitY)
		{
			desiredPosition.y = ConstrainAxis(desiredPosition.y, constrainYLoc);
		}
		if (!lockZLocAxis && limitZ)
		{
			desiredPosition.z = ConstrainAxis(desiredPosition.z, constrainZ);
		}
		if (!lockXRotAxis && limitXRot)
		{
			desiredPitch = ConstrainAxis(desiredPitch, constrainXRot);
		}
		if (!lockYRotAxis && limitY && yRotConstrainType != CameraRotConstrainType.LookAtTarget)
		{
			desiredSpin = ConstrainAxis(desiredSpin, constrainY);
		}
		if (!lockFOV && limitFOV)
		{
			desiredFOV = ConstrainAxis(desiredFOV, constrainFOV);
		}
	}

	private void Start()
	{
		ResetTarget();
		if ((bool)target)
		{
			SetTargetOriginalPosition();
			MoveCameraInstant();
		}
		StartCoroutine("_Update");
	}

	private IEnumerator _Update()
	{
		while (Application.isPlaying)
		{
			MoveCamera();
			yield return new WaitForFixedUpdate();
		}
	}

	public void SwitchTarget(Transform _target)
	{
		target = _target;
		originalTargetPosition = Vector3.zero;
	}

	private void SetTargetOriginalPosition()
	{
		if (!(originalTargetPosition == Vector3.zero))
		{
			return;
		}
		if (actFromDefaultPlayerStart)
		{
			if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>() && GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>().defaultPlayerStart != null)
			{
				originalTargetPosition = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>().defaultPlayerStart.transform.position;
			}
			else
			{
				originalTargetPosition = target.transform.position;
			}
		}
		else
		{
			originalTargetPosition = target.transform.position;
		}
	}

	private void TrackTarget2D_X()
	{
		if (target.transform.position.x < transform.position.x - xFreedom)
		{
			desiredPosition.x = target.transform.position.x + xFreedom;
		}
		else if (target.transform.position.x > transform.position.x + xFreedom)
		{
			desiredPosition.x = target.transform.position.x - xFreedom;
		}
	}

	private void TrackTarget2D_Z()
	{
		if (target.transform.position.z < transform.position.z - zFreedom)
		{
			desiredPosition.z = target.transform.position.z + zFreedom;
		}
		else if (target.transform.position.z > transform.position.z + zFreedom)
		{
			desiredPosition.z = target.transform.position.z - zFreedom;
		}
	}

	private float GetDesiredPosition(float originalValue, float gradient, float offset, CameraLocConstrainType constrainType)
	{
		float num = originalValue + offset;
		switch (constrainType)
		{
		case CameraLocConstrainType.TargetX:
			num += (target.transform.position.x - originalTargetPosition.x) * gradient;
			break;
		case CameraLocConstrainType.TargetZ:
			num += (target.transform.position.z - originalTargetPosition.z) * gradient;
			break;
		case CameraLocConstrainType.TargetIntoScreen:
			num += (PositionRelativeToCamera(originalTargetPosition).x - PositionRelativeToCamera(target.position).x) * gradient;
			break;
		case CameraLocConstrainType.TargetAcrossScreen:
			num += (PositionRelativeToCamera(originalTargetPosition).z - PositionRelativeToCamera(target.position).z) * gradient;
			break;
		}
		return num;
	}

	private void MoveCamera()
	{
		if (target == null)
		{
			return;
		}
		SetDesired();
		if (!lockXLocAxis || !lockYLocAxis || !lockZLocAxis)
		{
			transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * dampSpeed);
		}
		if (!lockFOV)
		{
			_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, desiredFOV, Time.deltaTime * dampSpeed);
		}
		float x = transform.eulerAngles.x;
		if (!lockXRotAxis)
		{
			float num = transform.eulerAngles.x;
			if (num > 180f)
			{
				num -= 360f;
			}
			x = Mathf.Lerp(num, desiredPitch, Time.deltaTime * dampSpeed);
		}
		if (!lockYRotAxis)
		{
			if (yRotConstrainType == CameraRotConstrainType.LookAtTarget)
			{
				if (!lockXRotAxis)
				{
					Debug.LogWarning(gameObject.name + " cannot obey Pitch rotation, since Spin rotation's 'Look At Target' is overriding.");
				}
				if ((bool)target)
				{
					Vector3 position = target.position;
					position.y += targetHeight;
					position.x += targetXOffset;
					position.z += targetZOffset;
					Quaternion b = Quaternion.LookRotation(position - transform.position);
					transform.rotation = Quaternion.Slerp(transform.rotation, b, Time.deltaTime * dampSpeed);
				}
				else if (!targetIsPlayer)
				{
					Debug.LogWarning(name + " has no target");
				}
			}
			else
			{
				float y = Mathf.Lerp(transform.eulerAngles.y, desiredSpin, Time.deltaTime * dampSpeed);
				transform.eulerAngles = new Vector3(x, y, transform.eulerAngles.z);
			}
		}
		else
		{
			transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		SetFocalPoint();
	}

	private void SetFocalPoint()
	{
		if (focalPointIsTarget && target != null)
		{
			focalDistance = Vector3.Dot(transform.forward, target.position);
			if (focalDistance < 0f)
			{
				focalDistance = 0f;
			}
		}
	}

	public override void MoveCameraInstant()
	{
		if (targetIsPlayer && (bool)KickStarter.player)
		{
			target = KickStarter.player.transform;
		}
		SetDesired();
		if (!lockXLocAxis || !lockYLocAxis || !lockZLocAxis)
		{
			transform.position = desiredPosition;
		}
		if (!lockFOV)
		{
			_camera.fieldOfView = desiredFOV;
		}
		float x = transform.eulerAngles.x;
		if (!lockXRotAxis)
		{
			x = desiredPitch;
		}
		if (!lockYRotAxis)
		{
			if (yRotConstrainType == CameraRotConstrainType.LookAtTarget)
			{
				if ((bool)target)
				{
					Vector3 position = target.position;
					position.y += targetHeight;
					position.x += targetXOffset;
					position.z += targetZOffset;
					Quaternion rotation = Quaternion.LookRotation(position - transform.position);
					transform.rotation = rotation;
				}
			}
			else
			{
				transform.eulerAngles = new Vector3(x, desiredSpin, transform.eulerAngles.z);
			}
		}
		else
		{
			transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		SetFocalPoint();
	}

	private void SetDesired()
	{
		if (lockXLocAxis)
		{
			desiredPosition.x = transform.position.x;
		}
		else
		{
			if ((bool)target)
			{
				if (xLocConstrainType == CameraLocConstrainType.SideScrolling)
				{
					TrackTarget2D_X();
				}
				else
				{
					desiredPosition.x = GetDesiredPosition(originalPosition.x, xGradient, xOffset, xLocConstrainType);
				}
			}
			if (limitX)
			{
				desiredPosition.x = ConstrainAxis(desiredPosition.x, constrainX);
			}
		}
		if (lockYLocAxis)
		{
			desiredPosition.y = transform.position.y;
		}
		else
		{
			if ((bool)target)
			{
				desiredPosition.y = originalPosition.y + yOffsetLoc + (target.transform.position.y - originalTargetPosition.y) * yGradientLoc;
			}
			if (limitYLoc)
			{
				desiredPosition.y = ConstrainAxis(desiredPosition.y, constrainYLoc);
			}
		}
		if (lockXRotAxis)
		{
			desiredPitch = transform.eulerAngles.x;
		}
		else
		{
			if ((bool)target)
			{
				desiredPitch = originalPitch + xOffsetRot + (originalTargetPosition.y - target.transform.position.y) * xGradientRot;
			}
			if (limitXRot)
			{
				desiredPitch = ConstrainAxis(desiredPitch, constrainXRot);
			}
			desiredPitch = Mathf.Clamp(desiredPitch, -85f, 85f);
		}
		if (lockYRotAxis)
		{
			desiredSpin = 0f;
		}
		else
		{
			if ((bool)target)
			{
				desiredSpin = GetDesiredPosition(originalSpin, yGradient, yOffset, (CameraLocConstrainType)yRotConstrainType);
			}
			if (limitY)
			{
				desiredSpin = ConstrainAxis(desiredSpin, constrainY);
			}
		}
		if (lockZLocAxis)
		{
			desiredPosition.z = transform.position.z;
		}
		else
		{
			if ((bool)target)
			{
				if (zLocConstrainType == CameraLocConstrainType.SideScrolling)
				{
					TrackTarget2D_Z();
				}
				else
				{
					desiredPosition.z = GetDesiredPosition(originalPosition.z, zGradient, zOffset, zLocConstrainType);
				}
			}
			if (limitZ)
			{
				desiredPosition.z = ConstrainAxis(desiredPosition.z, constrainZ);
			}
		}
		if (lockFOV)
		{
			desiredFOV = _camera.fieldOfView;
			return;
		}
		if ((bool)target)
		{
			desiredFOV = GetDesiredPosition(originalFOV, FOVGradient, FOVOffset, CameraLocConstrainType.TargetIntoScreen);
		}
		if (limitFOV)
		{
			desiredFOV = ConstrainAxis(desiredFOV, constrainFOV);
		}
	}
}
