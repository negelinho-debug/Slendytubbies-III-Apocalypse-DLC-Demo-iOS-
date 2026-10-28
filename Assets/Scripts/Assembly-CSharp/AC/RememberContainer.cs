using System.Collections.Generic;

namespace AC
{
	public class RememberContainer : ConstantID
	{
		public ContainerData SaveData()
		{
			ContainerData containerData = new ContainerData();
			containerData.objectID = constantID;
			if ((bool)GetComponent<Container>())
			{
				Container component = GetComponent<Container>();
				containerData.linkedIDs = new List<int>();
				containerData.counts = new List<int>();
				containerData.IDs = new List<int>();
				for (int i = 0; i < component.items.Count; i++)
				{
					containerData.linkedIDs.Add(component.items[i].linkedID);
					containerData.counts.Add(component.items[i].count);
					containerData.IDs.Add(component.items[i].id);
				}
			}
			return containerData;
		}

		public void LoadData(ContainerData data)
		{
			if ((bool)GetComponent<Container>())
			{
				Container component = GetComponent<Container>();
				component.items.Clear();
				for (int i = 0; i < data.IDs.Count; i++)
				{
					ContainerItem item = new ContainerItem(data.linkedIDs[i], data.counts[i], data.IDs[i]);
					component.items.Add(item);
				}
			}
		}
	}
}
