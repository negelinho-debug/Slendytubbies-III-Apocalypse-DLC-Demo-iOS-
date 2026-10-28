using UnityEngine;

namespace MagicalFX
{
	public class FX_HitSpawner : MonoBehaviour
	{
		public GameObject FXSpawn;

		public bool DestoyOnHit;

		public bool FixRotation;

		public float LifeTimeAfterHit = 1f;

		public float LifeTime;

		public string ignoreTag;

		private void Start()
		{
		}

		private void Spawn()
		{
			if (FXSpawn != null)
			{
				Quaternion rotation = transform.rotation;
				if (!FixRotation)
				{
					rotation = FXSpawn.transform.rotation;
				}
				GameObject gameObject = Object.Instantiate(FXSpawn, transform.position, rotation);
				if (LifeTime > 0f)
				{
					Object.Destroy(gameObject.gameObject, LifeTime);
				}
			}
			if (DestoyOnHit)
			{
				Object.Destroy(base.gameObject, LifeTimeAfterHit);
				if ((bool)base.gameObject.GetComponent<Collider>())
				{
					base.gameObject.GetComponent<Collider>().enabled = false;
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.transform.tag != ignoreTag || ignoreTag == string.Empty)
			{
				Spawn();
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.transform.tag != ignoreTag || ignoreTag == string.Empty)
			{
				Spawn();
			}
		}
	}
}
