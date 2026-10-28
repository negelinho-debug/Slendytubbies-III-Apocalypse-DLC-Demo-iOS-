using System.Collections;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public GUISkin menuSkin;

	public bool showMenu;

	public bool disclaimer;

	public bool notavailable;

	public bool credits;

	public bool settings;

	public bool isfullscreen;

	public string[] disclaimertext;

	public string[] campaigntext;

	public string[] customtext;

	public string[] characterctext;

	public string[] settingstext;

	public string[] creditstext;

	public string[] quittext;

	public string[] continuetext;

	public string[] notavailabletext;

	public string[] thecreditstext;

	public string[] fastest;

	public string[] fast;

	public string[] medium;

	public string[] high;

	public string[] highest;

	public string[] fullscreen;

	public string[] quality;

	public string[] resolution;

	public int languageID;

	private void Awake()
	{
		languageID = PlayerPrefs.GetInt("Language");
		isfullscreen = Screen.fullScreen;
	}

	private void Start()
	{
		StartCoroutine(ShowMenuNow());
		Cursor.visible = true;
		if (GameObject.Find("PersistentEngine") != null)
		{
			Object.Destroy(GameObject.Find("PersistentEngine").gameObject);
		}
	}

	private void Update()
	{
	}

	private IEnumerator ShowMenuNow()
	{
		yield return new WaitForSeconds(3f);
		if (PlayerPrefs.GetInt("FirstTime") == 0)
		{
			disclaimer = true;
		}
		else
		{
			showMenu = true;
		}
		PlayerPrefs.SetInt("FirstTime", 2);
	}

	private IEnumerator NewGame()
	{
		showMenu = false;
		FadeScreen fs = GameObject.Find("_Fade").GetComponent<FadeScreen>();
		fs.FadeIn();
		yield return new WaitForSeconds(5f);
		Application.LoadLevel(2);
	}

	private void OnGUI()
	{
		GUI.skin = menuSkin;
		Rect clientRect = new Rect(20f, Screen.height - 400, 250f, 350f);
		Rect clientRect2 = new Rect(Screen.width / 2 - 270, Screen.height / 2 - 190, 540f, 380f);
		Rect clientRect3 = new Rect(Screen.width / 2 - 270, Screen.height / 2 - 100, 540f, 200f);
		Rect clientRect4 = new Rect(Screen.width / 2 - 270, Screen.height / 2 - 190, 540f, 380f);
		Rect clientRect5 = new Rect(Screen.width / 2 - 270, Screen.height / 2 - 190, 540f, 380f);
		if (showMenu)
		{
			clientRect = GUI.Window(0, clientRect, MainWindow, "Main Menu");
		}
		if (disclaimer)
		{
			clientRect2 = GUI.Window(1, clientRect2, DisclaimerWindow, "Notice & Disclaimer");
		}
		if (notavailable)
		{
			clientRect3 = GUI.Window(2, clientRect3, NotAvailableWindow, "Not available");
		}
		if (credits)
		{
			clientRect4 = GUI.Window(5, clientRect4, CreditsWindow, "Credits");
		}
		if (settings)
		{
			clientRect5 = GUI.Window(6, clientRect5, SettingsWindow, "Settings");
		}
	}

	private void SettingsWindow(int windowID)
	{
		GUI.Label(new Rect(50f, 50f, 150f, 30f), resolution[languageID]);
		if (GUI.Button(new Rect(10f, 80f, 150f, 30f), "640x480"))
		{
			Screen.SetResolution(640, 480, isfullscreen);
		}
		if (GUI.Button(new Rect(10f, 110f, 150f, 30f), "800x600"))
		{
			Screen.SetResolution(800, 600, isfullscreen);
		}
		if (GUI.Button(new Rect(10f, 140f, 150f, 30f), "1024x768"))
		{
			Screen.SetResolution(1024, 768, isfullscreen);
		}
		if (GUI.Button(new Rect(10f, 170f, 150f, 30f), "1280x720"))
		{
			Screen.SetResolution(1280, 720, isfullscreen);
		}
		if (GUI.Button(new Rect(10f, 200f, 150f, 30f), "1360x768"))
		{
			Screen.SetResolution(1360, 768, isfullscreen);
		}
		if (GUI.Button(new Rect(10f, 230f, 150f, 30f), "Default"))
		{
			Screen.SetResolution(Screen.width, Screen.height, isfullscreen);
		}
		if (GUI.Button(new Rect(380f, 280f, 150f, 30f), fullscreen[languageID]))
		{
			isfullscreen = !isfullscreen;
			Screen.fullScreen = isfullscreen;
		}
		GUI.Label(new Rect(420f, 50f, 150f, 30f), quality[languageID]);
		if (GUI.Button(new Rect(380f, 80f, 150f, 30f), fastest[languageID]))
		{
			QualitySettings.SetQualityLevel(0);
		}
		if (GUI.Button(new Rect(380f, 110f, 150f, 30f), fast[languageID]))
		{
			QualitySettings.SetQualityLevel(1);
		}
		if (GUI.Button(new Rect(380f, 140f, 150f, 30f), medium[languageID]))
		{
			QualitySettings.SetQualityLevel(2);
		}
		if (GUI.Button(new Rect(380f, 170f, 150f, 30f), high[languageID]))
		{
			QualitySettings.SetQualityLevel(3);
		}
		if (GUI.Button(new Rect(380f, 200f, 150f, 30f), highest[languageID]))
		{
			QualitySettings.SetQualityLevel(4);
		}
		if (GUI.Button(new Rect(380f, 330f, 150f, 40f), continuetext[languageID]))
		{
			settings = false;
			showMenu = true;
		}
	}

	private void DisclaimerWindow(int windowID)
	{
		GUI.Label(new Rect(10f, 50f, 520f, 300f), disclaimertext[languageID]);
		if (GUI.Button(new Rect(380f, 330f, 150f, 40f), continuetext[languageID]))
		{
			disclaimer = false;
			showMenu = true;
		}
	}

	private void CreditsWindow(int windowID)
	{
		GUI.Label(new Rect(10f, 50f, 520f, 300f), thecreditstext[languageID]);
		if (GUI.Button(new Rect(380f, 330f, 150f, 40f), continuetext[languageID]))
		{
			credits = false;
			showMenu = true;
		}
	}

	private void NotAvailableWindow(int windowID)
	{
		GUI.Label(new Rect(10f, 50f, 520f, 100f), notavailabletext[languageID]);
		if (GUI.Button(new Rect(380f, 150f, 150f, 40f), continuetext[languageID]))
		{
			notavailable = false;
			showMenu = true;
		}
	}

	private void MainWindow(int windowID)
	{
		if (GUI.Button(new Rect(10f, 40f, 230f, 40f), campaigntext[languageID]))
		{
			StartCoroutine(NewGame());
		}
		if (GUI.Button(new Rect(10f, 90f, 230f, 40f), "<color=grey>" + customtext[languageID] + "</color>"))
		{
			showMenu = false;
			notavailable = true;
		}
		if (GUI.Button(new Rect(10f, 140f, 230f, 40f), "<color=grey>" + characterctext[languageID] + "</color>"))
		{
			showMenu = false;
			notavailable = true;
		}
		if (GUI.Button(new Rect(10f, 200f, 230f, 40f), settingstext[languageID]))
		{
			showMenu = false;
			settings = true;
		}
		if (GUI.Button(new Rect(10f, 250f, 230f, 40f), creditstext[languageID]))
		{
			showMenu = false;
			credits = true;
		}
		if (GUI.Button(new Rect(10f, 300f, 230f, 40f), quittext[languageID]))
		{
			Application.Quit();
		}
	}
}
