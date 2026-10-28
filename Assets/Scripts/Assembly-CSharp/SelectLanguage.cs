using System.Collections;
using UnityEngine;

public class SelectLanguage : MonoBehaviour
{
	public GUISkin menuSkin;

	public bool showMenu = true;

	public int doge;

	public string theurl;

	private void Awake()
	{
		doge = PlayerPrefs.GetInt("doge");
	}

	public void Start()
	{
		theurl = "zeogames.net";
	}

	public void SetPageAddress(string theres)
	{
		theurl = theres;
	}

	private IEnumerator Selected()
	{
		showMenu = false;
		FadeScreen fs = GameObject.Find("_Fade").GetComponent<FadeScreen>();
		fs.FadeOut();
		yield return new WaitForSeconds(4f);
		fs.FadeIn();
		yield return new WaitForSeconds(2f);
		Application.LoadLevel(1);
	}

	private void OnGUI()
	{
		GUI.skin = menuSkin;
		Rect clientRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400f, 400f);
		if (showMenu)
		{
			if (theurl.Contains("zeogames.net"))
			{
				clientRect = GUI.Window(0, clientRect, LanguageWindow, "Language");
				return;
			}
			GUI.Label(new Rect(Screen.width / 2 - 140, Screen.height / 2 - 50, 300f, 30f), "Play this game on gameshed.com");
			string text = GUI.TextField(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 20, 600f, 20f), "http://www.gameshed.com/Scary-Games/Slendytubbies-3--Campaign-Demo/play.html");
		}
	}

	private void LanguageWindow(int windowID)
	{
		if (GUI.Button(new Rect(10f, 40f, 380f, 80f), "English"))
		{
			PlayerPrefs.SetInt("Language", 0);
			StartCoroutine(Selected());
		}
		if (GUI.Button(new Rect(10f, 120f, 380f, 80f), "Español (BETA)"))
		{
			PlayerPrefs.SetInt("Language", 1);
			StartCoroutine(Selected());
		}
		if (GUI.Button(new Rect(10f, 200f, 380f, 80f), "Français (BETA)"))
		{
			PlayerPrefs.SetInt("Language", 3);
			StartCoroutine(Selected());
		}
		if (doge == 2 && GUI.Button(new Rect(10f, 280f, 380f, 80f), "Dogé"))
		{
			PlayerPrefs.SetInt("Language", 2);
			StartCoroutine(Selected());
		}
	}
}
