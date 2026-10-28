using System.Collections.Generic;
using AC;
using UnityEngine;

public class AnimEngine_Sprites2DToolkit : AnimEngine
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
		string text = action.clip2D;
		if (action.includeDirection)
		{
			text += action.animChar.GetSpriteDirection();
		}
		if (!action.isRunning)
		{
			action.isRunning = true;
			if (action.method == ActionCharAnim.AnimMethodChar.PlayCustom && action.clip2D != string.Empty)
			{
				action.animChar.charState = CharState.Custom;
				if (action.playMode == AnimPlayMode.Loop)
				{
					tk2DIntegration.PlayAnimation(action.animChar.spriteChild, text, true, WrapMode.Loop);
					action.willWait = false;
				}
				else
				{
					tk2DIntegration.PlayAnimation(action.animChar.spriteChild, text, true, WrapMode.Once);
				}
			}
			else if (action.method == ActionCharAnim.AnimMethodChar.ResetToIdle)
			{
				action.animChar.ResetBaseClips();
			}
			else if (action.method == ActionCharAnim.AnimMethodChar.SetStandard)
			{
				if (action.clip2D != string.Empty)
				{
					if (action.standard == AnimStandard.Idle)
					{
						action.animChar.idleAnimSprite = action.clip2D;
					}
					else if (action.standard == AnimStandard.Walk)
					{
						action.animChar.walkAnimSprite = action.clip2D;
					}
					else if (action.standard == AnimStandard.Talk)
					{
						action.animChar.talkAnimSprite = action.clip2D;
					}
					else if (action.standard == AnimStandard.Run)
					{
						action.animChar.runAnimSprite = action.clip2D;
					}
				}
				if (action.changeSpeed)
				{
					if (action.standard == AnimStandard.Walk)
					{
						action.animChar.walkSpeedScale = action.newSpeed;
					}
					else if (action.standard == AnimStandard.Run)
					{
						action.animChar.runSpeedScale = action.newSpeed;
					}
				}
				if (action.changeSound)
				{
					if (action.standard == AnimStandard.Walk)
					{
						if (action.newSound != null)
						{
							action.animChar.walkSound = action.newSound;
						}
						else
						{
							action.animChar.walkSound = null;
						}
					}
					else if (action.standard == AnimStandard.Run)
					{
						if (action.newSound != null)
						{
							action.animChar.runSound = action.newSound;
						}
						else
						{
							action.animChar.runSound = null;
						}
					}
				}
			}
			if (action.willWait && action.clip2D != string.Empty && action.method == ActionCharAnim.AnimMethodChar.PlayCustom)
			{
				return action.defaultPauseTime;
			}
		}
		else if ((bool)action.animChar.spriteChild && action.clip2D != string.Empty)
		{
			if (!tk2DIntegration.IsAnimationPlaying(action.animChar.spriteChild, action.clip2D))
			{
				action.isRunning = false;
				return 0f;
			}
			return action.defaultPauseTime / 6f;
		}
		return 0f;
	}

	public override void ActionCharAnimSkip(ActionCharAnim action)
	{
		string text = action.clip2D;
		if (action.includeDirection)
		{
			text += action.animChar.GetSpriteDirection();
		}
		if (action.method == ActionCharAnim.AnimMethodChar.PlayCustom && action.clip2D != string.Empty)
		{
			if (!action.willWait || action.playMode == AnimPlayMode.Loop)
			{
				action.animChar.charState = CharState.Custom;
				if (action.playMode == AnimPlayMode.Loop)
				{
					tk2DIntegration.PlayAnimation(action.animChar.spriteChild, text, true, WrapMode.Loop);
					action.willWait = false;
				}
				else
				{
					tk2DIntegration.PlayAnimation(action.animChar.spriteChild, text, true, WrapMode.Once);
				}
			}
			else if (action.playMode == AnimPlayMode.PlayOnce)
			{
				action.animChar.charState = CharState.Idle;
			}
		}
		else if (action.method == ActionCharAnim.AnimMethodChar.ResetToIdle)
		{
			action.animChar.ResetBaseClips();
		}
		else
		{
			if (action.method != ActionCharAnim.AnimMethodChar.SetStandard)
			{
				return;
			}
			if (action.clip2D != string.Empty)
			{
				if (action.standard == AnimStandard.Idle)
				{
					action.animChar.idleAnimSprite = action.clip2D;
				}
				else if (action.standard == AnimStandard.Walk)
				{
					action.animChar.walkAnimSprite = action.clip2D;
				}
				else if (action.standard == AnimStandard.Talk)
				{
					action.animChar.talkAnimSprite = action.clip2D;
				}
				else if (action.standard == AnimStandard.Run)
				{
					action.animChar.runAnimSprite = action.clip2D;
				}
			}
			if (action.changeSpeed)
			{
				if (action.standard == AnimStandard.Walk)
				{
					action.animChar.walkSpeedScale = action.newSpeed;
				}
				else if (action.standard == AnimStandard.Run)
				{
					action.animChar.runSpeedScale = action.newSpeed;
				}
			}
			if (!action.changeSound)
			{
				return;
			}
			if (action.standard == AnimStandard.Walk)
			{
				if (action.newSound != null)
				{
					action.animChar.walkSound = action.newSound;
				}
				else
				{
					action.animChar.walkSound = null;
				}
			}
			else if (action.standard == AnimStandard.Run)
			{
				if (action.newSound != null)
				{
					action.animChar.runSound = action.newSound;
				}
				else
				{
					action.animChar.runSound = null;
				}
			}
		}
	}

	public override void ActionAnimGUI(ActionAnim action, List<ActionParameter> parameters)
	{
	}

	public override string ActionAnimLabel(ActionAnim action)
	{
		string text = string.Empty;
		if ((bool)action._anim2D)
		{
			text = action._anim2D.name;
			if (action.method == AnimMethod.PlayCustom && action.clip2D != string.Empty)
			{
				text = text + " - " + action.clip2D;
			}
		}
		return text;
	}

	public override void ActionAnimAssignValues(ActionAnim action, List<ActionParameter> parameters)
	{
		action._anim2D = action.AssignFile(parameters, action.parameterID, action.constantID, action._anim2D);
	}

	public override float ActionAnimRun(ActionAnim action)
	{
		if (!action.isRunning)
		{
			action.isRunning = true;
			if ((bool)action._anim2D && action.clip2D != string.Empty)
			{
				if (action.method == AnimMethod.PlayCustom)
				{
					if (action.wrapMode2D == ActionAnim.WrapMode2D.Loop)
					{
						tk2DIntegration.PlayAnimation(action._anim2D, action.clip2D, true, WrapMode.Loop);
					}
					else if (action.wrapMode2D == ActionAnim.WrapMode2D.PingPong)
					{
						tk2DIntegration.PlayAnimation(action._anim2D, action.clip2D, true, WrapMode.PingPong);
					}
					else
					{
						tk2DIntegration.PlayAnimation(action._anim2D, action.clip2D, true, WrapMode.Once);
					}
					if (action.willWait)
					{
						return action.defaultPauseTime;
					}
				}
				else if (action.method == AnimMethod.StopCustom)
				{
					tk2DIntegration.StopAnimation(action._anim2D);
				}
				else if (action.method == AnimMethod.BlendShape)
				{
					Debug.LogWarning("BlendShapes not available for 2D animation.");
					return 0f;
				}
			}
		}
		else if ((bool)action._anim2D && action.clip2D != string.Empty)
		{
			if (tk2DIntegration.IsAnimationPlaying(action._anim2D, action.clip2D))
			{
				return Time.deltaTime;
			}
			action.isRunning = false;
		}
		return 0f;
	}

	public override void ActionAnimSkip(ActionAnim action)
	{
		if (action.isRunning)
		{
			return;
		}
		action.isRunning = true;
		if (!action._anim2D || !(action.clip2D != string.Empty))
		{
			return;
		}
		if (action.method == AnimMethod.PlayCustom)
		{
			if (action.wrapMode2D == ActionAnim.WrapMode2D.Loop)
			{
				tk2DIntegration.PlayAnimation(action._anim2D, action.clip2D, true, WrapMode.Loop);
			}
			else if (action.wrapMode2D == ActionAnim.WrapMode2D.PingPong)
			{
				tk2DIntegration.PlayAnimation(action._anim2D, action.clip2D, true, WrapMode.PingPong);
			}
			else
			{
				tk2DIntegration.PlayAnimation(action._anim2D, action.clip2D, true, WrapMode.Once);
			}
		}
		else if (action.method == AnimMethod.StopCustom)
		{
			tk2DIntegration.StopAnimation(action._anim2D);
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
		PlayStandardAnim(character.idleAnimSprite, true);
	}

	public override void PlayWalk()
	{
		PlayStandardAnim(character.walkAnimSprite, true);
	}

	public override void PlayRun()
	{
		PlayStandardAnim(character.runAnimSprite, true);
	}

	public override void PlayTalk()
	{
		if (character.LipSyncGameObject())
		{
			PlayStandardAnim(character.talkAnimSprite, true, character.GetLipSyncFrame());
		}
		else
		{
			PlayStandardAnim(character.talkAnimSprite, true);
		}
	}

	private void PlayStandardAnim(string clip, bool includeDirection)
	{
		PlayStandardAnim(clip, includeDirection, -1);
	}

	private void PlayStandardAnim(string clip, bool includeDirection, int frame)
	{
		if (clip != string.Empty && character != null)
		{
			string text = clip;
			if (includeDirection)
			{
				text += character.GetSpriteDirection();
			}
			if (!tk2DIntegration.PlayAnimation(character.spriteChild, text, frame))
			{
				tk2DIntegration.PlayAnimation(character.spriteChild, clip, frame);
			}
		}
	}
}
