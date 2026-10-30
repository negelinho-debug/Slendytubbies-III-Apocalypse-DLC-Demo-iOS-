using UnityEngine;

public class ParticleToggle : MonoBehaviour
{
	public ParticleSystem[] epeList;

	private void OnEnable()
	{
		ParticleSystem[] array = epeList;
		foreach (ParticleSystem ellipsoidParticleEmitter in array)
		{
			ellipsoidParticleEmitter.Play();
		}
	}

	private void OnDisable()
	{
		ParticleSystem[] array = epeList;
		foreach (ParticleSystem ellipsoidParticleEmitter in array)
		{
			ellipsoidParticleEmitter.Stop();
		}
	}
}
