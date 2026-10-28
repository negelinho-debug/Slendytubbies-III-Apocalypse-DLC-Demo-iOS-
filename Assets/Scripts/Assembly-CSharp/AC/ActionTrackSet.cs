using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionTrackSet : Action
	{
		public Moveable_Drag dragObject;

		public int dragParameterID = -1;

		public int dragConstantID;

		public float positionAlong;

		public int positionParameterID = -1;

		public float speed = 200f;

		public bool removePlayerControl;

		public bool isInstant;

		public ActionTrackSet()
		{
			isDisplayed = true;
			title = "Moveable: Set track position";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			dragObject = AssignFile(parameters, dragParameterID, dragConstantID, dragObject);
			positionAlong = AssignFloat(parameters, positionParameterID, positionAlong);
			positionAlong = Mathf.Max(0f, positionAlong);
			positionAlong = Mathf.Min(1f, positionAlong);
		}

		public override float Run()
		{
			if (dragObject == null)
			{
				isRunning = false;
				return 0f;
			}
			if (!isRunning)
			{
				isRunning = true;
				if (isInstant)
				{
					dragObject.AutoMoveAlongTrack(positionAlong, 0f, removePlayerControl);
				}
				else
				{
					dragObject.AutoMoveAlongTrack(positionAlong, speed, removePlayerControl);
				}
				if (willWait && !isInstant && speed > 0f)
				{
					return defaultPauseTime;
				}
				isRunning = false;
				return 0f;
			}
			if (dragObject.IsAutoMoving())
			{
				return defaultPauseTime;
			}
			isRunning = false;
			return 0f;
		}

		public override void Skip()
		{
			if (!(dragObject == null))
			{
				dragObject.AutoMoveAlongTrack(positionAlong, 0f, removePlayerControl);
			}
		}
	}
}
