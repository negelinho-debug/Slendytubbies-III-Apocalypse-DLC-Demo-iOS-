using UnityEngine;

public class toggleObject : MonoBehaviour
{
	public GameObject toToggle;

	public float delay = 1f;

	private float timer;

	private void Update()
	{
		timer += 1f * Time.deltaTime;
		if (timer >= delay)
		{
			toToggle.SetActive(!toToggle.activeSelf);
			timer = 0f;
		}
	}
}
