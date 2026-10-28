using UnityEngine;

namespace AC
{
	public class UltimateFPSIntegration : ScriptableObject
	{
		public static bool IsDefinePresent()
		{
			return false;
		}

		public static void _Update(GameState gameState)
		{
			if (gameState == GameState.Normal)
			{
				SetMovementState(true);
				SetCameraState(true);
			}
			else
			{
				SetMovementState(false);
				SetCameraState(false);
			}
		}

		public static void SetMovementState(bool state)
		{
			Debug.LogWarning("The 'UltimateFPSIsPresent' preprocessor is not defined - check your Player Settings.");
		}

		public static void SetCameraState(bool state)
		{
			Debug.Log("The 'UltimateFPSIsPresent' preprocessor is not defined - check your Player Settings.");
		}

		public static void Teleport(Vector3 position)
		{
		}

		public static void SetRotation(Vector3 rotation)
		{
		}

		public static void SetPitch(float pitch)
		{
		}

		public static void SetTilt(float tilt)
		{
		}
	}
}
