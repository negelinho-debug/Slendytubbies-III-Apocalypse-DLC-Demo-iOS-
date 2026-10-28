using UnityEngine;

namespace AC
{
	public class FaceFXIntegration : ScriptableObject
	{
		public static bool IsDefinePresent()
		{
			return false;
		}

		public static void Play(Char speaker, string name, AudioClip audioClip)
		{
			Debug.LogError("The 'FaceFXIsPresent' preprocessor define must be declared in the Player Settings.");
		}
	}
}
