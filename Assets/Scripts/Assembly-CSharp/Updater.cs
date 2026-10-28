using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Updater : MonoBehaviour
{
	public string currentVersion = "1.0";

	public int versionID;

	public string[] versions;

	public bool isUpdated;

	public bool checking;

	public Text versionText;

	public GameObject updateNowObj;

	public void Start()
	{
		checking = true;
		StartCoroutine(CheckForUpdate());
		versionText.text = string.Empty + currentVersion;
	}

	private IEnumerator CheckForUpdate()
	{
		checking = true;
		WWW www = new WWW("https://www.dropbox.com/s/s72tx7oitzu0ikq/s3ch4?dl=1");
		yield return www;
		versions = www.text.Split(":"[0]);
		if (versions.Length > versionID)
		{
			if (versions[versionID] == currentVersion)
			{
				isUpdated = true;
			}
			else
			{
				updateNowObj.SetActive(true);
			}
		}
		checking = false;
	}

	public void UpdateNow()
	{
		Application.OpenURL("http://zeoworks.com/games/Slendytubbies3C4.html");
	}
}
