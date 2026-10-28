using System;

namespace AC
{
	[Serializable]
	public class ActionDialogOption : Action
	{
		public enum SwitchType
		{
			On,
			Off,
			OnForever,
			OffForever
		}

		public SwitchType switchType;

		public int optionNumber;

		public int constantID;

		public Conversation linkedConversation;

		public ActionDialogOption()
		{
			isDisplayed = true;
			title = "Dialogue: Toggle option";
		}

		public override void AssignValues()
		{
			linkedConversation = AssignFile(constantID, linkedConversation);
		}

		public override float Run()
		{
			bool flag = false;
			bool isLocked = false;
			if (switchType == SwitchType.On || switchType == SwitchType.OnForever)
			{
				flag = true;
			}
			if (switchType == SwitchType.OffForever || switchType == SwitchType.OnForever)
			{
				isLocked = true;
			}
			if ((bool)linkedConversation)
			{
				linkedConversation.SetOption(optionNumber + 1, flag, isLocked);
			}
			return 0f;
		}
	}
}
