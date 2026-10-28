using UnityEngine;

namespace AC
{
	public class DragBase : Moveable
	{
		public bool isHeld;

		public bool invertInput;

		public float maxSpeed = 200f;

		public bool allowZooming;

		public float zoomSpeed = 60f;

		public float minZoom = 1f;

		public float maxZoom = 3f;

		public float rotationFactor = 1f;

		public Rigidbody _rigidbody;

		public bool showIcon;

		public int iconID = -1;

		public AudioClip moveSoundClip;

		public AudioClip collideSoundClip;

		public float slideSoundThreshold = 0.03f;

		public float slidePitchFactor = 1f;

		public bool onlyPlayLowerCollisionSound;

		public bool ignoreMoveableRigidbodies;

		public bool ignorePlayerCollider;

		public bool childrenShareLayer;

		protected Transform grabPoint;

		protected Transform cameraTransform;

		protected float distanceToCamera;

		protected float speedFactor = 0.16f;

		protected float originalDrag;

		protected float originalAngularDrag;

		protected int numCollisions;

		protected CursorIconBase icon;

		protected Sound collideSound;

		protected Sound moveSound;

		protected virtual void Awake()
		{
			GameObject gameObject = new GameObject();
			gameObject.name = name + " (Grab point)";
			grabPoint = gameObject.transform;
			grabPoint.parent = transform;
			if ((bool)moveSoundClip)
			{
				GameObject gameObject2 = new GameObject();
				gameObject2.name = name + " (Move sound)";
				gameObject2.transform.parent = transform;
				gameObject2.AddComponent<Sound>();
				gameObject2.GetComponent<AudioSource>().playOnAwake = false;
				moveSound = gameObject2.GetComponent<Sound>();
			}
			icon = GetMainIcon();
			if ((bool)Camera.main)
			{
				cameraTransform = Camera.main.transform;
			}
			if ((bool)GetComponent<Sound>())
			{
				collideSound = GetComponent<Sound>();
			}
			if ((bool)GetComponent<Rigidbody>())
			{
				_rigidbody = GetComponent<Rigidbody>();
			}
			else
			{
				Debug.LogWarning("A Rigidbody component is required for " + name);
			}
		}

		public void TurnOn()
		{
			PlaceOnLayer(LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer));
		}

		public void TurnOff()
		{
			PlaceOnLayer(LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.deactivatedLayer));
		}

		private void PlaceOnLayer(int layerName)
		{
			gameObject.layer = layerName;
			if (!childrenShareLayer)
			{
				return;
			}
			foreach (Transform item in base.transform)
			{
				item.gameObject.layer = layerName;
			}
		}

		public virtual void UpdateMovement()
		{
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag != "Player")
			{
				numCollisions++;
				if ((bool)collideSound && (bool)collideSoundClip && Time.time > 0f)
				{
					collideSound.Play(collideSoundClip, false);
				}
			}
		}

		private void OnCollisionExit(Collision collision)
		{
			if (collision.gameObject.tag != "Player")
			{
				numCollisions--;
			}
		}

		protected void PlayMoveSound(float speed, float trackValue)
		{
			if (speed > slideSoundThreshold)
			{
				moveSound.relativeVolume = speed - slideSoundThreshold;
				moveSound.SetMaxVolume();
				if (slidePitchFactor > 0f)
				{
					moveSound.GetComponent<AudioSource>().pitch = Mathf.Lerp(GetComponent<AudioSource>().pitch, Mathf.Min(1f, speed), Time.deltaTime * 5f);
				}
			}
			if (speed > slideSoundThreshold && !moveSound.IsPlaying())
			{
				moveSound.relativeVolume = speed - slideSoundThreshold;
				moveSound.Play(moveSoundClip, true);
			}
			else if (speed <= slideSoundThreshold && moveSound.IsPlaying() && !moveSound.isFading)
			{
				moveSound.FadeOut(0.2f);
			}
		}

		public virtual void DrawGrabIcon()
		{
			if (isHeld && showIcon && Camera.main.WorldToScreenPoint(transform.position).z > 0f && icon != null)
			{
				Vector3 vector = Camera.main.WorldToScreenPoint(grabPoint.position);
				icon.Draw(new Vector3(vector.x, vector.y));
			}
		}

		public virtual void LetGo()
		{
			Debug.Log("Letgo");
			isHeld = false;
			_rigidbody.drag = originalDrag;
			_rigidbody.angularDrag = originalAngularDrag;
		}

		public virtual void Grab(Vector3 grabPosition)
		{
			isHeld = true;
			grabPoint.position = grabPosition;
			originalDrag = _rigidbody.drag;
			originalAngularDrag = _rigidbody.angularDrag;
			_rigidbody.drag = 20f;
			_rigidbody.angularDrag = 20f;
		}

		public bool IsOnScreen()
		{
			Vector2 vector = Camera.main.WorldToScreenPoint(grabPoint.position);
			if (vector.x < 0f || vector.y < 0f || vector.x > AdvGame.GetMainGameViewSize().x || vector.y > AdvGame.GetMainGameViewSize().y)
			{
				return false;
			}
			return true;
		}

		public bool IsCloseToCamera(float maxDistance)
		{
			if ((GetGrabPosition() - KickStarter.mainCamera.transform.position).magnitude < maxDistance)
			{
				return true;
			}
			return false;
		}

		public virtual void ApplyDragForce(Vector3 force, Vector3 mousePosition, float distanceToCamera)
		{
		}

		protected void UpdateZoom()
		{
			float axis = Input.GetAxis("ZoomMoveable");
			Vector3 normalized = (transform.position - cameraTransform.position).normalized;
			if (distanceToCamera - minZoom < 1f && axis < 0f)
			{
				normalized *= distanceToCamera - minZoom;
			}
			else if (maxZoom - distanceToCamera < 1f && axis > 0f)
			{
				normalized *= maxZoom - distanceToCamera;
			}
			if ((distanceToCamera < minZoom && axis < 0f) || (distanceToCamera > maxZoom && axis > 0f))
			{
				_rigidbody.AddForce(-normalized * axis * zoomSpeed);
				_rigidbody.velocity = Vector3.zero;
			}
			else
			{
				_rigidbody.AddForce(normalized * axis * zoomSpeed);
			}
		}

		protected void LimitZoom()
		{
			if (distanceToCamera < minZoom)
			{
				transform.position = cameraTransform.position + (transform.position - cameraTransform.position) / (distanceToCamera / minZoom);
			}
			else if (distanceToCamera > maxZoom)
			{
				transform.position = cameraTransform.position + (transform.position - cameraTransform.position) / (distanceToCamera / maxZoom);
			}
		}

		public Vector3 GetGrabPosition()
		{
			return grabPoint.position;
		}

		private CursorIconBase GetMainIcon()
		{
			if (AdvGame.GetReferences().cursorManager == null || iconID < 0)
			{
				return null;
			}
			return AdvGame.GetReferences().cursorManager.GetCursorIconFromID(iconID);
		}
	}
}
