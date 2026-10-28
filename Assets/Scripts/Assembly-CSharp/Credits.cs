using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
	public GameObject pivot;

	public Image blackout;

	public AudioSource audio;

	private float endTime = 120f;

	public bool endNow;

	private float timer;

	private float fadeTimer = 0.5f;

	public bool isEnding;

	private float t;

	private Vector3 startPosition;

	private Vector3 targetPosition;

	private void Awake()
	{
		startPosition = pivot.transform.localPosition;
	}

	private void Update()
	{
		if (Input.GetButtonDown("Menu"))
		{
			endNow = true;
		}
		targetPosition = pivot.transform.localPosition;
		targetPosition.y = 3000f;
		t += Time.deltaTime / endTime;
		pivot.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
		timer += 1f * Time.deltaTime;
		if (timer >= endTime)
		{
			endNow = true;
		}
		if (endNow)
		{
			Color color = blackout.color;
			color.a += fadeTimer * Time.deltaTime;
			blackout.color = color;
			audio.volume -= fadeTimer * Time.deltaTime;
			if (!isEnding)
			{
				StartCoroutine(EndNow());
			}
		}
	}

	public void SetDestination(Vector3 destination, float time)
	{
		t = 0f;
		startPosition = pivot.transform.localPosition;
		endTime = time;
		targetPosition = destination;
	}

	private IEnumerator EndNow()
	{
		isEnding = true;
		yield return new WaitForSeconds(3f);
		Application.LoadLevel("MainMenu");
	}
}
