using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class SpeechManager : ScriptableObject
	{
		public float textScrollSpeed = 50f;

		public AudioClip textScrollCLip;

		public bool displayForever;

		public float screenTimeFactor = 0.1f;

		public bool allowSpeechSkipping;

		public bool allowGameplaySpeechSkipping;

		public bool searchAudioFiles = true;

		public bool forceSubtitles = true;

		public bool translateAudio = true;

		public bool scrollSubtitles = true;

		public bool endScrollBeforeSkip;

		public bool keepTextInBuffer;

		public bool separateLines;

		public float separateLinePause = 1f;

		public List<SpeechLine> lines = new List<SpeechLine>();

		public List<string> languages = new List<string>();

		public string[] sceneFiles;

		public bool ignoreOriginalText;

		public LipSyncMode lipSyncMode;

		public LipSyncOutput lipSyncOutput;

		public List<string> phonemes = new List<string>();

		public float lipSyncSpeed = 1f;

		public static string GetTranslation(int _lineID, int i)
		{
			SpeechManager speechManager = AdvGame.GetReferences().speechManager;
			if (_lineID == -1)
			{
				Debug.Log("Cannot find translation because the text has not been added to the Speech Manager.");
			}
			else
			{
				foreach (SpeechLine line in speechManager.lines)
				{
					if (line.lineID == _lineID)
					{
						if (line.translationText.Count > i - 1)
						{
							return line.translationText[i - 1];
						}
						Debug.LogWarning("A translation is being requested that does not exist!");
					}
				}
			}
			return string.Empty;
		}
	}
}
