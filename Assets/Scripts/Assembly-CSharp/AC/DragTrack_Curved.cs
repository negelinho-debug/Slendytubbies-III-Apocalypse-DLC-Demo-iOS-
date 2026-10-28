using UnityEngine;

namespace AC
{
	public class DragTrack_Curved : DragTrack
	{
		public float maxAngle = 60f;

		public float radius = 2f;

		public bool doLoop;

		private Vector3 startPosition;

		public override void AssignColliders(Moveable_Drag draggable)
		{
			if (draggable.maxCollider == null)
			{
				draggable.maxCollider = (Collider)Object.Instantiate(Resources.Load("DragCollider", typeof(Collider)));
			}
			if (draggable.minCollider == null)
			{
				draggable.minCollider = (Collider)Object.Instantiate(Resources.Load("DragCollider", typeof(Collider)));
			}
			if (maxAngle > 360f)
			{
				maxAngle = 360f;
			}
			float num = Mathf.Asin(draggable.colliderRadius / radius) * 57.29578f;
			draggable.maxCollider.transform.position = startPosition;
			draggable.maxCollider.transform.up = -transform.up;
			draggable.maxCollider.transform.RotateAround(transform.position, transform.forward, maxAngle + num);
			draggable.minCollider.transform.position = startPosition;
			draggable.minCollider.transform.up = transform.up;
			draggable.minCollider.transform.RotateAround(transform.position, transform.forward, 0f - num);
			base.AssignColliders(draggable);
		}

		public override void Connect(Moveable_Drag draggable)
		{
			if (draggable._rigidbody.useGravity)
			{
				draggable._rigidbody.useGravity = false;
				Debug.LogWarning("Curved tracks do not work with Rigidbodys that obey gravity - disabling");
			}
			startPosition = transform.position + radius * transform.right;
			if (doLoop)
			{
				maxAngle = 360f;
				base.AssignColliders(draggable);
			}
			else
			{
				AssignColliders(draggable);
			}
		}

		public override void ApplyAutoForce(float _position, float _speed, Moveable_Drag draggable)
		{
			Vector3 vector = draggable.transform.up * _speed;
			if (draggable.trackValue < _position)
			{
				draggable._rigidbody.AddForce(vector * Time.deltaTime);
			}
			else
			{
				draggable._rigidbody.AddForce(-vector * Time.deltaTime);
			}
		}

		public override void ApplyDragForce(Vector3 force, Moveable_Drag draggable)
		{
			float num = Vector3.Dot(force, draggable.transform.up);
			Vector3 force2 = draggable.transform.up * num;
			draggable._rigidbody.AddForce(force2);
		}

		public override void SetPositionAlong(float proportionAlong, Moveable_Drag draggable)
		{
			Quaternion angle = Quaternion.AngleAxis(proportionAlong * maxAngle, transform.forward);
			draggable.transform.position = RotatePointAroundPivot(startPosition, transform.position, angle);
			draggable.transform.rotation = Quaternion.AngleAxis(proportionAlong * maxAngle, transform.forward) * transform.rotation;
			if (!doLoop)
			{
				UpdateColliders(proportionAlong, draggable);
			}
		}

		private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle)
		{
			return angle * (point - pivot) + pivot;
		}

		public override float GetDecimalAlong(Moveable_Drag draggable)
		{
			float num = Vector3.Angle(-transform.right, draggable.transform.position - transform.position);
			if (num < 170f && Vector3.Dot(draggable.transform.position - transform.position, transform.up) < 0f)
			{
				num *= -1f;
			}
			return (180f - num) / maxAngle;
		}

		public override void SnapToTrack(Moveable_Drag draggable, bool onStart)
		{
			Vector3 vector = draggable.transform.position - transform.position;
			draggable.transform.position = transform.position + vector / (vector.magnitude / radius);
			if (onStart)
			{
				float num = GetDecimalAlong(draggable);
				if (num < 0f)
				{
					num = 0f;
				}
				else if (num > 1f)
				{
					num = 1f;
				}
				draggable.transform.rotation = Quaternion.AngleAxis(num * maxAngle, transform.forward) * transform.rotation;
				SetPositionAlong(num, draggable);
			}
			else
			{
				draggable.transform.rotation = Quaternion.AngleAxis(draggable.trackValue * maxAngle, transform.forward) * transform.rotation;
			}
		}

		public override void UpdateDraggable(Moveable_Drag draggable)
		{
			draggable.trackValue = GetDecimalAlong(draggable);
			SnapToTrack(draggable, false);
			if (!doLoop)
			{
				UpdateColliders(draggable.trackValue, draggable);
			}
		}

		private void UpdateColliders(float trackValue, Moveable_Drag draggable)
		{
			if (!(trackValue > 1f))
			{
				if (trackValue > 0.5f)
				{
					draggable.minCollider.isTrigger = true;
					draggable.maxCollider.isTrigger = false;
				}
				else
				{
					draggable.minCollider.isTrigger = false;
					draggable.maxCollider.isTrigger = true;
				}
			}
		}
	}
}
