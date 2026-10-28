using System;

[Serializable]
public class ActionType
{
	public string fileName;

	public string title;

	public bool isEnabled;

	public ActionType(string _fileName, string _title)
	{
		fileName = _fileName;
		title = _title;
		isEnabled = true;
	}
}
