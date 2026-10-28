using UnityEngine;

public class Invisible : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Renderer>().enabled = false;
	}
}
