using UnityEngine;

public class QuicktimeEvent : MonoBehaviour
{
	public string prefName;

	public string buttonName;

	public bool GreaterThan;

	private float direction = 0.1f;

	public bool hasCompleted;

	private void OnEnable()
	{
		hasCompleted = false;
		GetComponent<GUITexture>().color = Color.white;
		prefName = gameObject.name;
		PlayerPrefs.SetString(prefName, "0");
	}

	private void Update()
	{
		if (GreaterThan)
		{
			if (Input.GetAxis(buttonName) >= 0.01f && !hasCompleted)
			{
				GetComponent<GUITexture>().color = Color.green;
				hasCompleted = true;
				PlayerPrefs.SetString(prefName, "1");
				GetComponent<AudioSource>().Play();
			}
		}
		else if (Input.GetAxis(buttonName) <= -0.01f && !hasCompleted)
		{
			GetComponent<GUITexture>().color = Color.green;
			hasCompleted = true;
			PlayerPrefs.SetString(prefName, "1");
			GetComponent<AudioSource>().Play();
		}
		if (hasCompleted && !GetComponent<AudioSource>().isPlaying)
		{
			gameObject.SetActive(false);
		}
	}
}
