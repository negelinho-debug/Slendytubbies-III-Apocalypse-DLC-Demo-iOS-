using UnityEngine;

public class ChangeToFast : MonoBehaviour
{
	public bool isfast;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (!isfast)
			{
				QualitySettings.SetQualityLevel(1);
				isfast = true;
			}
			else
			{
				QualitySettings.SetQualityLevel(2);
				isfast = false;
			}
		}
	}
}
