using System;
using System.Collections;
using UnityEngine;

public class QuicktimeBob : MonoBehaviour
{
	public Transform cam;

	public float velocity;

	private float timer;

	private float bobbingSpeed = 0.125f;

	private float bobbingAmount = 0.05f;

	private float midpoint = 0.17f;

	private float actualSpeed;

	private Vector3 previous;

	private void Awake()
	{
		actualSpeed = bobbingSpeed;
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		GetComponent<AudioSource>().volume = 1f;
	}

	private void Update()
	{
		velocity = (cam.position - previous).magnitude / Time.deltaTime;
		previous = cam.position;
		float num = 0f;
		num = Mathf.Sin(timer);
		timer += actualSpeed;
		if (timer > (float)Math.PI * 2f)
		{
			timer -= (float)Math.PI * 2f;
		}
		Vector3 localPosition = transform.localPosition;
		if (num != 0f)
		{
			float num2 = num * bobbingAmount;
			float value = Mathf.Abs(velocity) + Mathf.Abs(velocity);
			value = Mathf.Clamp(value, 0f, 1f);
			num2 = value * num2;
			localPosition.y = midpoint + num2;
		}
		else
		{
			localPosition.y = midpoint;
		}
		actualSpeed = bobbingSpeed * (velocity / 3f);
		if (velocity > 2f)
		{
			GetComponent<AudioSource>().pitch = 1f + velocity / 25f;
			if (!GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().Play();
			}
		}
		transform.localPosition = localPosition;
	}
}
