using UnityEngine;

namespace AC
{
	public class PlayMakerIntegration : ScriptableObject
	{
		public static bool IsDefinePresent()
		{
			return false;
		}

		public static void CallEvent(GameObject linkedObject, string eventName, string fsmName)
		{
		}

		public static void CallEvent(GameObject linkedObject, string eventName)
		{
		}

		public static int GetGlobalInt(string _name)
		{
			return 0;
		}

		public static bool GetGlobalBool(string _name)
		{
			return false;
		}

		public static string GetGlobalString(string _name)
		{
			return string.Empty;
		}

		public static float GetGlobalFloat(string _name)
		{
			return 0f;
		}

		public static void SetGlobalInt(string _name, int _val)
		{
		}

		public static void SetGlobalBool(string _name, bool _val)
		{
		}

		public static void SetGlobalString(string _name, string _val)
		{
		}

		public static void SetGlobalFloat(string _name, float _val)
		{
		}
	}
}
