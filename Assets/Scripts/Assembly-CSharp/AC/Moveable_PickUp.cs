using UnityEngine;

namespace AC
{
	[RequireComponent(typeof(Rigidbody))]
	public class Moveable_PickUp : DragBase
	{
		public bool allowRotation;

		public float breakForce = 300f;

		public bool allowThrow;

		public float chargeTime = 0.5f;

		public float pullbackDistance = 0.6f;

		public float throwForce = 400f;

		private bool isChargingThrow;

		private float throwCharge;

		private float chargeStartTime;

		private bool inRotationMode;

		private FixedJoint fixedJoint;

		private float originalDistanceToCamera;

		private Vector3 lastPosition;

		private PlayerInput playerInput;

		protected override void Awake()
		{
			base.Awake();
			LimitCollisions();
			playerInput = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
		}

		public override void UpdateMovement()
		{
			if ((bool)moveSound && (bool)moveSoundClip && !inRotationMode)
			{
				if (numCollisions > 0)
				{
					PlayMoveSound(_rigidbody.velocity.magnitude, 0.5f);
				}
				else if (moveSound.IsPlaying())
				{
					moveSound.Stop();
				}
			}
		}

		private void ChargeThrow()
		{
			if (!isChargingThrow)
			{
				isChargingThrow = true;
				chargeStartTime = Time.time;
				throwCharge = 0f;
			}
			else if (throwCharge < 1f)
			{
				throwCharge = (Time.time - chargeStartTime) / chargeTime;
			}
			if (throwCharge > 1f)
			{
				throwCharge = 1f;
			}
		}

		private void ReleaseThrow()
		{
			LetGo();
			_rigidbody.useGravity = true;
			_rigidbody.drag = originalDrag;
			_rigidbody.angularDrag = originalAngularDrag;
			Vector3 normalized = (transform.position - cameraTransform.position).normalized;
			_rigidbody.AddForce(throwForce * throwCharge * normalized);
		}

		private void CreateFixedJoint()
		{
			GameObject gameObject = new GameObject(name + " (Joint)");
			Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			rigidbody.useGravity = false;
			this.fixedJoint = gameObject.AddComponent<FixedJoint>();
			FixedJoint fixedJoint = this.fixedJoint;
			float breakTorque = breakForce;
			this.fixedJoint.breakTorque = breakTorque;
			fixedJoint.breakForce = breakTorque;
			gameObject.AddComponent<JointBreaker>();
		}

		public override void Grab(Vector3 grabPosition)
		{
			base.Grab(grabPosition);
			inRotationMode = false;
			isChargingThrow = false;
			throwCharge = 0f;
			if (fixedJoint == null)
			{
				CreateFixedJoint();
			}
			Rigidbody rigidbody = _rigidbody;
			Vector3 zero = Vector3.zero;
			_rigidbody.angularVelocity = zero;
			rigidbody.velocity = zero;
			lastPosition = transform.position;
			originalDistanceToCamera = (grabPosition - cameraTransform.position).magnitude;
		}

		public override void LetGo()
		{
			if (fixedJoint != null && (bool)fixedJoint.connectedBody)
			{
				fixedJoint.connectedBody = null;
			}
			_rigidbody.drag = originalDrag;
			_rigidbody.angularDrag = originalAngularDrag;
			if (inRotationMode)
			{
				_rigidbody.velocity = Vector3.zero;
			}
			else if (!isChargingThrow)
			{
				Vector3 vector = transform.position - lastPosition;
				_rigidbody.AddForce(vector * Time.deltaTime * 100000f);
			}
			_rigidbody.useGravity = true;
			isHeld = false;
		}

		private void SetRotationMode(bool on)
		{
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.useGravity = !on;
			inRotationMode = on;
			if (on)
			{
				playerInput.StartRotatingObject();
				fixedJoint.connectedBody = null;
			}
			else
			{
				fixedJoint.connectedBody = _rigidbody;
			}
		}

		public override void ApplyDragForce(Vector3 force, Vector3 mousePos, float _distanceToCamera)
		{
			distanceToCamera = _distanceToCamera;
			if (allowThrow)
			{
				try
				{
					if (Input.GetButton("ThrowMoveable"))
					{
						ChargeThrow();
					}
					else if (isChargingThrow)
					{
						ReleaseThrow();
					}
				}
				catch
				{
				}
			}
			if (allowRotation)
			{
				try
				{
					if (Input.GetButton("RotateMoveable"))
					{
						SetRotationMode(true);
					}
					else if (Input.GetButtonUp("RotateMoveable"))
					{
						SetRotationMode(false);
					}
				}
				catch
				{
				}
				try
				{
					if (Input.GetButtonDown("RotateMoveableToggle"))
					{
						SetRotationMode(!inRotationMode);
					}
				}
				catch
				{
				}
			}
			force *= speedFactor * _rigidbody.drag * distanceToCamera * Time.deltaTime;
			if (force.magnitude > maxSpeed)
			{
				force *= maxSpeed / force.magnitude;
			}
			lastPosition = transform.position;
			if (inRotationMode)
			{
				Vector3 torque = Vector3.Cross(force, cameraTransform.forward);
				torque /= Mathf.Sqrt((grabPoint.position - transform.position).magnitude) * 2.4f * rotationFactor;
				_rigidbody.AddTorque(torque);
			}
			else
			{
				mousePos.z = originalDistanceToCamera - throwCharge * pullbackDistance;
				Vector3 position = Camera.main.ScreenToWorldPoint(mousePos);
				fixedJoint.transform.position = position;
				fixedJoint.connectedBody = _rigidbody;
			}
			if (allowZooming)
			{
				UpdateZoom();
			}
		}

		private new void UpdateZoom()
		{
			float axis = Input.GetAxis("ZoomMoveable");
			Vector3 normalized = (transform.position - cameraTransform.position).normalized;
			if (distanceToCamera - minZoom < 1f && axis < 0f)
			{
				normalized *= originalDistanceToCamera - minZoom;
			}
			else if (maxZoom - originalDistanceToCamera < 1f && axis > 0f)
			{
				normalized *= maxZoom - originalDistanceToCamera;
			}
			if ((!(originalDistanceToCamera <= minZoom) || !(axis < 0f)) && (!(originalDistanceToCamera >= maxZoom) || !(axis > 0f)))
			{
				originalDistanceToCamera += axis * zoomSpeed / 10f * Time.deltaTime;
			}
		}

		public void UnsetFixedJoint()
		{
			fixedJoint = null;
			isHeld = false;
		}

		protected void LimitCollisions()
		{
			Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				Collider[] array2 = componentsInChildren;
				foreach (Collider collider2 in array2)
				{
					if (!(collider == collider2))
					{
						Physics.IgnoreCollision(collider, collider2, true);
						Physics.IgnoreCollision(collider, collider2, true);
					}
				}
				if (ignorePlayerCollider && (bool)GameObject.FindWithTag("Player") && (bool)GameObject.FindWithTag("Player").GetComponent<Collider>())
				{
					Collider component = GameObject.FindWithTag("Player").GetComponent<Collider>();
					Physics.IgnoreCollision(component, collider, true);
				}
			}
		}

		private void OnDestroy()
		{
			if ((bool)fixedJoint)
			{
				Object.Destroy(fixedJoint.gameObject);
				fixedJoint = null;
			}
		}
	}
}
