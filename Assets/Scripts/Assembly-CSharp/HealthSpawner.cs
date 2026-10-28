using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
	public float spawnDelay = 10f;

	private float timer;

	public Transform toSpawn;

	public float radius = 20f;

	private Transform spawned;

	private void Update()
	{
		if (timer >= spawnDelay)
		{
			Vector3 position = transform.position + Random.insideUnitSphere * radius;
			position.y = transform.position.y + 0.5f;
			spawned = Object.Instantiate(toSpawn, position, transform.rotation);
			Object.Destroy(spawned.gameObject, spawnDelay);
			timer = 0f;
		}
		else
		{
			timer += 1f * Time.deltaTime;
		}
	}
}
