using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class CharacterSpeed : Action
	{
		public NPC npc;

		public float walkSpeed;

		public float runSpeed;

		public float audioPitch;

		public CharacterSpeed()
		{
			isDisplayed = true;
			title = "Custom: NPC Speed";
		}

		public override float Run()
		{
			npc.walkSpeedScale = walkSpeed;
			npc.runSpeedScale = runSpeed;
			if (npc.soundChild != null)
			{
				npc.soundChild.GetComponent<AudioSource>().pitch = audioPitch;
			}
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
