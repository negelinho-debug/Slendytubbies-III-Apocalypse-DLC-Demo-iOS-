using System.Collections.Generic;
using AC;
using UnityEngine;

public class AnimEngine_SpritesUnityComplex : AnimEngine
{
	public override void Declare(Char _character)
	{
		character = _character;
		turningIsLinear = true;
		rootMotion = false;
		isSpriteBased = true;
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
			if (action.parameterName != string.Empty)
			{
				if (action.mecanimCharParameter == MecanimCharParameter.MoveSpeedFloat)
				{
					action.animChar.moveSpeedParameter = action.parameterName;
				}
				else if (action.mecanimCharParameter == MecanimCharParameter.TalkBool)
				{
					action.animChar.talkParameter = action.parameterName;
				}
				else if (action.mecanimCharParameter == MecanimCharParameter.TurnFloat)
				{
					action.animChar.turnParameter = action.parameterName;
				}
			}
			if (action.mecanimCharParameter == MecanimCharParameter.MoveSpeedFloat && action.changeSound)
			{
				if (action.standard == AnimStandard.Walk)
				{
					action.animChar.walkSound = action.newSound;
				}
				else if (action.standard == AnimStandard.Run)
				{
					action.animChar.runSound = action.newSound;
				}
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
				animator.CrossFade(action.clip2D, action.fadeTime, action.layerInt);
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
		}
		return text;
	}

	public override void ActionAnimAssignValues(ActionAnim action, List<ActionParameter> parameters)
	{
		action.animator = action.AssignFile(parameters, action.parameterID, action.constantID, action.animator);
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
			if (action.methodMecanim == AnimMethodMecanim.PlayCustom && (bool)action.animator && action.clip2D != string.Empty)
			{
				action.animator.CrossFade(action.clip2D, action.fadeTime, action.layerInt);
				if (action.willWait)
				{
					return action.defaultPauseTime;
				}
			}
		}
		else if (action.methodMecanim == AnimMethodMecanim.PlayCustom && (bool)action.animator && action.clip2D != string.Empty)
		{
			if (action.animator.GetCurrentAnimatorStateInfo(action.layerInt).normalizedTime < 1f)
			{
				return action.defaultPauseTime / 6f;
			}
			action.isRunning = false;
			return 0f;
		}
		return 0f;
	}

	public override void ActionAnimSkip(ActionAnim action)
	{
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
		}
		else if (action.methodMecanim == AnimMethodMecanim.PlayCustom && (bool)action.animator && action.clip2D != string.Empty)
		{
			action.animator.CrossFade(action.clip2D, action.fadeTime, action.layerInt);
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
			action._char.spriteScale = (float)action.scale / 100f;
		}
		else if (action.renderLock_scale == RenderLock.Release)
		{
			action._char.lockScale = false;
		}
		if (action.renderLock_direction == RenderLock.Set)
		{
			action._char.lockDirection = true;
			action._char.SetSpriteDirection(action.direction);
		}
		else if (action.renderLock_direction == RenderLock.Release)
		{
			action._char.lockDirection = false;
		}
		return 0f;
	}

	public override void PlayIdle()
	{
		Animator animator = GetAnimator();
		if (!(animator == null))
		{
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
			SetDirection(animator);
		}
	}

	private void SetDirection(Animator animator)
	{
		if (character.directionParameter != string.Empty)
		{
			animator.SetInteger(character.directionParameter, character.GetSpriteDirectionInt());
		}
		if (character.angleParameter != string.Empty)
		{
			animator.SetFloat(character.angleParameter, character.GetSpriteAngle());
		}
	}

	public override void PlayWalk()
	{
		Animator animator = GetAnimator();
		if (animator == null)
		{
			return;
		}
		if (character.moveSpeedParameter != string.Empty)
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
		SetDirection(animator);
	}

	public override void PlayRun()
	{
		Animator animator = GetAnimator();
		if (animator == null)
		{
			return;
		}
		if (character.moveSpeedParameter != string.Empty)
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
		SetDirection(animator);
	}

	public override void PlayTalk()
	{
		Animator animator = GetAnimator();
		if (!(animator == null))
		{
			if (character.LipSyncGameObject() && character.phonemeParameter != string.Empty)
			{
				animator.SetInteger(character.phonemeParameter, character.GetLipSyncFrame());
			}
			if (character.talkParameter != string.Empty)
			{
				animator.SetBool(character.talkParameter, true);
			}
			SetDirection(animator);
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
			SetDirection(animator);
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
			SetDirection(animator);
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
