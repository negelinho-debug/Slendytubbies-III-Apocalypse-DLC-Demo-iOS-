using System.Collections.Generic;
using AC;
using UnityEngine;

public class AnimEngine_Mecanim : AnimEngine
{
	public override void Declare(Char _character)
	{
		character = _character;
		turningIsLinear = false;
		rootMotion = true;
	}

	public override void CharSettingsGUI()
	{
	}

	public override void ActionCharAnimGUI(ActionCharAnim action)
	{
	}

	public override float ActionCharAnimRun(ActionCharAnim action)
	{
		if (action.methodMecanim == AnimMethodCharMecanim.SetStandard)
		{
			if (action.mecanimCharParameter == MecanimCharParameter.MoveSpeedFloat)
			{
				action.animChar.moveSpeedParameter = action.parameterName;
				if (action.changeSpeed)
				{
					character.walkSpeedScale = action.newSpeed;
					character.runSpeedScale = action.parameterValue;
				}
			}
			else if (action.mecanimCharParameter == MecanimCharParameter.TalkBool)
			{
				action.animChar.talkParameter = action.parameterName;
			}
			else if (action.mecanimCharParameter == MecanimCharParameter.TurnFloat)
			{
				action.animChar.turnParameter = action.parameterName;
			}
			return 0f;
		}
		Animator animator = null;
		if ((bool)character.spriteChild && (bool)character.spriteChild.GetComponent<Animator>())
		{
			animator = character.spriteChild.GetComponent<Animator>();
		}
		if ((bool)character.GetComponent<Animator>())
		{
			animator = character.GetComponent<Animator>();
		}
		if (animator == null)
		{
			return 0f;
		}
		if (!action.isRunning)
		{
			action.isRunning = true;
			if (action.methodMecanim == AnimMethodCharMecanim.ChangeParameterValue)
			{
				if (action.parameterName != string.Empty)
				{
					if (action.mecanimParameterType == MecanimParameterType.Float)
					{
						animator.SetFloat(action.parameterName, action.parameterValue);
					}
					else if (action.mecanimParameterType == MecanimParameterType.Int)
					{
						animator.SetInteger(action.parameterName, (int)action.parameterValue);
					}
					else if (action.mecanimParameterType == MecanimParameterType.Bool)
					{
						bool value = false;
						if (action.parameterValue > 0f)
						{
							value = true;
						}
						animator.SetBool(action.parameterName, value);
					}
					else if (action.mecanimParameterType == MecanimParameterType.Trigger)
					{
						animator.SetTrigger(action.parameterName);
					}
				}
			}
			else if (action.methodMecanim == AnimMethodCharMecanim.PlayCustom && (bool)animator && action.clip2D != string.Empty)
			{
				string text = action.clip2D;
				if (action.includeDirection)
				{
					text += action.animChar.GetSpriteDirection();
				}
				animator.CrossFade(text, action.fadeTime, action.layerInt);
				if (action.willWait)
				{
					return action.defaultPauseTime;
				}
			}
		}
		else if (action.methodMecanim == AnimMethodCharMecanim.PlayCustom && (bool)animator && action.clip2D != string.Empty)
		{
			if (animator.GetCurrentAnimatorStateInfo(action.layerInt).normalizedTime < 0.98f)
			{
				return action.defaultPauseTime / 6f;
			}
			action.isRunning = false;
			return 0f;
		}
		return 0f;
	}

	public override void ActionCharAnimSkip(ActionCharAnim action)
	{
		if (action.methodMecanim != AnimMethodCharMecanim.ChangeParameterValue)
		{
			ActionCharAnimRun(action);
		}
	}

	public override void ActionCharHoldGUI(ActionCharHold action, List<ActionParameter> parameters)
	{
	}

	public override void ActionCharHoldRun(ActionCharHold action)
	{
		if (!action.objectToHold)
		{
			return;
		}
		Transform transform = ((action.hand != ActionCharHold.Hand.Left) ? action._char.rightHandBone : action._char.leftHandBone);
		if ((bool)transform)
		{
			action.objectToHold.transform.parent = transform;
			action.objectToHold.transform.localPosition = Vector3.zero;
			if (action.rotate90)
			{
				action.objectToHold.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
			}
			else
			{
				action.objectToHold.transform.localEulerAngles = Vector3.zero;
			}
		}
		else
		{
			Debug.Log("Cannot parent object - no hand bone found.");
		}
	}

	public override void ActionAnimGUI(ActionAnim action, List<ActionParameter> parameters)
	{
	}

	public override string ActionAnimLabel(ActionAnim action)
	{
		string text = string.Empty;
		if ((bool)action.animator)
		{
			text = action.animator.name;
			if (action.methodMecanim == AnimMethodMecanim.ChangeParameterValue && action.parameterName != string.Empty)
			{
				text = text + " - " + action.parameterName;
			}
			else if (action.methodMecanim == AnimMethodMecanim.BlendShape)
			{
				text += " - Shapekey";
			}
		}
		return text;
	}

	public override void ActionAnimAssignValues(ActionAnim action, List<ActionParameter> parameters)
	{
		action.animator = action.AssignFile(parameters, action.parameterID, action.constantID, action.animator);
		action.shapeObject = action.AssignFile(parameters, action.parameterID, action.constantID, action.shapeObject);
	}

	public override float ActionAnimRun(ActionAnim action)
	{
		if (!action.isRunning)
		{
			action.isRunning = true;
			if (action.methodMecanim == AnimMethodMecanim.ChangeParameterValue && (bool)action.animator && action.parameterName != string.Empty)
			{
				if (action.mecanimParameterType == MecanimParameterType.Float)
				{
					action.animator.SetFloat(action.parameterName, action.parameterValue);
				}
				else if (action.mecanimParameterType == MecanimParameterType.Int)
				{
					action.animator.SetInteger(action.parameterName, (int)action.parameterValue);
				}
				else if (action.mecanimParameterType == MecanimParameterType.Bool)
				{
					bool value = false;
					if (action.parameterValue > 0f)
					{
						value = true;
					}
					action.animator.SetBool(action.parameterName, value);
				}
				else if (action.mecanimParameterType == MecanimParameterType.Trigger)
				{
					action.animator.SetTrigger(action.parameterName);
				}
				return 0f;
			}
			if (action.methodMecanim == AnimMethodMecanim.PlayCustom && (bool)action.animator)
			{
				if (action.clip2D != string.Empty)
				{
					action.animator.CrossFade(action.clip2D, action.fadeTime, action.layerInt);
					if (action.willWait)
					{
						return action.defaultPauseTime;
					}
				}
			}
			else if (action.methodMecanim == AnimMethodMecanim.BlendShape && action.shapeKey > -1 && (bool)action.shapeObject)
			{
				action.shapeObject.Change(action.shapeKey, action.shapeValue, action.fadeTime);
				if (action.willWait)
				{
					return action.fadeTime;
				}
			}
		}
		else
		{
			if (action.methodMecanim == AnimMethodMecanim.BlendShape && (bool)action.shapeObject)
			{
				action.isRunning = false;
				return 0f;
			}
			if (action.methodMecanim == AnimMethodMecanim.PlayCustom && (bool)action.animator && action.clip2D != string.Empty)
			{
				if (action.animator.GetCurrentAnimatorStateInfo(action.layerInt).normalizedTime < 1f)
				{
					return action.defaultPauseTime / 6f;
				}
				action.isRunning = false;
				return 0f;
			}
		}
		return 0f;
	}

	public override void ActionAnimSkip(ActionAnim action)
	{
		if (action.methodMecanim == AnimMethodMecanim.BlendShape)
		{
			if ((bool)action.shapeObject)
			{
				action.shapeObject.Change(action.shapeKey, action.shapeValue, action.fadeTime);
			}
		}
		else if (action.methodMecanim == AnimMethodMecanim.PlayCustom)
		{
			ActionAnimRun(action);
		}
	}

	public override void ActionCharRenderGUI(ActionCharRender action)
	{
	}

	public override float ActionCharRenderRun(ActionCharRender action)
	{
		if (action.renderLock_scale == RenderLock.Set)
		{
			action._char.lockScale = true;
			if (action._char.spriteChild != null)
			{
				action._char.spriteScale = (float)action.scale / 100f;
			}
			else
			{
				float num = action.scale;
				action._char.transform.localScale = new Vector3(num, num, num);
			}
		}
		else if (action.renderLock_scale == RenderLock.Release)
		{
			action._char.lockScale = false;
		}
		return 0f;
	}

	public override void PlayIdle()
	{
		Animator animator = GetAnimator();
		if (animator == null)
		{
			return;
		}
		if (character.moveSpeedParameter != string.Empty)
		{
			animator.SetFloat(character.moveSpeedParameter, character.GetMoveSpeed());
		}
		if (character.talkParameter != string.Empty)
		{
			animator.SetBool(character.talkParameter, false);
		}
		if (character.turnParameter != string.Empty)
		{
			animator.SetFloat(character.turnParameter, 0f);
		}
		if (character is Player)
		{
			Player player = (Player)character;
			if (player.jumpParameter != string.Empty)
			{
				animator.SetBool(player.jumpParameter, false);
			}
		}
	}

	public override void PlayWalk()
	{
		Animator animator = null;
		if ((bool)character.spriteChild && (bool)character.spriteChild.GetComponent<Animator>())
		{
			animator = character.spriteChild.GetComponent<Animator>();
		}
		if ((bool)character.GetComponent<Animator>())
		{
			animator = character.GetComponent<Animator>();
		}
		if (!(animator == null) && character.moveSpeedParameter != string.Empty)
		{
			if (character.isReversing)
			{
				animator.SetFloat(character.moveSpeedParameter, 0f - character.GetMoveSpeed());
			}
			else
			{
				animator.SetFloat(character.moveSpeedParameter, character.GetMoveSpeed());
			}
		}
	}

	public override void PlayRun()
	{
		Animator animator = GetAnimator();
		if (!(animator == null) && character.moveSpeedParameter != string.Empty)
		{
			if (character.isReversing)
			{
				animator.SetFloat(character.moveSpeedParameter, 0f - character.GetMoveSpeed());
			}
			else
			{
				animator.SetFloat(character.moveSpeedParameter, character.GetMoveSpeed());
			}
		}
	}

	public override void PlayTalk()
	{
		Animator animator = GetAnimator();
		if (!(animator == null) && character.talkParameter != string.Empty)
		{
			animator.SetBool(character.talkParameter, true);
		}
	}

	public override void PlayVertical()
	{
		Animator animator = GetAnimator();
		if (!(animator == null) && character.verticalMovementParameter != string.Empty)
		{
			animator.SetFloat(character.verticalMovementParameter, character.heightChange);
		}
	}

	public override void PlayJump()
	{
		Animator animator = GetAnimator();
		if (!(animator == null) && character is Player)
		{
			Player player = (Player)character;
			if (player.jumpParameter != string.Empty)
			{
				animator.SetBool(player.jumpParameter, true);
			}
			if (character.talkParameter != string.Empty)
			{
				animator.SetBool(character.talkParameter, false);
			}
		}
	}

	public override void PlayTurnLeft()
	{
		Animator animator = GetAnimator();
		if (!(animator == null))
		{
			if (character.turnParameter != string.Empty)
			{
				animator.SetFloat(character.turnParameter, -1f);
			}
			if (character.talkParameter != string.Empty)
			{
				animator.SetBool(character.talkParameter, false);
			}
			if (character.moveSpeedParameter != string.Empty)
			{
				animator.SetFloat(character.moveSpeedParameter, 0f);
			}
		}
	}

	public override void PlayTurnRight()
	{
		Animator animator = GetAnimator();
		if (!(animator == null))
		{
			if (character.turnParameter != string.Empty)
			{
				animator.SetFloat(character.turnParameter, 1f);
			}
			if (character.talkParameter != string.Empty)
			{
				animator.SetBool(character.talkParameter, false);
			}
			if (character.moveSpeedParameter != string.Empty)
			{
				animator.SetFloat(character.moveSpeedParameter, 0f);
			}
		}
	}

	public override void TurnHead(Vector2 angles)
	{
		Animator animator = GetAnimator();
		if (!(animator == null))
		{
			if (character.headYawParameter != string.Empty)
			{
				animator.SetFloat(character.headYawParameter, angles.x);
			}
			if (character.headPitchParameter != string.Empty)
			{
				animator.SetFloat(character.headPitchParameter, angles.y);
			}
		}
	}

	private Animator GetAnimator()
	{
		if ((bool)character.spriteChild && (bool)character.spriteChild.GetComponent<Animator>())
		{
			return character.spriteChild.GetComponent<Animator>();
		}
		if ((bool)character.GetComponent<Animator>())
		{
			return character.GetComponent<Animator>();
		}
		return null;
	}
}
