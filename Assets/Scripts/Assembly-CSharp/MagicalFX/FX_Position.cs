using UnityEngine;

namespace MagicalFX
{
	public class FX_Position : MonoBehaviour
	{
		public Vector3 Offset = new Vector3(0f, 0.001f, 0f);

		public bool Normal;

		public SpawnMode Mode;

		private void Start()
		{
		}

		private void OnEnable()
		{
			if (Normal)
			{
				PlaceNormal(transform.position);
			}
		}

		public void PlaceNormal(Vector3 position)
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(position, -Vector3.up * 100f, out hitInfo))
			{
				transform.position = hitInfo.point + Offset;
				transform.forward = hitInfo.normal;
			}
			else
			{
				transform.position = position + Offset;
			}
		}
	}
}
