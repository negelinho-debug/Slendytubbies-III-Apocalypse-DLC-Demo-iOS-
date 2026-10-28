using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class NPC : Char
	{
		public Char followTarget;

		public bool followTargetIsPlayer;

		public float followFrequency;

		public float followDistance;

		public float followDistanceMax;

		public bool moveOutOfPlayersWay;

		public float minPlayerDistance = 1f;

		private NavigationManager navigationManager;

		private LayerMask LayerOn;

		private LayerMask LayerOff;

		private void Awake()
		{
			GetSettingsManager();
			LayerOn = LayerMask.NameToLayer(settingsManager.hotspotLayer);
			LayerOff = LayerMask.NameToLayer(settingsManager.deactivatedLayer);
			navigationManager = GameObject.FindWithTag("GameEngine").GetComponent<NavigationManager>();
			_Awake();
		}

		private void FixedUpdate()
		{
			if ((bool)activePath && (bool)followTarget)
			{
				FollowCheckDistance();
				FollowCheckDistanceMax();
			}
			if ((bool)activePath && !pausePath)
			{
				if (IsTurningBeforeWalking())
				{
					charState = CharState.Idle;
				}
				else
				{
					charState = CharState.Move;
					CheckIfStuck();
				}
			}
			_FixedUpdate();
		}

		private void Update()
		{
			if (moveOutOfPlayersWay && charState == CharState.Idle)
			{
				StayAwayFromPlayer();
			}
			_Update();
		}

		private void StayAwayFromPlayer()
		{
			if (!(followTarget == null) || !(KickStarter.player != null) || !(Vector3.Distance(transform.position, KickStarter.player.transform.position) < minPlayerDistance))
			{
				return;
			}
			Vector3[] array = TryNavPoint(transform.position - KickStarter.player.transform.position);
			int num = 0;
			if (array == null)
			{
				array = TryNavPoint(Vector3.Cross(transform.up, transform.position - KickStarter.player.transform.position).normalized);
				num++;
			}
			if (array == null)
			{
				array = TryNavPoint(Vector3.Cross(-transform.up, transform.position - KickStarter.player.transform.position).normalized);
				num++;
			}
			if (array == null)
			{
				array = TryNavPoint(KickStarter.player.transform.position - transform.position);
				num++;
			}
			if (array != null)
			{
				if (num == 0)
				{
					MoveAlongPoints(array, false);
				}
				else
				{
					MoveToPoint(array[array.Length - 1], false);
				}
			}
		}

		private Vector3[] TryNavPoint(Vector3 _direction)
		{
			Vector3 vector = transform.position + _direction.normalized * minPlayerDistance * 1.2f;
			if (settingsManager.ActInScreenSpace())
			{
				vector = AdvGame.GetScreenNavMesh(vector);
			}
			else if (settingsManager.cameraPerspective == CameraPerspective.ThreeD)
			{
				vector.y = transform.position.y;
			}
			Vector3[] pointsArray = navigationManager.navigationEngine.GetPointsArray(transform.position, vector);
			if (Vector3.Distance(pointsArray[pointsArray.Length - 1], transform.position) < minPlayerDistance * 0.6f)
			{
				return null;
			}
			return pointsArray;
		}

		public void FollowReset()
		{
			FollowStop();
			followTarget = null;
			followTargetIsPlayer = false;
			followFrequency = 0f;
			followDistance = 0f;
		}

		private void FollowUpdate()
		{
			if ((bool)followTarget)
			{
				FollowMove();
				Invoke("FollowUpdate", followFrequency);
			}
		}

		private void FollowMove()
		{
			float num = FollowCheckDistance();
			if (!(num > followDistance))
			{
				return;
			}
			Paths component = GetComponent<Paths>();
			if (component == null)
			{
				Debug.LogWarning("Cannot move a character with no Paths component");
				return;
			}
			component.pathType = AC_PathType.ForwardOnly;
			component.affectY = true;
			Vector3 vector = followTarget.transform.position;
			if ((bool)settingsManager && settingsManager.ActInScreenSpace())
			{
				vector = AdvGame.GetScreenNavMesh(vector);
			}
			Vector3[] pointData;
			if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<NavigationManager>())
			{
				pointData = GameObject.FindWithTag("GameEngine").GetComponent<NavigationManager>().navigationEngine.GetPointsArray(transform.position, vector);
			}
			else
			{
				List<Vector3> list = new List<Vector3>();
				list.Add(vector);
				pointData = list.ToArray();
			}
			if (num > followDistanceMax)
			{
				MoveAlongPoints(pointData, true);
			}
			else
			{
				MoveAlongPoints(pointData, false);
			}
		}

		private float FollowCheckDistance()
		{
			float num = Vector3.Distance(followTarget.transform.position, transform.position);
			if (num < followDistance)
			{
				EndPath();
			}
			return num;
		}

		private void FollowCheckDistanceMax()
		{
			if (!followTarget)
			{
				return;
			}
			if (FollowCheckDistance() > followDistanceMax)
			{
				if (!isRunning)
				{
					FollowMove();
				}
			}
			else if (isRunning)
			{
				FollowMove();
			}
		}

		private void FollowStop()
		{
			StopCoroutine("FollowUpdate");
			if (followTarget != null)
			{
				EndPath();
			}
		}

		public void FollowAssign(Char _followTarget, bool _followTargetIsPlayer, float _followFrequency, float _followDistance, float _followDistanceMax)
		{
			if (_followTargetIsPlayer)
			{
				_followTarget = KickStarter.player;
			}
			if (_followTarget == null || _followFrequency == 0f || _followFrequency < 0f || _followDistance <= 0f || _followDistanceMax <= 0f)
			{
				FollowReset();
				return;
			}
			followTarget = _followTarget;
			followTargetIsPlayer = _followTargetIsPlayer;
			followFrequency = _followFrequency;
			followDistance = _followDistance;
			followDistanceMax = _followDistanceMax;
			FollowUpdate();
		}

		private void TurnOn()
		{
			gameObject.layer = LayerOn;
		}

		private void TurnOff()
		{
			gameObject.layer = LayerOff;
		}
	}
}
