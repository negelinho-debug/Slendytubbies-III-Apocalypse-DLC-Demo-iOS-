using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionConversation : Action
	{
		public int parameterID = -1;

		public int constantID;

		public Conversation conversation;

		public ActionConversation()
		{
			isDisplayed = true;
			title = "Dialogue: Start conversation";
			numSockets = 0;
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			conversation = AssignFile(parameters, parameterID, constantID, conversation);
		}

		public override float Run()
		{
			if ((bool)conversation)
			{
				conversation.Interact();
			}
			return 0f;
		}

		public override int End(List<Action> actions)
		{
			return -1;
		}
	}
}
