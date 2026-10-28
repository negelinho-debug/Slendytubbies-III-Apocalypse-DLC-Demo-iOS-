namespace AC
{
	public class RememberConversation : ConstantID
	{
		public ConversationData SaveData()
		{
			ConversationData conversationData = new ConversationData();
			conversationData.objectID = constantID;
			if ((bool)GetComponent<Conversation>())
			{
				conversationData.optionStates = GetComponent<Conversation>().GetOptionStates();
				conversationData.optionLocks = GetComponent<Conversation>().GetOptionLocks();
			}
			return conversationData;
		}

		public void LoadData(ConversationData data)
		{
			if ((bool)GetComponent<Conversation>())
			{
				GetComponent<Conversation>().SetOptionStates(data.optionStates);
				GetComponent<Conversation>().SetOptionLocks(data.optionLocks);
			}
		}
	}
}
