using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ButtonDialog
	{
		public string label = "(Not set)";

		public Texture2D icon;

		public bool isOn;

		public bool isLocked;

		public ConversationAction conversationAction;

		public Conversation newConversation;

		public int lineID = -1;

		public int ID;

		public bool isEditing;

		public DialogueOption dialogueOption;

		public ActionListAsset assetFile;

		public ButtonDialog(int[] idArray)
		{
			label = string.Empty;
			icon = null;
			isOn = true;
			isLocked = false;
			conversationAction = ConversationAction.ReturnToConversation;
			assetFile = null;
			newConversation = null;
			dialogueOption = null;
			lineID = -1;
			ID = 1;
			isEditing = false;
			foreach (int num in idArray)
			{
				if (ID == num)
				{
					ID++;
				}
			}
		}
	}
}
