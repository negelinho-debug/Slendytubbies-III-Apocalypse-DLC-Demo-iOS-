using UnityEngine;

public class RotateIt : MonoBehaviour
{
	private void Update()
	{
		transform.Rotate(Vector3.forward * Time.deltaTime * 10f);
	}
}
