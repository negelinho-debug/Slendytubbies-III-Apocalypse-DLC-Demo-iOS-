using System;

namespace AC
{
	[Serializable]
	public class ActionEnd
	{
		public ResultAction resultAction;

		public int skipAction;

		public Action skipActionActual;

		public Cutscene linkedCutscene;

		public ActionListAsset linkedAsset;

		public ActionEnd()
		{
			resultAction = ResultAction.Continue;
			skipAction = -1;
			skipActionActual = null;
			linkedCutscene = null;
			linkedAsset = null;
		}
	}
}
