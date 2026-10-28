using UnityEngine;

public class BattleAudioTweak : MonoBehaviour
{
	public AudioSource audioSource;

	public float endTime;

	public float resetTime;

	private void Update()
	{
		if (audioSource.time >= endTime)
		{
			audioSource.time = resetTime;
		}
	}
}
