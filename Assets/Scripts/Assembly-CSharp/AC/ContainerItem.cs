using System;

namespace AC
{
	[Serializable]
	public class ContainerItem
	{
		public int linkedID;

		public int count;

		public int id;

		public ContainerItem(int _linkedID, int[] idArray)
		{
			count = 1;
			linkedID = _linkedID;
			id = 0;
			foreach (int num in idArray)
			{
				if (id == num)
				{
					id++;
				}
			}
		}

		public ContainerItem(int _linkedID, int _count, int[] idArray)
		{
			count = _count;
			linkedID = _linkedID;
			id = 0;
			foreach (int num in idArray)
			{
				if (id == num)
				{
					id++;
				}
			}
		}

		public ContainerItem(int _linkedID, int _count, int _id)
		{
			linkedID = _linkedID;
			count = _count;
			id = _id;
		}
	}
}
