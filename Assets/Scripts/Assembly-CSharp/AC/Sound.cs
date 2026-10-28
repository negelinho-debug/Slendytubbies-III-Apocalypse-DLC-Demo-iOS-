using System.Collections;
using UnityEngine;

namespace AC
{
	[RequireComponent(typeof(AudioSource))]
	public class Sound : MonoBehaviour
	{
		public SoundType soundType;

		public bool playWhilePaused;

		public float relativeVolume = 1f;

		public bool surviveSceneChange;

		private float maxVolume = 1f;

		private float fadeStartTime;

		private float fadeEndTime;

		private FadeType fadeType;

		[HideInInspector]
		public bool isFading;

		private Options options;

		private AudioSource audioSource;

		private float setTime;

		private bool updateTime;

		public bool canDestroy
		{
			get
			{
				if (soundType == SoundType.Music && surviveSceneChange && !audioSource.isPlaying)
				{
					return true;
				}
				return false;
			}
		}

		private void Awake()
		{
			if (soundType == SoundType.Music && surviveSceneChange)
			{
				Object.DontDestroyOnLoad(this);
			}
			if ((bool)GetComponent<AudioSource>())
			{
				audioSource = GetComponent<AudioSource>();
			}
			audioSource.ignoreListenerPause = playWhilePaused;
		}

		private void Start()
		{
			StartCoroutine("_Update");
		}

		public void AfterLoading()
		{
			if (audioSource == null && (bool)GetComponent<AudioSource>())
			{
				audioSource = GetComponent<AudioSource>();
			}
			if ((bool)audioSource)
			{
				audioSource.ignoreListenerPause = playWhilePaused;
				if (audioSource.playOnAwake)
				{
					FadeIn(0.5f, audioSource.loop);
				}
				else
				{
					SetMaxVolume();
				}
			}
			else
			{
				Debug.LogWarning("Sound object " + name + " has no AudioSource component.");
			}
		}

		private IEnumerator _Update()
		{
			while (Application.isPlaying)
			{
				UpdateVolume();
				yield return new WaitForFixedUpdate();
			}
		}

		private void UpdateVolume()
		{
			if (!isFading || !audioSource.isPlaying)
			{
				return;
			}
			float num = (Time.time - fadeStartTime) / (fadeEndTime - fadeStartTime);
			if (fadeType == FadeType.fadeIn)
			{
				if (num > 1f)
				{
					audioSource.volume = maxVolume;
					isFading = false;
				}
				else
				{
					audioSource.volume = num * maxVolume;
				}
			}
			else if (fadeType == FadeType.fadeOut)
			{
				if (num > 1f)
				{
					audioSource.volume = 0f;
					audioSource.Stop();
					isFading = false;
				}
				else
				{
					audioSource.volume = (1f - num) * maxVolume;
				}
			}
		}

		public void Interact()
		{
			isFading = false;
			SetMaxVolume();
			Play(audioSource.loop);
		}

		public void FadeIn(float fadeTime, bool loop)
		{
			audioSource.loop = loop;
			fadeStartTime = Time.time;
			fadeEndTime = Time.time + fadeTime;
			fadeType = FadeType.fadeIn;
			SetMaxVolume();
			isFading = true;
			audioSource.volume = 0f;
			audioSource.Play();
		}

		public void FadeOut(float fadeTime)
		{
			if (audioSource.isPlaying)
			{
				fadeStartTime = Time.time;
				fadeEndTime = Time.time + fadeTime;
				fadeType = FadeType.fadeOut;
				SetMaxVolume();
				isFading = true;
			}
		}

		public void Play()
		{
			if (Time.timeScale != 0f || !playWhilePaused)
			{
				isFading = false;
				SetMaxVolume();
				audioSource.Play();
			}
		}

		public void Play(bool loop)
		{
			audioSource.loop = loop;
			Play();
		}

		public void Play(AudioClip clip, bool loop)
		{
			audioSource.clip = clip;
			audioSource.loop = loop;
			Play();
		}

		public void SetMaxVolume()
		{
			maxVolume = relativeVolume;
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<Options>())
			{
				options = GameObject.FindWithTag("PersistentEngine").GetComponent<Options>();
			}
			if ((bool)options && options.optionsData != null && soundType != SoundType.Other)
			{
				if (soundType == SoundType.Music)
				{
					maxVolume *= options.optionsData.musicVolume;
				}
				else if (soundType == SoundType.SFX)
				{
					maxVolume *= options.optionsData.sfxVolume;
				}
			}
			if (!isFading)
			{
				audioSource.volume = maxVolume;
			}
		}

		public void Stop()
		{
			audioSource.Stop();
		}

		public void EndOldMusic(Sound newSound)
		{
			if (soundType == SoundType.Music && audioSource.isPlaying && this != newSound && (!isFading || fadeType == FadeType.fadeIn))
			{
				FadeOut(0.1f);
			}
		}

		public bool IsPlaying()
		{
			return audioSource.isPlaying;
		}

		public bool IsPlaying(AudioClip clip)
		{
			if (audioSource != null && clip != null && audioSource.clip != null && audioSource.clip == clip && audioSource.isPlaying)
			{
				return true;
			}
			return false;
		}

		public bool IsPlayingSameMusic(AudioClip clip)
		{
			if (soundType == SoundType.Music && audioSource != null && clip != null && audioSource.clip != null && audioSource.clip == clip && audioSource.isPlaying)
			{
				return true;
			}
			return false;
		}
	}
}
