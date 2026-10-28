using System;

namespace AC
{
	[Serializable]
	public class NPCData
	{
		public int objectID;

		public bool isOn;

		public float LocX;

		public float LocY;

		public float LocZ;

		public float RotX;

		public float RotY;

		public float RotZ;

		public float ScaleX;

		public float ScaleY;

		public float ScaleZ;

		public string idleAnim;

		public string walkAnim;

		public string talkAnim;

		public string runAnim;

		public int walkSound;

		public int runSound;

		public int portraitGraphic;

		public float walkSpeed;

		public float runSpeed;

		public bool lockDirection;

		public string spriteDirection;

		public bool lockScale;

		public float spriteScale;

		public bool lockSorting;

		public int sortingOrder;

		public string sortingLayer;

		public int pathID;

		public int targetNode;

		public int prevNode;

		public string pathData;

		public bool isRunning;

		public int lastPathID;

		public int lastTargetNode;

		public int lastPrevNode;

		public int followTargetID;

		public bool followTargetIsPlayer;

		public float followFrequency;

		public float followDistance;

		public float followDistanceMax;

		public bool isHeadTurning;

		public float headTargetX;

		public float headTargetY;

		public float headTargetZ;

		public string speechLabel;
	}
}
