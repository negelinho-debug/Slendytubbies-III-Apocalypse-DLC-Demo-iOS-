using UnityEngine;

namespace AC
{
	public struct SaveFile
	{
		public int ID;

		public string label;

		public Texture2D screenShot;

		public SaveFile(int _ID, string _label, Texture2D _screenShot)
		{
			ID = _ID;
			label = _label;
			screenShot = _screenShot;
		}
	}
}
