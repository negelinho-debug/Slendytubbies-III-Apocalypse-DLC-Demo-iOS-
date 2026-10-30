using TranslatorSystem;
using UnityEngine;
using UnityEngine.UI;

public class TranslateText : MonoBehaviour
{
	public string fileName = "Campaign";

	public int ID = 1;

	private string orgText;

	private void Awake()
	{
		orgText = GetComponent<Text>().text;
	}

	private void OnEnable()
	{
		Translator translator = new Translator();
		if (PlayerPrefs.GetInt("Language") > 0)
		{
			GetComponent<Text>().text = orgText;
		}
		else if (PlayerPrefs.GetInt("Language") == 0)
		{
			GetComponent<Text>().text = orgText;
		}
	}
}
