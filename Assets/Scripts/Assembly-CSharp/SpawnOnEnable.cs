using UnityEngine;

public class SpawnOnEnable : MonoBehaviour
{
	public Transform toSpawn;

	private void OnEnable()
	{
		Object.Instantiate(toSpawn, transform.position, transform.rotation);
	}
}
