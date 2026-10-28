using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionCheckMultiple : Action
	{
		public List<ActionEnd> endings = new List<ActionEnd>();

		public ActionCheckMultiple()
		{
			numSockets = 2;
		}

		public override int End(List<Action> actions)
		{
			return ProcessResult(-1, actions);
		}

		protected int ProcessResult(int i, List<Action> actions)
		{
			if (endings.Count > i && i >= 0 && endings[i] != null)
			{
				ActionEnd actionEnd = endings[i];
				if (actionEnd.resultAction == ResultAction.Continue)
				{
					return -3;
				}
				if (actionEnd.resultAction == ResultAction.Stop)
				{
					return -1;
				}
				if (actionEnd.resultAction == ResultAction.Skip)
				{
					int num = actionEnd.skipAction;
					if ((bool)actionEnd.skipActionActual && actions.Contains(actionEnd.skipActionActual))
					{
						num = actions.IndexOf(actionEnd.skipActionActual);
					}
					else if (num == -1)
					{
						num = 0;
					}
					return num;
				}
				if (actionEnd.resultAction == ResultAction.RunCutscene)
				{
					if (isAssetFile && actionEnd.linkedAsset != null)
					{
						AdvGame.RunActionListAsset(actionEnd.linkedAsset);
					}
					else if (!isAssetFile && actionEnd.linkedCutscene != null)
					{
						actionEnd.linkedCutscene.SendMessage("Interact");
					}
					return -2;
				}
			}
			return 0;
		}

		public virtual bool CheckCondition()
		{
			return false;
		}
	}
}
