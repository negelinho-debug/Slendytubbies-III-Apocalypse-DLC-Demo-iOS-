using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class Button
	{
		public Interaction interaction;

		public ActionListAsset assetFile;

		public GameObject customScriptObject;

		public string customScriptFunction = string.Empty;

		public bool isDisabled;

		public int invID;

		public int iconID = -1;

		public PlayerAction playerAction;

		public bool setProximity;

		public float proximity = 1f;

		public bool faceAfter;

		public bool isBlocking;

		public bool IsButtonModified()
		{
			if (interaction != null || assetFile != null || customScriptObject != null || customScriptFunction != string.Empty || isDisabled || playerAction != PlayerAction.DoNothing || setProximity || proximity != 1f || faceAfter || isBlocking)
			{
				return true;
			}
			return false;
		}

		public void CopyButton(Button _button)
		{
			interaction = _button.interaction;
			assetFile = _button.assetFile;
			customScriptObject = _button.customScriptObject;
			customScriptFunction = _button.customScriptFunction;
			isDisabled = _button.isDisabled;
			invID = _button.invID;
			iconID = _button.iconID;
			playerAction = _button.playerAction;
			setProximity = _button.setProximity;
			proximity = _button.proximity;
			faceAfter = _button.faceAfter;
			isBlocking = _button.isBlocking;
		}
	}
}
