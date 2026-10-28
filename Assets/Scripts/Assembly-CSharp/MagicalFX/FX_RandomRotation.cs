using UnityEngine;

namespace MagicalFX
{
	public class FX_RandomRotation : MonoBehaviour
	{
		public Vector3 Rotation;

		private void Start()
		{
			transform.Rotate(new Vector3(Random.Range(0f - Rotation.x, Rotation.x), Random.Range(0f - Rotation.y, Rotation.y), Random.Range(0f - Rotation.z, Rotation.z)));
		}
	}
}
