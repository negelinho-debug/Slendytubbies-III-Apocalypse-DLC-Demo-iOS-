using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
	public float blinkDelay = 1f;

	public int totalBlinks;

	private float timer;

	private int blinkCount;

	private void Update()
	{
		if (timer < blinkDelay)
		{
			timer += Time.deltaTime;
		}
		else
		{
			GetComponent<Text>().enabled = !GetComponent<Text>().enabled;
			timer = 0f;
			blinkCount++;
		}
		if (blinkCount >= totalBlinks)
		{
			enabled = false;
		}
	}
}
