using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionCheckFrame : Action
	{
		public Transform myActivate;

		public Animation animObj;

		public string clipName;

		public float frameCheck;

		public float totalFrames;

		public ActionCheckFrame()
		{
			isDisplayed = true;
			title = "Object: Wait Until Frame";
		}

		public override float Run()
		{
			float num = animObj[clipName].time / animObj[clipName].length;
			float num2 = num * totalFrames;
			if (!isRunning)
			{
				isRunning = true;
				return defaultPauseTime;
			}
			if (num2 >= frameCheck)
			{
				isRunning = false;
				return 0f;
			}
			return defaultPauseTime;
		}
	}
}
