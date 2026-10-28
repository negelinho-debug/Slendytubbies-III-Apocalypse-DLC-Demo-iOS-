using System.Collections.Generic;
using AC;
using UnityEngine;

public class AnimEngine_Legacy : AnimEngine
{
	public override void CharSettingsGUI()
	{
	}

	public override void ActionCharAnimGUI(ActionCharAnim action)
	{
	}

	public override float ActionCharAnimRun(ActionCharAnim action)
	{
		if (action.animChar == null)
		{
			return 0f;
		}
		Animation animation = null;
		if ((bool)action.animChar.spriteChild && (bool)action.animChar.spriteChild.GetComponent<Animation>())
		{
			animation = action.animChar.spriteChild.GetComponent<Animation>();
		}
		if ((bool)character.GetComponent<Animation>())
		{
			animation = action.animChar.GetComponent<Animation>();
		}
		if (!action.isRunning)
		{
			action.isRunning = true;
			if (action.method == ActionCharAnim.AnimMethodChar.PlayCustom && (bool)action.clip)
			{
				AdvGame.CleanUnusedClips(animation);
				WrapMode wrapMode = WrapMode.Once;
				Transform mixingBone = null;
				if (action.layer == AnimLayer.Base)
				{
					action.animChar.charState = CharState.Custom;
					action.blendMode = AnimationBlendMode.Blend;
					action.playMode = (AnimPlayMode)action.playModeBase;
				}
				else if (action.layer == AnimLayer.UpperBody)
				{
					mixingBone = action.animChar.upperBodyBone;
				}
				else if (action.layer == AnimLayer.LeftArm)
				{
					mixingBone = action.animChar.leftArmBone;
				}
				else if (action.layer == AnimLayer.RightArm)
				{
					mixingBone = action.animChar.rightArmBone;
				}
				else if (action.layer == AnimLayer.Neck || action.layer == AnimLayer.Head || action.layer == AnimLayer.Face || action.layer == AnimLayer.Mouth)
				{
					mixingBone = action.animChar.neckBone;
				}
				if (action.playMode == AnimPlayMode.PlayOnceAndClamp)
				{
					wrapMode = WrapMode.ClampForever;
				}
				else if (action.playMode == AnimPlayMode.Loop)
				{
					wrapMode = WrapMode.Loop;
				}
				AdvGame.PlayAnimClip(animation, AdvGame.GetAnimLayerInt(action.layer), action.clip, action.blendMode, wrapMode, action.fadeTime, mixingBone, false);
			}
			else if (action.method == ActionCharAnim.AnimMethodChar.StopCustom && (bool)action.clip)
			{
				if (action.clip != action.animChar.idleAnim && action.clip != action.animChar.walkAnim)
				{
					animation.Blend(action.clip.name, 0f, action.fadeTime);
				}
			}
			else if (action.method == ActionCharAnim.AnimMethodChar.ResetToIdle)
			{
				action.animChar.ResetBaseClips();
				action.animChar.charState = CharState.Idle;
				AdvGame.CleanUnusedClips(animation);
			}
			else if (action.method == ActionCharAnim.AnimMethodChar.SetStandard)
			{
				if (action.clip != null)
				{
					if (action.standard == AnimStandard.Idle)
					{
						action.animChar.idleAnim = action.clip;
					}
					else if (action.standard == AnimStandard.Walk)
					{
						action.animChar.walkAnim = action.clip;
					}
					else if (action.standard == AnimStandard.Run)
					{
						action.animChar.runAnim = action.clip;
					}
					else if (action.standard == AnimStandard.Talk)
					{
						action.animChar.talkAnim = action.clip;
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
			if (action.willWait && (bool)action.clip)
			{
				if (action.method == ActionCharAnim.AnimMethodChar.PlayCustom)
				{
					return action.defaultPauseTime;
				}
				if (action.method == ActionCharAnim.AnimMethodChar.StopCustom)
				{
					return action.fadeTime;
				}
			}
			return 0f;
		}
		if ((bool)action.animChar._animation[action.clip.name] && action.animChar._animation[action.clip.name].normalizedTime < 1f && action.animChar._animation.IsPlaying(action.clip.name))
		{
			return action.defaultPauseTime;
		}
		action.isRunning = false;
		if (action.playMode == AnimPlayMode.PlayOnce)
		{
			action.animChar._animation.Blend(action.clip.name, 0f, action.fadeTime);
			if (action.layer == AnimLayer.Base && action.method == ActionCharAnim.AnimMethodChar.PlayCustom)
			{
				action.animChar.charState = CharState.Idle;
				action.animChar.ResetBaseClips();
			}
		}
		AdvGame.CleanUnusedClips(animation);
		return 0f;
	}

	public override void ActionCharAnimSkip(ActionCharAnim action)
	{
		if (action.animChar == null)
		{
			return;
		}
		Animation animation = null;
		if ((bool)action.animChar.spriteChild && (bool)action.animChar.spriteChild.GetComponent<Animation>())
		{
			animation = action.animChar.spriteChild.GetComponent<Animation>();
		}
		if ((bool)character._animation)
		{
			animation = action.animChar._animation;
		}
		if (action.method == ActionCharAnim.AnimMethodChar.PlayCustom && (bool)action.clip)
		{
			if (action.layer == AnimLayer.Base)
			{
				action.animChar.charState = CharState.Custom;
				action.blendMode = AnimationBlendMode.Blend;
				action.playMode = (AnimPlayMode)action.playModeBase;
			}
			if (action.playMode == AnimPlayMode.PlayOnce)
			{
				if (action.layer == AnimLayer.Base && action.method == ActionCharAnim.AnimMethodChar.PlayCustom)
				{
					action.animChar.charState = CharState.Idle;
					action.animChar.ResetBaseClips();
				}
			}
			else
			{
				AdvGame.CleanUnusedClips(animation);
				WrapMode wrapMode = WrapMode.Once;
				Transform mixingBone = null;
				if (action.layer == AnimLayer.UpperBody)
				{
					mixingBone = action.animChar.upperBodyBone;
				}
				else if (action.layer == AnimLayer.LeftArm)
				{
					mixingBone = action.animChar.leftArmBone;
				}
				else if (action.layer == AnimLayer.RightArm)
				{
					mixingBone = action.animChar.rightArmBone;
				}
				else if (action.layer == AnimLayer.Neck || action.layer == AnimLayer.Head || action.layer == AnimLayer.Face || action.layer == AnimLayer.Mouth)
				{
					mixingBone = action.animChar.neckBone;
				}
				if (action.playMode == AnimPlayMode.PlayOnceAndClamp)
				{
					wrapMode = WrapMode.ClampForever;
				}
				else if (action.playMode == AnimPlayMode.Loop)
				{
					wrapMode = WrapMode.Loop;
				}
				AdvGame.PlayAnimClipFrame(animation, AdvGame.GetAnimLayerInt(action.layer), action.clip, action.blendMode, wrapMode, action.fadeTime, mixingBone, 1f);
			}
			AdvGame.CleanUnusedClips(animation);
		}
		else if (action.method == ActionCharAnim.AnimMethodChar.StopCustom && (bool)action.clip)
		{
			if (action.clip != action.animChar.idleAnim && action.clip != action.animChar.walkAnim)
			{
				animation.Blend(action.clip.name, 0f, 0f);
			}
		}
		else if (action.method == ActionCharAnim.AnimMethodChar.ResetToIdle)
		{
			action.animChar.ResetBaseClips();
			action.animChar.charState = CharState.Idle;
			AdvGame.CleanUnusedClips(animation);
		}
		else
		{
			if (action.method != ActionCharAnim.AnimMethodChar.SetStandard)
			{
				return;
			}
			if (action.clip != null)
			{
				if (action.standard == AnimStandard.Idle)
				{
					action.animChar.idleAnim = action.clip;
				}
				else if (action.standard == AnimStandard.Walk)
				{
					action.animChar.walkAnim = action.clip;
				}
				else if (action.standard == AnimStandard.Run)
				{
					action.animChar.runAnim = action.clip;
				}
				else if (action.standard == AnimStandard.Talk)
				{
					action.animChar.talkAnim = action.clip;
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

	public override void ActionSpeechGUI(ActionSpeech action)
	{
	}

	public override void ActionSpeechRun(ActionSpeech action)
	{
		if (action.speaker.talkingAnimation == TalkingAnimation.CustomFace && ((bool)action.headClip || (bool)action.mouthClip))
		{
			AdvGame.CleanUnusedClips(action.speaker.GetComponent<Animation>());
			if ((bool)action.headClip)
			{
				AdvGame.PlayAnimClip(action.speaker.GetComponent<Animation>(), AdvGame.GetAnimLayerInt(AnimLayer.Head), action.headClip, AnimationBlendMode.Additive, WrapMode.Once, 0f, action.speaker.neckBone, false);
			}
			if ((bool)action.mouthClip)
			{
				AdvGame.PlayAnimClip(action.speaker.GetComponent<Animation>(), AdvGame.GetAnimLayerInt(AnimLayer.Mouth), action.mouthClip, AnimationBlendMode.Additive, WrapMode.Once, 0f, action.speaker.neckBone, false);
			}
		}
	}

	public override void ActionSpeechSkip(ActionSpeech action)
	{
		if (action.speaker.talkingAnimation == TalkingAnimation.CustomFace && ((bool)action.headClip || (bool)action.mouthClip))
		{
			AdvGame.CleanUnusedClips(action.speaker.GetComponent<Animation>());
			if ((bool)action.headClip)
			{
				AdvGame.PlayAnimClipFrame(action.speaker.GetComponent<Animation>(), AdvGame.GetAnimLayerInt(AnimLayer.Head), action.headClip, AnimationBlendMode.Additive, WrapMode.Once, 0f, action.speaker.neckBone, 1f);
			}
			if ((bool)action.mouthClip)
			{
				AdvGame.PlayAnimClipFrame(action.speaker.GetComponent<Animation>(), AdvGame.GetAnimLayerInt(AnimLayer.Mouth), action.mouthClip, AnimationBlendMode.Additive, WrapMode.Once, 0f, action.speaker.neckBone, 1f);
			}
		}
	}

	public override void ActionAnimGUI(ActionAnim action, List<ActionParameter> parameters)
	{
	}

	public override string ActionAnimLabel(ActionAnim action)
	{
		string text = string.Empty;
		if ((bool)action._anim)
		{
			text = action._anim.name;
			if (action.method == AnimMethod.PlayCustom && (bool)action.clip)
			{
				text = text + " - Play " + action.clip.name;
			}
			else if (action.method == AnimMethod.StopCustom && (bool)action.clip)
			{
				text = text + " - Stop " + action.clip.name;
			}
			else if (action.method == AnimMethod.BlendShape)
			{
				text += " - Shapekey";
			}
		}
		return text;
	}

	public override void ActionAnimAssignValues(ActionAnim action, List<ActionParameter> parameters)
	{
		action._anim = action.AssignFile(parameters, action.parameterID, action.constantID, action._anim);
		action.shapeObject = action.AssignFile(parameters, action.parameterID, action.constantID, action.shapeObject);
	}

	public override float ActionAnimRun(ActionAnim action)
	{
		if (!action.isRunning)
		{
			action.isRunning = true;
			if (action.method == AnimMethod.PlayCustom && (bool)action._anim && (bool)action.clip)
			{
				AdvGame.CleanUnusedClips(action._anim);
				WrapMode wrapMode = WrapMode.Once;
				if (action.playMode == AnimPlayMode.PlayOnceAndClamp)
				{
					wrapMode = WrapMode.ClampForever;
				}
				else if (action.playMode == AnimPlayMode.Loop)
				{
					wrapMode = WrapMode.Loop;
				}
				AdvGame.PlayAnimClip(action._anim, 0, action.clip, action.blendMode, wrapMode, action.fadeTime, null, false);
			}
			else if (action.method == AnimMethod.StopCustom && (bool)action._anim && (bool)action.clip)
			{
				AdvGame.CleanUnusedClips(action._anim);
				action._anim.Blend(action.clip.name, 0f, action.fadeTime);
			}
			else if (action.method == AnimMethod.BlendShape && action.shapeKey > -1 && (bool)action.shapeObject)
			{
				action.shapeObject.Change(action.shapeKey, action.shapeValue, action.fadeTime);
				if (action.willWait)
				{
					return action.fadeTime;
				}
			}
			if (action.willWait)
			{
				return action.defaultPauseTime;
			}
		}
		else
		{
			if (action.method == AnimMethod.PlayCustom && (bool)action._anim && (bool)action.clip)
			{
				if (!action._anim.IsPlaying(action.clip.name))
				{
					action.isRunning = false;
					return 0f;
				}
				return action.defaultPauseTime;
			}
			if (action.method == AnimMethod.BlendShape && (bool)action.shapeObject)
			{
				action.isRunning = false;
				return 0f;
			}
		}
		return 0f;
	}

	public override void ActionAnimSkip(ActionAnim action)
	{
		if (action.method == AnimMethod.PlayCustom && (bool)action._anim && (bool)action.clip)
		{
			AdvGame.CleanUnusedClips(action._anim);
			WrapMode wrapMode = WrapMode.Once;
			if (action.playMode == AnimPlayMode.PlayOnceAndClamp)
			{
				wrapMode = WrapMode.ClampForever;
			}
			else if (action.playMode == AnimPlayMode.Loop)
			{
				wrapMode = WrapMode.Loop;
			}
			AdvGame.PlayAnimClipFrame(action._anim, 0, action.clip, action.blendMode, wrapMode, 0f, null, 1f);
		}
		else if (action.method == AnimMethod.StopCustom && (bool)action._anim && (bool)action.clip)
		{
			AdvGame.CleanUnusedClips(action._anim);
			action._anim.Blend(action.clip.name, 0f, 0f);
		}
		else if (action.method == AnimMethod.BlendShape && action.shapeKey > -1 && (bool)action.shapeObject)
		{
			action.shapeObject.Change(action.shapeKey, action.shapeValue, 0f);
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
		PlayStandardAnim(character.idleAnim, true, false);
	}

	public override void PlayWalk()
	{
		PlayStandardAnim(character.walkAnim, true, character.isReversing);
	}

	public override void PlayRun()
	{
		PlayStandardAnim(character.runAnim, true, character.isReversing);
	}

	public override void PlayTalk()
	{
		PlayStandardAnim(character.talkAnim, true, false);
	}

	public override void PlayJump()
	{
		if (character is Player)
		{
			Player player = (Player)character;
			if ((bool)player.jumpAnim)
			{
				PlayStandardAnim(player.jumpAnim, false, false);
			}
			else
			{
				PlayIdle();
			}
		}
		else
		{
			PlayIdle();
		}
	}

	public override void PlayTurnLeft()
	{
		if ((bool)character.turnLeftAnim)
		{
			PlayStandardAnim(character.turnLeftAnim, false, false);
		}
		else
		{
			PlayIdle();
		}
	}

	public override void PlayTurnRight()
	{
		if ((bool)character.turnRightAnim)
		{
			PlayStandardAnim(character.turnRightAnim, false, false);
		}
		else
		{
			PlayIdle();
		}
	}

	public override void TurnHead(Vector2 angles)
	{
		if (character == null)
		{
			return;
		}
		Animation animation = null;
		if ((bool)character.spriteChild && (bool)character.spriteChild.GetComponent<Animation>())
		{
			animation = character.spriteChild.GetComponent<Animation>();
		}
		if ((bool)character.GetComponent<Animation>())
		{
			animation = character.GetComponent<Animation>();
		}
		if (animation == null)
		{
			return;
		}
		if ((bool)character.headLookLeftAnim && (bool)character.headLookRightAnim)
		{
			if (angles.x < 0f)
			{
				animation.Stop(character.headLookRightAnim.name);
				AdvGame.PlayAnimClipFrame(animation, AdvGame.GetAnimLayerInt(AnimLayer.Neck), character.headLookLeftAnim, AnimationBlendMode.Additive, WrapMode.ClampForever, 0f, character.neckBone, 1f);
				animation[character.headLookLeftAnim.name].weight = 0f - angles.x;
				animation[character.headLookLeftAnim.name].speed = 0f;
			}
			else if (angles.x > 0f)
			{
				animation.Stop(character.headLookLeftAnim.name);
				AdvGame.PlayAnimClipFrame(animation, AdvGame.GetAnimLayerInt(AnimLayer.Neck), character.headLookRightAnim, AnimationBlendMode.Additive, WrapMode.ClampForever, 0f, character.neckBone, 1f);
				animation[character.headLookRightAnim.name].weight = angles.x;
				animation[character.headLookRightAnim.name].speed = 0f;
			}
			else
			{
				animation.Stop(character.headLookLeftAnim.name);
				animation.Stop(character.headLookRightAnim.name);
			}
		}
		if ((bool)character.headLookUpAnim && (bool)character.headLookDownAnim)
		{
			if (angles.y < 0f)
			{
				animation.Stop(character.headLookUpAnim.name);
				AdvGame.PlayAnimClipFrame(animation, AdvGame.GetAnimLayerInt(AnimLayer.Neck) + 1, character.headLookDownAnim, AnimationBlendMode.Additive, WrapMode.ClampForever, 0f, character.neckBone, 1f);
				animation[character.headLookDownAnim.name].weight = 0f - angles.y;
				animation[character.headLookDownAnim.name].speed = 0f;
			}
			else if (angles.y > 0f)
			{
				animation.Stop(character.headLookDownAnim.name);
				AdvGame.PlayAnimClipFrame(animation, AdvGame.GetAnimLayerInt(AnimLayer.Neck) + 1, character.headLookUpAnim, AnimationBlendMode.Additive, WrapMode.ClampForever, 0f, character.neckBone, 1f);
				animation[character.headLookUpAnim.name].weight = angles.y;
				animation[character.headLookUpAnim.name].speed = 0f;
			}
			else
			{
				animation.Stop(character.headLookDownAnim.name);
				animation.Stop(character.headLookUpAnim.name);
			}
		}
	}

	private void PlayStandardAnim(AnimationClip clip, bool doLoop, bool reverse)
	{
		if (character == null)
		{
			return;
		}
		Animation animation = null;
		if ((bool)character.spriteChild && (bool)character.spriteChild.GetComponent<Animation>())
		{
			animation = character.spriteChild.GetComponent<Animation>();
		}
		if ((bool)character.GetComponent<Animation>())
		{
			animation = character.GetComponent<Animation>();
		}
		if (!(animation != null))
		{
			return;
		}
		if (clip != null && animation[clip.name] != null)
		{
			if (!animation[clip.name].enabled)
			{
				if (doLoop)
				{
					AdvGame.PlayAnimClip(animation, AdvGame.GetAnimLayerInt(AnimLayer.Base), clip, AnimationBlendMode.Blend, WrapMode.Loop, character.animCrossfadeSpeed, null, reverse);
				}
				else
				{
					AdvGame.PlayAnimClip(animation, AdvGame.GetAnimLayerInt(AnimLayer.Base), clip, AnimationBlendMode.Blend, WrapMode.Once, character.animCrossfadeSpeed, null, reverse);
				}
			}
		}
		else if (doLoop)
		{
			AdvGame.PlayAnimClip(animation, AdvGame.GetAnimLayerInt(AnimLayer.Base), clip, AnimationBlendMode.Blend, WrapMode.Loop, character.animCrossfadeSpeed, null, reverse);
		}
		else
		{
			AdvGame.PlayAnimClip(animation, AdvGame.GetAnimLayerInt(AnimLayer.Base), clip, AnimationBlendMode.Blend, WrapMode.Once, character.animCrossfadeSpeed, null, reverse);
		}
	}
}
