using TranslatorSystem;
using UnityEngine;

public class GUITextLanguage : MonoBehaviour
{
	private int languageID;

	public int ID;

	public string[] languagetext;

	private void Awake()
	{
		languageID = 0;
		Translator translator = new Translator();
		if (PlayerPrefs.GetInt("Language") > 0)
		{
			languagetext[languageID] = translator.Translate(ID, "Campaign");
		}
	}
}
