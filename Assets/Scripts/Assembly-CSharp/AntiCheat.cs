using CodeStage.AntiCheat.Detectors;
using UnityEngine;

public class AntiCheat : MonoBehaviour
{
	private void Start()
	{
		// Missing anti-cheat plugin.
	}

	private void OnSpeedHackDetected()
	{
		MonoBehaviour.print("ERROR");
		Application.Quit();
	}

	private void OnInjectionDetector()
	{
		MonoBehaviour.print("ERROR");
		Application.Quit();
	}

	private void OnObscuredCheatingDetector()
	{
		MonoBehaviour.print("ERROR");
		Application.Quit();
	}
}
