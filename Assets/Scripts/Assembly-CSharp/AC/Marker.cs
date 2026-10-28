using UnityEngine;

namespace AC
{
	public class Marker : MonoBehaviour
	{
		protected void Awake()
		{
			if ((bool)GetComponent<Renderer>())
			{
				GetComponent<Renderer>().enabled = false;
			}
			if ((bool)AdvGame.GetReferences().settingsManager && AdvGame.GetReferences().settingsManager.IsUnity2D())
			{
				transform.RotateAround(transform.position, Vector3.right, 90f);
				transform.RotateAround(transform.position, transform.right, -90f);
			}
		}
	}
}
