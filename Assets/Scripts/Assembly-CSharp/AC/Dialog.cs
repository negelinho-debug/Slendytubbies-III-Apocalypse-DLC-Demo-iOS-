using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class Dialog : MonoBehaviour
	{
		[HideInInspector]
		public bool isBackground;

		private bool currentLineHasAudio;

		private Char speakerChar;

		private string speakerName;

		private bool isSkippable;

		private string displayText = string.Empty;

		private string fullText = string.Empty;

		private float displayDuration;

		private float endTime;

		[HideInInspector]
		public bool pauseGap;

		private int gapIndex = -1;

		private List<SpeechGap> speechGaps = new List<SpeechGap>();

		private string allGapText;

		private Options options;

		private AudioSource defaultAudioSource;

		private PlayerInput playerInput;

		private SpeechManager speechManager;

		private StateHandler stateHandler;

		public bool isMessageAlive { get; set; }

		public bool foundAudio { get; set; }

		private void Awake()
		{
			playerInput = GetComponent<PlayerInput>();
			if (AdvGame.GetReferences() == null)
			{
				Debug.LogError("A References file is required - please use the Adventure Creator window to create one.");
			}
			else
			{
				speechManager = AdvGame.GetReferences().speechManager;
				if (speechManager.textScrollSpeed == 0f)
				{
					Debug.LogError("Cannot have a Text Scroll Speed of zero - please amend your Speech Manager");
				}
			}
			if ((bool)GetComponent<SceneSettings>() && (bool)GetComponent<SceneSettings>().defaultSound && (bool)GetComponent<SceneSettings>().defaultSound.GetComponent<AudioSource>())
			{
				defaultAudioSource = GetComponent<SceneSettings>().defaultSound.GetComponent<AudioSource>();
			}
		}

		private void Start()
		{
			stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
			options = stateHandler.GetComponent<Options>();
		}

		public void UpdateSkipDialogue()
		{
			if (!isSkippable || !isMessageAlive || !(playerInput != null))
			{
				return;
			}
			if (pauseGap && !isBackground)
			{
				if (playerInput.mouseState != MouseState.SingleClick && playerInput.mouseState != MouseState.RightClick)
				{
					return;
				}
				if ((float)speechGaps[gapIndex].waitTime < 0f)
				{
					playerInput.mouseState = MouseState.Normal;
					pauseGap = false;
					if (speechManager.scrollSubtitles)
					{
						StartCoroutine("EndMessage");
					}
				}
				else if (speechManager.allowSpeechSkipping)
				{
					playerInput.mouseState = MouseState.Normal;
					pauseGap = false;
				}
			}
			else if (speechManager.displayForever)
			{
				if (playerInput.mouseState == MouseState.SingleClick || playerInput.mouseState == MouseState.RightClick)
				{
					playerInput.mouseState = MouseState.Normal;
					if (stateHandler.gameState == GameState.Cutscene)
					{
						if (speechManager.endScrollBeforeSkip && speechManager.scrollSubtitles && displayText != fullText)
						{
							StopCoroutine("StartMessage");
							displayText = fullText;
						}
						else
						{
							isMessageAlive = false;
							StartCoroutine("EndMessage");
						}
					}
				}
				else if (Time.time > endTime && isBackground)
				{
					StartCoroutine("EndMessage");
				}
			}
			else if ((playerInput.mouseState == MouseState.SingleClick || playerInput.mouseState == MouseState.RightClick) && (bool)speechManager && speechManager.allowSpeechSkipping)
			{
				playerInput.mouseState = MouseState.Normal;
				if (stateHandler.gameState != GameState.Cutscene && (!speechManager.allowGameplaySpeechSkipping || stateHandler.gameState != GameState.Normal))
				{
					return;
				}
				if (speechManager.endScrollBeforeSkip && speechManager.scrollSubtitles && displayText != fullText)
				{
					if (speechGaps.Count > 0 && speechGaps.Count > gapIndex)
					{
						while (gapIndex < speechGaps.Count && speechGaps[gapIndex].waitTime >= 0)
						{
							gapIndex++;
						}
						if (gapIndex == speechGaps.Count)
						{
							StopCoroutine("StartMessage");
							displayText = fullText;
						}
						else
						{
							pauseGap = true;
							displayText = fullText.Substring(0, speechGaps[gapIndex].characterIndex);
						}
					}
					else
					{
						StopCoroutine("StartMessage");
						displayText = fullText;
					}
				}
				else
				{
					isMessageAlive = false;
					StartCoroutine("EndMessage");
				}
			}
			else if (Time.time > endTime)
			{
				StartCoroutine("EndMessage");
			}
		}

		public string GetSpeaker()
		{
			if ((bool)speakerChar)
			{
				if (speakerChar.speechLabel != string.Empty)
				{
					return speakerChar.speechLabel;
				}
				return speakerChar.name;
			}
			return string.Empty;
		}

		public Char GetSpeakingCharacter()
		{
			return speakerChar;
		}

		public Texture2D GetPortrait()
		{
			if ((bool)speakerChar && (bool)speakerChar.portraitIcon.texture)
			{
				return speakerChar.portraitIcon.texture;
			}
			return null;
		}

		public bool IsAnimating()
		{
			if ((bool)speakerChar && speakerChar.portraitIcon.isAnimated)
			{
				return true;
			}
			return false;
		}

		public Rect GetAnimatedRect()
		{
			if (speakerChar != null && speakerChar.portraitIcon != null)
			{
				if (speakerChar.isLipSyncing)
				{
					return speakerChar.portraitIcon.GetAnimatedRect(speakerChar.GetLipSyncFrame());
				}
				return speakerChar.portraitIcon.GetAnimatedRect();
			}
			return new Rect(0f, 0f, 0f, 0f);
		}

		public Color GetColour()
		{
			if ((bool)speakerChar)
			{
				return speakerChar.speechColor;
			}
			return Color.white;
		}

		public string GetLine()
		{
			if (speechManager.keepTextInBuffer)
			{
				return displayText;
			}
			if (isMessageAlive && isSkippable)
			{
				return displayText;
			}
			return string.Empty;
		}

		public string GetFullLine()
		{
			if (isMessageAlive && isSkippable)
			{
				return fullText;
			}
			return string.Empty;
		}

		private IEnumerator EndMessage()
		{
			StopCoroutine("StartMessage");
			isSkippable = false;
			if ((bool)speakerChar)
			{
				speakerChar.StopSpeaking();
			}
			if (gapIndex >= 0 && gapIndex < speechGaps.Count)
			{
				gapIndex++;
				StartCoroutine("StartMessage", allGapText);
			}
			else
			{
				yield return new WaitForSeconds(0.1f);
				isMessageAlive = false;
			}
		}

		private IEnumerator StartMessage(string message)
		{
			isMessageAlive = true;
			isSkippable = true;
			displayText = string.Empty;
			message = AdvGame.ConvertTokens(message);
			fullText = message;
			pauseGap = false;
			endTime = Time.time + displayDuration;
			if (speechManager.scrollSubtitles)
			{
				float amount = 0f;
				while (amount < 1f)
				{
					if (!pauseGap)
					{
						amount += speechManager.textScrollSpeed / 100f / (float)message.Length;
						if (amount > 1f)
						{
							amount = 1f;
						}
						int currentCharIndex = (int)(amount * (float)message.Length);
						if (gapIndex > 0)
						{
							currentCharIndex += speechGaps[gapIndex - 1].characterIndex;
							if (currentCharIndex > message.Length)
							{
								currentCharIndex = message.Length;
							}
						}
						string newText = (displayText = message.Substring(0, currentCharIndex));
						if (gapIndex >= 0 && speechGaps.Count > gapIndex && currentCharIndex == speechGaps[gapIndex].characterIndex)
						{
							float waitTime = speechGaps[gapIndex].waitTime;
							pauseGap = true;
							if (waitTime >= 0f)
							{
								float pauseEndTime = Time.time + waitTime;
								while (pauseGap && Time.time < pauseEndTime)
								{
									yield return new WaitForFixedUpdate();
								}
							}
							else
							{
								while (pauseGap)
								{
									yield return new WaitForFixedUpdate();
								}
							}
							pauseGap = false;
							gapIndex++;
							amount = 0f;
						}
						if (displayText != newText && (bool)speechManager.textScrollCLip && !currentLineHasAudio)
						{
							if ((bool)defaultAudioSource)
							{
								if (!defaultAudioSource.isPlaying)
								{
									defaultAudioSource.PlayOneShot(speechManager.textScrollCLip);
								}
							}
							else
							{
								Debug.LogWarning("Cannot play text scroll audio clip as no 'Default' sound prefab has been defined in the Scene Manager");
							}
						}
					}
					yield return new WaitForFixedUpdate();
				}
				displayText = message;
			}
			else
			{
				if (gapIndex >= 0 && speechGaps.Count >= gapIndex)
				{
					if (gapIndex == speechGaps.Count)
					{
						displayText = message;
						foreach (SpeechGap speechGap in speechGaps)
						{
							endTime -= speechGap.waitTime;
						}
					}
					else
					{
						float num = speechGaps[gapIndex].waitTime;
						displayText = message.Substring(0, speechGaps[gapIndex].characterIndex);
						if (num >= 0f)
						{
							endTime = Time.time + num;
						}
						else
						{
							pauseGap = true;
						}
					}
				}
				else
				{
					displayText = message;
				}
				yield return new WaitForSeconds((float)message.Length / speechManager.textScrollSpeed);
			}
			if (endTime == Time.time)
			{
				endTime += 2f;
			}
		}

		public float StartDialog(Char _speakerChar, string message, int lineNumber, string language, bool _isBackground)
		{
			isMessageAlive = false;
			currentLineHasAudio = false;
			isBackground = _isBackground;
			if ((bool)_speakerChar)
			{
				speakerChar = _speakerChar;
				speakerChar.isTalking = true;
				speakerName = _speakerChar.name;
				if ((bool)_speakerChar.GetComponent<Player>())
				{
					speakerName = "Player";
				}
				if (_speakerChar.portraitIcon != null)
				{
					_speakerChar.portraitIcon.Reset();
				}
				if ((bool)_speakerChar.GetComponent<Hotspot>() && _speakerChar.GetComponent<Hotspot>().hotspotName != string.Empty)
				{
					speakerName = _speakerChar.GetComponent<Hotspot>().hotspotName;
				}
				if (speechManager.lipSyncMode == LipSyncMode.Off)
				{
					speakerChar.isLipSyncing = false;
				}
				else if (speechManager.lipSyncMode == LipSyncMode.FromSpeechText || speechManager.lipSyncMode == LipSyncMode.ReadPamelaFile || speechManager.lipSyncMode == LipSyncMode.ReadSapiFile)
				{
					speakerChar.StartLipSync(GenerateLipSyncShapes(speechManager.lipSyncMode, lineNumber, speakerName, language, message));
				}
			}
			else
			{
				if ((bool)speakerChar)
				{
					speakerChar.isTalking = false;
				}
				speakerChar = null;
				speakerName = "Narrator";
			}
			if (lineNumber > -1 && speakerName != string.Empty && speechManager.searchAudioFiles)
			{
				string text = "Speech/";
				if (language != string.Empty && speechManager.translateAudio)
				{
					text = text + language + "/";
				}
				text = text + speakerName + lineNumber;
				foundAudio = false;
				AudioClip audioClip = Resources.Load(text) as AudioClip;
				if ((bool)audioClip)
				{
					AudioSource audioSource = null;
					currentLineHasAudio = true;
					if (_speakerChar != null)
					{
						if (speechManager.lipSyncMode == LipSyncMode.FaceFX)
						{
							FaceFXIntegration.Play(speakerChar, speakerName + lineNumber, audioClip);
						}
						if ((bool)_speakerChar.GetComponent<AudioSource>())
						{
							_speakerChar.GetComponent<AudioSource>().volume = options.optionsData.speechVolume;
							audioSource = _speakerChar.GetComponent<AudioSource>();
						}
						else
						{
							Debug.LogWarning(_speakerChar.name + " has no audio source component!");
						}
					}
					else if ((bool)KickStarter.player && (bool)KickStarter.player.GetComponent<AudioSource>())
					{
						KickStarter.player.GetComponent<AudioSource>().volume = options.optionsData.speechVolume;
						audioSource = KickStarter.player.GetComponent<AudioSource>();
					}
					else if (defaultAudioSource != null)
					{
						audioSource = defaultAudioSource;
					}
					if (audioSource != null)
					{
						audioSource.clip = audioClip;
						audioSource.loop = false;
						audioSource.Play();
						foundAudio = true;
					}
					displayDuration = audioClip.length;
				}
				else
				{
					displayDuration = speechManager.screenTimeFactor * (float)message.Length;
					if (displayDuration < 0.5f)
					{
						displayDuration = 0.5f;
					}
					Debug.Log("Cannot find audio file: " + text);
				}
			}
			else
			{
				displayDuration = speechManager.screenTimeFactor * (float)message.Length;
				if (displayDuration < 0.5f)
				{
					displayDuration = 0.5f;
				}
			}
			message = DetermineGaps(message);
			if (speechGaps.Count > 0)
			{
				gapIndex = 0;
				allGapText = message;
				foreach (SpeechGap speechGap in speechGaps)
				{
					displayDuration += speechGap.waitTime;
				}
			}
			else
			{
				gapIndex = -1;
			}
			StopCoroutine("StartMessage");
			StartCoroutine("StartMessage", message);
			return displayDuration;
		}

		public void KillDialog(bool forceMenusOff)
		{
			isSkippable = false;
			isMessageAlive = false;
			StopCoroutine("StartMessage");
			StopCoroutine("EndMessage");
			if ((bool)speakerChar)
			{
				speakerChar.StopSpeaking();
			}
			if (forceMenusOff)
			{
				GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>().ForceOffSubtitles();
			}
		}

		private void OnDestroy()
		{
			playerInput = null;
			speakerChar = null;
			speechManager = null;
			defaultAudioSource = null;
		}

		private string DetermineGaps(string _text)
		{
			speechGaps.Clear();
			if (_text != null && _text.Contains("[wait"))
			{
				while (_text.Contains("[wait:"))
				{
					int num = _text.IndexOf("[wait:");
					int num2 = num + 7;
					if (_text.Substring(num2 + 1, 1) == "]")
					{
						num2++;
					}
					string value = _text.Substring(num + 6, num2 - num - 6);
					speechGaps.Add(new SpeechGap(num, IntParseFast(value)));
					_text = _text.Substring(0, num) + _text.Substring(num2 + 1);
				}
				while (_text.Contains("[wait]"))
				{
					int num3 = _text.IndexOf("[wait]");
					speechGaps.Add(new SpeechGap(num3, -1));
					_text = _text.Substring(0, num3) + _text.Substring(num3 + 6);
				}
			}
			return _text;
		}

		private int IntParseFast(string value)
		{
			int num = 0;
			foreach (char c in value)
			{
				num = 10 * num + (int)char.GetNumericValue(c);
			}
			return num;
		}

		public bool HasPausing()
		{
			if (speechGaps.Count > 0)
			{
				return true;
			}
			return false;
		}

		public List<LipSyncShape> GenerateLipSyncShapes(LipSyncMode _lipSyncMode, int lineNumber, string speakerName, string language, string _message)
		{
			List<LipSyncShape> list = new List<LipSyncShape>();
			list.Add(new LipSyncShape(0, 0f, speechManager.lipSyncSpeed));
			TextAsset textAsset = null;
			if (lineNumber > -1 && speakerName != string.Empty && speechManager.searchAudioFiles)
			{
				string text = "Lipsync/";
				if (language != string.Empty && speechManager.translateAudio)
				{
					text = text + language + "/";
				}
				text = text + speakerName + lineNumber;
				textAsset = Resources.Load(text) as TextAsset;
			}
			if (_lipSyncMode == LipSyncMode.ReadPamelaFile && textAsset != null)
			{
				string[] array = textAsset.text.Split('\n');
				bool flag = false;
				float result = 24f;
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!flag)
					{
						if (text2.Contains("framespersecond:"))
						{
							string[] array3 = text2.Split(':');
							float.TryParse(array3[1], out result);
						}
						else if (text2.Contains("[Speech]"))
						{
							flag = true;
						}
					}
					else
					{
						if (!text2.Contains(":"))
						{
							continue;
						}
						string[] array4 = text2.Split(':');
						float result2 = 0f;
						float.TryParse(array4[0], out result2);
						string text3 = array4[1].ToLower().Substring(0, array4[1].Length - 1);
						bool flag2 = false;
						foreach (string phoneme in speechManager.phonemes)
						{
							string[] array5 = phoneme.ToLower().Split("/"[0]);
							if (flag2)
							{
								continue;
							}
							string[] array6 = array5;
							foreach (string text4 in array6)
							{
								if (text4 == text3)
								{
									int frame = speechManager.phonemes.IndexOf(phoneme);
									list.Add(new LipSyncShape(frame, result2, speechManager.lipSyncSpeed, result));
									flag2 = true;
								}
							}
						}
						if (!flag2)
						{
							list.Add(new LipSyncShape(0, result2, speechManager.lipSyncSpeed, result));
						}
					}
				}
			}
			else if (_lipSyncMode == LipSyncMode.ReadSapiFile && textAsset != null)
			{
				string[] array7 = textAsset.text.Split('\n');
				string[] array8 = array7;
				foreach (string text5 in array8)
				{
					if (!text5.StartsWith("phn "))
					{
						continue;
					}
					string[] array9 = text5.Split(' ');
					float result3 = 0f;
					float.TryParse(array9[1], out result3);
					string text6 = array9[4].ToLower().Substring(0, array9[4].Length - 1);
					bool flag3 = false;
					foreach (string phoneme2 in speechManager.phonemes)
					{
						string[] array10 = phoneme2.ToLower().Split("/"[0]);
						if (flag3)
						{
							continue;
						}
						string[] array11 = array10;
						foreach (string text7 in array11)
						{
							if (text7 == text6)
							{
								int frame2 = speechManager.phonemes.IndexOf(phoneme2);
								list.Add(new LipSyncShape(frame2, result3, speechManager.lipSyncSpeed, 60f));
								flag3 = true;
							}
						}
					}
					if (!flag3)
					{
						list.Add(new LipSyncShape(0, result3, speechManager.lipSyncSpeed, 60f));
					}
				}
			}
			else if (_lipSyncMode == LipSyncMode.FromSpeechText)
			{
				for (int m = 0; m < _message.Length; m++)
				{
					int num = Mathf.Min(5, _message.Length - m);
					for (int num2 = num; num2 > 0; num2--)
					{
						string text8 = _message.Substring(m, num2);
						text8 = text8.ToLower();
						foreach (string phoneme3 in speechManager.phonemes)
						{
							string[] array12 = phoneme3.ToLower().Split("/"[0]);
							string[] array13 = array12;
							foreach (string text9 in array13)
							{
								if (text9 == text8)
								{
									int frame3 = speechManager.phonemes.IndexOf(phoneme3);
									list.Add(new LipSyncShape(frame3, m, speechManager.lipSyncSpeed));
									m += num2;
									num2 = Mathf.Min(5, _message.Length - m);
									break;
								}
							}
						}
					}
					list.Add(new LipSyncShape(0, m, speechManager.lipSyncSpeed));
				}
			}
			if (list.Count > 1)
			{
				list.Sort((LipSyncShape a, LipSyncShape b) => a.timeIndex.CompareTo(b.timeIndex));
			}
			return list;
		}
	}
}
