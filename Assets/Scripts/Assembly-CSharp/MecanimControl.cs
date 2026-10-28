using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MecanimControl : MonoBehaviour
{
	public delegate void AnimEvent(AnimationData animationData);

	public AnimationData defaultAnimation = new AnimationData();

	public AnimationData[] animations = new AnimationData[0];

	public bool debugMode;

	public bool alwaysPlay;

	public bool overrideRootMotion;

	public float defaultTransitionDuration = 0.15f;

	public WrapMode defaultWrapMode = WrapMode.Loop;

	private Animator animator;

	private int state1Hash;

	private int state2Hash;

	private RuntimeAnimatorController controller1;

	private RuntimeAnimatorController controller2;

	private RuntimeAnimatorController controller3;

	private RuntimeAnimatorController controller4;

	private AnimationData currentAnimationData;

	private bool currentMirror;

	public static event AnimEvent OnAnimationBegin;

	public static event AnimEvent OnAnimationEnd;

	public static event AnimEvent OnAnimationLoop;

	private void Awake()
	{
		animator = gameObject.GetComponent<Animator>();
		controller1 = (RuntimeAnimatorController)Resources.Load("controller1");
		controller2 = (RuntimeAnimatorController)Resources.Load("controller2");
		controller3 = (RuntimeAnimatorController)Resources.Load("controller3");
		controller4 = (RuntimeAnimatorController)Resources.Load("controller4");
		AnimationData[] array = animations;
		foreach (AnimationData animationData in array)
		{
			if (animationData.wrapMode == WrapMode.Default)
			{
				animationData.wrapMode = defaultWrapMode;
			}
			animationData.clip.wrapMode = animationData.wrapMode;
		}
	}

	private void Start()
	{
		if (defaultAnimation.clip == null && animations.Length > 0)
		{
			SetDefaultClip(animations[0].clip, "Default", animations[0].speed, animations[0].wrapMode, false);
		}
		if (!(defaultAnimation.clip != null))
		{
			return;
		}
		AnimationData[] array = animations;
		foreach (AnimationData animationData in array)
		{
			if (animationData.clip == defaultAnimation.clip)
			{
				defaultAnimation.clip = Object.Instantiate(defaultAnimation.clip);
			}
		}
		AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController();
		animatorOverrideController.runtimeAnimatorController = controller1;
		currentAnimationData = defaultAnimation;
		currentAnimationData.stateName = "State2";
		currentAnimationData.length = currentAnimationData.clip.length;
		animatorOverrideController["State1"] = currentAnimationData.clip;
		animatorOverrideController["State2"] = currentAnimationData.clip;
		animator.runtimeAnimatorController = animatorOverrideController;
		animator.Play("State2", 0, 0f);
		if (overrideRootMotion)
		{
			animator.applyRootMotion = currentAnimationData.applyRootMotion;
		}
		SetSpeed(currentAnimationData.speed);
	}

	private void FixedUpdate()
	{
		if (currentAnimationData.clip == null)
		{
			return;
		}
		if (currentAnimationData.secondsPlayed == currentAnimationData.length)
		{
			if (currentAnimationData.clip.wrapMode == WrapMode.Loop || currentAnimationData.clip.wrapMode == WrapMode.PingPong)
			{
				if (OnAnimationLoop != null)
				{
					OnAnimationLoop(currentAnimationData);
				}
				currentAnimationData.timesPlayed++;
				if (currentAnimationData.clip.wrapMode == WrapMode.Loop)
				{
					SetCurrentClipPosition(0f);
				}
				if (currentAnimationData.clip.wrapMode == WrapMode.PingPong)
				{
					SetSpeed(currentAnimationData.clipName, 0f - currentAnimationData.speed);
					SetCurrentClipPosition(0f);
				}
			}
			else if (currentAnimationData.timesPlayed == 0)
			{
				if (OnAnimationEnd != null)
				{
					OnAnimationEnd(currentAnimationData);
				}
				currentAnimationData.timesPlayed = 1;
				if (currentAnimationData.clip.wrapMode == WrapMode.Once && alwaysPlay)
				{
					Play(defaultAnimation, currentMirror);
				}
				else if (!alwaysPlay)
				{
					animator.speed = 0f;
				}
			}
		}
		else
		{
			currentAnimationData.secondsPlayed += Time.fixedDeltaTime * animator.speed;
			if (currentAnimationData.secondsPlayed > currentAnimationData.length)
			{
				currentAnimationData.secondsPlayed = currentAnimationData.length;
			}
		}
	}

	private void OnGUI()
	{
		if (debugMode)
		{
			GUI.Box(new Rect(Screen.width - 340, 40f, 340f, 400f), "Animation Data");
			GUI.BeginGroup(new Rect(Screen.width - 330, 60f, 400f, 400f));
			AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
			AnimatorClipInfo[] array = currentAnimatorClipInfo;
			for (int i = 0; i < array.Length; i++)
			{
				AnimatorClipInfo animatorClipInfo = array[i];
				AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
				GUILayout.Label(animatorClipInfo.clip.name);
				GUILayout.Label("-Wrap Mode: " + animatorClipInfo.clip.wrapMode);
				GUILayout.Label("-Is Playing: " + IsPlaying(animatorClipInfo.clip));
				GUILayout.Label("-Blend Weight: " + animatorClipInfo.weight);
				GUILayout.Label("-Normalized Time: " + currentAnimatorStateInfo.normalizedTime);
				GUILayout.Label("-Length: " + animatorClipInfo.clip.length);
				GUILayout.Label("----");
			}
			GUILayout.Label("--Current Animation Data--");
			GUILayout.Label("-Current Clip Name: " + currentAnimationData.clipName);
			GUILayout.Label("-Current Speed: " + GetSpeed());
			GUILayout.Label("-Times Played: " + currentAnimationData.timesPlayed);
			GUILayout.Label("-Seconds Played: " + currentAnimationData.secondsPlayed);
			GUILayout.Label("-Emul. Normalized: " + currentAnimationData.secondsPlayed / currentAnimationData.length);
			GUILayout.Label("-Lengh: " + currentAnimationData.length);
			GUI.EndGroup();
		}
	}

	public void RemoveClip(string name)
	{
		List<AnimationData> list = new List<AnimationData>(animations);
		list.Remove(GetAnimationData(name));
		animations = list.ToArray();
	}

	public void RemoveClip(AnimationClip clip)
	{
		List<AnimationData> list = new List<AnimationData>(animations);
		list.Remove(GetAnimationData(clip));
		animations = list.ToArray();
	}

	public void SetDefaultClip(AnimationClip clip, string name, float speed, WrapMode wrapMode, bool mirror)
	{
		defaultAnimation.clip = Object.Instantiate(clip);
		defaultAnimation.clip.wrapMode = wrapMode;
		defaultAnimation.clipName = name;
		defaultAnimation.speed = speed;
		defaultAnimation.transitionDuration = -1f;
		defaultAnimation.wrapMode = wrapMode;
	}

	public void AddClip(AnimationClip clip, string newName)
	{
		AddClip(clip, newName, 1f, defaultWrapMode);
	}

	public void AddClip(AnimationClip clip, string newName, float speed, WrapMode wrapMode)
	{
		if (GetAnimationData(newName) != null)
		{
			Debug.LogWarning("An animation with the name '" + newName + "' already exists.");
		}
		AnimationData animationData = new AnimationData();
		animationData.clip = Object.Instantiate(clip);
		if (wrapMode == WrapMode.Default)
		{
			wrapMode = defaultWrapMode;
		}
		animationData.clip.wrapMode = wrapMode;
		animationData.clip.name = newName;
		animationData.clipName = newName;
		animationData.speed = speed;
		animationData.length = clip.length;
		animationData.wrapMode = wrapMode;
		List<AnimationData> list = new List<AnimationData>(animations);
		list.Add(animationData);
		animations = list.ToArray();
	}

	public AnimationData GetAnimationData(string clipName)
	{
		AnimationData[] array = animations;
		foreach (AnimationData animationData in array)
		{
			if (animationData.clipName == clipName)
			{
				return animationData;
			}
		}
		if (clipName == defaultAnimation.clipName)
		{
			return defaultAnimation;
		}
		return null;
	}

	public AnimationData GetAnimationData(AnimationClip clip)
	{
		AnimationData[] array = animations;
		foreach (AnimationData animationData in array)
		{
			if (animationData.clip == clip)
			{
				return animationData;
			}
		}
		if (clip == defaultAnimation.clip)
		{
			return defaultAnimation;
		}
		return null;
	}

	public void CrossFade(string clipName, float blendingTime)
	{
		CrossFade(clipName, blendingTime, 0f, currentMirror);
	}

	public void CrossFade(string clipName, float blendingTime, float normalizedTime, bool mirror)
	{
		_playAnimation(GetAnimationData(clipName), blendingTime, normalizedTime, mirror);
	}

	public void CrossFade(AnimationData animationData, float blendingTime, float normalizedTime, bool mirror)
	{
		_playAnimation(animationData, blendingTime, normalizedTime, mirror);
	}

	public void Play(string clipName, float blendingTime, float normalizedTime, bool mirror)
	{
		_playAnimation(GetAnimationData(clipName), blendingTime, normalizedTime, mirror);
	}

	public void Play(AnimationClip clip, float blendingTime, float normalizedTime, bool mirror)
	{
		_playAnimation(GetAnimationData(clip), blendingTime, normalizedTime, mirror);
	}

	public void Play(string clipName, bool mirror)
	{
		_playAnimation(GetAnimationData(clipName), 0f, 0f, mirror);
	}

	public void Play(string clipName)
	{
		_playAnimation(GetAnimationData(clipName), 0f, 0f, currentMirror);
	}

	public void Play(AnimationClip clip, bool mirror)
	{
		_playAnimation(GetAnimationData(clip), 0f, 0f, mirror);
	}

	public void Play(AnimationClip clip)
	{
		_playAnimation(GetAnimationData(clip), 0f, 0f, currentMirror);
	}

	public void Play(AnimationData animationData, bool mirror)
	{
		_playAnimation(animationData, animationData.transitionDuration, 0f, mirror);
	}

	public void Play(AnimationData animationData)
	{
		_playAnimation(animationData, animationData.transitionDuration, 0f, currentMirror);
	}

	public void Play(AnimationData animationData, float blendingTime, float normalizedTime, bool mirror)
	{
		_playAnimation(animationData, blendingTime, normalizedTime, mirror);
	}

	public void Play()
	{
		animator.speed = Mathf.Abs(currentAnimationData.speed);
	}

	private void _playAnimation(AnimationData targetAnimationData, float blendingTime, float normalizedTime, bool mirror)
	{
		if (targetAnimationData == null || targetAnimationData.clip == null)
		{
			return;
		}
		AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController();
		currentMirror = mirror;
		float num = Mathf.Abs(targetAnimationData.speed);
		float currentClipPosition = GetCurrentClipPosition();
		if (mirror)
		{
			if (targetAnimationData.speed > 0f)
			{
				animatorOverrideController.runtimeAnimatorController = controller2;
			}
			else
			{
				animatorOverrideController.runtimeAnimatorController = controller4;
			}
		}
		else if (targetAnimationData.speed > 0f)
		{
			animatorOverrideController.runtimeAnimatorController = controller1;
		}
		else
		{
			animatorOverrideController.runtimeAnimatorController = controller3;
		}
		animatorOverrideController["State1"] = currentAnimationData.clip;
		animatorOverrideController["State2"] = targetAnimationData.clip;
		if (blendingTime == -1f)
		{
			blendingTime = currentAnimationData.transitionDuration;
		}
		if (blendingTime == -1f)
		{
			blendingTime = defaultTransitionDuration;
		}
		if (blendingTime <= 0f)
		{
			animator.runtimeAnimatorController = animatorOverrideController;
			animator.Play("State2", 0, normalizedTime);
		}
		else
		{
			animator.runtimeAnimatorController = animatorOverrideController;
			currentAnimationData.stateName = "State1";
			SetCurrentClipPosition(currentClipPosition);
			animator.Update(0f);
			animator.CrossFade("State2", blendingTime / num, 0, normalizedTime);
		}
		targetAnimationData.timesPlayed = 0;
		targetAnimationData.secondsPlayed = normalizedTime * targetAnimationData.clip.length / num;
		targetAnimationData.length = targetAnimationData.clip.length;
		if (overrideRootMotion)
		{
			animator.applyRootMotion = targetAnimationData.applyRootMotion;
		}
		SetSpeed(targetAnimationData.speed);
		currentAnimationData = targetAnimationData;
		currentAnimationData.stateName = "State2";
		if (OnAnimationBegin != null)
		{
			OnAnimationBegin(currentAnimationData);
		}
	}

	public bool IsPlaying(string clipName)
	{
		return IsPlaying(GetAnimationData(clipName));
	}

	public bool IsPlaying(string clipName, float weight)
	{
		return IsPlaying(GetAnimationData(clipName), weight);
	}

	public bool IsPlaying(AnimationClip clip)
	{
		return IsPlaying(GetAnimationData(clip));
	}

	public bool IsPlaying(AnimationClip clip, float weight)
	{
		return IsPlaying(GetAnimationData(clip), weight);
	}

	public bool IsPlaying(AnimationData animData)
	{
		return IsPlaying(animData, 0f);
	}

	public bool IsPlaying(AnimationData animData, float weight)
	{
		if (animData == null)
		{
			return false;
		}
		if (currentAnimationData == null)
		{
			return false;
		}
		if (currentAnimationData == animData && animData.wrapMode == WrapMode.Once && animData.timesPlayed > 0)
		{
			return false;
		}
		if (currentAnimationData == animData)
		{
			return true;
		}
		AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
		AnimatorClipInfo[] array = currentAnimatorClipInfo;
		for (int i = 0; i < array.Length; i++)
		{
			AnimatorClipInfo animatorClipInfo = array[i];
			if (animData.clip == animatorClipInfo.clip && animatorClipInfo.weight >= weight)
			{
				return true;
			}
		}
		return false;
	}

	public string GetCurrentClipName()
	{
		return currentAnimationData.clipName;
	}

	public AnimationData GetCurrentAnimationData()
	{
		return currentAnimationData;
	}

	public int GetCurrentClipPlayCount()
	{
		return currentAnimationData.timesPlayed;
	}

	public float GetCurrentClipTime()
	{
		return currentAnimationData.secondsPlayed;
	}

	public float GetCurrentClipLength()
	{
		return currentAnimationData.length;
	}

	public void SetCurrentClipPosition(float normalizedTime)
	{
		SetCurrentClipPosition(normalizedTime, false);
	}

	public void SetCurrentClipPosition(float normalizedTime, bool pause)
	{
		animator.Play(currentAnimationData.stateName, 0, normalizedTime);
		currentAnimationData.secondsPlayed = normalizedTime * currentAnimationData.length;
		if (pause)
		{
			Pause();
		}
	}

	public float GetCurrentClipPosition()
	{
		return currentAnimationData.secondsPlayed / currentAnimationData.length;
	}

	public void Stop()
	{
		Play(defaultAnimation.clip, defaultTransitionDuration, 0f, currentMirror);
	}

	public void Pause()
	{
		animator.speed = 0f;
	}

	public void SetSpeed(AnimationClip clip, float speed)
	{
		AnimationData animationData = GetAnimationData(clip);
		animationData.speed = speed;
		if (IsPlaying(clip))
		{
			SetSpeed(speed);
		}
	}

	public void SetSpeed(string clipName, float speed)
	{
		AnimationData animationData = GetAnimationData(clipName);
		if (animationData.speed != speed || animator.speed != Mathf.Abs(speed))
		{
			animationData.speed = speed;
			if (IsPlaying(clipName))
			{
				SetSpeed(speed);
			}
		}
	}

	public void SetSpeed(float speed)
	{
		animator.speed = Mathf.Abs(speed);
	}

	public void RestoreSpeed()
	{
		SetSpeed(currentAnimationData.speed);
	}

	public void Rewind()
	{
		SetSpeed(0f - currentAnimationData.speed);
	}

	public void SetWrapMode(WrapMode wrapMode)
	{
		defaultWrapMode = wrapMode;
	}

	public void SetWrapMode(AnimationData animationData, WrapMode wrapMode)
	{
		animationData.wrapMode = wrapMode;
		animationData.clip.wrapMode = wrapMode;
	}

	public void SetWrapMode(AnimationClip clip, WrapMode wrapMode)
	{
		AnimationData animationData = GetAnimationData(clip);
		animationData.wrapMode = wrapMode;
		animationData.clip.wrapMode = wrapMode;
	}

	public void SetWrapMode(string clipName, WrapMode wrapMode)
	{
		AnimationData animationData = GetAnimationData(clipName);
		animationData.wrapMode = wrapMode;
		animationData.clip.wrapMode = wrapMode;
	}

	public float GetSpeed(AnimationClip clip)
	{
		AnimationData animationData = GetAnimationData(clip);
		return animationData.speed;
	}

	public float GetSpeed(string clipName)
	{
		AnimationData animationData = GetAnimationData(clipName);
		return animationData.speed;
	}

	public float GetSpeed()
	{
		return animator.speed;
	}

	public bool GetMirror()
	{
		return currentMirror;
	}

	public void SetMirror(bool toggle)
	{
		SetMirror(toggle, 0f, false);
	}

	public void SetMirror(bool toggle, float blendingTime)
	{
		SetMirror(toggle, blendingTime, false);
	}

	public void SetMirror(bool toggle, float blendingTime, bool forceMirror)
	{
		if (currentMirror != toggle || forceMirror)
		{
			if (blendingTime == 0f)
			{
				blendingTime = defaultTransitionDuration;
			}
			_playAnimation(currentAnimationData, blendingTime, GetCurrentClipPosition(), toggle);
		}
	}
}
