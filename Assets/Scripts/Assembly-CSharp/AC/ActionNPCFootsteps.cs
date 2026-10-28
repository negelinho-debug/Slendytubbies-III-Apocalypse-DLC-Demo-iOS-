using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionNPCFootsteps : Action
	{
		public NPC npc;

		public AudioClip walksound;

		public AudioClip runsound;

		public ActionNPCFootsteps()
		{
			isDisplayed = true;
			title = "Custom: NPC footsteps";
		}

		public override float Run()
		{
			npc.walkSound = walksound;
			npc.runSound = runsound;
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
