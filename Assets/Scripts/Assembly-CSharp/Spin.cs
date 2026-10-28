using UnityEngine;

public class Spin : MonoBehaviour
{
	public float speed;

	public Vector3 direction;

	private void Update()
	{
		transform.Rotate(direction * speed * 100f * Time.deltaTime);
	}
}
