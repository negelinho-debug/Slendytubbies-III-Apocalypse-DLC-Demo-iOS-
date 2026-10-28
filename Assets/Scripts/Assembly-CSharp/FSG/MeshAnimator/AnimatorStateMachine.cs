using System;
using UnityEngine;

namespace FSG.MeshAnimator
{
	public class AnimatorStateMachine : MonoBehaviour
	{
		public MeshAnimator meshAnimator;

		public bool crossFade;

		private void Start()
		{
			this.meshAnimator.Play();
			MeshAnimator meshAnimator = this.meshAnimator;
			meshAnimator.OnAnimationFinished = (Action<string>)Delegate.Combine(meshAnimator.OnAnimationFinished, new Action<string>(OnAnimationFinished));
		}

		private void OnAnimationFinished(string anim)
		{
			int index = 0;
			switch (anim)
			{
			case "idle":
				index = 2;
				break;
			case "run_forward":
				index = 1;
				break;
			case "run_backward":
				index = 3;
				break;
			case "run_left":
				index = 4;
				break;
			case "run_right":
				index = 0;
				break;
			}
			if (crossFade)
			{
				meshAnimator.Crossfade(index);
			}
			else
			{
				meshAnimator.Play(index);
			}
		}
	}
}
