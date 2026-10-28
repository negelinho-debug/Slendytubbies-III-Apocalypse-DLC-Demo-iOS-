using UnityEngine;

public class ChooseLanguage : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SelectLanguage(int id)
	{
		PlayerPrefs.SetInt("Language", id);
	}
}
