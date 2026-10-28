using UnityEngine;

namespace MagicalFX
{
	public class FX_SpawnDirection : MonoBehaviour
	{
		public int Number = 10;

		public float Frequency = 1f;

		public bool FixRotation;

		public bool Normal;

		public GameObject FXSpawn;

		public float LifeTime;

		public float TimeSpawn;

		private float timeTemp;

		public bool UseObjectForward = true;

		public Vector3 Direction = Vector3.forward;

		private Vector3 corDirection;

		private int counter;

		private void Start()
		{
			corDirection = Direction;
			counter = 0;
			timeTemp = Time.time;
			if (!(TimeSpawn <= 0f))
			{
				return;
			}
			for (int i = 0; i < Number - 1; i++)
			{
				if (UseObjectForward)
				{
					corDirection = Direction + transform.forward;
				}
				Spawn(transform.position + corDirection * Frequency * i);
			}
			Object.Destroy(gameObject);
		}

		private void Update()
		{
			corDirection = Direction;
			if (counter >= Number - 1)
			{
				Object.Destroy(gameObject);
			}
			if (TimeSpawn > 0f && Time.time > timeTemp + TimeSpawn)
			{
				if (UseObjectForward)
				{
					corDirection = Direction + transform.forward;
				}
				Spawn(transform.position + corDirection * Frequency * counter);
				counter++;
				timeTemp = Time.time;
			}
		}

		private void Spawn(Vector3 position)
		{
			if (FXSpawn != null)
			{
				Quaternion rotation = transform.rotation;
				if (!FixRotation)
				{
					rotation = FXSpawn.transform.rotation;
				}
				GameObject gameObject = Object.Instantiate(FXSpawn, position, rotation);
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
	}
}
