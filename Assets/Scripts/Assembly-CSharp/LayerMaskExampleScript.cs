using UnityEngine;

public class LayerMaskExampleScript : MonoBehaviour
{
	private Animator animator;

	public float weight;

	public float startTime;

	public float timer;

	public bool timerBool = true;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (weight == 1f && timerBool)
		{
			startTime = Time.time;
			timerBool = false;
		}
		else if (weight == 0f && !timerBool)
		{
			startTime = Time.time;
			timerBool = true;
		}
		timer = (Time.time - startTime) * 0.7f;
		if (timerBool)
		{
			weight = Mathf.Lerp(0f, 1f, timer);
		}
		else
		{
			weight = Mathf.Lerp(1f, 0f, timer);
		}
		animator.SetLayerWeight(1, weight);
	}
}
