using UnityEngine;

public class ArmSlerp : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Update()
	{
		Vector3 localPosition = transform.localPosition;
		int num = Random.Range(-2, 3);
		int num2 = Random.Range(-1, 10);
		if (num2 == 1)
		{
			transform.position = Vector3.Slerp(localPosition, new Vector3(localPosition.x, localPosition.y + (float)num, localPosition.z), 1f);
		}
	}
}
