using System.Collections.Generic;
using UnityEngine;

namespace FSG.MeshAnimator
{
	public class MeshAnimatorManager : MonoBehaviour
	{
		private static MeshAnimatorManager mInstance = null;

		private static List<MeshAnimator> mAnimators = new List<MeshAnimator>(100);

		public static int AnimatorCount
		{
			get
			{
				if ((bool)Instance)
				{
					return mAnimators.Count;
				}
				return 0;
			}
		}

		public static MeshAnimatorManager Instance
		{
			get
			{
				if (mInstance == null)
				{
					mInstance = Object.FindObjectOfType<MeshAnimatorManager>();
					if (mInstance == null)
					{
						mInstance = new GameObject("MeshAnimatorManager").AddComponent<MeshAnimatorManager>();
					}
				}
				return mInstance;
			}
		}

		public static void AddAnimator(MeshAnimator animator)
		{
			if ((bool)Instance)
			{
				mAnimators.Add(animator);
			}
		}

		public static void RemoveAnimator(MeshAnimator animator)
		{
			mAnimators.Remove(animator);
		}

		private void Update()
		{
			float time = Time.time;
			int count = mAnimators.Count;
			for (int i = 0; i < count; i++)
			{
				MeshAnimator meshAnimator = mAnimators[i];
				if (time >= meshAnimator.nextTick)
				{
					meshAnimator.UpdateTick(time);
				}
			}
		}
	}
}
