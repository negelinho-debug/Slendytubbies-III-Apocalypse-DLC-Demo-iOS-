using System;

[Serializable]
public class InvBin
{
	public string label;

	public int id;

	public InvBin()
	{
		label = string.Empty;
		id = 0;
	}

	public InvBin(int[] idArray)
	{
		id = 0;
		foreach (int num in idArray)
		{
			if (id == num)
			{
				id++;
			}
		}
		label = "Category " + (id + 1);
	}
}
