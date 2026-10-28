using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ContainerData
	{
		public int objectID;

		public List<int> linkedIDs = new List<int>();

		public List<int> counts = new List<int>();

		public List<int> IDs = new List<int>();
	}
}
