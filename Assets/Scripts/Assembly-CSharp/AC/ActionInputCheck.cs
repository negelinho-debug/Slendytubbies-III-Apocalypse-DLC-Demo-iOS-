using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionInputCheck : ActionCheck
	{
		public string inputName;

		public int parameterID = -1;

		public InputCheckType checkType;

		public IntCondition axisCondition;

		public float axisValue;

		public ActionInputCheck()
		{
			isDisplayed = true;
			title = "Input: Check";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			inputName = AssignString(parameters, parameterID, inputName);
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				return defaultPauseTime;
			}
			isRunning = false;
			return 0f;
		}

		public override bool CheckCondition()
		{
			if (inputName != string.Empty)
			{
				if (checkType == InputCheckType.Button && Input.GetButton(inputName))
				{
					return true;
				}
				if (checkType == InputCheckType.Axis)
				{
					return CheckAxisValue(Input.GetAxis(inputName));
				}
			}
			return false;
		}

		private bool CheckAxisValue(float fieldValue)
		{
			if (axisCondition == IntCondition.EqualTo)
			{
				if (fieldValue == axisValue)
				{
					return true;
				}
			}
			else if (axisCondition == IntCondition.NotEqualTo)
			{
				if (fieldValue != axisValue)
				{
					return true;
				}
			}
			else if (axisCondition == IntCondition.LessThan)
			{
				if (fieldValue < axisValue)
				{
					return true;
				}
			}
			else if (axisCondition == IntCondition.MoreThan && fieldValue > axisValue)
			{
				return true;
			}
			return false;
		}
	}
}
