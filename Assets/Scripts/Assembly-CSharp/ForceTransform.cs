using UnityEngine;

public class ForceTransform : MonoBehaviour
{
	public bool usePosition;

	public bool useRotation;

	public bool useScale;

	private Vector3 orgPos;

	private Vector3 orgRot;

	private Vector3 orgScale;

	private void Awake()
	{
		if (usePosition)
		{
			orgPos = transform.localPosition;
		}
		if (useRotation)
		{
			orgRot = transform.localEulerAngles;
		}
		if (useScale)
		{
			orgScale = transform.localScale;
		}
	}

	private void LateUpdate()
	{
		if (usePosition)
		{
			transform.localPosition = orgPos;
		}
		if (useRotation)
		{
			transform.localEulerAngles = orgRot;
		}
		if (useScale)
		{
			transform.localScale = orgScale;
		}
	}
}
