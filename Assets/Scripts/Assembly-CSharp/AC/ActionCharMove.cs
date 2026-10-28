using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionCharMove : Action
	{
		public enum MovePathMethod
		{
			MoveOnNewPath,
			StopMoving,
			ResumeLastSetPath
		}

		public MovePathMethod movePathMethod;

		public int charToMoveParameterID = -1;

		public int movePathParameterID = -1;

		public int charToMoveID;

		public int movePathID;

		public Paths movePath;

		public bool isPlayer;

		public Char charToMove;

		public bool doTeleport;

		public bool doStop;

		public ActionCharMove()
		{
			isDisplayed = true;
			title = "Character: Move along path";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			charToMove = AssignFile(parameters, charToMoveParameterID, charToMoveID, charToMove);
			movePath = AssignFile(parameters, movePathParameterID, movePathID, movePath);
			if (isPlayer)
			{
				charToMove = KickStarter.player;
			}
		}

		private void UpgradeSelf()
		{
			if (doStop)
			{
				doStop = false;
				movePathMethod = MovePathMethod.StopMoving;
				if (Application.isPlaying)
				{
					Debug.Log("'Character: Move along path' Action has been temporarily upgraded - - please view it's Inspector when the game ends and save the scene.");
				}
				else
				{
					Debug.Log("Upgraded 'Character: Move along path' Action, please save the scene.");
				}
			}
		}

		public override float Run()
		{
			UpgradeSelf();
			if ((bool)movePath && (bool)movePath.GetComponent<Char>())
			{
				Debug.LogWarning("Can't follow a Path attached to a Character!");
				return 0f;
			}
			if (!isRunning)
			{
				isRunning = true;
				if ((bool)charToMove)
				{
					if (charToMove is NPC)
					{
						NPC nPC = (NPC)charToMove;
						nPC.FollowReset();
					}
					if (movePathMethod == MovePathMethod.StopMoving)
					{
						charToMove.EndPath();
					}
					else if (movePathMethod == MovePathMethod.MoveOnNewPath)
					{
						if ((bool)movePath)
						{
							if (doTeleport)
							{
								charToMove.Teleport(movePath.transform.position);
								if (movePath.nodes.Count > 1)
								{
									charToMove.SetLookDirection(movePath.nodes[1] - movePath.nodes[0], true);
								}
							}
							if (isPlayer && movePath.pathType != AC_PathType.ForwardOnly)
							{
								Debug.LogWarning("Cannot move player along a non-forward only path, as this will create an indefinite cutscene.");
							}
							else
							{
								if (willWait && movePath.pathType != AC_PathType.ForwardOnly)
								{
									willWait = false;
									Debug.LogWarning("Cannot pause while character moves along a non-forward only path, as this will create an indefinite cutscene.");
								}
								charToMove.SetPath(movePath);
								if (willWait)
								{
									return defaultPauseTime;
								}
							}
						}
					}
					else if (movePathMethod == MovePathMethod.ResumeLastSetPath)
					{
						charToMove.ResumeLastPath();
					}
				}
				return 0f;
			}
			if (charToMove.GetPath() != movePath)
			{
				isRunning = false;
				return 0f;
			}
			return defaultPauseTime;
		}

		public override void Skip()
		{
			if (!charToMove)
			{
				return;
			}
			if (charToMove is NPC)
			{
				NPC nPC = (NPC)charToMove;
				nPC.FollowReset();
			}
			if (doStop)
			{
				charToMove.EndPath();
			}
			else
			{
				if (!movePath)
				{
					return;
				}
				if (movePath.pathType == AC_PathType.ForwardOnly)
				{
					int num = movePath.nodes.Count - 1;
					charToMove.transform.position = movePath.nodes[num];
					if (num > 0)
					{
						charToMove.SetLookDirection(movePath.nodes[num] - movePath.nodes[num - 1], true);
					}
					return;
				}
				if (doTeleport)
				{
					charToMove.transform.position = movePath.transform.position;
					if (movePath.nodes.Count > 1)
					{
						charToMove.SetLookDirection(movePath.nodes[1] - movePath.nodes[0], true);
					}
				}
				if (!isPlayer)
				{
					charToMove.SetPath(movePath);
				}
			}
		}
	}
}
