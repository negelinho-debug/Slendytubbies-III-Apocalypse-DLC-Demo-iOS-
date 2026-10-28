using UnityEngine;

public class ExplodeOnEnable : MonoBehaviour
{
	private void Start()
	{
		GetComponent<MeshExploder>().Explode();
		Object.Destroy(gameObject);
	}
}
