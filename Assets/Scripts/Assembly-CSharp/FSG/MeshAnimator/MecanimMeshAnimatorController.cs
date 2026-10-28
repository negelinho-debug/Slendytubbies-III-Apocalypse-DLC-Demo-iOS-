using System.Collections.Generic;
using UnityEngine;

namespace FSG.MeshAnimator
{
	public class MecanimMeshAnimatorController : MonoBehaviour
	{
		public Animator animator;

		public MeshAnimator meshAnimator;

		public bool crossFade;

		private Dictionary<int, string> animHashes = new Dictionary<int, string>();

		private string cAnim = string.Empty;

		protected virtual void Awake()
		{
			if (!meshAnimator)
			{
				Debug.LogError("MecanimMeshAnimatorController.meshAnimator is null", this);
				return;
			}
			for (int i = 0; i < meshAnimator.animations.Length; i++)
			{
				animHashes.Add(Animator.StringToHash("Base Layer." + meshAnimator.animations[i].name), meshAnimator.animations[i].name);
			}
		}

		protected virtual void LateUpdate()
		{
			int fullPathHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
			if (animHashes.ContainsKey(fullPathHash) && cAnim != animHashes[fullPathHash])
			{
				cAnim = animHashes[fullPathHash];
				if (crossFade)
				{
					meshAnimator.Crossfade(animHashes[fullPathHash]);
				}
				else
				{
					meshAnimator.Play(animHashes[fullPathHash]);
				}
			}
		}
	}
}
