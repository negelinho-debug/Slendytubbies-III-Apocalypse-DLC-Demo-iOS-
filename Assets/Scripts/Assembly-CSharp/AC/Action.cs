using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public abstract class Action : ScriptableObject
	{
		public int numSockets = 1;

		public bool willWait;

		public float defaultPauseTime = 0.1f;

		public bool isRunning;

		public int lastResult = -10;

		public int id;

		public bool isDisplayed;

		public string title;

		public ResultAction endAction;

		public int skipAction = -1;

		public Action skipActionActual;

		public Cutscene linkedCutscene;

		public ActionListAsset linkedAsset;

		public bool isMarked;

		public bool isEnabled = true;

		public bool isAssetFile;

		public Rect nodeRect = new Rect(0f, 0f, 300f, 60f);

		public Action()
		{
			isDisplayed = true;
		}

		public virtual float Run()
		{
			return defaultPauseTime;
		}

		public virtual void Skip()
		{
			Run();
		}

		public virtual void ShowGUI(List<ActionParameter> parameters)
		{
			ShowGUI();
		}

		public virtual void ShowGUI()
		{
		}

		public virtual int End(List<Action> actions)
		{
			if (endAction == ResultAction.Stop)
			{
				return -1;
			}
			if (endAction == ResultAction.Skip)
			{
				int num = skipAction;
				if ((bool)skipActionActual && actions.Contains(skipActionActual))
				{
					num = actions.IndexOf(skipActionActual);
				}
				else if (num == -1)
				{
					num = 0;
				}
				return num;
			}
			if (endAction == ResultAction.RunCutscene && ((isAssetFile && linkedAsset != null) || (!isAssetFile && linkedCutscene != null)))
			{
				return -2;
			}
			return -3;
		}

		public virtual void AssignValues(List<ActionParameter> parameters)
		{
			AssignValues();
		}

		public virtual void AssignValues()
		{
		}

		protected ActionParameter GetParameterWithID(List<ActionParameter> parameters, int _id)
		{
			if (parameters != null && _id >= 0)
			{
				foreach (ActionParameter parameter in parameters)
				{
					if (parameter.ID == _id)
					{
						return parameter;
					}
				}
			}
			return null;
		}

		protected string AssignString(List<ActionParameter> parameters, int _parameterID, string field)
		{
			ActionParameter parameterWithID = GetParameterWithID(parameters, _parameterID);
			if (parameterWithID != null && parameterWithID.parameterType == ParameterType.String)
			{
				return parameterWithID.stringValue;
			}
			return field;
		}

		protected BoolValue AssignBoolean(List<ActionParameter> parameters, int _parameterID, BoolValue field)
		{
			ActionParameter parameterWithID = GetParameterWithID(parameters, _parameterID);
			if (parameterWithID != null && parameterWithID.parameterType == ParameterType.Boolean)
			{
				if (parameterWithID.intValue == 1)
				{
					return BoolValue.True;
				}
				return BoolValue.False;
			}
			return field;
		}

		protected int AssignInteger(List<ActionParameter> parameters, int _parameterID, int field)
		{
			ActionParameter parameterWithID = GetParameterWithID(parameters, _parameterID);
			if (parameterWithID != null && parameterWithID.parameterType == ParameterType.Integer)
			{
				return parameterWithID.intValue;
			}
			return field;
		}

		protected float AssignFloat(List<ActionParameter> parameters, int _parameterID, float field)
		{
			ActionParameter parameterWithID = GetParameterWithID(parameters, _parameterID);
			if (parameterWithID != null && parameterWithID.parameterType == ParameterType.Float)
			{
				return parameterWithID.floatValue;
			}
			return field;
		}

		protected int AssignVariableID(List<ActionParameter> parameters, int _parameterID, int field)
		{
			ActionParameter parameterWithID = GetParameterWithID(parameters, _parameterID);
			if (parameterWithID != null && (parameterWithID.parameterType == ParameterType.GlobalVariable || parameterWithID.parameterType == ParameterType.LocalVariable))
			{
				return parameterWithID.intValue;
			}
			return field;
		}

		protected int AssignInvItemID(List<ActionParameter> parameters, int _parameterID, int field)
		{
			ActionParameter parameterWithID = GetParameterWithID(parameters, _parameterID);
			if (parameterWithID != null && parameterWithID.parameterType == ParameterType.InventoryItem)
			{
				return parameterWithID.intValue;
			}
			return field;
		}

		public Transform AssignFile(List<ActionParameter> parameters, int _parameterID, int _constantID, Transform field)
		{
			Transform result = field;
			ActionParameter parameterWithID = GetParameterWithID(parameters, _parameterID);
			if (parameterWithID != null && parameterWithID.parameterType == ParameterType.GameObject)
			{
				if (!isAssetFile && parameterWithID.gameObject != null)
				{
					result = parameterWithID.gameObject.transform;
				}
				else if (parameterWithID.intValue != 0)
				{
					ConstantID constantID = Serializer.returnComponent<ConstantID>(parameterWithID.intValue);
					if (constantID != null)
					{
						result = constantID.gameObject.transform;
					}
				}
			}
			else if (_constantID != 0)
			{
				ConstantID constantID2 = Serializer.returnComponent<ConstantID>(_constantID);
				if (constantID2 != null)
				{
					result = constantID2.gameObject.transform;
				}
			}
			return result;
		}

		protected GameObject AssignFile(List<ActionParameter> parameters, int _parameterID, int _constantID, GameObject field)
		{
			GameObject result = field;
			ActionParameter parameterWithID = GetParameterWithID(parameters, _parameterID);
			if (parameterWithID != null && parameterWithID.parameterType == ParameterType.GameObject)
			{
				if (!isAssetFile && parameterWithID.gameObject != null)
				{
					result = parameterWithID.gameObject;
				}
				else if (parameterWithID.intValue != 0)
				{
					ConstantID constantID = Serializer.returnComponent<ConstantID>(parameterWithID.intValue);
					if (constantID != null)
					{
						result = constantID.gameObject;
					}
				}
			}
			else if (_constantID != 0)
			{
				ConstantID constantID2 = Serializer.returnComponent<ConstantID>(_constantID);
				if (constantID2 != null)
				{
					result = constantID2.gameObject;
				}
			}
			return result;
		}

		public T AssignFile<T>(List<ActionParameter> parameters, int _parameterID, int _constantID, T field) where T : Behaviour
		{
			T result = field;
			ActionParameter parameterWithID = GetParameterWithID(parameters, _parameterID);
			if (parameterWithID != null && parameterWithID.parameterType == ParameterType.GameObject)
			{
				if (!isAssetFile && parameterWithID.gameObject != null && (bool)parameterWithID.gameObject.GetComponent<T>())
				{
					result = parameterWithID.gameObject.GetComponent<T>();
				}
				else if (parameterWithID.intValue != 0)
				{
					result = Serializer.returnComponent<T>(parameterWithID.intValue);
				}
			}
			else if (_constantID != 0)
			{
				result = Serializer.returnComponent<T>(_constantID);
			}
			return result;
		}

		public T AssignFile<T>(int _constantID, T field) where T : Behaviour
		{
			if (_constantID != 0)
			{
				T val = Serializer.returnComponent<T>(_constantID);
				if (val != null)
				{
					return val;
				}
			}
			return field;
		}

		protected GameObject AssignFile(int _constantID, GameObject field)
		{
			if (_constantID != 0)
			{
				ConstantID constantID = Serializer.returnComponent<ConstantID>(_constantID);
				if (constantID != null)
				{
					return constantID.gameObject;
				}
			}
			return field;
		}

		public Transform AssignFile(int _constantID, Transform field)
		{
			if (_constantID != 0)
			{
				ConstantID constantID = Serializer.returnComponent<ConstantID>(_constantID);
				if (constantID != null)
				{
					return constantID.transform;
				}
			}
			return field;
		}
	}
}
