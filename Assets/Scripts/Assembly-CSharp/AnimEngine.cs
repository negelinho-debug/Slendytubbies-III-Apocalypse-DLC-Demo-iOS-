using System;
using System.Collections.Generic;
using AC;
using UnityEngine;

[Serializable]
public class AnimEngine : ScriptableObject
{
	public AC.Char character;

	public bool turningIsLinear;

	public bool rootMotion;

	public bool isSpriteBased;

	public virtual void Declare(AC.Char _character)
	{
		character = _character;
		turningIsLinear = false;
		rootMotion = false;
		isSpriteBased = false;
	}

	public virtual void CharSettingsGUI()
	{
	}

	public virtual void ActionCharAnimGUI(ActionCharAnim action)
	{
	}

	public virtual float ActionCharAnimRun(ActionCharAnim action)
	{
		return 0f;
	}

	public virtual void ActionCharAnimSkip(ActionCharAnim action)
	{
		ActionCharAnimRun(action);
	}

	public virtual void ActionCharHoldGUI(ActionCharHold action, List<ActionParameter> parameters)
	{
	}

	public virtual void ActionCharHoldRun(ActionCharHold action)
	{
	}

	public virtual void ActionSpeechGUI(ActionSpeech action)
	{
	}

	public virtual void ActionSpeechRun(ActionSpeech action)
	{
	}

	public virtual void ActionSpeechSkip(ActionSpeech action)
	{
		ActionSpeechRun(action);
	}

	public virtual void ActionAnimGUI(ActionAnim action, List<ActionParameter> parameters)
	{
	}

	public virtual string ActionAnimLabel(ActionAnim action)
	{
		return string.Empty;
	}

	public virtual void ActionAnimAssignValues(ActionAnim action, List<ActionParameter> parameters)
	{
	}

	public virtual float ActionAnimRun(ActionAnim action)
	{
		return 0f;
	}

	public virtual void ActionAnimSkip(ActionAnim action)
	{
		ActionAnimRun(action);
	}

	public virtual void ActionCharRenderGUI(ActionCharRender action)
	{
	}

	public virtual float ActionCharRenderRun(ActionCharRender action)
	{
		return 0f;
	}

	public virtual void PlayIdle()
	{
	}

	public virtual void PlayWalk()
	{
	}

	public virtual void PlayRun()
	{
	}

	public virtual void PlayTalk()
	{
	}

	public virtual void PlayVertical()
	{
	}

	public virtual void PlayJump()
	{
		PlayIdle();
	}

	public virtual void PlayTurnLeft()
	{
		PlayIdle();
	}

	public virtual void PlayTurnRight()
	{
		PlayIdle();
	}

	public virtual void TurnHead(Vector2 angles)
	{
	}
}
