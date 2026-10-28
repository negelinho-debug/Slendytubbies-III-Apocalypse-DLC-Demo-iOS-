using UnityEngine;
using UnityEngine.UI;

public class TranslateCredits : MonoBehaviour
{
	private void Start()
	{
		int num = PlayerPrefs.GetInt("Language");
		TextAsset textAsset = Resources.Load<TextAsset>(string.Empty + num + "/Credits");
		GetComponent<Text>().text = textAsset.text;
	}
}
