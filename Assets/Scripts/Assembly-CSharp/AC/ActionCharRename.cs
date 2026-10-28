using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionCharRename : Action
	{
		public int _charID;

		public Char _char;

		public bool isPlayer;

		public string newName;

		public ActionCharRename()
		{
			isDisplayed = true;
			title = "Character: Rename";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			_char = AssignFile(_charID, _char);
			if (isPlayer)
			{
				_char = KickStarter.player;
			}
		}

		public override float Run()
		{
			if ((bool)_char && newName != string.Empty)
			{
				_char.speechLabel = newName;
			}
			return 0f;
		}
	}
}
