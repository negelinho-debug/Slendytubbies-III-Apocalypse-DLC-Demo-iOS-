using UnityEngine;

namespace AC
{
	public class GameCameraAnimated : _Camera
	{
		public bool followCursor;

		public Vector2 cursorInfluence = new Vector2(0.3f, 0.1f);

		public AnimationClip clip;

		public bool loopClip;

		public bool playOnStart;

		public AnimatedCameraType animatedCameraType;

		public Paths pathToFollow;

		private float pathLength;

		public void Start()
		{
			if (animatedCameraType == AnimatedCameraType.PlayWhenActive)
			{
				if (playOnStart)
				{
					PlayClip();
				}
			}
			else if ((bool)pathToFollow)
			{
				pathLength = pathToFollow.GetTotalLength();
				ResetTarget();
				if ((bool)target)
				{
					MoveCameraInstant();
				}
			}
		}

		private void Update()
		{
			MoveCamera();
		}

		public void SwitchTarget(Transform _target)
		{
			target = _target;
		}

		public bool isPlaying()
		{
			if ((bool)clip && (bool)GetComponent<Animation>() && GetComponent<Animation>().IsPlaying(clip.name))
			{
				return true;
			}
			return false;
		}

		public void PlayClip()
		{
			if (GetComponent<Animation>() == null)
			{
				Debug.LogError("Cannot play animation on " + name + " - no Animation component is attached.");
			}
			else if ((bool)clip && animatedCameraType == AnimatedCameraType.PlayWhenActive)
			{
				WrapMode wrapMode = WrapMode.Once;
				if (loopClip)
				{
					wrapMode = WrapMode.Loop;
				}
				AdvGame.PlayAnimClip(GetComponent<Animation>(), 0, clip, AnimationBlendMode.Blend, wrapMode, 0f, null, false);
			}
		}

		public override void MoveCameraInstant()
		{
			MoveCamera();
		}

		private void MoveCamera()
		{
			if ((bool)target && animatedCameraType == AnimatedCameraType.SyncWithTargetMovement && (bool)clip && (bool)target)
			{
				AdvGame.PlayAnimClipFrame(GetComponent<Animation>(), 0, clip, AnimationBlendMode.Blend, WrapMode.Once, 0f, null, GetProgress());
			}
		}

		private float GetProgress()
		{
			if (pathToFollow.nodes.Count <= 1)
			{
				return 0f;
			}
			double num = 1000.0;
			Vector3 vector = Vector3.zero;
			int num2 = 0;
			for (num2 = 1; num2 < pathToFollow.nodes.Count; num2++)
			{
				Vector3 p = pathToFollow.nodes[num2 - 1];
				Vector3 p2 = pathToFollow.nodes[num2];
				Vector3 nearestPointOnSegment = GetNearestPointOnSegment(p, p2);
				if (nearestPointOnSegment != vector)
				{
					float num3 = Mathf.Sqrt(Vector3.Distance(target.position, nearestPointOnSegment));
					if (!((double)num3 < num))
					{
						break;
					}
					num = num3;
					vector = nearestPointOnSegment;
				}
			}
			return (pathToFollow.GetLengthToNode(num2 - 2) + Vector3.Distance(pathToFollow.nodes[num2 - 2], vector)) / pathLength;
		}

		private Vector3 GetNearestPointOnSegment(Vector3 p1, Vector3 p2)
		{
			float num = (p1.x - p2.x) * (p1.x - p2.x) + (p1.z - p2.z) * (p1.z - p2.z);
			float num2 = ((target.position.x - p1.x) * (p2.x - p1.x) + (target.position.z - p1.z) * (p2.z - p1.z)) / num;
			if (num2 < 0f)
			{
				return p1;
			}
			if (num2 > 1f)
			{
				return p2;
			}
			return new Vector3(p1.x + num2 * (p2.x - p1.x), 0f, p1.z + num2 * (p2.z - p1.z));
		}
	}
}
