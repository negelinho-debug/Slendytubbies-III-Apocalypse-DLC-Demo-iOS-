using UnityEngine;

namespace AC
{
	public class JointBreaker : MonoBehaviour
	{
		private void OnJointBreak(float breakForce)
		{
			GetComponent<FixedJoint>().connectedBody.GetComponent<Moveable_PickUp>().UnsetFixedJoint();
			Object.Destroy(gameObject);
		}
	}
}
