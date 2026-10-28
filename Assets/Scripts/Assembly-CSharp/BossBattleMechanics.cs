using System;
using System.Collections.Generic;
using AC;
using UnityEngine;

public class BossBattleMechanics : MonoBehaviour
{
	[Serializable]
	public class stage
	{
		public int minHealthFor = 150;

		public Cutscene cutscene;

		public Transform toDisable;

		public Transform toEnable;

		public AudioClip newAudio;

		public float skipAudioToTime = 106f;

		public bool waitUntilAfterCutscene;

		public bool hasChanged;

		public float audioFade = 2f;

		public BattleAudioTweak tweakToDisable;
	}

	public BattleHealth bossHealth;

	public BattleHealth playerHealth;

	public AudioSource music1;

	public AudioSource music2;

	public float maxVolume = 0.7f;

	public List<stage> stages;

	public Cutscene playerDeath;

	public Cutscene bossDeath;

	private bool changeMusic;

	private bool playerIsDead;

	private bool bossIsDead;

	private float correctFadeTime = 2f;

	private void OnEnable()
	{
		if (changeMusic)
		{
			music2.Stop();
			music2.clip = music1.clip;
			music2.volume = maxVolume;
			music2.Play();
			music2.time = music1.time;
			music1.volume = 0f;
			music1.clip = stages[0].newAudio;
			music1.Play();
			music1.time = stages[0].skipAudioToTime;
			if (stages[0].tweakToDisable != null)
			{
				stages[0].tweakToDisable.gameObject.SetActive(false);
			}
			stages.RemoveAt(0);
			changeMusic = false;
		}
	}

	private void Update()
	{
		if (stages.Count > 0 && bossHealth.health < stages[0].minHealthFor && !stages[0].hasChanged)
		{
			if (stages[0].cutscene != null)
			{
				stages[0].cutscene.SendMessage("Interact");
			}
			correctFadeTime = stages[0].audioFade;
			if (!stages[0].waitUntilAfterCutscene)
			{
				music2.Stop();
				music2.clip = music1.clip;
				music2.volume = maxVolume;
				music2.Play();
				music2.time = music1.time;
				music1.volume = 0f;
				music1.clip = stages[0].newAudio;
				music1.Play();
				music1.time = stages[0].skipAudioToTime;
				if (stages[0].tweakToDisable != null)
				{
					stages[0].tweakToDisable.gameObject.SetActive(false);
				}
				stages.RemoveAt(0);
			}
			else
			{
				stages[0].hasChanged = true;
				changeMusic = true;
			}
		}
		if (music2.volume != 0f)
		{
			music2.volume -= Time.deltaTime / correctFadeTime;
		}
		else
		{
			music2.Stop();
		}
		if (music1.volume < maxVolume)
		{
			music1.volume += Time.deltaTime / correctFadeTime;
		}
		else
		{
			music1.volume = maxVolume;
		}
		if (playerHealth.health <= 0 && !playerIsDead && playerDeath != null)
		{
			playerDeath.SendMessage("Interact");
			playerIsDead = true;
		}
		if (bossHealth.health <= 0 && !bossIsDead && bossDeath != null)
		{
			bossDeath.SendMessage("Interact");
			bossIsDead = true;
		}
	}
}
