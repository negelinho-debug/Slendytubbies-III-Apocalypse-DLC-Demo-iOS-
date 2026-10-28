using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class LocalVariables : MonoBehaviour
	{
		public List<GVar> localVars = new List<GVar>();

		public static GVar GetVariable(int _id)
		{
			if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<LocalVariables>())
			{
				foreach (GVar localVar in GameObject.FindWithTag("GameEngine").GetComponent<LocalVariables>().localVars)
				{
					if (localVar.id == _id)
					{
						return localVar;
					}
				}
			}
			return null;
		}

		public static List<GVar> GetAllVars()
		{
			if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<LocalVariables>())
			{
				return GameObject.FindWithTag("GameEngine").GetComponent<LocalVariables>().localVars;
			}
			return null;
		}

		public static int GetIntegerValue(int _id)
		{
			return GetVariable(_id).val;
		}

		public static bool GetBooleanValue(int _id)
		{
			if (GetVariable(_id).val == 1)
			{
				return true;
			}
			return false;
		}

		public static string GetStringValue(int _id)
		{
			return GetVariable(_id).textVal;
		}

		public static float GetFloatValue(int _id)
		{
			return GetVariable(_id).floatVal;
		}

		public static void SetIntegerValue(int _id, int _value)
		{
			GetVariable(_id).val = _value;
		}

		public static void SetBooleanValue(int _id, bool _value)
		{
			if (_value)
			{
				GetVariable(_id).val = 1;
			}
			else
			{
				GetVariable(_id).val = 0;
			}
		}

		public static void SetStringValue(int _id, string _value)
		{
			GetVariable(_id).textVal = _value;
		}

		public static void SetFloatValue(int _id, float _value)
		{
			GetVariable(_id).floatVal = _value;
		}
	}
}
