using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionCamera : Action
	{
		public int constantID;

		public int parameterID = -1;

		public _Camera linkedCamera;

		public float transitionTime;

		public AnimationCurve timeCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		public MoveMethod moveMethod;

		public bool returnToLast;

		public ActionCamera()
		{
			isDisplayed = true;
			title = "Camera: Switch";
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
				if ((bool)mainCamera)
				{
					_Camera camera = linkedCamera;
					if (returnToLast && (bool)mainCamera.lastNavCamera)
					{
						camera = ((!(mainCamera.lastNavCamera != null) || !(mainCamera.attachedCamera == mainCamera.lastNavCamera)) ? mainCamera.lastNavCamera : mainCamera.lastNavCamera2);
					}
					if ((bool)camera && mainCamera.attachedCamera != camera)
					{
						if (camera is GameCameraThirdPerson)
						{
							GameCameraThirdPerson gameCameraThirdPerson = (GameCameraThirdPerson)camera;
							gameCameraThirdPerson.ResetRotation();
						}
						else if (camera is GameCameraAnimated)
						{
							GameCameraAnimated gameCameraAnimated = (GameCameraAnimated)camera;
							gameCameraAnimated.PlayClip();
						}
						mainCamera.SetGameCamera(camera);
						if (transitionTime > 0f)
						{
							if (linkedCamera is GameCamera25D)
							{
								mainCamera.SnapToAttached();
								Debug.LogWarning("Switching to a 2.5D camera (" + linkedCamera.name + ") must be instantaneous.");
							}
							else
							{
								mainCamera.SmoothChange(transitionTime, moveMethod, timeCurve);
								if (willWait)
								{
									return transitionTime;
								}
							}
						}
						else
						{
							if (!returnToLast)
							{
								linkedCamera.MoveCameraInstant();
							}
							mainCamera.SnapToAttached();
							if (linkedCamera is GameCameraAnimated && willWait)
							{
								return defaultPauseTime;
							}
						}
					}
				}
				return 0f;
			}
			if (linkedCamera is GameCameraAnimated && willWait)
			{
				GameCameraAnimated gameCameraAnimated2 = (GameCameraAnimated)linkedCamera;
				if (gameCameraAnimated2.isPlaying())
				{
					return defaultPauseTime;
				}
				isRunning = false;
				return 0f;
			}
			isRunning = false;
			return 0f;
		}

		public override void Skip()
		{
			MainCamera mainCamera = KickStarter.mainCamera;
			if (!mainCamera)
			{
				return;
			}
			_Camera camera = linkedCamera;
			if (returnToLast && (bool)mainCamera.lastNavCamera)
			{
				camera = ((!(mainCamera.lastNavCamera != null) || !(mainCamera.attachedCamera == mainCamera.lastNavCamera)) ? mainCamera.lastNavCamera : mainCamera.lastNavCamera2);
			}
			if ((bool)camera)
			{
				if (camera is GameCameraThirdPerson)
				{
					GameCameraThirdPerson gameCameraThirdPerson = (GameCameraThirdPerson)camera;
					gameCameraThirdPerson.ResetRotation();
				}
				mainCamera.SetGameCamera(camera);
				if (!returnToLast)
				{
					linkedCamera.MoveCameraInstant();
				}
				mainCamera.SnapToAttached();
			}
		}
	}
}
