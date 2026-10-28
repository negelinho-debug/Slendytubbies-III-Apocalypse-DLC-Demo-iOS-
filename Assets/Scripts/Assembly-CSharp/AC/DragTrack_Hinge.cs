using UnityEngine;

namespace AC
{
	public class DragTrack_Hinge : DragTrack
	{
		public float maxAngle = 60f;

		public float radius = 2f;

		public bool doLoop;

		public bool limitRevolutions;

		public int maxRevolutions;

		public bool alignDragToFront;

		public override void AssignColliders(Moveable_Drag draggable)
		{
		}

		public override void Connect(Moveable_Drag draggable)
		{
			LimitCollisions(draggable);
			if (doLoop)
			{
				maxAngle = 360f;
			}
		}

		public override void ApplyAutoForce(float _position, float _speed, Moveable_Drag draggable)
		{
			Vector3 vector = draggable.transform.forward * _speed;
			if (draggable.trackValue < _position)
			{
				draggable._rigidbody.AddTorque(vector * Time.deltaTime);
			}
			else
			{
				draggable._rigidbody.AddTorque(-vector * Time.deltaTime);
			}
		}

		public override void ApplyDragForce(Vector3 force, Moveable_Drag draggable)
		{
			float num = 0f;
			Vector3 vector = Vector2.zero;
			if (!alignDragToFront)
			{
				num = Vector3.Dot(force, draggable.transform.up);
				vector = GetAxisOffset(draggable.GetGrabPosition());
				if (Vector3.Dot(draggable.transform.right, vector) < 0f)
				{
					num *= -1f;
				}
			}
			else
			{
				num = Vector3.Dot(force, transform.up);
				vector = GetAxisOffset(draggable._dragVector);
				if (Vector3.Dot(transform.right, vector) < 0f)
				{
					num *= -1f;
				}
			}
			Vector3 normalized = (draggable.transform.forward * num).normalized;
			normalized *= force.magnitude;
			normalized /= vector.magnitude / 0.43f;
			draggable._rigidbody.AddTorque(normalized);
		}

		private Vector3 GetAxisOffset(Vector3 grabPosition)
		{
			float num = Vector3.Dot(grabPosition, transform.forward);
			Vector3 vector = transform.position + transform.forward * num;
			return grabPosition - vector;
		}

		public override void SetPositionAlong(float proportionAlong, Moveable_Drag draggable)
		{
			draggable.transform.position = transform.position;
			draggable.transform.rotation = Quaternion.AngleAxis(proportionAlong * maxAngle, transform.forward) * transform.rotation;
		}

		public override float GetDecimalAlong(Moveable_Drag draggable)
		{
			float num = Vector3.Angle(transform.up, draggable.transform.up);
			if (Vector3.Dot(-transform.right, draggable.transform.up) < 0f)
			{
				num = 360f - num;
			}
			if (num > 180f + maxAngle / 2f)
			{
				num = 0f;
			}
			return num / maxAngle;
		}

		public override void SnapToTrack(Moveable_Drag draggable, bool onStart)
		{
			draggable.transform.position = transform.position;
			if (onStart)
			{
				draggable.transform.rotation = transform.rotation;
				draggable.trackValue = 0f;
			}
		}

		public override void UpdateDraggable(Moveable_Drag draggable)
		{
			float trackValue = draggable.trackValue;
			draggable.transform.position = transform.position;
			draggable.trackValue = GetDecimalAlong(draggable);
			if (draggable.trackValue <= 0f || draggable.trackValue > 1f)
			{
				if (draggable.trackValue < 0f)
				{
					draggable.trackValue = 0f;
				}
				else if (draggable.trackValue > 1f)
				{
					draggable.trackValue = 1f;
				}
				SetPositionAlong(draggable.trackValue, draggable);
				draggable._rigidbody.angularVelocity = Vector3.zero;
			}
			if (doLoop && limitRevolutions)
			{
				if (trackValue < 0.1f && draggable.trackValue > 0.9f)
				{
					draggable.revolutions--;
				}
				else if (trackValue > 0.9f && draggable.trackValue < 0.1f)
				{
					draggable.revolutions++;
				}
				if (draggable.revolutions < 0)
				{
					draggable.revolutions = 0;
					draggable.trackValue = 0f;
					SetPositionAlong(draggable.trackValue, draggable);
					draggable._rigidbody.angularVelocity = Vector3.zero;
				}
				else if (draggable.revolutions > maxRevolutions - 1)
				{
					draggable.revolutions = maxRevolutions - 1;
					draggable.trackValue = 1f;
					SetPositionAlong(draggable.trackValue, draggable);
					draggable._rigidbody.angularVelocity = Vector3.zero;
				}
			}
		}
	}
}
