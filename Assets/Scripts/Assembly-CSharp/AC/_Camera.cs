using UnityEngine;

namespace AC
{
	public class _Camera : MonoBehaviour
	{
		public Camera _camera;

		public bool targetIsPlayer = true;

		public Transform target;

		public bool isDragControlled;

		public float focalDistance = 10f;

		protected Vector2 inputMovement;

		protected virtual void Awake()
		{
			if ((bool)GetComponent<Camera>())
			{
				_camera = GetComponent<Camera>();
				if ((bool)KickStarter.mainCamera)
				{
					_camera.enabled = false;
				}
			}
			else
			{
				Debug.LogWarning(name + " has no Camera component!");
			}
		}

		public void SetCameraComponent()
		{
			if (_camera == null && (bool)GetComponent<Camera>())
			{
				_camera = GetComponent<Camera>();
			}
		}

		public void ResetTarget()
		{
			if (targetIsPlayer && (bool)KickStarter.player)
			{
				target = KickStarter.player.transform;
			}
		}

		public Vector3 PositionRelativeToCamera(Vector3 _position)
		{
			return _position.x * ForwardVector() + _position.z * RightVector();
		}

		public Vector3 RightVector()
		{
			return transform.right;
		}

		public Vector3 ForwardVector()
		{
			Vector3 forward = transform.forward;
			forward.y = 0f;
			return forward;
		}

		public virtual void MoveCameraInstant()
		{
		}

		protected float ConstrainAxis(float desired, Vector2 range)
		{
			desired = ((range.x < range.y) ? Mathf.Clamp(desired, range.x, range.y) : ((!(range.x > range.y)) ? range.x : Mathf.Clamp(desired, range.y, range.x)));
			return desired;
		}

		public void SetSplitScreen()
		{
			_camera.enabled = true;
			_camera.rect = KickStarter.mainCamera.GetSplitScreenRect(false);
		}

		public void RemoveSplitScreen()
		{
			if (_camera.enabled)
			{
				_camera.rect = new Rect(0f, 0f, 1f, 1f);
				_camera.enabled = false;
			}
		}
	}
}
