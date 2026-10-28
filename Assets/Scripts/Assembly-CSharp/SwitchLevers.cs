using UnityEngine;

public class SwitchLevers : MonoBehaviour
{
	public GameObject[] levers;

	private void OnEnable()
	{
		SwitchNow();
	}

	private void SwitchNow()
	{
		for (int i = 0; i < levers.Length; i++)
		{
			if (levers[i].name == "0")
			{
				levers[i].name = "1";
			}
			else
			{
				levers[i].name = "0";
			}
		}
	}

	private void OnDisable()
	{
	}
}
