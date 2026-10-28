using System;

namespace AC
{
	[Serializable]
	public struct MainData
	{
		public int currentPlayerID;

		public float timeScale;

		public string runtimeVariablesData;

		public string menuLockData;

		public string menuElementVisibilityData;

		public string menuJournalData;

		public int activeArrows;

		public int activeConversation;

		public int selectedInventoryID;

		public bool cursorIsOff;

		public bool inputIsOff;

		public bool interactionIsOff;

		public bool menuIsOff;

		public bool movementIsOff;

		public bool cameraIsOff;

		public bool triggerIsOff;

		public bool playerIsOff;
	}
}
