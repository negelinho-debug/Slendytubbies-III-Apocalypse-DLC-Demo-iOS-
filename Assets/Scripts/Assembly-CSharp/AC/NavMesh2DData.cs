using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class NavMesh2DData
	{
		public int objectID;

		public List<int> linkedIDs = new List<int>();
	}
}
