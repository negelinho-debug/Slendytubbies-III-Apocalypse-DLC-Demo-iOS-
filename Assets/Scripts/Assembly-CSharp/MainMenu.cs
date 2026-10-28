using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[Serializable]
	public class language
	{
		public string name;

		public Sprite icon;
	}

	public FadeScreen fadeScreen;

	public AudioSource music;

	public GameObject MenuItems;

	public GameObject continueButton;

	public Image languageButton;

	public GameObject zeoworksMessage;

	public List<language> languages;

	public GUISkin guiSkin;

	public Transform antiCheat;

	public string startingChapter = "DEMOLEVEL";

	private bool firstTime;

	private bool fadeMusic;

	private float timer;

	private bool showLanguageOptions;

	private void Awake()
	{
		Time.timeScale = 1f;
		if (!PlayerPrefs.HasKey("Language"))
		{
			LanguageOptions();
			firstTime = true;
			GetComponent<Splashes>().enabled = false;
			return;
		}
		if (PlayerPrefs.GetInt("Language") > 1)
		{
			PlayerPrefs.SetInt("Language", 0);
		}
		languageButton.sprite = languages[PlayerPrefs.GetInt("Language")].icon;
	}

	private void Start()
	{
		if (PlayerPrefs.HasKey("Chapter") || PlayerPrefs.GetString("Chapter") != string.Empty)
		{
			continueButton.SetActive(true);
		}
		if (GameObject.FindWithTag("AntiCheat") == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object.Instantiate(antiCheat, transform.position, transform.rotation).gameObject);
		}
	}

	private void Update()
	{
		if (fadeMusic)
		{
			music.volume -= Time.deltaTime / 4f;
		}
		if (zeoworksMessage.activeSelf)
		{
			timer += 1f * Time.deltaTime;
			if (Input.anyKeyDown || timer > 10f)
			{
				zeoworksMessage.SetActive(false);
				MenuItems.SetActive(true);
				firstTime = false;
				GetComponent<Splashes>().enabled = true;
			}
		}
	}

	public void StartNewGame()
	{
		if (PlayerPrefs.GetString("Chapter").ToLower().Contains("ending"))
		{
			MonoBehaviour.print("Current Ending: " + PlayerPrefs.GetString("Chapter"));
			PlayerPrefs.SetString("CH3_ENDING", PlayerPrefs.GetString("Chapter"));
		}
		PlayerPrefs.SetString("Chapter", startingChapter);
		StartCoroutine(GoToLoading("Loading"));
	}

	public void ContinueGame()
	{
		StartCoroutine(GoToLoading("Loading"));
	}

	public void LoadChapter(string chapter)
	{
		PlayerPrefs.SetString("Chapter", chapter);
		StartCoroutine(GoToLoading("Loading"));
	}

	public void LoadCredits()
	{
		StartCoroutine(GoToLoading("Credits"));
	}

	public void LanguageOptions()
	{
		MenuItems.SetActive(false);
		showLanguageOptions = true;
	}

	public void SetLanguage(int id)
	{
		PlayerPrefs.SetInt("Language", id);
	}

	private IEnumerator GoToLoading(string sceneName)
	{
		MenuItems.SetActive(false);
		fadeMusic = true;
		if (fadeScreen != null)
		{
			fadeScreen.fadein = true;
		}
		yield return new WaitForSeconds(4f);
		SceneManager.LoadScene(sceneName);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	private void OnGUI()
	{
		if (!showLanguageOptions)
		{
			return;
		}
		GUI.skin = guiSkin;
		int num = Screen.height / 15;
		for (int i = 0; i < languages.Count; i++)
		{
			if (GUI.Button(new Rect(0f, Screen.height - num * languages.Count + i * num - num, Screen.width, num), "<size=" + (float)num / 1.5f + ">" + languages[i].name + "</size>"))
			{
				showLanguageOptions = false;
				PlayerPrefs.SetInt("Language", i);
				if (firstTime)
				{
					zeoworksMessage.SetActive(true);
				}
				else
				{
					MenuItems.SetActive(true);
				}
				languageButton.sprite = languages[i].icon;
			}
		}
	}
}
