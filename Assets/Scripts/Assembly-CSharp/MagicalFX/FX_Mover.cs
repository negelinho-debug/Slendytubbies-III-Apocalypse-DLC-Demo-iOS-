using UnityEngine;

namespace MagicalFX
{
	[RequireComponent(typeof(Rigidbody))]
	public class FX_Mover : MonoBehaviour
	{
		public float Speed = 1f;

		public Vector3 Noise = Vector3.zero;

		public float Damping = 0.3f;

		private Quaternion direction;

		private void Start()
		{
			direction = Quaternion.LookRotation(transform.forward * 1000f);
			transform.Rotate(new Vector3(Random.Range(0f - Noise.x, Noise.x), Random.Range(0f - Noise.y, Noise.y), Random.Range(0f - Noise.z, Noise.z)));
		}

		private void LateUpdate()
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, direction, Damping);
			transform.position += transform.forward * Speed * Time.deltaTime;
		}
	}
}
