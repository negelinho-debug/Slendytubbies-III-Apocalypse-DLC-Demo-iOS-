using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class GVar
	{
		public string label;

		public int val;

		public float floatVal;

		public string textVal;

		public string[] popUps;

		public VariableType type;

		public int id;

		public bool isEditing;

		public VarLink link;

		public string pmVar;

		public bool updateLinkOnStart;

		public GVar()
		{
			val = 0;
			floatVal = 0f;
			textVal = string.Empty;
			type = VariableType.Boolean;
			id = 0;
			link = VarLink.None;
			pmVar = string.Empty;
			popUps = null;
			updateLinkOnStart = false;
			label = "Variable " + (id + 1);
		}

		public GVar(int[] idArray)
		{
			val = 0;
			floatVal = 0f;
			textVal = string.Empty;
			type = VariableType.Boolean;
			id = 0;
			link = VarLink.None;
			pmVar = string.Empty;
			popUps = null;
			updateLinkOnStart = false;
			foreach (int num in idArray)
			{
				if (id == num)
				{
					id++;
				}
			}
			label = "Variable " + (id + 1);
		}

		public GVar(GVar assetVar)
		{
			val = assetVar.val;
			floatVal = assetVar.floatVal;
			textVal = assetVar.textVal;
			type = assetVar.type;
			id = assetVar.id;
			label = assetVar.label;
			link = assetVar.link;
			pmVar = assetVar.pmVar;
			popUps = assetVar.popUps;
			updateLinkOnStart = assetVar.updateLinkOnStart;
		}

		public void Download()
		{
			if (link != VarLink.PlaymakerGlobalVariable || !(pmVar != string.Empty) || !PlayMakerIntegration.IsDefinePresent())
			{
				return;
			}
			if (type == VariableType.Integer || type == VariableType.PopUp)
			{
				val = PlayMakerIntegration.GetGlobalInt(pmVar);
			}
			else if (type == VariableType.Boolean)
			{
				if (PlayMakerIntegration.GetGlobalBool(pmVar))
				{
					val = 1;
				}
				else
				{
					val = 0;
				}
			}
			else if (type == VariableType.String)
			{
				textVal = PlayMakerIntegration.GetGlobalString(pmVar);
			}
			else if (type == VariableType.Float)
			{
				floatVal = PlayMakerIntegration.GetGlobalFloat(pmVar);
			}
		}

		public void Upload()
		{
			if (link == VarLink.PlaymakerGlobalVariable && pmVar != string.Empty)
			{
				if (!PlayMakerIntegration.IsDefinePresent())
				{
					return;
				}
				if (type == VariableType.Integer || type == VariableType.PopUp)
				{
					PlayMakerIntegration.SetGlobalInt(pmVar, val);
				}
				else if (type == VariableType.Boolean)
				{
					if (val == 1)
					{
						PlayMakerIntegration.SetGlobalBool(pmVar, true);
					}
					else
					{
						PlayMakerIntegration.SetGlobalBool(pmVar, false);
					}
				}
				else if (type == VariableType.String)
				{
					PlayMakerIntegration.SetGlobalString(pmVar, textVal);
				}
				else if (type == VariableType.Float)
				{
					PlayMakerIntegration.SetGlobalFloat(pmVar, floatVal);
				}
			}
			else if (link == VarLink.OptionsData)
			{
				GameObject.FindWithTag("PersistentEngine").GetComponent<Options>().SavePrefs();
			}
		}

		public void SetValue(string newValue)
		{
			textVal = newValue;
		}

		public void SetValue(float newValue, SetVarMethod setVarMethod)
		{
			switch (setVarMethod)
			{
			case SetVarMethod.IncreaseByValue:
				floatVal += newValue;
				break;
			case SetVarMethod.SetAsRandom:
				floatVal = UnityEngine.Random.Range(0f, newValue);
				break;
			default:
				floatVal = newValue;
				break;
			}
		}

		public void SetValue(int newValue)
		{
			val = newValue;
			if (type == VariableType.PopUp)
			{
				if (val < 0)
				{
					val = 0;
				}
				else if (val >= popUps.Length)
				{
					val = popUps.Length - 1;
				}
			}
		}

		public void SetValue(int newValue, SetVarMethod setVarMethod)
		{
			switch (setVarMethod)
			{
			case SetVarMethod.IncreaseByValue:
				val += newValue;
				break;
			case SetVarMethod.SetAsRandom:
				val = UnityEngine.Random.Range(0, newValue);
				break;
			default:
				val = newValue;
				break;
			}
			if (type == VariableType.Boolean)
			{
				if (val > 0)
				{
					val = 1;
				}
				else
				{
					val = 0;
				}
			}
		}

		public string GetValue()
		{
			if (type == VariableType.Integer)
			{
				return val.ToString();
			}
			if (type == VariableType.PopUp)
			{
				if (popUps == null || popUps.Length == 0)
				{
					return string.Empty;
				}
				val = Mathf.Max(0, val);
				val = Mathf.Min(val, popUps.Length - 1);
				return popUps[val];
			}
			if (type == VariableType.String)
			{
				return textVal;
			}
			if (type == VariableType.Float)
			{
				return floatVal.ToString();
			}
			if (val == 0)
			{
				return "False";
			}
			return "True";
		}
	}
}
