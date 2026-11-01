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
        if (MenuItems == null ||
            languageButton == null ||
            languages == null ||
            languages.Count == 0)
        {
            Debug.LogWarning(
                "[FIX10] Original menu references missing");
            return;
        }
        if (!PlayerPrefs.HasKey("Language"))
            PlayerPrefs.SetInt("Language", 0);
        int id = Mathf.Clamp(
            PlayerPrefs.GetInt("Language"),
            0, languages.Count - 1);
        PlayerPrefs.SetInt("Language", id);
        languageButton.sprite = languages[id].icon;
    }


    private void Start()
    {
        if (continueButton != null &&
            !string.IsNullOrEmpty(
                PlayerPrefs.GetString("Chapter")))
            continueButton.SetActive(true);
        if (antiCheat != null &&
            GameObject.FindWithTag("AntiCheat") == null)
            UnityEngine.Object.DontDestroyOnLoad(
                UnityEngine.Object.Instantiate(
                    antiCheat, transform.position,
                    transform.rotation).gameObject);
    }


    private void Update()
    {
        if (fadeMusic && music != null)
            music.volume -= Time.deltaTime / 4f;
        if (zeoworksMessage != null &&
            zeoworksMessage.activeSelf)
        {
            timer += Time.deltaTime;
            if (Input.anyKeyDown || timer > 10f)
            {
                zeoworksMessage.SetActive(false);
                if (MenuItems != null)
                    MenuItems.SetActive(true);
                firstTime = false;
                Splashes splash = GetComponent<Splashes>();
                if (splash != null &&
                    splash.splashesContent != null)
                    splash.enabled = true;
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
    #if UNITY_IOS && !UNITY_EDITOR
        if (MenuItems == null ||
            languageButton == null ||
            languages == null ||
            languages.Count == 0)
        {
            GUI.skin = null;
            GUI.color = Color.white;
            float w = Screen.width, h = Screen.height;
            GUI.Box(
                new Rect(w*.2f,h*.12f,w*.6f,h*.76f),
                "SLENDYTUBBIES III : APOCALYPSE\nFIX 10 - ORIGINAL SCENE RECOVERY");

            if (GUI.Button(
                new Rect(w*.27f,h*.37f,w*.46f,h*.15f),
                "PLAY ORIGINAL DEMOLEVEL"))
            {
                PlayerPrefs.SetString(
                    "Chapter", startingChapter);
                PlayerPrefs.Save();
                SceneManager.LoadScene(startingChapter);
            }
            if (GUI.Button(
                new Rect(w*.27f,h*.57f,w*.46f,h*.15f),
                "TRY ORIGINAL LOADING"))
            {
                PlayerPrefs.SetString(
                    "Chapter", startingChapter);
                PlayerPrefs.Save();
                SceneManager.LoadScene("Loading");
            }
            return;
        }
    #endif
        if (!showLanguageOptions) return;
        GUI.skin = guiSkin;
        int num = Screen.height / 15;
        for (int i = 0; i < languages.Count; i++)
        {
            if (GUI.Button(
                new Rect(0f,
                    Screen.height - num * languages.Count
                    + i * num - num,
                    Screen.width, num),
                languages[i].name))
            {
                showLanguageOptions = false;
                PlayerPrefs.SetInt("Language", i);
                if (firstTime &&
                    zeoworksMessage != null)
                    zeoworksMessage.SetActive(true);
                else if (MenuItems != null)
                    MenuItems.SetActive(true);
                if (languageButton != null)
                    languageButton.sprite =
                        languages[i].icon;
            }
        }
    }
}
