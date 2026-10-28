using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AudioManager : MonoBehaviour
{
	public Slider audioVolume;

	private void Awake()
	{
		audioVolume.value -= PlayerPrefs.GetFloat("GlobalVolume");
		AudioListener.volume = audioVolume.value;
		AudioListener.volume = 1f;
	}

	private void SetListenerVolume()
	{
		AudioListener.volume = audioVolume.value;
		PlayerPrefs.SetFloat("GlobalVolume", 1f - audioVolume.value);
	}

	public void ApplySettings()
	{
		SetListenerVolume();
	}
}
