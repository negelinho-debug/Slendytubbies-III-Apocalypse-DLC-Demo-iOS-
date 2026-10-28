using UnityEngine;

namespace MagicalFX
{
	public class FX_LifeTime : MonoBehaviour
	{
		public float LifeTime = 3f;

		public GameObject SpawnAfterDead;

		private float timeTemp;

		private void Start()
		{
			if (SpawnAfterDead == null)
			{
				Object.Destroy(gameObject, LifeTime);
			}
			else
			{
				timeTemp = Time.time;
			}
		}

		private void Update()
		{
			if (SpawnAfterDead != null && Time.time > timeTemp + LifeTime)
			{
				Object.Destroy(gameObject);
				Object.Instantiate(SpawnAfterDead, transform.position, SpawnAfterDead.transform.rotation);
			}
		}
	}
}
