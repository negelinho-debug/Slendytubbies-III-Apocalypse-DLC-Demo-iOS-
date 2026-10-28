using UnityEngine;

namespace AC
{
	public class DragTrack : MonoBehaviour
	{
		public PhysicMaterial colliderMaterial;

		public float discSize = 0.2f;

		public virtual void AssignColliders(Moveable_Drag draggable)
		{
			draggable.maxCollider.transform.rotation = Quaternion.AngleAxis(90f, draggable.maxCollider.transform.right) * draggable.maxCollider.transform.rotation;
			draggable.minCollider.transform.rotation = Quaternion.AngleAxis(90f, draggable.minCollider.transform.right) * draggable.minCollider.transform.rotation;
			if ((bool)colliderMaterial)
			{
				draggable.maxCollider.material = colliderMaterial;
				draggable.minCollider.material = colliderMaterial;
			}
			draggable.maxCollider.transform.parent = transform;
			draggable.minCollider.transform.parent = transform;
			draggable.maxCollider.name = draggable.name + "_UpperLimit";
			draggable.minCollider.name = draggable.name + "_LowerLimit";
			LimitCollisions(draggable);
		}

		public virtual float GetDecimalAlong(Moveable_Drag draggable)
		{
			return 0f;
		}

		public virtual void SetPositionAlong(float dec, Moveable_Drag draggable)
		{
		}

		public virtual void Connect(Moveable_Drag draggable)
		{
		}

		public virtual void ApplyDragForce(Vector3 force, Moveable_Drag draggable)
		{
		}

		public virtual void ApplyAutoForce(float _position, float _speed, Moveable_Drag draggable)
		{
		}

		public virtual void UpdateDraggable(Moveable_Drag draggable)
		{
			draggable.trackValue = GetDecimalAlong(draggable);
		}

		public virtual void SnapToTrack(Moveable_Drag draggable, bool onStart)
		{
		}

		protected void LimitCollisions(Moveable_Drag draggable)
		{
			Collider[] array = Object.FindObjectsOfType(typeof(Collider)) as Collider[];
			Collider[] componentsInChildren = draggable.GetComponentsInChildren<Collider>();
			if (draggable.minCollider != null && draggable.maxCollider != null)
			{
				Collider[] array2 = array;
				foreach (Collider collider in array2)
				{
					if (collider.enabled)
					{
						if (collider != draggable.minCollider)
						{
							Physics.IgnoreCollision(collider, draggable.minCollider, true);
						}
						if (collider != draggable.maxCollider)
						{
							Physics.IgnoreCollision(collider, draggable.maxCollider, true);
						}
					}
				}
			}
			Collider[] array3 = array;
			foreach (Collider collider2 in array3)
			{
				Collider[] array4 = componentsInChildren;
				foreach (Collider collider3 in array4)
				{
					if (!(collider2 == collider3))
					{
						bool ignore = true;
						if ((draggable.minCollider != null && draggable.minCollider == collider2) || (draggable.maxCollider != null && draggable.maxCollider == collider2))
						{
							ignore = false;
						}
						else if (collider2.gameObject.tag == "Player")
						{
							ignore = draggable.ignorePlayerCollider;
						}
						else if ((bool)collider2.GetComponent<Rigidbody>() && collider2.gameObject != draggable.gameObject)
						{
							ignore = (bool)collider2.GetComponent<Moveable>() && draggable.ignoreMoveableRigidbodies;
						}
						if (collider2.enabled && collider3.enabled)
						{
							Physics.IgnoreCollision(collider2, collider3, ignore);
						}
					}
				}
			}
			if (!(draggable.minCollider != null) || !(draggable.maxCollider != null))
			{
				return;
			}
			Collider[] array5 = componentsInChildren;
			foreach (Collider collider4 in array5)
			{
				if (collider4.enabled && draggable.minCollider.enabled)
				{
					Physics.IgnoreCollision(collider4, draggable.minCollider, false);
				}
				if (collider4.enabled && draggable.maxCollider.enabled)
				{
					Physics.IgnoreCollision(collider4, draggable.maxCollider, false);
				}
			}
		}

		public virtual bool IconIsStationary()
		{
			return false;
		}
	}
}
