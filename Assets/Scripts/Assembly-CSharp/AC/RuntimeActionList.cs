using UnityEngine;

namespace AC
{
	public class RuntimeActionList : ActionList
	{
		public ActionListAsset assetSource;

		public void DownloadActions(ActionListAsset actionListAsset, Conversation endConversation, int i)
		{
			name = actionListAsset.name;
			assetSource = actionListAsset;
			useParameters = actionListAsset.useParameters;
			parameters = actionListAsset.parameters;
			unfreezePauseMenus = actionListAsset.unfreezePauseMenus;
			actionListType = actionListAsset.actionListType;
			if (actionListAsset.actionListType == ActionListType.PauseGameplay)
			{
				isSkippable = actionListAsset.isSkippable;
			}
			else
			{
				isSkippable = false;
			}
			conversation = endConversation;
			actions.Clear();
			foreach (Action action in actionListAsset.actions)
			{
				actions.Add(action);
			}
			if (!useParameters)
			{
				foreach (Action action2 in actions)
				{
					action2.AssignValues(null);
				}
			}
			Interact(i);
		}

		public void DestroySelf()
		{
			Object.Destroy(gameObject);
		}
	}
}
