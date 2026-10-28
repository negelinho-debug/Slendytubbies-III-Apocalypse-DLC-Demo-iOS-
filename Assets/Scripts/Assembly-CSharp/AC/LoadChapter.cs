using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AC
{
	[Serializable]
	public class LoadChapter : Action
	{
		public string scene;

		public LoadChapter()
		{
			isDisplayed = true;
			title = "Custom: Load Chapter";
		}

		public override float Run()
		{
			PlayerPrefs.SetString("Chapter", scene);
			UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
			if (!isRunning)
			{
				isRunning = true;
				return defaultPauseTime;
			}
			isRunning = false;
			return 0f;
		}
	}
}
