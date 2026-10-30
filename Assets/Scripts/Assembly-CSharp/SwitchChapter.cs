using System;
using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using TranslatorSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchChapter : MonoBehaviour
{
	[Serializable]
	public class chapterInfo
	{
		public string sceneName;

		public Sprite chapterImage;

		public bool skip;

		public bool isEnding;

		public string[] title;

		public string[] description;

		public string unlockHat;

		public int lineID;

		public int lineDescriptionID;
	}

	public string chapterName;

	public int language;

	public Text title;

	public Text description;

	public Text End;

	public Image background;

	public Text anyKeyText;

	public GameObject autosavingButton;

	public List<chapterInfo> chapters;

	public FadeScreen fadeScreen;

	private bool isEnding;

	private bool canContinue;

	private bool isContinue;

	private void Awake()
	{
		if ((bool)GameObject.Find("You"))
		{
			UnityEngine.Object.Destroy(GameObject.Find("You"));
		}
		if ((bool)GameObject.Find("PersistentEngine"))
		{
			UnityEngine.Object.Destroy(GameObject.Find("PersistentEngine"));
		}
		chapterName = PlayerPrefs.GetString("Chapter");
		for (int i = 0; i < chapters.Count; i++)
		{
			if (!(chapters[i].sceneName == chapterName))
			{
				continue;
			}
			if (chapters[i].skip)
			{
				canContinue = true;
				isContinue = true;
				StartCoroutine(fadeToNewScene());
				continue;
			}
			isEnding = chapters[i].isEnding;
			if (!isEnding)
			{
				if (PlayerPrefs.GetInt("Language") > 0)
				{
					Translator translator = new Translator();
					chapters[i].title[0] = chapters[i].title[0];
					chapters[i].description[0] = chapters[i].description[0];
				}
				title.text = chapters[i].title[language];
				description.text = chapters[i].description[language];
				background.sprite = chapters[i].chapterImage;
			}
			else
			{
				End.text = chapters[i].title[language];
				autosavingButton.SetActive(false);
			}
			fadeScreen.fadeout = true;
			if (chapters[i].unlockHat != string.Empty)
			{
				PlayerPrefs.SetString(chapters[i].unlockHat, "1");
			}
		}
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(7f);
		if (!isEnding)
		{
			anyKeyText.enabled = true;
		}
		canContinue = true;
	}

	private void Update()
	{
		if (canContinue && !isContinue)
		{
			if (Input.anyKeyDown)
			{
				StartCoroutine(fadeToNewScene());
				isContinue = true;
			}
			if (isEnding)
			{
				StartCoroutine(fadeToNewScene());
			}
		}
	}

	private IEnumerator fadeToNewScene()
	{
		if (!isEnding)
		{
			anyKeyText.enabled = false;
		}
		fadeScreen.fadein = true;
		yield return new WaitForSeconds(5f);
		if (!isEnding)
		{
			SceneManager.LoadScene(chapterName);
			yield break;
		}
		PlayerPrefs.SetString("Chapter", "Chapter 3_1");
		SceneManager.LoadScene("Credits");
	}
}
