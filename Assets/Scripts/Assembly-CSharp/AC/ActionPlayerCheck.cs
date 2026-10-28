using System;

namespace AC
{
	[Serializable]
	public class ActionPlayerCheck : ActionCheck
	{
		public int playerID;

		public int playerNumber;

		private SettingsManager settingsManager;

		public ActionPlayerCheck()
		{
			isDisplayed = true;
			title = "Player: Check";
		}

		public override bool CheckCondition()
		{
			if ((bool)KickStarter.player && KickStarter.player.ID == playerID)
			{
				return true;
			}
			return false;
		}
	}
}
