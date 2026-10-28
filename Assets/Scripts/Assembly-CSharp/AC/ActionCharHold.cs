using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionCharHold : Action
	{
		public enum Hand
		{
			Left,
			Right
		}

		public int objectToHoldParameterID = -1;

		public int _charID;

		public int objectToHoldID;

		public GameObject objectToHold;

		public bool isPlayer;

		public Char _char;

		public bool rotate90;

		public Hand hand;

		public ActionCharHold()
		{
			isDisplayed = true;
			title = "Character: Hold object";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			_char = AssignFile(_charID, _char);
			objectToHold = AssignFile(parameters, objectToHoldParameterID, objectToHoldID, objectToHold);
			if (isPlayer)
			{
				_char = KickStarter.player;
			}
		}

		public override float Run()
		{
			if ((bool)_char)
			{
				if (_char.animEngine == null)
				{
					_char.ResetAnimationEngine();
				}
				if (_char.animEngine != null)
				{
					_char.animEngine.ActionCharHoldRun(this);
				}
			}
			else
			{
				Debug.LogWarning("Could not create animation engine!");
			}
			return 0f;
		}
	}
}
