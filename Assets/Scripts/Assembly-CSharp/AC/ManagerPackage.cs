using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ManagerPackage : ScriptableObject
	{
		public ActionsManager actionsManager;

		public SceneManager sceneManager;

		public SettingsManager settingsManager;

		public InventoryManager inventoryManager;

		public VariablesManager variablesManager;

		public SpeechManager speechManager;

		public CursorManager cursorManager;

		public MenuManager menuManager;

		public void AssignManagers()
		{
			if (AdvGame.GetReferences() != null)
			{
				if ((bool)actionsManager)
				{
					AdvGame.GetReferences().sceneManager = sceneManager;
				}
				if ((bool)actionsManager)
				{
					AdvGame.GetReferences().settingsManager = settingsManager;
				}
				if ((bool)actionsManager)
				{
					AdvGame.GetReferences().actionsManager = actionsManager;
				}
				if ((bool)actionsManager)
				{
					AdvGame.GetReferences().variablesManager = variablesManager;
				}
				if ((bool)actionsManager)
				{
					AdvGame.GetReferences().inventoryManager = inventoryManager;
				}
				if ((bool)actionsManager)
				{
					AdvGame.GetReferences().speechManager = speechManager;
				}
				if ((bool)actionsManager)
				{
					AdvGame.GetReferences().cursorManager = cursorManager;
				}
				if ((bool)actionsManager)
				{
					AdvGame.GetReferences().menuManager = menuManager;
				}
				Debug.Log("Managers assigned.");
			}
			else
			{
				Debug.LogError("Can't assign managers - no References file found in Resources folder.");
			}
		}
	}
}
