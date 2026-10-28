using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionCharPathFind : Action
	{
		public int charToMoveParameterID = -1;

		public int markerParameterID = -1;

		public int charToMoveID;

		public int markerID;

		public Marker marker;

		public bool isPlayer;

		public Char charToMove;

		public PathSpeed speed;

		public bool pathFind = true;

		public bool doFloat;

		public ActionCharPathFind()
		{
			isDisplayed = true;
			title = "Character: Move to point";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			charToMove = AssignFile(parameters, charToMoveParameterID, charToMoveID, charToMove);
			marker = AssignFile(parameters, markerParameterID, markerID, marker);
			if (isPlayer)
			{
				charToMove = KickStarter.player;
			}
		}

		public override float Run()
		{
			if (!isRunning)
			{
				isRunning = true;
				if ((bool)charToMove && (bool)marker)
				{
					Paths component = charToMove.GetComponent<Paths>();
					if (component == null)
					{
						Debug.LogWarning("Cannot move a character with no Paths component");
					}
					else
					{
						if (charToMove is NPC)
						{
							NPC nPC = (NPC)charToMove;
							nPC.FollowReset();
						}
						component.pathType = AC_PathType.ForwardOnly;
						component.pathSpeed = speed;
						component.affectY = true;
						Vector3 vector = marker.transform.position;
						SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
						if ((bool)settingsManager && settingsManager.ActInScreenSpace())
						{
							vector = AdvGame.GetScreenNavMesh(vector);
						}
						Vector3[] pointData;
						if (pathFind && (bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<NavigationManager>())
						{
							pointData = GameObject.FindWithTag("GameEngine").GetComponent<NavigationManager>().navigationEngine.GetPointsArray(charToMove.transform.position, vector);
						}
						else
						{
							List<Vector3> list = new List<Vector3>();
							list.Add(vector);
							pointData = list.ToArray();
						}
						if (speed == PathSpeed.Walk)
						{
							charToMove.MoveAlongPoints(pointData, false);
						}
						else
						{
							charToMove.MoveAlongPoints(pointData, true);
						}
						if (!pathFind && doFloat)
						{
							charToMove.activePath.affectY = true;
						}
						else
						{
							charToMove.activePath.affectY = false;
						}
						if (willWait)
						{
							return defaultPauseTime;
						}
					}
				}
				return 0f;
			}
			if (charToMove.GetPath() == null)
			{
				isRunning = false;
				return 0f;
			}
			return defaultPauseTime;
		}

		public override void Skip()
		{
			if ((bool)charToMove && (bool)marker)
			{
				charToMove.EndPath();
				if (charToMove is NPC)
				{
					NPC nPC = (NPC)charToMove;
					nPC.FollowReset();
				}
				Vector3 vector = marker.transform.position;
				SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
				if ((bool)settingsManager && settingsManager.ActInScreenSpace())
				{
					vector = AdvGame.GetScreenNavMesh(vector);
				}
				Vector3[] array;
				if (pathFind && (bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<NavigationManager>())
				{
					array = GameObject.FindWithTag("GameEngine").GetComponent<NavigationManager>().navigationEngine.GetPointsArray(charToMove.transform.position, vector);
				}
				else
				{
					List<Vector3> list = new List<Vector3>();
					list.Add(vector);
					array = list.ToArray();
				}
				int num = array.Length - 1;
				if (num > 0)
				{
					charToMove.SetLookDirection(array[num] - array[num - 1], true);
				}
				else
				{
					charToMove.SetLookDirection(array[num] - charToMove.transform.position, true);
				}
				charToMove.Teleport(array[num]);
			}
		}
	}
}
