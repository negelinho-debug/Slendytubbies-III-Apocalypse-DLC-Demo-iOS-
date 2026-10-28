using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class ActionCheck : Action
	{
		public ResultAction resultActionTrue;

		public int skipActionTrue = -1;

		public Action skipActionTrueActual;

		public Cutscene linkedCutsceneTrue;

		public ActionListAsset linkedAssetTrue;

		public ResultAction resultActionFail = ResultAction.Stop;

		public int skipActionFail = -1;

		public Action skipActionFailActual;

		public Cutscene linkedCutsceneFail;

		public ActionListAsset linkedAssetFail;

		public ActionCheck()
		{
			numSockets = 2;
		}

		public override int End(List<Action> actions)
		{
			return ProcessResult(CheckCondition(), actions);
		}

		protected int ProcessResult(bool result, List<Action> actions)
		{
			if (result)
			{
				if (resultActionTrue == ResultAction.Continue)
				{
					return -3;
				}
				if (resultActionTrue == ResultAction.Stop)
				{
					return -1;
				}
				if (resultActionTrue == ResultAction.Skip)
				{
					int num = skipActionTrue;
					if ((bool)skipActionTrueActual && actions.Contains(skipActionTrueActual))
					{
						num = actions.IndexOf(skipActionTrueActual);
					}
					else if (num == -1)
					{
						num = 0;
					}
					return num;
				}
				if (resultActionTrue == ResultAction.RunCutscene)
				{
					if (isAssetFile && linkedAssetTrue != null)
					{
						AdvGame.RunActionListAsset(linkedAssetTrue);
					}
					else if (!isAssetFile && linkedCutsceneTrue != null)
					{
						linkedCutsceneTrue.SendMessage("Interact");
					}
					return -2;
				}
			}
			else
			{
				if (resultActionFail == ResultAction.Continue)
				{
					return -3;
				}
				if (resultActionFail == ResultAction.Stop)
				{
					return -1;
				}
				if (resultActionFail == ResultAction.Skip)
				{
					int num2 = skipActionFail;
					if ((bool)skipActionFailActual && actions.Contains(skipActionFailActual))
					{
						num2 = actions.IndexOf(skipActionFailActual);
					}
					else if (num2 == -1)
					{
						num2 = 0;
					}
					return num2;
				}
				if (resultActionFail == ResultAction.RunCutscene)
				{
					if (isAssetFile && linkedAssetFail != null)
					{
						AdvGame.RunActionListAsset(linkedAssetFail);
					}
					else if (!isAssetFile && linkedCutsceneFail != null)
					{
						linkedCutsceneFail.SendMessage("Interact");
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
