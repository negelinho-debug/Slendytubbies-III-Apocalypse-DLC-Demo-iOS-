using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionSpeech : Action
	{
		public int constantID;

		public int parameterID = -1;

		public bool isPlayer;

		public Char speaker;

		public string messageText;

		public int lineID;

		public bool isBackground;

		public AnimationClip headClip;

		public AnimationClip mouthClip;

		public bool play2DHeadAnim;

		public string headClip2D = string.Empty;

		public int headLayer;

		public bool play2DMouthAnim;

		public string mouthClip2D = string.Empty;

		public int mouthLayer;

		public float waitTimeOffset;

		private float endTime;

		private bool stopAction;

		private int splitNumber;

		private bool splitDelay;

		private Dialog dialog;

		private StateHandler stateHandler;

		private SpeechManager speechManager;

		public ActionSpeech()
		{
			isDisplayed = true;
			title = "Dialogue: Play speech";
			lineID = -1;
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			speaker = AssignFile(parameters, parameterID, constantID, speaker);
			if (isPlayer)
			{
				speaker = KickStarter.player;
			}
		}

		public override float Run()
		{
			if (speechManager == null)
			{
				speechManager = AdvGame.GetReferences().speechManager;
			}
			if (speechManager == null)
			{
				Debug.Log("No Speech Manager present");
				return 0f;
			}
			dialog = GameObject.FindWithTag("GameEngine").GetComponent<Dialog>();
			stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
			if ((bool)dialog && (bool)stateHandler)
			{
				if (!isRunning)
				{
					stopAction = false;
					isRunning = true;
					splitDelay = false;
					splitNumber = 0;
					endTime = Time.time + StartSpeech();
					if (isBackground)
					{
						isRunning = false;
						return 0f;
					}
					return defaultPauseTime;
				}
				if (stopAction)
				{
					isRunning = false;
					return 0f;
				}
				if (!dialog.isMessageAlive)
				{
					if (speechManager.separateLines)
					{
						if (splitDelay)
						{
							splitDelay = false;
							endTime = Time.time + StartSpeech();
							return defaultPauseTime;
						}
						splitNumber++;
						string[] array = messageText.Split('\n');
						if (array.Length > splitNumber)
						{
							splitDelay = true;
							return speechManager.separateLinePause;
						}
					}
					if (waitTimeOffset <= 0f)
					{
						isRunning = false;
						return 0f;
					}
					stopAction = true;
					return waitTimeOffset;
				}
				if (speechManager.displayForever || dialog.pauseGap)
				{
					return defaultPauseTime;
				}
				if (speechManager.separateLines)
				{
					return defaultPauseTime;
				}
				if (!dialog.HasPausing())
				{
					if (Time.time < endTime)
					{
						return defaultPauseTime;
					}
					isRunning = false;
					return 0f;
				}
			}
			return 0f;
		}

		public override void Skip()
		{
			GameObject.FindWithTag("GameEngine").GetComponent<Dialog>().KillDialog(true);
			if ((bool)speaker)
			{
				speaker.isTalking = false;
				if (speaker.animEngine == null)
				{
					speaker.ResetAnimationEngine();
				}
				if (speaker.animEngine != null)
				{
					speaker.animEngine.ActionSpeechSkip(this);
				}
			}
		}

		private float StartSpeech()
		{
			string text = messageText;
			string language = string.Empty;
			if (Options.GetLanguage() > 0)
			{
				text = SpeechManager.GetTranslation(lineID, Options.GetLanguage());
				language = speechManager.languages[Options.GetLanguage()];
			}
			bool flag = false;
			bool flag2 = false;
			if (speechManager.separateLines)
			{
				string[] array = text.Split('\n');
				text = array[splitNumber];
				if (array.Length > 1)
				{
					flag = true;
					if (array.Length == splitNumber + 1)
					{
						flag2 = true;
					}
				}
			}
			if (text != string.Empty)
			{
				dialog.KillDialog(true);
				float num = dialog.StartDialog(speaker, text, lineID, language, isBackground);
				if ((bool)speaker)
				{
					if (speaker.animEngine == null)
					{
						speaker.ResetAnimationEngine();
					}
					if (speaker.animEngine != null)
					{
						speaker.animEngine.ActionSpeechRun(this);
					}
				}
				if (flag2)
				{
					return num + waitTimeOffset;
				}
				if (flag)
				{
					return num;
				}
				if (!isBackground)
				{
					return num + waitTimeOffset;
				}
			}
			return 0f;
		}
	}
}
