using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionCameraCrossfade : Action
	{
		public int parameterID = -1;

		public int constantID;

		public _Camera linkedCamera;

		public float transitionTime;

		public ActionCameraCrossfade()
		{
			isDisplayed = true;
			title = "Camera: Crossfade";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			linkedCamera = AssignFile(parameters, parameterID, constantID, linkedCamera);
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				MainCamera mainCamera = KickStarter.mainCamera;
				if (linkedCamera != null && mainCamera.attachedCamera != linkedCamera)
				{
					if (linkedCamera is GameCameraThirdPerson)
					{
						GameCameraThirdPerson gameCameraThirdPerson = (GameCameraThirdPerson)linkedCamera;
						gameCameraThirdPerson.ResetRotation();
					}
					else if (linkedCamera is GameCameraAnimated)
					{
						GameCameraAnimated gameCameraAnimated = (GameCameraAnimated)linkedCamera;
						gameCameraAnimated.PlayClip();
					}
					linkedCamera.MoveCameraInstant();
					mainCamera.Crossfade(transitionTime, linkedCamera);
					if (transitionTime > 0f && willWait)
					{
						return transitionTime;
					}
				}
			}
			else
			{
				isRunning = false;
			}
			return 0f;
		}

		public override void Skip()
		{
			MainCamera mainCamera = KickStarter.mainCamera;
			if (linkedCamera != null && mainCamera.attachedCamera != linkedCamera)
			{
				if (linkedCamera is GameCameraThirdPerson)
				{
					GameCameraThirdPerson gameCameraThirdPerson = (GameCameraThirdPerson)linkedCamera;
					gameCameraThirdPerson.ResetRotation();
				}
				else if (linkedCamera is GameCameraAnimated)
				{
					GameCameraAnimated gameCameraAnimated = (GameCameraAnimated)linkedCamera;
					gameCameraAnimated.PlayClip();
				}
				mainCamera.SetGameCamera(linkedCamera);
				linkedCamera.MoveCameraInstant();
				mainCamera.SnapToAttached();
			}
		}
	}
}
