using UnityEngine;

namespace AC
{
	public class tk2DIntegration : ScriptableObject
	{
		public static bool IsDefinePresent()
		{
			return false;
		}

		public static bool PlayAnimation(Transform sprite, string clipName, int frame)
		{
			Debug.Log("The 'tk2DIsPresent' preprocessor is not defined - check your Build Settings.");
			return true;
		}

		public static bool PlayAnimation(Transform sprite, string clipName, bool changeWrapMode, WrapMode wrapMode, int frame)
		{
			Debug.Log("The 'tk2DIsPresent' preprocessor is not defined - check your Build Settings.");
			return true;
		}

		public static bool PlayAnimation(Transform sprite, string clipName, bool changeWrapMode, WrapMode wrapMode)
		{
			Debug.Log("The 'tk2DIsPresent' preprocessor is not defined - check your Build Settings.");
			return true;
		}

		public static void StopAnimation(Transform sprite)
		{
			Debug.Log("The 'tk2DIsPresent' preprocessor is not defined - check your Build Settings.");
		}

		public static bool IsAnimationPlaying(Transform sprite, string clipName)
		{
			return false;
		}
	}
}
