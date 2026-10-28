using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionVarCopy : Action
	{
		public int oldParameterID = -1;

		public int oldVariableID;

		public VariableLocation oldLocation;

		public int newParameterID = -1;

		public int newVariableID;

		public VariableLocation newLocation;

		private LocalVariables localVariables;

		private VariablesManager variablesManager;

		public ActionVarCopy()
		{
			isDisplayed = true;
			title = "Variable: Copy";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			oldVariableID = AssignVariableID(parameters, oldParameterID, oldVariableID);
			newVariableID = AssignVariableID(parameters, newParameterID, newVariableID);
		}

		public override float Run()
		{
			if (newVariableID != -1 && oldVariableID != -1)
			{
				GVar oldVar = ((oldLocation != VariableLocation.Global) ? LocalVariables.GetVariable(oldVariableID) : RuntimeVariables.GetVariable(oldVariableID));
				if (newLocation == VariableLocation.Local && !isAssetFile)
				{
					SetVariable(LocalVariables.GetVariable(newVariableID), VariableLocation.Local, oldVar);
				}
				else
				{
					SetVariable(RuntimeVariables.GetVariable(newVariableID), VariableLocation.Global, oldVar);
				}
			}
			return 0f;
		}

		private void SetVariable(GVar newVar, VariableLocation _newLocation, GVar oldVar)
		{
			if (newVar == null || oldVar == null)
			{
				Debug.LogWarning("Cannot copy variable since it cannot be found!");
				return;
			}
			if (oldLocation == VariableLocation.Global)
			{
				oldVar.Download();
			}
			if (newVar.type == VariableType.Integer || newVar.type == VariableType.Boolean)
			{
				int val = oldVar.val;
				newVar.SetValue(val, SetVarMethod.SetValue);
			}
			else if (newVar.type == VariableType.Float)
			{
				float floatVal = oldVar.floatVal;
				newVar.SetValue(floatVal, SetVarMethod.SetValue);
			}
			else if (newVar.type == VariableType.String)
			{
				string textVal = oldVar.textVal;
				newVar.SetValue(textVal);
			}
			if (_newLocation == VariableLocation.Global)
			{
				newVar.Upload();
			}
			GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>().VariableChanged();
		}
	}
}
