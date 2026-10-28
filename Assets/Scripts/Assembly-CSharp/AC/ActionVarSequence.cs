using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionVarSequence : ActionCheckMultiple
	{
		public int parameterID = -1;

		public int variableID;

		public int variableNumber;

		public bool doLoop;

		public VariableLocation location;

		private LocalVariables localVariables;

		private VariablesManager variablesManager;

		public ActionVarSequence()
		{
			isDisplayed = true;
			title = "Variable: Run sequence";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			variableID = AssignVariableID(parameters, parameterID, variableID);
		}

		public override int End(List<Action> actions)
		{
			if (numSockets <= 0)
			{
				Debug.LogWarning("Could not compute Random check because no values were possible!");
				return -1;
			}
			if (variableID == -1)
			{
				return 0;
			}
			GVar gVar = null;
			gVar = ((location != VariableLocation.Local || isAssetFile) ? RuntimeVariables.GetVariable(variableID) : LocalVariables.GetVariable(variableID));
			if (gVar != null)
			{
				if (gVar.type == VariableType.Integer)
				{
					gVar.Download();
					if (gVar.val < 1)
					{
						gVar.val = 1;
					}
					int i = gVar.val - 1;
					gVar.val++;
					if (gVar.val > numSockets)
					{
						if (doLoop)
						{
							gVar.val = 1;
						}
						else
						{
							gVar.val = numSockets;
						}
					}
					gVar.Upload();
					return ProcessResult(i, actions);
				}
				Debug.LogWarning("Variable: Run sequence Action is referencing a Variable that does not exist!");
			}
			return 0;
		}
	}
}
