using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionCharAnim : Action
	{
		public enum AnimMethodChar
		{
			PlayCustom,
			StopCustom,
			ResetToIdle,
			SetStandard
		}

		public int parameterID = -1;

		public int constantID;

		public AnimEngine editingAnimEngine;

		public bool isPlayer;

		public Char animChar;

		public AnimationClip clip;

		public string clip2D;

		public AnimMethodChar method;

		public AnimationBlendMode blendMode;

		public AnimLayer layer;

		public AnimStandard standard;

		public bool includeDirection;

		public bool changeSound;

		public AudioClip newSound;

		public int layerInt;

		public bool idleAfter = true;

		public bool idleAfterCustom;

		public AnimPlayMode playMode;

		public AnimPlayModeBase playModeBase = AnimPlayModeBase.PlayOnceAndClamp;

		public float fadeTime;

		public bool changeSpeed;

		public float newSpeed;

		public AnimMethodCharMecanim methodMecanim;

		public MecanimCharParameter mecanimCharParameter;

		public MecanimParameterType mecanimParameterType;

		public string parameterName;

		public float parameterValue;

		public ActionCharAnim()
		{
			isDisplayed = true;
			title = "Character: Animate";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			animChar = AssignFile(parameters, parameterID, constantID, animChar);
			if (isPlayer)
			{
				animChar = KickStarter.player;
			}
			if ((bool)animChar && animChar.animEngine == null)
			{
				animChar.ResetAnimationEngine();
			}
		}

		public override float Run()
		{
			if ((bool)animChar)
			{
				if (animChar.animEngine == null)
				{
					animChar.ResetAnimationEngine();
				}
				if (animChar.animEngine != null)
				{
					return animChar.animEngine.ActionCharAnimRun(this);
				}
				Debug.LogWarning("Could not create animation engine for " + animChar.name);
			}
			else
			{
				Debug.LogWarning("Could not create animation engine!");
			}
			return 0f;
		}

		public override void Skip()
		{
			if ((bool)animChar && animChar.animEngine != null)
			{
				animChar.animEngine.ActionCharAnimSkip(this);
			}
		}
	}
}
