using System;

namespace AC
{
	[Serializable]
	public class CursorIcon : CursorIconBase
	{
		public string label;

		public int lineID = -1;

		public int id;

		public CursorIcon()
		{
			texture = null;
			id = 0;
			lineID = -1;
			isAnimated = false;
			numFrames = 1;
			size = 0.04f;
			label = "Icon " + (id + 1);
		}

		public CursorIcon(int[] idArray)
		{
			texture = null;
			id = 0;
			lineID = -1;
			isAnimated = false;
			numFrames = 1;
			foreach (int num in idArray)
			{
				if (id == num)
				{
					id++;
				}
			}
			label = "Icon " + (id + 1);
		}

		public void Copy(CursorIcon _cursorIcon)
		{
			label = _cursorIcon.label;
			lineID = _cursorIcon.lineID;
			id = _cursorIcon.id;
			Copy((CursorIconBase)_cursorIcon);
		}
	}
}
