using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionQuickTime : Action
	{
		public GameObject quickTimeObj;

		public Animation animObj;

		public string clipName;

		public float startAtFrame;

		public float endAtFrame;

		public float totalFrames;

		public ResultAction resultActionTrue;

		public ResultAction resultActionTrue2;

		public Cutscene cutScene;

		public Cutscene cutScene2;

		private bool isActive;

		private bool result;

		public ActionQuickTime()
		{
			isDisplayed = true;
			title = "Custom: Quicktime";
		}

		public override float Run()
		{
			float num = animObj[clipName].time / animObj[clipName].length;
			float num2 = num * totalFrames;
			if (num2 >= startAtFrame && !isActive)
			{
				quickTimeObj.SetActive(true);
				isActive = true;
			}
			if (num2 >= endAtFrame)
			{
				if (PlayerPrefs.GetString(quickTimeObj.name) == "1")
				{
					result = true;
					endAction = resultActionTrue;
					if (resultActionTrue == ResultAction.RunCutscene)
					{
						linkedCutscene = cutScene;
					}
				}
				else
				{
					result = false;
					endAction = resultActionTrue2;
					if (resultActionTrue2 == ResultAction.RunCutscene)
					{
						linkedCutscene = cutScene2;
					}
				}
				quickTimeObj.SetActive(false);
				isActive = false;
			}
			if (!isRunning)
			{
				isRunning = true;
				return defaultPauseTime;
			}
			if (num2 >= endAtFrame)
			{
				isRunning = false;
				isActive = false;
				num2 = 0f;
				num = 0f;
				return 0f;
			}
			return defaultPauseTime;
		}
	}
}
