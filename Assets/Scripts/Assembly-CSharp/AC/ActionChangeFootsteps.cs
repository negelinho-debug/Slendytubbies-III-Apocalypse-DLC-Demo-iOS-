using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionChangeFootsteps : Action
	{
		private Player player1;

		public AudioClip walksound;

		public AudioClip runsound;

		public ActionChangeFootsteps()
		{
			isDisplayed = true;
			title = "Player: Change footsteps";
		}

		public override float Run()
		{
			player1 = KickStarter.player;
			player1.walkSound = walksound;
			player1.runSound = runsound;
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
