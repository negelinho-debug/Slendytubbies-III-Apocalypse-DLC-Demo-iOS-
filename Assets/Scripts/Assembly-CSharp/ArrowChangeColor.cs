using UnityEngine;

public class ArrowChangeColor : MonoBehaviour
{
	public string buttonName;

	public bool GreaterThan;

	private float direction = 0.1f;

	private void Start()
	{
	}

	private void Update()
	{
		if (GreaterThan)
		{
			if (Input.GetAxis(buttonName) >= 0.01f)
			{
				GetComponent<UnityEngine.UI.Graphic>().color = Color.green;
			}
			else
			{
				GetComponent<UnityEngine.UI.Graphic>().color = Color.white;
			}
		}
		else if (Input.GetAxis(buttonName) <= -0.01f)
		{
			GetComponent<UnityEngine.UI.Graphic>().color = Color.green;
		}
		else
		{
			GetComponent<UnityEngine.UI.Graphic>().color = Color.white;
		}
	}
}
