using System.Collections;
using UnityEngine;

namespace AC
{
	public class GameCamera2D : _Camera
	{
		public bool lockHorizontal = true;

		public bool lockVertical = true;

		public bool limitHorizontal;

		public bool limitVertical;

		public Vector2 constrainHorizontal;

		public Vector2 constrainVertical;

		public Vector2 freedom = Vector2.zero;

		public float dampSpeed = 0.9f;

		public Vector2 afterOffset = Vector2.zero;

		public Vector2 perspectiveOffset = Vector2.zero;

		private Vector2 originalPosition = Vector2.zero;

		private Vector2 desiredOffset = Vector2.zero;

		private SettingsManager settingsManager;

		protected override void Awake()
		{
			originalPosition = new Vector2(transform.position.x, transform.position.y);
			base.Awake();
			settingsManager = AdvGame.GetReferences().settingsManager;
		}

		private void Start()
		{
			ResetTarget();
			if ((bool)target)
			{
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
		}

		private void SetDesired()
		{
			Vector2 offsetForPosition = GetOffsetForPosition(target.transform.position);
			if (offsetForPosition.x < perspectiveOffset.x - freedom.x)
			{
				desiredOffset.x = offsetForPosition.x + freedom.x;
			}
			else if (offsetForPosition.x > perspectiveOffset.x + freedom.x)
			{
				desiredOffset.x = offsetForPosition.x - freedom.x;
			}
			desiredOffset.x += afterOffset.x;
			if (limitHorizontal)
			{
				desiredOffset.x = ConstrainAxis(desiredOffset.x, constrainHorizontal);
			}
			if (offsetForPosition.y < perspectiveOffset.y - freedom.y)
			{
				desiredOffset.y = offsetForPosition.y + freedom.y;
			}
			else if (offsetForPosition.y > perspectiveOffset.y + freedom.y)
			{
				desiredOffset.y = offsetForPosition.y - freedom.y;
			}
			desiredOffset.y += afterOffset.y;
			if (limitVertical)
			{
				desiredOffset.y = ConstrainAxis(desiredOffset.y, constrainVertical);
			}
		}

		private void MoveCamera()
		{
			if (targetIsPlayer && (bool)KickStarter.player)
			{
				target = KickStarter.player.transform;
			}
			if ((bool)target && (!lockHorizontal || !lockVertical))
			{
				SetDesired();
				if (!lockHorizontal)
				{
					perspectiveOffset.x = Mathf.Lerp(perspectiveOffset.x, desiredOffset.x, Time.deltaTime * dampSpeed);
				}
				if (!lockVertical)
				{
					perspectiveOffset.y = Mathf.Lerp(perspectiveOffset.y, desiredOffset.y, Time.deltaTime * dampSpeed);
				}
			}
			else if (!GetComponent<Camera>().orthographic)
			{
				SnapToOffset();
			}
			SetProjection();
		}

		public override void MoveCameraInstant()
		{
			if (targetIsPlayer && (bool)KickStarter.player)
			{
				target = KickStarter.player.transform;
			}
			if ((bool)target && (!lockHorizontal || !lockVertical))
			{
				SetDesired();
				if (!lockHorizontal)
				{
					perspectiveOffset.x = desiredOffset.x;
				}
				if (!lockVertical)
				{
					perspectiveOffset.y = desiredOffset.y;
				}
			}
			SetProjection();
		}

		private void SetProjection()
		{
			if (!_camera.orthographic)
			{
				_camera.projectionMatrix = AdvGame.SetVanishingPoint(_camera, perspectiveOffset);
			}
			else
			{
				transform.position = new Vector3(originalPosition.x + perspectiveOffset.x, originalPosition.y + perspectiveOffset.y, transform.position.z);
			}
		}

		public void SnapToOffset()
		{
			perspectiveOffset = afterOffset;
			SetProjection();
		}

		public IEnumerator ResetProjection()
		{
			transform.position = new Vector3(originalPosition.x, originalPosition.y, transform.position.z);
			yield return new WaitForFixedUpdate();
			_camera.ResetProjectionMatrix();
		}

		private Vector2 GetOffsetForPosition(Vector3 targetPosition)
		{
			Vector2 result = default(Vector2);
			float num = 93f - 299f * _camera.nearClipPlane;
			if ((bool)settingsManager && settingsManager.IsTopDown())
			{
				if (_camera.orthographic)
				{
					result.x = transform.position.x;
					result.y = transform.position.z;
				}
				else
				{
					result.x = (0f - (targetPosition.x - transform.position.x)) / (num * (targetPosition.y - transform.position.y));
					result.y = (0f - (targetPosition.z - transform.position.z)) / (num * (targetPosition.y - transform.position.y));
				}
			}
			else if (_camera.orthographic)
			{
				result.x = targetPosition.x;
				result.y = targetPosition.y;
			}
			else
			{
				result.x = (targetPosition.x - transform.position.x) / (num * (targetPosition.z - transform.position.z));
				result.y = (targetPosition.y - transform.position.y) / (num * (targetPosition.z - transform.position.z));
			}
			return result;
		}

		public void SetCorrectRotation()
		{
			if (settingsManager == null)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			if (_camera == null)
			{
				_camera = GetComponent<Camera>();
			}
			if (settingsManager != null)
			{
				if (settingsManager.IsTopDown())
				{
					transform.rotation = Quaternion.Euler(90f, 0f, 0f);
					return;
				}
				if (settingsManager.IsUnity2D())
				{
					_camera.orthographic = true;
				}
			}
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}

		public bool IsCorrectRotation()
		{
			if (settingsManager == null)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
			}
			if (settingsManager != null && settingsManager.IsTopDown())
			{
				if (transform.rotation == Quaternion.Euler(90f, 0f, 0f))
				{
					return true;
				}
				return false;
			}
			if (transform.rotation == Quaternion.Euler(0f, 0f, 0f))
			{
				return true;
			}
			return false;
		}

		private void OnDestroy()
		{
			settingsManager = null;
		}
	}
}
