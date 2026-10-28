using UnityEngine;

public class AnimationControlDemo : MonoBehaviour
{
	private MecanimControl mecanimControl;

	private bool mirror;

	private float animSpeed = 1f;

	private float blendDuration = 0.1f;

	private float currentRotation = 90f;

	private void Start()
	{
		mecanimControl = gameObject.GetComponent<MecanimControl>();
	}

	private void OnGUI()
	{
		GUILayout.Label("Speed (" + animSpeed + ")");
		animSpeed = GUILayout.HorizontalSlider(animSpeed, 0f, 10f);
		mecanimControl.SetSpeed(animSpeed);
		GUILayout.Label("Rotation (" + currentRotation + ")");
		currentRotation = GUILayout.HorizontalSlider(currentRotation, 0f, 360f);
		transform.localEulerAngles = new Vector3(0f, currentRotation, 0f);
		GUILayout.Label("Blending (" + blendDuration + ")");
		blendDuration = GUILayout.HorizontalSlider(blendDuration, 0f, 1f);
		mecanimControl.defaultTransitionDuration = blendDuration;
		GUILayout.Space(10f);
		if (GUILayout.Button("Mirror " + mirror))
		{
			mirror = !mirror;
			mecanimControl.SetMirror(mirror);
		}
		if (GUILayout.Button("Debug " + mecanimControl.debugMode))
		{
			mecanimControl.debugMode = !mecanimControl.debugMode;
		}
		GUILayout.Space(10f);
		AnimationData[] animations = mecanimControl.animations;
		foreach (AnimationData animationData in animations)
		{
			if (GUILayout.Button(animationData.clipName))
			{
				mecanimControl.Play(animationData, mirror);
			}
		}
	}
}
