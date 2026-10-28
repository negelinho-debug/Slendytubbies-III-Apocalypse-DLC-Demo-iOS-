using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionPlayerLock : Action
	{
		public LockType doUpLock = LockType.NoChange;

		public LockType doDownLock = LockType.NoChange;

		public LockType doLeftLock = LockType.NoChange;

		public LockType doRightLock = LockType.NoChange;

		public PlayerMoveLock doRunLock = PlayerMoveLock.NoChange;

		public LockType freeAimLock = LockType.NoChange;

		public LockType doGravityLock = LockType.NoChange;

		public Paths movePath;

		public ActionPlayerLock()
		{
			isDisplayed = true;
			title = "Player: Constrain";
		}

		public override float Run()
		{
			PlayerInput component = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
			Player player = KickStarter.player;
			if ((bool)component)
			{
				if (IsSingleLockMovement())
				{
					doLeftLock = doUpLock;
					doRightLock = doUpLock;
					doDownLock = doUpLock;
				}
				if (doUpLock == LockType.Disabled)
				{
					component.SetUpLock(true);
				}
				else if (doUpLock == LockType.Enabled)
				{
					component.SetUpLock(false);
				}
				if (doDownLock == LockType.Disabled)
				{
					component.isDownLocked = true;
				}
				else if (doDownLock == LockType.Enabled)
				{
					component.isDownLocked = false;
				}
				if (doLeftLock == LockType.Disabled)
				{
					component.isLeftLocked = true;
				}
				else if (doLeftLock == LockType.Enabled)
				{
					component.isLeftLocked = false;
				}
				if (doRightLock == LockType.Disabled)
				{
					component.isRightLocked = true;
				}
				else if (doRightLock == LockType.Enabled)
				{
					component.isRightLocked = false;
				}
				if (IsInFirstPerson())
				{
					if (freeAimLock == LockType.Disabled)
					{
						component.SetFreeAimLock(true);
					}
					else if (freeAimLock == LockType.Enabled)
					{
						component.SetFreeAimLock(false);
					}
				}
				if (IsUltimateFPS())
				{
					return 0f;
				}
				if (doRunLock != PlayerMoveLock.NoChange)
				{
					component.runLock = doRunLock;
				}
			}
			if ((bool)player)
			{
				if ((bool)movePath)
				{
					player.SetLockedPath(movePath);
					player.SetMoveDirectionAsForward();
				}
				else if ((bool)player.activePath)
				{
					player.EndPath();
				}
				if (doGravityLock == LockType.Enabled)
				{
					player.ignoreGravity = false;
				}
				else if (doGravityLock == LockType.Disabled)
				{
					player.ignoreGravity = true;
				}
			}
			return 0f;
		}

		private bool IsSingleLockMovement()
		{
			if ((bool)AdvGame.GetReferences().settingsManager)
			{
				SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
				if (settingsManager.movementMethod == MovementMethod.PointAndClick || settingsManager.movementMethod == MovementMethod.Drag || settingsManager.movementMethod == MovementMethod.UltimateFPS || settingsManager.movementMethod == MovementMethod.StraightToCursor)
				{
					return true;
				}
			}
			return false;
		}

		private bool IsInFirstPerson()
		{
			if ((bool)AdvGame.GetReferences().settingsManager && AdvGame.GetReferences().settingsManager.IsInFirstPerson())
			{
				return true;
			}
			return false;
		}

		private bool IsUltimateFPS()
		{
			if ((bool)AdvGame.GetReferences().settingsManager && AdvGame.GetReferences().settingsManager.movementMethod == MovementMethod.UltimateFPS)
			{
				return true;
			}
			return false;
		}
	}
}
