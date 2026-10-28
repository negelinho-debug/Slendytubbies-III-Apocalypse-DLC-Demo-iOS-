using UnityEngine;

namespace AC
{
	public class DragTrack_Straight : DragTrack
	{
		public DragRotationType rotationType;

		public float maxDistance = 2f;

		public bool dragMustScrew;

		public float screwThread = 1f;

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
			draggable.maxCollider.transform.position = transform.position + transform.up * maxDistance + transform.up * draggable.colliderRadius;
			draggable.minCollider.transform.position = transform.position - transform.up * draggable.colliderRadius;
			draggable.minCollider.transform.up = transform.up;
			draggable.maxCollider.transform.up = -transform.up;
			base.AssignColliders(draggable);
		}

		public override void Connect(Moveable_Drag draggable)
		{
			AssignColliders(draggable);
		}

		public override float GetDecimalAlong(Moveable_Drag draggable)
		{
			return (draggable.transform.position - transform.position).magnitude / maxDistance;
		}

		public override void SetPositionAlong(float proportionAlong, Moveable_Drag draggable)
		{
			draggable.transform.position = transform.position + transform.up * proportionAlong * maxDistance;
			if (rotationType != DragRotationType.None)
			{
				SetRotation(draggable, proportionAlong);
			}
		}

		public override void SnapToTrack(Moveable_Drag draggable, bool onStart)
		{
			Vector3 lhs = draggable.transform.position - transform.position;
			float num = Vector3.Dot(lhs, transform.up) / maxDistance;
			if (onStart)
			{
				if (num < 0f)
				{
					num = 0f;
				}
				else if (num > 1f)
				{
					num = 1f;
				}
				if (rotationType != DragRotationType.None)
				{
					SetRotation(draggable, num);
				}
				Rigidbody rigidbody = draggable._rigidbody;
				Vector3 zero = Vector3.zero;
				draggable._rigidbody.angularVelocity = zero;
				rigidbody.velocity = zero;
			}
			draggable.transform.position = transform.position + transform.up * num * maxDistance;
		}

		public override void ApplyAutoForce(float _position, float _speed, Moveable_Drag draggable)
		{
			Vector3 vector = transform.up * _speed;
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
			float num = 0f;
			if (rotationType == DragRotationType.Screw)
			{
				if (dragMustScrew)
				{
					draggable.UpdateScrewVector();
				}
				num = Vector3.Dot(force, draggable._dragVector);
			}
			else
			{
				num = Vector3.Dot(force, transform.up);
			}
			Vector3 force2 = transform.up * num;
			if (rotationType == DragRotationType.Screw)
			{
				if (dragMustScrew)
				{
					force2 = (transform.up * num).normalized * force.magnitude;
					force2 /= Mathf.Sqrt((draggable.GetGrabPosition() - draggable.transform.position).magnitude) / 0.4f;
				}
				force2 /= Mathf.Sqrt(screwThread);
			}
			draggable._rigidbody.AddForce(force2);
		}

		public override bool IconIsStationary()
		{
			if (rotationType == DragRotationType.Roll || (rotationType == DragRotationType.Screw && !dragMustScrew))
			{
				return true;
			}
			return false;
		}

		public override void UpdateDraggable(Moveable_Drag draggable)
		{
			SnapToTrack(draggable, false);
			draggable.trackValue = GetDecimalAlong(draggable);
			if (rotationType != DragRotationType.None)
			{
				SetRotation(draggable, draggable.trackValue);
			}
		}

		private void SetRotation(Moveable_Drag draggable, float proportionAlong)
		{
			float num = proportionAlong * maxDistance / draggable.colliderRadius / 2f * 57.29578f;
			if (rotationType == DragRotationType.Roll)
			{
				draggable._rigidbody.rotation = Quaternion.AngleAxis(num, transform.forward) * transform.rotation;
			}
			else if (rotationType == DragRotationType.Screw)
			{
				draggable._rigidbody.rotation = Quaternion.AngleAxis(num * screwThread, transform.up) * transform.rotation;
			}
		}
	}
}
