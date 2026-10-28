using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionSystemLock : Action
	{
		public bool changeMovementMethod;

		public MovementMethod newMovementMethod;

		public LockType cursorLock = LockType.NoChange;

		public LockType inputLock = LockType.NoChange;

		public LockType interactionLock = LockType.NoChange;

		public LockType menuLock = LockType.NoChange;

		public LockType movementLock = LockType.NoChange;

		public LockType cameraLock = LockType.NoChange;

		public LockType triggerLock = LockType.NoChange;

		public LockType playerLock = LockType.NoChange;

		public LockType saveLock = LockType.NoChange;

		public ActionSystemLock()
		{
			isDisplayed = true;
			title = "Engine: Manage systems";
		}

		public override float Run()
		{
			if (changeMovementMethod)
			{
				AdvGame.GetReferences().settingsManager.movementMethod = newMovementMethod;
			}
			StateHandler component = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
			if (cursorLock == LockType.Enabled)
			{
				component.cursorIsOff = false;
			}
			else if (cursorLock == LockType.Disabled)
			{
				component.cursorIsOff = true;
			}
			if (inputLock == LockType.Enabled)
			{
				component.inputIsOff = false;
			}
			else if (inputLock == LockType.Disabled)
			{
				component.inputIsOff = true;
			}
			if (interactionLock == LockType.Enabled)
			{
				component.interactionIsOff = false;
			}
			else if (interactionLock == LockType.Disabled)
			{
				component.interactionIsOff = true;
			}
			if (menuLock == LockType.Enabled)
			{
				component.menuIsOff = false;
			}
			else if (menuLock == LockType.Disabled)
			{
				component.menuIsOff = true;
			}
			if (movementLock == LockType.Enabled)
			{
				component.movementIsOff = false;
			}
			else if (movementLock == LockType.Disabled)
			{
				component.movementIsOff = true;
			}
			if (cameraLock == LockType.Enabled)
			{
				component.cameraIsOff = false;
			}
			else if (cameraLock == LockType.Disabled)
			{
				component.cameraIsOff = true;
			}
			if (triggerLock == LockType.Enabled)
			{
				component.triggerIsOff = false;
			}
			else if (triggerLock == LockType.Disabled)
			{
				component.triggerIsOff = true;
			}
			if (playerLock == LockType.Enabled)
			{
				component.playerIsOff = false;
			}
			else if (playerLock == LockType.Disabled)
			{
				component.playerIsOff = true;
			}
			if (saveLock == LockType.Disabled)
			{
				GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>().lockSave = true;
			}
			else if (saveLock == LockType.Enabled)
			{
				GameObject.FindWithTag("PersistentEngine").GetComponent<PlayerMenus>().lockSave = false;
			}
			return 0f;
		}
	}
}
