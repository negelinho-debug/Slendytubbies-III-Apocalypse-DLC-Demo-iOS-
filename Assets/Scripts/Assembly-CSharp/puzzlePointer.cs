using UnityEngine;

public class puzzlePointer : MonoBehaviour
{
	public Animation[] checkObjs;

	public int[] goodObjID;

	public float offset;

	private void Update()
	{
		transform.localEulerAngles = new Vector3(0f, 0f - offset, 0f);
		float num = offset / (float)checkObjs.Length;
		float num2 = 0f - offset;
		for (int i = 0; i < checkObjs.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < goodObjID.Length; j++)
			{
				if (i == goodObjID[j])
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (checkObjs[i].name == "1")
				{
					num2 += num * 2.575f;
				}
			}
			else if (checkObjs[i].name == "1")
			{
				num2 -= num;
			}
		}
		transform.localEulerAngles = new Vector3(0f, num2, 0f);
	}
}
