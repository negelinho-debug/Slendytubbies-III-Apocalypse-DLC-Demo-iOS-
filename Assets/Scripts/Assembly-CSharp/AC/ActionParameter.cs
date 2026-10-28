using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionParameter
	{
		public string label = string.Empty;

		public int ID;

		public int intValue = -1;

		public float floatValue;

		public string stringValue = string.Empty;

		public GameObject gameObject;

		public ParameterType parameterType;

		public ActionParameter(int[] idArray)
		{
			label = string.Empty;
			ID = 0;
			intValue = -1;
			floatValue = 0f;
			stringValue = string.Empty;
			gameObject = null;
			parameterType = ParameterType.GameObject;
			foreach (int num in idArray)
			{
				if (ID == num)
				{
					ID++;
				}
			}
			label = "Parameter " + (ID + 1);
		}

		public ActionParameter(int id)
		{
			label = string.Empty;
			ID = id;
			intValue = -1;
			floatValue = 0f;
			stringValue = string.Empty;
			gameObject = null;
			parameterType = ParameterType.GameObject;
			label = "Parameter " + (ID + 1);
		}

		public void Reset()
		{
			intValue = -1;
			floatValue = 0f;
			stringValue = string.Empty;
			gameObject = null;
		}

		public void SetValue(int _value)
		{
			intValue = _value;
			floatValue = 0f;
			stringValue = string.Empty;
			gameObject = null;
		}

		public void SetValue(float _value)
		{
			floatValue = _value;
			stringValue = string.Empty;
			intValue = -1;
			gameObject = null;
		}

		public void SetValue(string _value)
		{
			stringValue = _value;
			floatValue = 0f;
			intValue = -1;
			gameObject = null;
		}

		public void SetValue(bool _value)
		{
			stringValue = string.Empty;
			floatValue = 0f;
			intValue = -1;
			gameObject = null;
		}

		public void SetValue(GameObject _object)
		{
			gameObject = _object;
			floatValue = 0f;
			stringValue = string.Empty;
			intValue = -1;
		}
	}
}
