using System.Collections.Generic;
using System.Linq;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
	public Dropdown window;

	public Dropdown resolutionSettings;

	public Dropdown qualitySettings;

	public Dropdown textureQuality;

	public Dropdown antialiasing;

	public Dropdown anisotropicTextures;

	public Dropdown vsync;

	public Slider mouseSensitivity;

	private List<QualityData> currentQualities = new List<QualityData>();

	public Resolution[] resolutions;

	private int curLanguage;

	private void Awake()
	{
		if (resolutionSettings == null)
		{
			return;
		}
		resolutionSettings.onValueChanged.AddListener(delegate
		{
			OnResolutionChange();
		});
		resolutions = Screen.resolutions;
		Resolution[] array = resolutions;
		for (int num = 0; num < array.Length; num++)
		{
			Resolution resolution = array[num];
			resolutionSettings.options.Add(new Dropdown.OptionData(resolution.ToString()));
		}
		for (int num2 = 0; num2 <= 1; num2++)
		{
			if (num2 == 0)
			{
				window.options.Add(new Dropdown.OptionData("Fullscreen"));
			}
			else
			{
				window.options.Add(new Dropdown.OptionData("Windowed"));
			}
		}
		if (Screen.fullScreen)
		{
			window.value = 0;
		}
		else
		{
			window.value = 1;
		}
		if (window.value == 0 && !Screen.fullScreen)
		{
			Screen.fullScreen = true;
		}
		if (window.value == 1 && Screen.fullScreen)
		{
			Screen.fullScreen = false;
		}
	}

	private void Update()
	{
		if (window == null)
		{
			return;
		}
		if (window.value == 0 && !Screen.fullScreen)
		{
			Screen.fullScreen = true;
		}
		if (window.value == 1 && Screen.fullScreen)
		{
			Screen.fullScreen = false;
		}
		if (!PlayerPrefs.HasKey("Language") || curLanguage == PlayerPrefs.GetInt("Language"))
		{
			return;
		}
		curLanguage = PlayerPrefs.GetInt("Language");
		for (int i = 0; i < window.options.Count; i++)
		{
			string text = "Lemon";
			if (i == 0)
			{
				if (curLanguage == 0)
				{
					text = "Fullscreen";
				}
				if (curLanguage == 1)
				{
					text = "Pantalla Completa";
				}
			}
			else
			{
				if (curLanguage == 0)
				{
					text = "Windowed";
				}
				if (curLanguage == 1)
				{
					text = "Ventana";
				}
			}
			window.options[i].text = text;
			window.captionText.text = text;
		}
	}

	public void OnResolutionChange()
	{
		Screen.SetResolution(resolutions[resolutionSettings.value].width, resolutions[resolutionSettings.value].height, Screen.fullScreen);
	}

	private void Start()
	{
		if (resolutionSettings != null)
		{
			GetGlobalSettings();
			SetQualitySettingsInOptions();
			qualitySettings.value = QualitySettings.GetQualityLevel();
		}
		if (mouseSensitivity != null)
		{
			if (ObscuredPrefs.HasKey("Mouse"))
			{
				mouseSensitivity.value = ObscuredPrefs.GetInt("Mouse");
			}
			else
			{
				mouseSensitivity.value = 1f;
			}
		}
		if (resolutionSettings == null)
		{
			return;
		}
		for (int i = 0; i < resolutions.Length; i++)
		{
			if (resolutions[i].height == Screen.height && resolutions[i].width == Screen.width)
			{
				resolutionSettings.value = i;
				resolutionSettings.captionText.text = resolutionSettings.options[i].text;
			}
		}
	}

	public void SetSensitivity()
	{
		ObscuredPrefs.SetInt("Mouse", (int)mouseSensitivity.value);
	}

	public void SetGlobalValues()
	{
		QualityData qualityData = currentQualities[qualitySettings.value];
		textureQuality.value = qualityData.textureQuality;
		antialiasing.value = (int)qualityData.antialiasingValues;
		anisotropicTextures.value = qualityData.anisotropic;
		vsync.value = qualityData.vsync;
	}

	public void GetGlobalSettings()
	{
		int qualityLevel = QualitySettings.GetQualityLevel();
		for (int i = 0; i < QualitySettings.names.Length; i++)
		{
			QualitySettings.SetQualityLevel(i, false);
			currentQualities.Add(new QualityData
			{
				QualityLevelName = QualitySettings.names[QualitySettings.GetQualityLevel()],
				antialiasingValues = (AntialiasingValues)QualitySettings.antiAliasing,
				anisotropic = (int)QualitySettings.anisotropicFiltering,
				textureQuality = QualitySettings.masterTextureLimit,
				vsync = QualitySettings.vSyncCount
			});
		}
		QualitySettings.SetQualityLevel(qualityLevel);
	}

	private void SetQualitySettingsInOptions()
	{
		qualitySettings.ClearOptions();
		qualitySettings.AddOptions(QualitySettings.names.ToList());
	}

	private void SetQualityCount()
	{
		QualitySettings.SetQualityLevel(qualitySettings.value);
	}

	private void SetTextureQuality()
	{
		QualitySettings.masterTextureLimit = textureQuality.value;
	}

	private void SetAntialiasingLevel()
	{
		if (antialiasing.value == 3)
		{
			QualitySettings.antiAliasing = 8;
		}
		else
		{
			QualitySettings.antiAliasing = antialiasing.value * 2;
		}
	}

	private void SetAnisotropicTextures()
	{
		QualitySettings.anisotropicFiltering = (AnisotropicFiltering)anisotropicTextures.value;
	}

	private void SetVsync()
	{
		QualitySettings.vSyncCount = vsync.value;
	}

	public void ApplySettings()
	{
		SetQualityCount();
		SetTextureQuality();
		SetAntialiasingLevel();
		SetAnisotropicTextures();
		SetVsync();
	}
}
