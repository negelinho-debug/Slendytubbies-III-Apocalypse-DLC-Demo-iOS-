using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionVarSet : Action
	{
		public SetVarMethod setVarMethod;

		public SetVarMethodString setVarMethodString;

		public SetVarMethodIntBool setVarMethodIntBool;

		public int parameterID = -1;

		public int variableID;

		public int variableNumber;

		public int setParameterID = -1;

		public int intValue;

		public float floatValue;

		public BoolValue boolValue;

		public string stringValue;

		public string formula;

		public VariableLocation location;

		public string menuName;

		public string elementName;

		public Animator animator;

		public string parameterName;

		private LocalVariables localVariables;

		private VariablesManager variablesManager;

		public ActionVarSet()
		{
			isDisplayed = true;
			title = "Variable: Set";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			variableID = AssignVariableID(parameters, parameterID, variableID);
			intValue = AssignInteger(parameters, setParameterID, intValue);
			boolValue = AssignBoolean(parameters, setParameterID, boolValue);
			floatValue = AssignFloat(parameters, setParameterID, floatValue);
			stringValue = AssignString(parameters, setParameterID, stringValue);
			formula = AssignString(parameters, setParameterID, formula);
		}

		public override float Run()
		{
			if (variableID != -1)
			{
				if (location == VariableLocation.Local && !isAssetFile)
				{
					SetVariable(LocalVariables.GetVariable(variableID), VariableLocation.Local);
				}
				else
				{
					SetVariable(RuntimeVariables.GetVariable(variableID), VariableLocation.Global);
				}
			}
			return 0f;
		}

		private void SetVariable(GVar var, VariableLocation location)
		{
			if (var == null)
			{
				return;
			}
			if (location == VariableLocation.Global)
			{
				var.Download();
			}
			if (var.type == VariableType.Integer)
			{
				int newValue = 0;
				if (setVarMethodIntBool == SetVarMethodIntBool.EnteredHere)
				{
					newValue = ((setVarMethod != SetVarMethod.Formula) ? intValue : ((int)AdvGame.CalculateFormula(AdvGame.ConvertTokens(formula))));
				}
				else if (setVarMethodIntBool == SetVarMethodIntBool.SetAsMecanimParameter && (bool)animator && parameterName != string.Empty)
				{
					newValue = animator.GetInteger(parameterName);
					setVarMethod = SetVarMethod.SetValue;
				}
				var.SetValue(newValue, setVarMethod);
			}
			if (var.type == VariableType.Float)
			{
				float newValue2 = 0f;
				if (setVarMethodIntBool == SetVarMethodIntBool.EnteredHere)
				{
					newValue2 = ((setVarMethod != SetVarMethod.Formula) ? floatValue : ((float)AdvGame.CalculateFormula(AdvGame.ConvertTokens(formula))));
				}
				else if (setVarMethodIntBool == SetVarMethodIntBool.SetAsMecanimParameter && (bool)animator && parameterName != string.Empty)
				{
					newValue2 = animator.GetFloat(parameterName);
					setVarMethod = SetVarMethod.SetValue;
				}
				var.SetValue(newValue2, setVarMethod);
			}
			else if (var.type == VariableType.Boolean)
			{
				int newValue3 = 0;
				if (setVarMethodIntBool == SetVarMethodIntBool.EnteredHere)
				{
					newValue3 = (int)boolValue;
				}
				else if (setVarMethodIntBool == SetVarMethodIntBool.SetAsMecanimParameter && (bool)animator && parameterName != string.Empty && animator.GetBool(parameterName))
				{
					newValue3 = 1;
				}
				var.SetValue(newValue3, SetVarMethod.SetValue);
			}
			else if (var.type == VariableType.PopUp)
			{
				var.SetValue(intValue);
			}
			else if (var.type == VariableType.String)
			{
				string text = string.Empty;
				if (setVarMethodString == SetVarMethodString.EnteredHere)
				{
					text = AdvGame.ConvertTokens(stringValue);
				}
				else if (setVarMethodString == SetVarMethodString.SetAsMenuInputLabel)
				{
					if (PlayerMenus.GetElementWithName(menuName, elementName) != null)
					{
						MenuInput menuInput = (MenuInput)PlayerMenus.GetElementWithName(menuName, elementName);
						text = menuInput.label;
						if ((Options.GetLanguageName() == "Arabic" || Options.GetLanguageName() == "Hebrew") && text.Length > 0)
						{
							char[] array = text.ToCharArray();
							text = string.Empty;
							for (int num = array.Length - 1; num >= 0; num--)
							{
								text += array[num];
							}
						}
					}
					else
					{
						Debug.LogWarning("Could not find MenuInput '" + elementName + "' in Menu '" + menuName + "'");
					}
				}
				var.SetValue(text);
			}
			if (location == VariableLocation.Global)
			{
				var.Upload();
			}
			GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>().VariableChanged();
		}
	}
}
