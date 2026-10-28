using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionCameraSplit : Action
	{
		public int parameterID1 = -1;

		public int parameterID2 = -1;

		public int constantID1;

		public int constantID2;

		public float splitAmount1 = 0.49f;

		public float splitAmount2 = 0.49f;

		public _Camera cam1;

		public _Camera cam2;

		public bool turnOff;

		public MenuOrientation orientation;

		public bool mainIsTopLeft;

		public ActionCameraSplit()
		{
			isDisplayed = true;
			title = "Camera: Split-screen";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			cam1 = AssignFile(parameters, parameterID1, constantID1, cam1);
			cam2 = AssignFile(parameters, parameterID2, constantID2, cam2);
		}

		public override float Run()
		{
			MainCamera mainCamera = KickStarter.mainCamera;
			if (turnOff)
			{
				mainCamera.RemoveSplitScreen();
				return 0f;
			}
			if (cam1 == null || cam2 == null)
			{
				return 0f;
			}
			if (mainIsTopLeft)
			{
				mainCamera.SetSplitScreen(cam1, cam2, orientation, mainIsTopLeft, splitAmount1, splitAmount2);
			}
			else
			{
				mainCamera.SetSplitScreen(cam2, cam1, orientation, mainIsTopLeft, splitAmount1, splitAmount2);
			}
			return 0f;
		}
	}
}
