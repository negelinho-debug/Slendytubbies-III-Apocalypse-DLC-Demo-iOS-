using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionDisableNearest : Action
	{
		public string tagName;

		public ActionDisableNearest()
		{
			isDisplayed = true;
			title = "Custom: Disable Nearest";
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				GameObject[] array = GameObject.FindGameObjectsWithTag(tagName);
				Transform transform = GameObject.FindWithTag("Player").transform;
				Transform transform2 = null;
				float num = 1000f;
				for (int i = 0; i < array.Length; i++)
				{
					float num2 = Vector3.Distance(transform.position, array[i].transform.position);
					if (num2 < num)
					{
						num = num2;
						transform2 = array[i].transform;
					}
				}
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(false);
				}
				return defaultPauseTime;
			}
			isRunning = false;
			return 0f;
		}
	}
}
