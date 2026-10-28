using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionTransform : Action
	{
		public enum ToBy
		{
			To,
			By
		}

		public Marker marker;

		public int markerParameterID = -1;

		public int markerID;

		public bool doEulerRotation;

		public AnimationCurve timeCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		public int parameterID = -1;

		public int constantID;

		public Moveable linkedProp;

		public float transitionTime;

		public Vector3 newVector;

		public TransformType transformType;

		public MoveMethod moveMethod;

		public ToBy toBy;

		public ActionTransform()
		{
			isDisplayed = true;
			title = "Object: Transform";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			linkedProp = AssignFile(parameters, parameterID, constantID, linkedProp);
			marker = AssignFile(parameters, markerParameterID, markerID, marker);
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				if ((bool)linkedProp)
				{
					RunToTime(transitionTime);
					if (willWait && transitionTime > 0f)
					{
						return defaultPauseTime;
					}
				}
			}
			else if ((bool)linkedProp)
			{
				if (linkedProp.isMoving)
				{
					return defaultPauseTime;
				}
				isRunning = false;
			}
			return 0f;
		}

		public override void Skip()
		{
			if ((bool)linkedProp)
			{
				RunToTime(0f);
			}
		}

		private void RunToTime(float _time)
		{
			if (transformType == TransformType.CopyMarker)
			{
				if ((bool)marker)
				{
					linkedProp.Move(marker, moveMethod, _time, timeCurve);
				}
				return;
			}
			Vector3 localEulerAngles = newVector;
			if (transformType == TransformType.Translate)
			{
				if (toBy == ToBy.By)
				{
					localEulerAngles += linkedProp.transform.localPosition;
				}
			}
			else if (transformType == TransformType.Rotate)
			{
				if (toBy == ToBy.By)
				{
					Quaternion localRotation = linkedProp.transform.localRotation;
					linkedProp.transform.Rotate(localEulerAngles, Space.World);
					localEulerAngles = linkedProp.transform.localEulerAngles;
					linkedProp.transform.localRotation = localRotation;
				}
			}
			else if (transformType == TransformType.Scale && toBy == ToBy.By)
			{
				localEulerAngles += linkedProp.transform.localScale;
			}
			if (transformType == TransformType.Rotate)
			{
				if (toBy == ToBy.By)
				{
					linkedProp.Move(localEulerAngles, moveMethod, _time, transformType, !doEulerRotation, timeCurve);
				}
				else
				{
					linkedProp.Move(localEulerAngles, moveMethod, _time, transformType, doEulerRotation, timeCurve);
				}
			}
			else
			{
				linkedProp.Move(localEulerAngles, moveMethod, _time, transformType, false, timeCurve);
			}
		}
	}
}
