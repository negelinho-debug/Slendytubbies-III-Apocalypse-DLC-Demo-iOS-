using UnityEngine;

namespace MagicalFX
{
	public class FX_MoverRandom : MonoBehaviour
	{
		public float Speed = 1f;

		public Vector3 Noise = Vector3.zero;

		private void Start()
		{
		}

		private void FixedUpdate()
		{
			transform.position += transform.forward * Speed * Time.fixedDeltaTime;
			transform.position += new Vector3(Random.Range(0f - Noise.x, Noise.x), Random.Range(0f - Noise.y, Noise.y), Random.Range(0f - Noise.z, Noise.z)) * Time.fixedDeltaTime;
		}
	}
}
