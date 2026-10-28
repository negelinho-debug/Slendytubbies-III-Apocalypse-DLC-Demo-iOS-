using System.Collections.Generic;
using UnityEngine;

public class automaticDoors : MonoBehaviour
{
	public float openSpeed = 5f;

	public List<Light> lights = new List<Light>();

	public Color openedColor = new Color(1f, 1f, 1f, 1f);

	public Color closedColor = new Color(0f, 0f, 0f, 0f);

	public Transform doorsWing;

	public Vector3 openedPosition = new Vector3(0f, 0f, 0f);

	private Vector3 closedPosition = new Vector3(0f, 0f, 0f);

	private int state;

	private float lerper;

	public bool openable = true;

	public AudioClip openSFX;

	public AudioClip closeSFX;

	private void Start()
	{
		closedPosition = doorsWing.transform.localPosition;
		if (openable)
		{
			foreach (Light light in lights)
			{
				light.color = openedColor;
			}
			return;
		}
		if (openable)
		{
			return;
		}
		foreach (Light light2 in lights)
		{
			light2.color = closedColor;
		}
	}

	private void Update()
	{
		if (state == 0)
		{
			return;
		}
		if (state == 1)
		{
			if (lerper < 1f)
			{
				lerper += Time.deltaTime * openSpeed;
				doorsWing.localPosition = Vector3.Lerp(closedPosition, openedPosition, lerper);
			}
			else
			{
				state = 2;
			}
		}
		else if (state != 2 && state == 3)
		{
			if (lerper > 0f)
			{
				lerper -= Time.deltaTime * openSpeed;
				doorsWing.localPosition = Vector3.Lerp(closedPosition, openedPosition, lerper);
			}
			else
			{
				state = 0;
			}
		}
	}

	public void Open()
	{
		if (openable)
		{
			state = 1;
			if ((bool)openSFX)
			{
				GetComponent<AudioSource>().PlayOneShot(openSFX);
			}
		}
	}

	public void Close()
	{
		if (openable)
		{
			state = 3;
			if ((bool)closeSFX)
			{
				GetComponent<AudioSource>().PlayOneShot(closeSFX);
			}
		}
	}

	public void setOpenable(bool o)
	{
		openable = o;
		if (openable)
		{
			foreach (Light light in lights)
			{
				light.color = openedColor;
			}
			return;
		}
		if (openable)
		{
			return;
		}
		foreach (Light light2 in lights)
		{
			light2.color = closedColor;
		}
	}
}
