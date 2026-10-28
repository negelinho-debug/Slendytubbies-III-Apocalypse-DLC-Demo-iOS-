using System.Collections.Generic;

namespace AC
{
	public class RememberShapeable : ConstantID
	{
		public ShapeableData SaveData()
		{
			ShapeableData shapeableData = new ShapeableData();
			shapeableData.objectID = constantID;
			if ((bool)GetComponent<Shapeable>())
			{
				Shapeable component = GetComponent<Shapeable>();
				shapeableData.activeKeyIDs = new List<int>();
				shapeableData.values = new List<float>();
				foreach (ShapeGroup shapeGroup in component.shapeGroups)
				{
					shapeableData.activeKeyIDs.Add(shapeGroup.GetActiveKeyID());
					shapeableData.values.Add(shapeGroup.GetActiveKeyValue());
				}
			}
			return shapeableData;
		}

		public void LoadData(ShapeableData data)
		{
			if ((bool)GetComponent<Shapeable>())
			{
				Shapeable component = GetComponent<Shapeable>();
				for (int i = 0; i < data.activeKeyIDs.Count; i++)
				{
					component.shapeGroups[i].SetActive(data.activeKeyIDs[i], data.values[i], 0f, MoveMethod.Linear, null);
				}
			}
		}
	}
}
