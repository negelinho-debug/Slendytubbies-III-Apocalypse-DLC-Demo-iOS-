using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ConversationData
	{
		public int objectID;

		public List<bool> optionStates = new List<bool>();

		public List<bool> optionLocks = new List<bool>();
	}
}
