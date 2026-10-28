using UnityEngine;

public class IKScript : MonoBehaviour
{
	public Animator animator;

	public Transform LHandPos1;

	public float LHandWeight;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void OnAnimatorIK(int layerIndex)
	{
		LHandWeight = animator.GetFloat("LHandWeight");
		animator.SetIKPosition(AvatarIKGoal.LeftHand, LHandPos1.position);
		animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, LHandWeight);
	}
}
