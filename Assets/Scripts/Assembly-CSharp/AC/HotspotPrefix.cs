using System;

namespace AC
{
	[Serializable]
	public class HotspotPrefix
	{
		public string label;

		public int lineID;

		public HotspotPrefix(string text)
		{
			label = text;
			lineID = -1;
		}
	}
}
