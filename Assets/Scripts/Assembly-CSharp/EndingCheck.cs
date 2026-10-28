using UnityEngine;

public class EndingCheck : MonoBehaviour
{
	public string changeChapter = "Ending 1";

	public bool changeNow;

	private void Start()
	{
	}

	private void Update()
	{
		if (changeNow)
		{
			PlayerPrefs.SetString("Chapter", changeChapter);
			changeNow = false;
		}
	}

	private void OnGUI()
	{
		GUILayout.Label("Current Chapter: " + PlayerPrefs.GetString("Chapter"));
		GUILayout.Label("Current Chapter: " + PlayerPrefs.GetString("CH3_ENDING"));
		changeChapter = GUILayout.TextField(changeChapter);
		if (GUILayout.Button("Change Now"))
		{
			changeNow = true;
		}
	}
}
