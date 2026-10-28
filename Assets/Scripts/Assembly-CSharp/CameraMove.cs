using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public float speed = 1f;

	private void Update()
	{
		Vector3 position = transform.position;
		position.x += speed;
		transform.position = position;
	}
}
