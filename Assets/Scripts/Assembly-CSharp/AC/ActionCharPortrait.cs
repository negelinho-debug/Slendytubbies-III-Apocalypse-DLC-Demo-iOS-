using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionCharPortrait : Action
	{
		public int parameterID = -1;

		public int constantID;

		public bool isPlayer;

		public Char _char;

		public Texture2D newPortraitGraphic;

		public ActionCharPortrait()
		{
			isDisplayed = true;
			title = "Character: Switch Portrait";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			_char = AssignFile(parameters, parameterID, constantID, _char);
			if (isPlayer)
			{
				_char = KickStarter.player;
			}
		}

		public override float Run()
		{
			if ((bool)_char)
			{
				_char.portraitIcon.texture = newPortraitGraphic;
			}
			return 0f;
		}
	}
}
