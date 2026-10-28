using UnityEngine;

public class EndDEMO : MonoBehaviour
{
	private void Start()
	{
		PlayerPrefs.SetInt("doge", 2);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Application.LoadLevel(1);
		}
	}
}
