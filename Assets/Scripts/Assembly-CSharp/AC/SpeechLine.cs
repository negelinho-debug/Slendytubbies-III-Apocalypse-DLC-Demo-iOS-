using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class SpeechLine
	{
		public int lineID;

		public string scene;

		public string owner;

		public string text;

		public AC_TextType textType;

		public List<string> translationText = new List<string>();

		private GUIStyle style;

		public SpeechLine()
		{
			lineID = 0;
			scene = string.Empty;
			owner = string.Empty;
			text = string.Empty;
			translationText = new List<string>();
			textType = AC_TextType.Speech;
		}

		public SpeechLine(int _id, string _scene, string _text, int _languagues, AC_TextType _textType)
		{
			lineID = _id;
			scene = _scene;
			owner = string.Empty;
			text = _text;
			textType = _textType;
			translationText = new List<string>();
			for (int i = 0; i < _languagues; i++)
			{
				translationText.Add(_text);
			}
		}

		public SpeechLine(int[] idArray, string _scene, string _text, int _languagues, AC_TextType _textType)
		{
			lineID = 0;
			foreach (int num in idArray)
			{
				if (lineID == num)
				{
					lineID++;
				}
			}
			scene = _scene;
			owner = string.Empty;
			text = _text;
			textType = _textType;
			translationText = new List<string>();
			for (int j = 0; j < _languagues; j++)
			{
				translationText.Add(_text);
			}
		}

		public SpeechLine(int _id, string _scene, string _owner, string _text, int _languagues, AC_TextType _textType)
		{
			lineID = _id;
			scene = _scene;
			owner = _owner;
			text = _text;
			textType = _textType;
			translationText = new List<string>();
			for (int i = 0; i < _languagues; i++)
			{
				translationText.Add(_text);
			}
		}

		public SpeechLine(int[] idArray, string _scene, string _owner, string _text, int _languagues, AC_TextType _textType)
		{
			lineID = 0;
			foreach (int num in idArray)
			{
				if (lineID == num)
				{
					lineID++;
				}
			}
			scene = _scene;
			owner = _owner;
			text = _text;
			textType = _textType;
			translationText = new List<string>();
			for (int j = 0; j < _languagues; j++)
			{
				translationText.Add(_text);
			}
		}
	}
}
