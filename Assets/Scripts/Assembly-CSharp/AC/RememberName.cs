namespace AC
{
	public class RememberName : ConstantID
	{
		public NameData SaveData()
		{
			NameData nameData = new NameData();
			nameData.objectID = constantID;
			nameData.newName = gameObject.name;
			return nameData;
		}

		public void LoadData(NameData data)
		{
			gameObject.name = data.newName;
		}
	}
}
