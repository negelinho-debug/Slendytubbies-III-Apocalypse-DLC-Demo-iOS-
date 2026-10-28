using System;

namespace AC
{
	[Serializable]
	public class NodeCommand
	{
		public Cutscene cutscene;

		public ActionListAsset actionListAsset;

		public int parameterID;

		public NodeCommand()
		{
			cutscene = null;
			actionListAsset = null;
			parameterID = -1;
		}
	}
}
