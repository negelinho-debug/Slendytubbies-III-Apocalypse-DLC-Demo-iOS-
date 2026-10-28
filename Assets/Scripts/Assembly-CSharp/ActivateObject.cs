using UnityEngine;

public class ActivateObject : MonoBehaviour
{
	public string parent1;

	public string toactivate;

	private void Start()
	{
		Transform transform = GameObject.Find(parent1).transform;
		transform.Find(toactivate).gameObject.SetActive(true);
		Object.Destroy(gameObject);
	}
}
