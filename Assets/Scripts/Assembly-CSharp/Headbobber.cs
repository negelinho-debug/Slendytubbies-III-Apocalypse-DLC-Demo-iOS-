using System;
using UnityEngine;

public class Headbobber : MonoBehaviour
{
	private float timer;

	private float bobbingSpeed = 0.125f;

	private float bobbingAmount = 0.05f;

	private float midpoint = 0.7f;

	private float actualSpeed;

	private void Awake()
	{
		actualSpeed = bobbingSpeed;
	}

	private void FixedUpdate()
	{
		float num = 0f;
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		if (Mathf.Abs(axis) == 0f && Mathf.Abs(axis2) == 0f)
		{
			timer = 0f;
		}
		else
		{
			num = Mathf.Sin(timer);
			timer += actualSpeed;
			if (timer > (float)Math.PI * 2f)
			{
				timer -= (float)Math.PI * 2f;
			}
		}
		Vector3 localPosition = transform.localPosition;
		if (num != 0f)
		{
			float num2 = num * bobbingAmount;
			float value = Mathf.Abs(axis) + Mathf.Abs(axis2);
			value = Mathf.Clamp(value, 0f, 1f);
			num2 = value * num2;
			localPosition.y = midpoint + num2;
		}
		else
		{
			localPosition.y = midpoint;
		}
		transform.localPosition = localPosition;
		if (Input.GetButton("Run") && !Input.GetButton("Crouch"))
		{
			actualSpeed = bobbingSpeed * 2f;
		}
		else
		{
			actualSpeed = bobbingSpeed;
		}
	}
}
