using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionAnim : Action
	{
		public enum WrapMode2D
		{
			Once,
			Loop,
			PingPong
		}

		public int parameterID = -1;

		public int constantID;

		public Animation _anim;

		public AnimationClip clip;

		public float fadeTime;

		public Transform _anim2D;

		public Animator animator;

		public string clip2D;

		public WrapMode2D wrapMode2D;

		public int layerInt;

		public Shapeable shapeObject;

		public int shapeKey;

		public float shapeValue;

		public float Speed = 1f;

		public bool OnlySpeed;

		public bool isPlayer;

		public AnimMethodMecanim methodMecanim;

		public MecanimParameterType mecanimParameterType;

		public string parameterName;

		public float parameterValue;

		public AnimMethod method;

		public AnimationBlendMode blendMode;

		public AnimPlayMode playMode;

		public AnimationEngine animationEngine;

		public AnimEngine animEngine;

		public ActionAnim()
		{
			isDisplayed = true;
			title = "Object: Animate";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			if (animEngine == null)
			{
				ResetAnimationEngine();
			}
			if (animEngine != null)
			{
				animEngine.ActionAnimAssignValues(this, parameters);
			}
			if (method == AnimMethod.BlendShape && isPlayer)
			{
				if ((bool)KickStarter.player && (bool)KickStarter.player.GetComponent<Shapeable>())
				{
					shapeObject = KickStarter.player.GetComponent<Shapeable>();
					return;
				}
				shapeObject = null;
				Debug.LogWarning("Cannot BlendShape Player since cannot find Shapeable script on Player.");
			}
		}

		public override float Run()
		{
			if (_anim != null && OnlySpeed)
			{
				_anim[clip.name].speed = Speed;
				return 0f;
			}
			if (animEngine != null)
			{
				return animEngine.ActionAnimRun(this);
			}
			Debug.LogError("Could not create animation engine!");
			return 0f;
		}

		public override void Skip()
		{
			if (animEngine != null)
			{
				animEngine.ActionAnimSkip(this);
			}
		}

		private void ResetAnimationEngine()
		{
			string text = "AnimEngine_" + animationEngine;
			if (animEngine == null || animEngine.ToString() != text)
			{
				animEngine = (AnimEngine)ScriptableObject.CreateInstance(text);
			}
		}
	}
}
