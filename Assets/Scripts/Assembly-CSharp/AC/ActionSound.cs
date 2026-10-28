using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionSound : Action
	{
		public enum SoundAction
		{
			Play,
			FadeIn,
			FadeOut,
			Stop
		}

		public int constantID;

		public int parameterID = -1;

		public Sound soundObject;

		public AudioClip audioClip;

		public float fadeTime;

		public bool loop;

		public SoundAction soundAction;

		public bool affectChildren;

		public ActionSound()
		{
			isDisplayed = true;
			title = "Engine: Play sound";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			soundObject = AssignFile(parameters, parameterID, constantID, soundObject);
		}

		public override float Run()
		{
			if ((bool)soundObject)
			{
				if ((bool)audioClip && (bool)soundObject.GetComponent<AudioSource>() && (soundAction == SoundAction.Play || soundAction == SoundAction.FadeIn))
				{
					soundObject.GetComponent<AudioSource>().clip = audioClip;
				}
				if (soundObject.soundType == SoundType.Music && (soundAction == SoundAction.Play || soundAction == SoundAction.FadeIn))
				{
					Sound[] array = UnityEngine.Object.FindObjectsOfType(typeof(Sound)) as Sound[];
					Sound[] array2 = array;
					foreach (Sound sound in array2)
					{
						if (sound.IsPlayingSameMusic(audioClip))
						{
							return 0f;
						}
						sound.EndOldMusic(soundObject);
					}
				}
				if (soundAction == SoundAction.Play)
				{
					soundObject.Play(loop);
				}
				else if (soundAction == SoundAction.FadeIn)
				{
					if (fadeTime == 0f)
					{
						soundObject.Play(loop);
					}
					else
					{
						soundObject.FadeIn(fadeTime, loop);
					}
				}
				else if (soundAction == SoundAction.FadeOut)
				{
					if (fadeTime == 0f)
					{
						soundObject.Stop();
					}
					else
					{
						soundObject.FadeOut(fadeTime);
					}
				}
				else if (soundAction == SoundAction.Stop)
				{
					soundObject.Stop();
					if (affectChildren)
					{
						foreach (Transform item in soundObject.transform)
						{
							if ((bool)item.GetComponent<Sound>())
							{
								item.GetComponent<Sound>().Stop();
							}
						}
					}
				}
			}
			return 0f;
		}

		public override void Skip()
		{
			if ((bool)soundObject && (loop || soundObject.soundType == SoundType.Music || soundAction == SoundAction.Stop))
			{
				Run();
			}
		}
	}
}
