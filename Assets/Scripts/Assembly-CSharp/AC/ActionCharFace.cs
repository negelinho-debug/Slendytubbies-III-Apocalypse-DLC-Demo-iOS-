using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionCharFace : Action
	{
		public int charToMoveParameterID = -1;

		public int faceObjectParameterID = -1;

		public int charToMoveID;

		public int faceObjectID;

		public bool isInstant;

		public Char charToMove;

		public GameObject faceObject;

		public bool copyRotation;

		public bool facePlayer;

		public CharFaceType faceType;

		public bool isPlayer;

		public bool lookUpDown;

		public bool stopLooking;

		public ActionCharFace()
		{
			isDisplayed = true;
			title = "Character: Face object";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			charToMove = AssignFile(parameters, charToMoveParameterID, charToMoveID, charToMove);
			faceObject = AssignFile(parameters, faceObjectParameterID, faceObjectID, faceObject);
			if (isPlayer)
			{
				charToMove = KickStarter.player;
			}
			else if (facePlayer && (bool)KickStarter.player)
			{
				faceObject = KickStarter.player.gameObject;
			}
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				if (faceObject == null && (faceType == CharFaceType.Body || (faceType == CharFaceType.Head && !stopLooking)))
				{
					return 0f;
				}
				if ((bool)charToMove)
				{
					if (faceType == CharFaceType.Body)
					{
						SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
						if (lookUpDown && isPlayer && (bool)settingsManager && settingsManager.IsInFirstPerson())
						{
							Player player = (Player)charToMove;
							player.SetTilt(faceObject.transform.position, isInstant);
						}
						charToMove.SetLookDirection(GetLookVector(settingsManager), isInstant);
					}
					else if (faceType == CharFaceType.Head)
					{
						if (stopLooking)
						{
							charToMove.ClearHeadTurnTarget(HeadFacing.Manual, isInstant);
						}
						else
						{
							Vector3 position = faceObject.transform.position;
							if ((bool)faceObject.GetComponent<Hotspot>())
							{
								position = faceObject.GetComponent<Hotspot>().GetIconPosition();
							}
							charToMove.SetHeadTurnTarget(position, isInstant);
						}
					}
					if (isInstant)
					{
						return 0f;
					}
					if (faceType == CharFaceType.Body)
					{
						charToMove.Halt();
					}
					if (willWait)
					{
						return defaultPauseTime;
					}
					return 0f;
				}
				return 0f;
			}
			if (faceType == CharFaceType.Head && charToMove.IsMovingHead())
			{
				return defaultPauseTime;
			}
			if (faceType == CharFaceType.Body && charToMove.IsTurning())
			{
				return defaultPauseTime;
			}
			isRunning = false;
			return 0f;
		}

		public override void Skip()
		{
			if ((faceObject == null && (faceType == CharFaceType.Body || (faceType == CharFaceType.Head && !stopLooking))) || !charToMove)
			{
				return;
			}
			if (faceType == CharFaceType.Body)
			{
				SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
				charToMove.SetLookDirection(GetLookVector(settingsManager), true);
				if (lookUpDown && isPlayer && (bool)settingsManager && settingsManager.IsInFirstPerson())
				{
					Player player = (Player)charToMove;
					player.SetTilt(faceObject.transform.position, true);
				}
			}
			else if (faceType == CharFaceType.Head)
			{
				if (stopLooking)
				{
					charToMove.ClearHeadTurnTarget(HeadFacing.Manual, true);
				}
				else
				{
					charToMove.SetHeadTurnTarget(faceObject.transform.position, true);
				}
			}
		}

		private Vector3 GetLookVector(SettingsManager settingsManager)
		{
			Vector3 result = faceObject.transform.position - charToMove.transform.position;
			if (copyRotation)
			{
				result = faceObject.transform.forward;
			}
			else if (settingsManager.ActInScreenSpace())
			{
				result = AdvGame.GetScreenDirection(charToMove.transform.position, faceObject.transform.position);
			}
			return result;
		}
	}
}
