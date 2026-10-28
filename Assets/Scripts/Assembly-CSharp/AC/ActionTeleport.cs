using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AC
{
	[Serializable]
	public class ActionTeleport : Action
	{
		public int obToMoveParameterID = -1;

		public int obToMoveID;

		public int markerParameterID = -1;

		public int markerID;

		public PositionRelativeTo positionRelativeTo;

		public bool isPlayer;

		public GameObject obToMove;

		public Marker teleporter;

		public bool copyRotation;

		public ActionTeleport()
		{
			isDisplayed = true;
			title = "Object: Teleport";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			obToMove = AssignFile(parameters, obToMoveParameterID, obToMoveID, obToMove);
			teleporter = AssignFile(parameters, markerParameterID, markerID, teleporter);
			if (isPlayer && (bool)KickStarter.player)
			{
				obToMove = KickStarter.player.gameObject;
			}
		}

		public override float Run()
		{
			if ((bool)teleporter && (bool)obToMove)
			{
				if (obToMove.GetComponent<NavMeshAgent>() != null)
				{
					obToMove.GetComponent<NavMeshAgent>().enabled = false;
				}
				Vector3 position = teleporter.transform.position;
				Quaternion rotation = teleporter.transform.rotation;
				if (positionRelativeTo == PositionRelativeTo.RelativeToActiveCamera)
				{
					Transform transform = KickStarter.mainCamera.transform;
					float x = teleporter.transform.position.x;
					float y = teleporter.transform.position.y;
					float z = teleporter.transform.position.z;
					position = transform.position + transform.forward * z + transform.right * x + transform.up * y;
					rotation.eulerAngles += transform.transform.rotation.eulerAngles;
				}
				else if (positionRelativeTo == PositionRelativeTo.RelativeToPlayer && !isPlayer && (bool)KickStarter.player)
				{
					Transform transform2 = KickStarter.player.transform;
					float x2 = teleporter.transform.position.x;
					float y2 = teleporter.transform.position.y;
					float z2 = teleporter.transform.position.z;
					position = transform2.position + transform2.forward * z2 + transform2.right * x2 + transform2.up * y2;
					rotation.eulerAngles += transform2.rotation.eulerAngles;
				}
				if ((bool)obToMove.GetComponent<Char>())
				{
					obToMove.GetComponent<Char>().Teleport(position);
				}
				else
				{
					obToMove.transform.position = position;
				}
				if (copyRotation)
				{
					obToMove.transform.rotation = rotation;
					if ((bool)obToMove.GetComponent<Char>())
					{
						obToMove.GetComponent<Char>().SetLookDirection(teleporter.transform.forward, true);
						obToMove.GetComponent<Char>().Halt();
					}
				}
				if (obToMove.GetComponent<NavMeshAgent>() != null)
				{
					obToMove.GetComponent<NavMeshAgent>().enabled = true;
				}
				if (obToMove.GetComponent<NavMeshAgent>() != null)
				{
					obToMove.GetComponent<NavMeshAgent>().Warp(teleporter.transform.position);
				}
			}
			return 0f;
		}
	}
}
