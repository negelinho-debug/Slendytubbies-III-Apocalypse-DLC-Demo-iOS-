using UnityEngine;

namespace MagicalFX
{
	public class FX_Spawner : MonoBehaviour
	{
		public bool FixRotation;

		public bool Normal;

		public GameObject FXSpawn;

		public float LifeTime;

		public float TimeSpawn;

		private float timeTemp;

		private void Start()
		{
			timeTemp = Time.time;
			if (FXSpawn != null && TimeSpawn <= 0f)
			{
				Quaternion rotation = transform.rotation;
				if (!FixRotation)
				{
					rotation = FXSpawn.transform.rotation;
				}
				GameObject gameObject = Object.Instantiate(FXSpawn, transform.position, rotation);
				if (Normal)
				{
					gameObject.transform.forward = transform.forward;
				}
				if (LifeTime > 0f)
				{
					Object.Destroy(gameObject.gameObject, LifeTime);
				}
			}
		}

		private void Update()
		{
			if (!(TimeSpawn > 0f) || !(Time.time > timeTemp + TimeSpawn))
			{
				return;
			}
			Debug.Log("spawn");
			if (FXSpawn != null)
			{
				Quaternion rotation = transform.rotation;
				if (!FixRotation)
				{
					rotation = FXSpawn.transform.rotation;
				}
				GameObject gameObject = Object.Instantiate(FXSpawn, transform.position, rotation);
				if (Normal)
				{
					gameObject.transform.forward = transform.forward;
				}
				if (LifeTime > 0f)
				{
					Object.Destroy(gameObject.gameObject, LifeTime);
				}
			}
			timeTemp = Time.time;
		}
	}
}
