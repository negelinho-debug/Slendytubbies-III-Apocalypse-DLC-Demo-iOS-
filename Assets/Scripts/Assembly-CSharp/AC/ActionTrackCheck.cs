using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionTrackCheck : ActionCheck
	{
		public Moveable_Drag dragObject;

		public float checkPosition;

		public float errorMargin = 0.05f;

		public IntCondition condition;

		public ActionTrackCheck()
		{
			isDisplayed = true;
			title = "Moveable: Check track position";
		}

		public override int End(List<Action> actions)
		{
			return ProcessResult(CheckCondition(), actions);
		}

		public override bool CheckCondition()
		{
			float positionAlong = dragObject.GetPositionAlong();
			if (condition == IntCondition.EqualTo)
			{
				if (positionAlong > checkPosition - errorMargin || positionAlong < checkPosition + errorMargin)
				{
					return true;
				}
			}
			else if (condition == IntCondition.NotEqualTo)
			{
				if (positionAlong < checkPosition - errorMargin || positionAlong > checkPosition + errorMargin)
				{
					return true;
				}
			}
			else if (condition == IntCondition.LessThan)
			{
				if (positionAlong < checkPosition)
				{
					return true;
				}
			}
			else if (condition == IntCondition.MoreThan && positionAlong > checkPosition)
			{
				return true;
			}
			return false;
		}
	}
}
