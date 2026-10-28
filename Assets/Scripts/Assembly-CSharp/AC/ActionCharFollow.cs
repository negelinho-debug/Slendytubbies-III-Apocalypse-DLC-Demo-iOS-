using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionCharFollow : Action
	{
		public enum FollowType
		{
			StartFollowing,
			StopFollowing
		}

		public int npcToMoveParameterID = -1;

		public int charToFollowParameterID = -1;

		public int npcToMoveID;

		public int charToFollowID;

		public NPC npcToMove;

		public Char charToFollow;

		public bool followPlayer;

		public float updateFrequency = 2f;

		public float followDistance = 1f;

		public float followDistanceMax = 15f;

		public FollowType followType;

		public ActionCharFollow()
		{
			isDisplayed = true;
			title = "Character: NPC follow";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			npcToMove = AssignFile(parameters, npcToMoveParameterID, npcToMoveID, npcToMove);
			charToFollow = AssignFile(parameters, charToFollowParameterID, charToFollowID, charToFollow);
		}

		public override float Run()
		{
			if ((bool)npcToMove)
			{
				if (followType == FollowType.StopFollowing)
				{
					npcToMove.FollowReset();
					return 0f;
				}
				if (followPlayer || charToFollow != npcToMove)
				{
					npcToMove.FollowAssign(charToFollow, followPlayer, updateFrequency, followDistance, followDistanceMax);
				}
			}
			return 0f;
		}
	}
}
