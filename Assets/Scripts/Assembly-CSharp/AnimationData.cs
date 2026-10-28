using System;
using UnityEngine;

[Serializable]
public class AnimationData
{
	public AnimationClip clip;

	public string clipName;

	public float speed = 1f;

	public float transitionDuration = -1f;

	public WrapMode wrapMode;

	public bool applyRootMotion;

	[HideInInspector]
	public int timesPlayed;

	[HideInInspector]
	public float secondsPlayed;

	[HideInInspector]
	public float length;

	[HideInInspector]
	public int stateHash;

	[HideInInspector]
	public string stateName;
}
