using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
	public enum lightType
	{
		flicker,
		pulsate
	}

	public lightType type;

	private Light light;

	public float speed;

	public float noise;

	private void Awake()
	{
		light = GetComponent<Light>();
	}

	private void Start()
	{
		light.enabled = false;
		if (type == lightType.flicker)
		{
			StartCoroutine(Flicker());
		}
		else if (type != lightType.pulsate)
		{
		}
	}

	private void Update()
	{
	}

	private IEnumerator Flicker()
	{
		light.enabled = true;
		float randNoise = (float)Random.Range(-1, 1) * Random.Range(0f - noise, noise);
		yield return new WaitForSeconds(speed + randNoise);
		light.enabled = false;
		yield return new WaitForSeconds(speed);
		StartCoroutine(Flicker());
	}
}
