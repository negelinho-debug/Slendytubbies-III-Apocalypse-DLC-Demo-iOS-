using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionEnableTransform : Action
	{
		public Transform myActivate;

		public string ParentObj;

		public string ToActivate;

		public ActionEnableTransform()
		{
			isDisplayed = true;
			title = "Object: Activate Transform";
		}

		public override float Run()
		{
			Transform transform = UnityEngine.Object.Instantiate(Resources.Load("ActivateTransform", typeof(Transform))) as Transform;
			transform.GetComponent<ActivateObject>().parent1 = ParentObj;
			transform.GetComponent<ActivateObject>().toactivate = ToActivate;
			transform.GetComponent<ActivateObject>().enabled = true;
			if (!isRunning)
			{
				isRunning = true;
				return defaultPauseTime;
			}
			isRunning = false;
			return 0f;
		}
	}
}
