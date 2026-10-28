using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionCharFaceDirection : Action
	{
		public int charToMoveParameterID = -1;

		public int charToMoveID;

		public bool isInstant;

		public CharDirection direction;

		public Char charToMove;

		public bool isPlayer;

		public ActionCharFaceDirection()
		{
			isDisplayed = true;
			title = "Character: Face direction";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			charToMove = AssignFile(parameters, charToMoveParameterID, charToMoveID, charToMove);
			if (isPlayer)
			{
				charToMove = KickStarter.player;
			}
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				if ((bool)charToMove)
				{
					charToMove.SetLookDirection(GetLookVector(), isInstant);
					if (!isInstant)
					{
						charToMove.Halt();
						if (willWait)
						{
							return defaultPauseTime;
						}
					}
				}
				return 0f;
			}
			if (charToMove.IsTurning())
			{
				return defaultPauseTime;
			}
			isRunning = false;
			return 0f;
		}

		public override void Skip()
		{
			if ((bool)charToMove)
			{
				charToMove.SetLookDirection(GetLookVector(), true);
			}
		}

		private Vector3 GetLookVector()
		{
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Camera.main.transform.up;
			Vector3 vector3 = Camera.main.transform.right - new Vector3(0f, 0.01f);
			if (AdvGame.GetReferences().settingsManager.IsTopDown())
			{
				vector2 = -Camera.main.transform.forward;
			}
			if (direction == CharDirection.Down)
			{
				vector = -vector2;
			}
			else if (direction == CharDirection.Left)
			{
				vector = -vector3;
			}
			else if (direction == CharDirection.Right)
			{
				vector = vector3;
			}
			else if (direction == CharDirection.Up)
			{
				vector = vector2;
			}
			else if (direction == CharDirection.DownLeft)
			{
				vector = (-vector2 - vector3).normalized;
			}
			else if (direction == CharDirection.DownRight)
			{
				vector = (-vector2 + vector3).normalized;
			}
			else if (direction == CharDirection.UpLeft)
			{
				vector = (vector2 - vector3).normalized;
			}
			else if (direction == CharDirection.UpRight)
			{
				vector = (vector2 + vector3).normalized;
			}
			return new Vector3(vector.x, 0f, vector.y);
		}
	}
}
