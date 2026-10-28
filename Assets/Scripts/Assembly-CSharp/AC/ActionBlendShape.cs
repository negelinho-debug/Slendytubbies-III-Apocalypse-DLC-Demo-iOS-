using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionBlendShape : Action
	{
		public int parameterID = -1;

		public int constantID;

		public Shapeable shapeObject;

		public int shapeGroupID;

		public int shapeKeyID;

		public float shapeValue;

		public bool isPlayer;

		public bool disableAllKeys;

		public float fadeTime;

		public MoveMethod moveMethod = MoveMethod.Smooth;

		public AnimationCurve timeCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		public ActionBlendShape()
		{
			isDisplayed = true;
			title = "Object: Blend shape";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			shapeObject = AssignFile(parameters, parameterID, constantID, shapeObject);
			if (isPlayer)
			{
				if ((bool)KickStarter.player && (bool)KickStarter.player.GetShapeable())
				{
					shapeObject = KickStarter.player.GetShapeable();
					return;
				}
				shapeObject = null;
				Debug.LogWarning("Cannot BlendShape Player since cannot find Shapeable script on Player.");
			}
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				if ((bool)shapeObject)
				{
					DoShape(fadeTime);
					if (willWait)
					{
						return fadeTime;
					}
				}
				return 0f;
			}
			isRunning = false;
			return 0f;
		}

		public override void Skip()
		{
			DoShape(0f);
		}

		private void DoShape(float _time)
		{
			if ((bool)shapeObject)
			{
				if (disableAllKeys)
				{
					shapeObject.DisableAllKeys(shapeGroupID, _time, moveMethod, timeCurve);
				}
				else
				{
					shapeObject.SetActiveKey(shapeGroupID, shapeKeyID, shapeValue, _time, moveMethod, timeCurve);
				}
			}
		}
	}
}
