using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class PlayerSpeed : Action
	{
		public float walkSpeed;

		public float runSpeed;

		public float audioPitch;

		public float animationSpeed = 1f;

		public PlayerSpeed()
		{
			isDisplayed = true;
			title = "Custom: Movement Speed";
		}

		public override float Run()
		{
			GameObject gameObject = GameObject.FindWithTag("Player");
			gameObject.GetComponent<Player>().walkSpeedScale = walkSpeed;
			gameObject.GetComponent<Player>().runSpeedScale = runSpeed;
			gameObject.GetComponent<Player>().soundChild.GetComponent<AudioSource>().pitch = audioPitch;
			gameObject.GetComponent<Player>().animator.SetFloat("AnimSpeed", animationSpeed);
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
