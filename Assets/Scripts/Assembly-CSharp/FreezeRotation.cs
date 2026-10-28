using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
	public bool getRot;

	public Quaternion rot;

	private void Awake()
	{
		if (getRot)
		{
			rot = transform.rotation;
		}
	}

	private void Update()
	{
		transform.rotation = rot;
	}
}
