using UnityEngine;

public class PuzzleDisableIf : MonoBehaviour
{
	public GameObject toAffect;

	public Animation[] checkObjs;

	public string checkName = "1";

	private void Update()
	{
		bool flag = false;
		for (int i = 0; i < checkObjs.Length; i++)
		{
			if (checkObjs[i].name == checkName)
			{
				flag = true;
			}
		}
		if (flag)
		{
			toAffect.SetActive(false);
		}
		else
		{
			toAffect.SetActive(true);
		}
	}
}
