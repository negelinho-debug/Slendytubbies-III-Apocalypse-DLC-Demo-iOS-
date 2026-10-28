using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightSwitch : MonoBehaviour
{
	public bool enableOnStart;

	private void Awake()
	{
		Switch(enableOnStart);
	}

	public void TurnOn()
	{
		Switch(true);
	}

	public void TurnOff()
	{
		Switch(false);
	}

	private void Switch(bool turnOn)
	{
		GetComponent<Light>().enabled = turnOn;
	}
}
