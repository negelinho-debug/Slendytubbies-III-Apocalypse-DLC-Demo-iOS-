using UnityEngine;

public class ParticleSwitch : MonoBehaviour
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

	public void Interact()
	{
		if ((bool)GetComponent<ParticleSystem>())
		{
			GetComponent<ParticleSystem>().Emit(GetComponent<ParticleSystem>().maxParticles);
		}
	}

	private void Switch(bool turnOn)
	{
		if ((bool)GetComponent<ParticleSystem>())
		{
			if (turnOn)
			{
				GetComponent<ParticleSystem>().Play();
			}
			else
			{
				GetComponent<ParticleSystem>().Stop();
			}
		}
	}
}
