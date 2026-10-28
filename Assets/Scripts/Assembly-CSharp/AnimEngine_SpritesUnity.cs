using System.Collections.Generic;
using AC;
using UnityEngine;

public class AnimEngine_SpritesUnity : AnimEngine
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
				if ((bool)action.animChar.spriteChild && (bool)action.animChar.spriteChild.GetComponent<Animator>())
				{
					action.animChar.charState = CharState.Custom;
					action.animChar.spriteChild.GetComponent<Animator>().CrossFade(text, action.fadeTime, action.layerInt);
				}
			}
			else if (action.method == ActionCharAnim.AnimMethodChar.ResetToIdle)
			{
				if (action.idleAfterCustom)
				{
					action.layerInt = 0;
					return action.defaultPauseTime;
				}
				action.animChar.ResetBaseClips();
				action.animChar.charState = CharState.Idle;
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
			return 0f;
		}
		if ((bool)action.animChar.spriteChild && (bool)action.animChar.spriteChild.GetComponent<Animator>())
		{
			float length = action.animChar.spriteChild.GetComponent<Animator>().GetCurrentAnimatorStateInfo(action.layerInt).length;
			float num = (1f - action.animChar.spriteChild.GetComponent<Animator>().GetCurrentAnimatorStateInfo(action.layerInt).normalizedTime) * length;
			num -= 0.1f;
			if (num > 0f)
			{
				return num;
			}
			if (action.method == ActionCharAnim.AnimMethodChar.ResetToIdle)
			{
				action.animChar.ResetBaseClips();
				action.animChar.charState = CharState.Idle;
			}
			else if (action.idleAfter)
			{
				action.animChar.charState = CharState.Idle;
			}
			action.isRunning = false;
			return 0f;
		}
		action.isRunning = false;
		action.animChar.charState = CharState.Idle;
		return 0f;
	}

	public override void ActionCharAnimSkip(ActionCharAnim action)
	{
		if (action.method == ActionCharAnim.AnimMethodChar.SetStandard)
		{
			ActionCharAnimRun(action);
			return;
		}
		if (action.method == ActionCharAnim.AnimMethodChar.ResetToIdle)
		{
			action.animChar.ResetBaseClips();
			action.animChar.charState = CharState.Idle;
			return;
		}
		string text = action.clip2D;
		if (action.includeDirection)
		{
			text += action.animChar.GetSpriteDirection();
		}
		if (action.method == ActionCharAnim.AnimMethodChar.PlayCustom)
		{
			if (action.willWait && action.idleAfter)
			{
				action.animChar.charState = CharState.Idle;
				return;
			}
			action.animChar.charState = CharState.Custom;
			action.animChar.spriteChild.GetComponent<Animator>().Play(text, action.layerInt, 0.8f);
		}
	}

	public override void ActionSpeechGUI(ActionSpeech action)
	{
	}

	public override void ActionSpeechRun(ActionSpeech action)
	{
		if (action.speaker.talkingAnimation != TalkingAnimation.CustomFace)
		{
			return;
		}
		if (action.play2DHeadAnim && action.headClip2D != string.Empty)
		{
			try
			{
				action.speaker.GetComponent<Animator>().Play(action.headClip2D, action.headLayer);
			}
			catch
			{
			}
		}
		if (action.play2DMouthAnim && action.mouthClip2D != string.Empty)
		{
			try
			{
				action.speaker.GetComponent<Animator>().Play(action.mouthClip2D, action.mouthLayer);
			}
			catch
			{
			}
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
			if (action.method == AnimMethod.PlayCustom && action.clip2D != string.Empty)
			{
				text = text + " - " + action.clip2D;
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
			if ((bool)action.animator && action.clip2D != string.Empty)
			{
				if (action.method == AnimMethod.PlayCustom)
				{
					action.animator.CrossFade(action.clip2D, action.fadeTime, action.layerInt);
					if (action.willWait)
					{
						return action.defaultPauseTime;
					}
				}
				else if (action.method == AnimMethod.BlendShape)
				{
					Debug.LogWarning("BlendShapes not available for 2D animation.");
					return 0f;
				}
			}
		}
		else if ((bool)action.animator && action.clip2D != string.Empty)
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
		if ((bool)action.animator && action.clip2D != string.Empty && action.method == AnimMethod.PlayCustom)
		{
			action.animator.Play(action.clip2D, action.layerInt, 0.8f);
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
		PlayStandardAnim(character.idleAnimSprite, character.doDirections);
	}

	public override void PlayWalk()
	{
		PlayStandardAnim(character.walkAnimSprite, character.doDirections);
	}

	public override void PlayRun()
	{
		if (character.runAnimSprite != string.Empty)
		{
			PlayStandardAnim(character.runAnimSprite, character.doDirections);
		}
		else
		{
			PlayWalk();
		}
	}

	public override void PlayTalk()
	{
		if (character.LipSyncGameObject())
		{
			string text = character.talkAnimSprite;
			if (character.doDirections)
			{
				text += character.GetSpriteDirection();
			}
			character.animator.speed = 0f;
			try
			{
				character.animator.Play(text, 0, character.GetLipSyncNormalised());
			}
			catch
			{
				Debug.LogError("Cannot play clip " + text + " on " + character.name);
			}
			character.animator.speed = 1f;
		}
		else
		{
			PlayStandardAnim(character.talkAnimSprite, character.doDirections);
		}
	}

	private void PlayStandardAnim(string clip, bool includeDirection)
	{
		if (!character || !character.animator || !(clip != string.Empty))
		{
			return;
		}
		if (includeDirection)
		{
			clip += character.GetSpriteDirection();
		}
		if (character.crossfadeAnims)
		{
			try
			{
				character.animator.CrossFade(clip, character.animCrossfadeSpeed, 0);
				return;
			}
			catch
			{
				Debug.LogError("Cannot play clip " + clip + " on " + character.name);
				return;
			}
		}
		try
		{
			character.animator.Play(clip, 0);
		}
		catch
		{
			Debug.LogError("Cannot play clip " + clip + " on " + character.name);
		}
	}
}
