using UnityEngine;

public class ForceLookAt : MonoBehaviour
{
	public Transform lookAt;

	private void LateUpdate()
	{
		transform.LookAt(lookAt);
		transform.localEulerAngles = new Vector3(0f - transform.localEulerAngles.x, -180f, -90f);
	}
}
