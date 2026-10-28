using UnityEngine;

public class KeepInRatio : MonoBehaviour
{
	public Vector4 ratio;

	private Vector3 localPosition;

	private void Awake()
	{
		localPosition = transform.localPosition;
	}

	private void Update()
	{
		transform.localPosition = localPosition;
		if (transform.position.x < ratio.x)
		{
			Vector3 position = transform.position;
			position.x = ratio.x;
			transform.position = position;
		}
		if (transform.position.x > ratio.y)
		{
			Vector3 position2 = transform.position;
			position2.x = ratio.y;
			transform.position = position2;
		}
		if (transform.position.z > ratio.z)
		{
			Vector3 position3 = transform.position;
			position3.z = ratio.z;
			transform.position = position3;
		}
		if (transform.position.z < ratio.w)
		{
			Vector3 position4 = transform.position;
			position4.z = ratio.w;
			transform.position = position4;
		}
	}
}
