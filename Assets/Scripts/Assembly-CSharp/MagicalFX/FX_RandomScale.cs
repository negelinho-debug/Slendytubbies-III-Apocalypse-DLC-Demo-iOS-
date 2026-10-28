using UnityEngine;

namespace MagicalFX
{
	public class FX_RandomScale : MonoBehaviour
	{
		public float ScaleMin;

		public float ScaleMax = 1f;

		private void Start()
		{
			transform.localScale *= Random.Range(ScaleMin, ScaleMax);
		}

		private void Update()
		{
		}
	}
}
