using UnityEngine;

namespace AC
{
	public class Options : MonoBehaviour
	{
		public OptionsData optionsData;

		private string ppKey = "Options";

		private void Awake()
		{
			optionsData = new OptionsData();
			LoadPrefs();
			if (optionsData.language == 0 && (bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().speechManager && AdvGame.GetReferences().speechManager.ignoreOriginalText && AdvGame.GetReferences().speechManager.languages.Count > 1)
			{
				optionsData.language = PlayerPrefs.GetInt("Language");
				SavePrefs();
			}
			optionsData.language = PlayerPrefs.GetInt("Language");
			OnLevelWasLoaded();
		}

		public void SavePrefs()
		{
			RuntimeVariables.DownloadAll();
			optionsData.linkedVariables = SaveSystem.CreateVariablesData(GetComponent<RuntimeVariables>().globalVars, true, VariableLocation.Global);
			string value = Serializer.SerializeObjectBinary(optionsData);
			PlayerPrefs.SetString(ppKey, value);
			Debug.Log("PlayerPrefs saved.");
		}

		private void LoadPrefs()
		{
			if (PlayerPrefs.HasKey(ppKey))
			{
				string pString = PlayerPrefs.GetString(ppKey);
				optionsData = Serializer.DeserializeObjectBinary<OptionsData>(pString);
				Debug.Log("PlayerPrefs loaded.");
			}
		}

		private void OnLevelWasLoaded()
		{
			SetVolume(SoundType.Music);
			SetVolume(SoundType.SFX);
		}

		public void SetVolume(SoundType _soundType)
		{
			Sound[] array = Object.FindObjectsOfType(typeof(Sound)) as Sound[];
			Sound[] array2 = array;
			foreach (Sound sound in array2)
			{
				if (sound.soundType == _soundType)
				{
					sound.AfterLoading();
				}
			}
		}

		public static void SetLanguage(int i)
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<Options>() && GameObject.FindWithTag("PersistentEngine").GetComponent<Options>().optionsData != null)
			{
				Options component = GameObject.FindWithTag("PersistentEngine").GetComponent<Options>();
				component.optionsData.language = PlayerPrefs.GetInt("Language");
				component.SavePrefs();
			}
			else
			{
				Debug.LogWarning("Could not find Options data!");
			}
		}

		public static string GetLanguageName()
		{
			return AdvGame.GetReferences().speechManager.languages[GetLanguage()];
		}

		public static int GetLanguage()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<Options>() && GameObject.FindWithTag("PersistentEngine").GetComponent<Options>().optionsData != null)
			{
				return GameObject.FindWithTag("PersistentEngine").GetComponent<Options>().optionsData.language;
			}
			return 0;
		}
	}
}
