using UnityEngine;

public class Flashes : MonoBehaviour
{
	public FadeScreen flashObj;

	private void OnEnable()
	{
		flashObj.FadeOut();
	}

	private void OnDisable()
	{
		flashObj.FadeOut();
	}
}
