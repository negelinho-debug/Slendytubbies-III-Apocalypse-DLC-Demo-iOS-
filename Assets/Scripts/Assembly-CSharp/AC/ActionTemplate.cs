using System;

namespace AC
{
	[Serializable]
	public class ActionTemplate : Action
	{
		public ActionTemplate()
		{
			isDisplayed = true;
			title = "Action: Template";
		}

		public override float Run()
		{
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
