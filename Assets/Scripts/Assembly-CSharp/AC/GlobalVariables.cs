using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class GlobalVariables : MonoBehaviour
	{
		public static List<GVar> GetAllVars()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeVariables>())
			{
				return GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeVariables>().globalVars;
			}
			return null;
		}

		public static void UploadAll()
		{
			if (!GameObject.FindWithTag("PersistentEngine") || !GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeVariables>())
			{
				return;
			}
			foreach (GVar globalVar in GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeVariables>().globalVars)
			{
				globalVar.Upload();
			}
		}

		public static void DownloadAll()
		{
			if (!GameObject.FindWithTag("PersistentEngine") || !GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeVariables>())
			{
				return;
			}
			foreach (GVar globalVar in GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeVariables>().globalVars)
			{
				globalVar.Download();
			}
		}

		public static GVar GetVariable(int _id)
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeVariables>())
			{
				foreach (GVar globalVar in GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeVariables>().globalVars)
				{
					if (globalVar.id == _id)
					{
						return globalVar;
					}
				}
			}
			return null;
		}

		public static int GetIntegerValue(int _id)
		{
			return RuntimeVariables.GetVariable(_id).val;
		}

		public static bool GetBooleanValue(int _id)
		{
			if (RuntimeVariables.GetVariable(_id).val == 1)
			{
				return true;
			}
			return false;
		}

		public static string GetStringValue(int _id)
		{
			return RuntimeVariables.GetVariable(_id).textVal;
		}

		public static float GetFloatValue(int _id)
		{
			return RuntimeVariables.GetVariable(_id).floatVal;
		}

		public static void SetIntegerValue(int _id, int _value)
		{
			RuntimeVariables.GetVariable(_id).val = _value;
		}

		public static void SetBooleanValue(int _id, bool _value)
		{
			if (_value)
			{
				RuntimeVariables.GetVariable(_id).val = 1;
			}
			else
			{
				RuntimeVariables.GetVariable(_id).val = 0;
			}
		}

		public static void SetStringValue(int _id, string _value)
		{
			RuntimeVariables.GetVariable(_id).textVal = _value;
		}

		public static void SetFloatValue(int _id, float _value)
		{
			RuntimeVariables.GetVariable(_id).floatVal = _value;
		}
	}
}
