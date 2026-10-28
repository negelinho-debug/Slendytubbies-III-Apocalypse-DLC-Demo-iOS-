using System;

[Serializable]
public class OptionsData
{
	public int language;

	public bool showSubtitles;

	public float sfxVolume;

	public float musicVolume;

	public float speechVolume;

	public string linkedVariables = string.Empty;

	public OptionsData()
	{
		language = 0;
		showSubtitles = false;
		sfxVolume = 0.9f;
		musicVolume = 0.6f;
		speechVolume = 1f;
		linkedVariables = string.Empty;
	}
}
