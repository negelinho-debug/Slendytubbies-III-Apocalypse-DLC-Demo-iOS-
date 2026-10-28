using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionVarCheck : ActionCheck
	{
		public enum BoolCondition
		{
			EqualTo,
			NotEqualTo
		}

		public int parameterID = -1;

		public int variableID;

		public int variableNumber;

		public int checkParameterID = -1;

		public GetVarMethod getVarMethod;

		public int compareVariableID;

		public int intValue;

		public float floatValue;

		public IntCondition intCondition;

		public bool isAdditive;

		public BoolValue boolValue = BoolValue.True;

		public BoolCondition boolCondition;

		public string stringValue;

		public VariableLocation location;

		private LocalVariables localVariables;

		private VariablesManager variablesManager;

		public ActionVarCheck()
		{
			isDisplayed = true;
			title = "Variable: Check";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			variableID = AssignVariableID(parameters, parameterID, variableID);
			intValue = AssignInteger(parameters, checkParameterID, intValue);
			boolValue = AssignBoolean(parameters, checkParameterID, boolValue);
			floatValue = AssignFloat(parameters, checkParameterID, floatValue);
			stringValue = AssignString(parameters, checkParameterID, stringValue);
			compareVariableID = AssignVariableID(parameters, checkParameterID, compareVariableID);
		}

		public override int End(List<Action> actions)
		{
			if (variableID == -1)
			{
				return 0;
			}
			GVar gVar = null;
			if (getVarMethod == GetVarMethod.GlobalVariable || getVarMethod == GetVarMethod.LocalVariable)
			{
				if (compareVariableID == -1)
				{
					return 0;
				}
				if (getVarMethod == GetVarMethod.GlobalVariable)
				{
					gVar = RuntimeVariables.GetVariable(compareVariableID);
					gVar.Download();
				}
				else if (getVarMethod == GetVarMethod.LocalVariable && !isAssetFile)
				{
					gVar = LocalVariables.GetVariable(compareVariableID);
				}
			}
			if (location == VariableLocation.Local && !isAssetFile)
			{
				return ProcessResult(CheckCondition(LocalVariables.GetVariable(variableID), gVar), actions);
			}
			GVar variable = RuntimeVariables.GetVariable(variableID);
			if (variable != null)
			{
				variable.Download();
				return ProcessResult(CheckCondition(variable, gVar), actions);
			}
			return -1;
		}

		private bool CheckCondition(GVar _var, GVar _compareVar)
		{
			if (_var == null)
			{
				Debug.LogWarning("Cannot check state of variable since it cannot be found!");
				return false;
			}
			if (_compareVar != null && _var != null && _compareVar.type != _var.type)
			{
				Debug.LogWarning("Cannot compare " + _var.label + " and " + _compareVar.label + " as they are not the same type!");
				return false;
			}
			if (_var.type == VariableType.Boolean)
			{
				int val = _var.val;
				int val2 = (int)boolValue;
				if (_compareVar != null)
				{
					val2 = _compareVar.val;
				}
				if (boolCondition == BoolCondition.EqualTo)
				{
					if (val == val2)
					{
						return true;
					}
				}
				else if (val != val2)
				{
					return true;
				}
			}
			else if (_var.type == VariableType.Integer || _var.type == VariableType.PopUp)
			{
				int val3 = _var.val;
				int val4 = intValue;
				if (_compareVar != null)
				{
					val4 = _compareVar.val;
				}
				if (intCondition == IntCondition.EqualTo)
				{
					if (val3 == val4)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.NotEqualTo)
				{
					if (val3 != val4)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.LessThan)
				{
					if (val3 < val4)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.MoreThan && val3 > val4)
				{
					return true;
				}
			}
			else if (_var.type == VariableType.Float)
			{
				float floatVal = _var.floatVal;
				float floatVal2 = floatValue;
				if (_compareVar != null)
				{
					floatVal2 = _compareVar.floatVal;
				}
				if (intCondition == IntCondition.EqualTo)
				{
					if (floatVal == floatVal2)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.NotEqualTo)
				{
					if (floatVal != floatVal2)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.LessThan)
				{
					if (floatVal < floatVal2)
					{
						return true;
					}
				}
				else if (intCondition == IntCondition.MoreThan && floatVal > floatVal2)
				{
					return true;
				}
			}
			else if (_var.type == VariableType.String)
			{
				string textVal = _var.textVal;
				string text = AdvGame.ConvertTokens(stringValue);
				if (_compareVar != null)
				{
					text = _compareVar.textVal;
				}
				if (boolCondition == BoolCondition.EqualTo)
				{
					if (textVal == text)
					{
						return true;
					}
				}
				else if (textVal != text)
				{
					return true;
				}
			}
			return false;
		}

		private int GetVarNumber(List<GVar> vars, int ID)
		{
			int num = 0;
			foreach (GVar var in vars)
			{
				if (var.id == ID)
				{
					return num;
				}
				num++;
			}
			return -1;
		}
	}
}
