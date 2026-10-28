using UnityEngine;

public class ParticleToggle : MonoBehaviour
{
	public EllipsoidParticleEmitter[] epeList;

	private void OnEnable()
	{
		EllipsoidParticleEmitter[] array = epeList;
		foreach (EllipsoidParticleEmitter ellipsoidParticleEmitter in array)
		{
			ellipsoidParticleEmitter.emit = true;
		}
	}

	private void OnDisable()
	{
		EllipsoidParticleEmitter[] array = epeList;
		foreach (EllipsoidParticleEmitter ellipsoidParticleEmitter in array)
		{
			ellipsoidParticleEmitter.emit = false;
		}
	}
}
