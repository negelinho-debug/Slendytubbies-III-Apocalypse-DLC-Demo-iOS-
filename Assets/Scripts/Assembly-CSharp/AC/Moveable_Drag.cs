using UnityEngine;

namespace AC
{
	[RequireComponent(typeof(Rigidbody))]
	public class Moveable_Drag : DragBase
	{
		public DragMode dragMode;

		public DragTrack track;

		public bool setOnStart = true;

		public float trackValueOnStart;

		public Interaction interactionOnMove;

		public AlignDragMovement alignMovement;

		public Transform plane;

		public bool noGravityWhenHeld = true;

		private Vector3 grabPositionRelative;

		[HideInInspector]
		public float colliderRadius = 0.5f;

		[HideInInspector]
		public float trackValue;

		[HideInInspector]
		public Vector3 _dragVector;

		[HideInInspector]
		public Collider maxCollider;

		[HideInInspector]
		public Collider minCollider;

		[HideInInspector]
		public int revolutions;

		private bool canPlayCollideSound;

		private bool targetStartedGreater;

		private float targetTrackValue;

		private float targetTrackSpeed;

		private ActionListManager actionListManager;

		protected override void Awake()
		{
			base.Awake();
			actionListManager = GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>();
			if ((bool)_rigidbody)
			{
				SetGravity(true);
			}
			if ((bool)GetComponent<SphereCollider>())
			{
				colliderRadius = GetComponent<SphereCollider>().radius * transform.localScale.x;
			}
		}

		private void Start()
		{
			if (track != null)
			{
				track.Connect(this);
				if (setOnStart)
				{
					track.SetPositionAlong(trackValueOnStart, this);
				}
				else
				{
					track.SnapToTrack(this, true);
				}
				trackValue = track.GetDecimalAlong(this);
			}
		}

		public float GetPositionAlong()
		{
			if (dragMode == DragMode.LockToTrack && (bool)track && track is DragTrack_Hinge)
			{
				return trackValue + (float)revolutions;
			}
			return trackValue;
		}

		public override void UpdateMovement()
		{
			if (dragMode == DragMode.LockToTrack && (bool)track)
			{
				if ((bool)track && (_rigidbody.angularVelocity != Vector3.zero || _rigidbody.velocity != Vector3.zero))
				{
					track.UpdateDraggable(this);
					if ((bool)interactionOnMove && gameObject.layer != LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.deactivatedLayer) && !actionListManager.IsListRunning(interactionOnMove))
					{
						interactionOnMove.Interact();
					}
				}
				else if (targetTrackSpeed > 0f)
				{
					trackValue = track.GetDecimalAlong(this);
				}
				if (targetTrackSpeed > 0f)
				{
					if ((targetTrackValue == 0f && trackValue < 0.01f) || (targetTrackValue == 1f && trackValue > 0.99f))
					{
						StopAutoMove();
					}
					else if ((targetStartedGreater && targetTrackValue > trackValue) || (!targetStartedGreater && targetTrackValue < trackValue))
					{
						track.ApplyAutoForce(targetTrackValue, targetTrackSpeed, this);
					}
					else
					{
						StopAutoMove();
					}
				}
				if ((bool)collideSound && (bool)collideSoundClip && track is DragTrack_Hinge)
				{
					if (trackValue > 0.05f && trackValue < 0.95f)
					{
						canPlayCollideSound = true;
					}
					else if ((trackValue == 0f || (!onlyPlayLowerCollisionSound && trackValue == 1f)) && canPlayCollideSound)
					{
						canPlayCollideSound = false;
						collideSound.Play(collideSoundClip, false);
					}
				}
				if (targetTrackSpeed == 0f && !isHeld && (trackValue == 0f || trackValue == 1f))
				{
					_rigidbody.isKinematic = true;
				}
				else
				{
					_rigidbody.isKinematic = false;
				}
			}
			else if (isHeld && dragMode == DragMode.RotateOnly && allowZooming && distanceToCamera > 0f)
			{
				LimitZoom();
			}
			if ((bool)moveSoundClip && (bool)moveSound)
			{
				if (dragMode == DragMode.LockToTrack && (bool)track && track is DragTrack_Hinge)
				{
					PlayMoveSound(_rigidbody.angularVelocity.magnitude, trackValue);
				}
				else
				{
					PlayMoveSound(_rigidbody.velocity.magnitude, trackValue);
				}
			}
		}

		public override void DrawGrabIcon()
		{
			if (isHeld && showIcon && Camera.main.WorldToScreenPoint(transform.position).z > 0f && icon != null)
			{
				if (dragMode == DragMode.LockToTrack && (bool)track && track.IconIsStationary())
				{
					Vector3 vector = Camera.main.WorldToScreenPoint(grabPositionRelative + transform.position);
					icon.Draw(new Vector3(vector.x, vector.y));
				}
				else
				{
					Vector3 vector2 = Camera.main.WorldToScreenPoint(grabPoint.position);
					icon.Draw(new Vector3(vector2.x, vector2.y));
				}
			}
		}

		public override void ApplyDragForce(Vector3 force, Vector3 mousePosition, float _distanceToCamera)
		{
			distanceToCamera = _distanceToCamera;
			force *= speedFactor * _rigidbody.drag * distanceToCamera * Time.deltaTime;
			if (force.magnitude > maxSpeed)
			{
				force *= maxSpeed / force.magnitude;
			}
			if (dragMode == DragMode.LockToTrack)
			{
				if ((bool)track)
				{
					track.ApplyDragForce(force, this);
				}
				return;
			}
			Vector3 vector = Vector3.Cross(force, cameraTransform.forward);
			if (dragMode == DragMode.MoveAlongPlane)
			{
				if (alignMovement == AlignDragMovement.AlignToPlane)
				{
					if ((bool)plane)
					{
						_rigidbody.AddForceAtPosition(Vector3.Cross(vector, plane.up), transform.position + plane.up * colliderRadius);
					}
					else
					{
						Debug.LogWarning("No alignment plane assigned to " + name);
					}
				}
				else
				{
					_rigidbody.AddForceAtPosition(force, transform.position - cameraTransform.forward * colliderRadius);
				}
			}
			else if (dragMode == DragMode.RotateOnly)
			{
				vector /= Mathf.Sqrt((grabPoint.position - transform.position).magnitude) * 2.4f * rotationFactor;
				_rigidbody.AddTorque(vector);
				if (allowZooming)
				{
					UpdateZoom();
				}
			}
		}

		public override void LetGo()
		{
			isHeld = false;
			if (targetTrackSpeed <= 0f)
			{
				_rigidbody.drag = originalDrag;
				_rigidbody.angularDrag = originalAngularDrag;
			}
			SetGravity(true);
			if (dragMode == DragMode.RotateOnly)
			{
				_rigidbody.velocity = Vector3.zero;
			}
		}

		public override void Grab(Vector3 grabPosition)
		{
			if (targetTrackSpeed <= 0f)
			{
				originalDrag = _rigidbody.drag;
				originalAngularDrag = _rigidbody.angularDrag;
			}
			isHeld = true;
			grabPoint.position = grabPosition;
			grabPositionRelative = grabPosition - transform.position;
			_rigidbody.drag = 20f;
			_rigidbody.angularDrag = 20f;
			if (dragMode == DragMode.LockToTrack && (bool)track)
			{
				if (track is DragTrack_Straight)
				{
					UpdateScrewVector();
				}
				else if (track is DragTrack_Hinge)
				{
					_dragVector = grabPosition;
				}
			}
			SetGravity(false);
			if (dragMode == DragMode.RotateOnly)
			{
				_rigidbody.velocity = Vector3.zero;
			}
		}

		private void SetGravity(bool value)
		{
			if (dragMode != DragMode.LockToTrack && noGravityWhenHeld)
			{
				_rigidbody.useGravity = value;
			}
		}

		public void UpdateScrewVector()
		{
			float num = Vector3.Dot(grabPoint.position - transform.position, transform.forward);
			float num2 = Vector3.Dot(grabPoint.position - transform.position, transform.right);
			_dragVector = transform.forward * (0f - num2) + transform.right * num;
		}

		public void StopAutoMove()
		{
			targetTrackSpeed = 0f;
			track.SetPositionAlong(targetTrackValue, this);
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			_rigidbody.drag = originalDrag;
			_rigidbody.angularDrag = originalAngularDrag;
		}

		public bool IsAutoMoving()
		{
			if (targetTrackSpeed > 0f)
			{
				return true;
			}
			return false;
		}

		public void AutoMoveAlongTrack(float _targetTrackValue, float _targetTrackSpeed, bool removePlayerControl)
		{
			if (dragMode == DragMode.LockToTrack && track != null)
			{
				if (_targetTrackSpeed < 0f)
				{
					targetTrackSpeed = 0f;
					return;
				}
				if (_targetTrackSpeed == 0f)
				{
					targetTrackValue = _targetTrackValue;
					StopAutoMove();
					return;
				}
				if (removePlayerControl)
				{
					isHeld = false;
				}
				targetTrackValue = _targetTrackValue;
				targetTrackSpeed = _targetTrackSpeed * 20f;
				if (targetTrackValue > trackValue)
				{
					targetStartedGreater = true;
				}
				else
				{
					targetStartedGreater = false;
				}
				if (!isHeld)
				{
					originalDrag = _rigidbody.drag;
					originalAngularDrag = _rigidbody.angularDrag;
				}
				_rigidbody.drag = 20f;
				_rigidbody.angularDrag = 20f;
			}
			else
			{
				Debug.LogWarning("Cannot move " + name + " along a track, because no track has been assigned to it");
				targetTrackSpeed = 0f;
			}
		}
	}
}
