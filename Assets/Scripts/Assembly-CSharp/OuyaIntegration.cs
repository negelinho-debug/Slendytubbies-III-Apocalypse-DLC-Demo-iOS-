using UnityEngine;

public static class OuyaIntegration
{
	public static Vector3 mousePosition => Input.mousePosition;

	public static bool IsDefinePresent()
	{
		return false;
	}

	public static float GetAxis(string axisName)
	{
		return Input.GetAxis(axisName);
	}

	public static float GetAxisRaw(string axisName)
	{
		return Input.GetAxisRaw(axisName);
	}

	public static bool GetButton(string buttonName)
	{
		return Input.GetButton(buttonName);
	}

	public static bool GetButtonDown(string buttonName)
	{
		return Input.GetButtonDown(buttonName);
	}

	public static bool GetMouseButton(int button)
	{
		return Input.GetMouseButton(button);
	}

	public static bool GetMouseButtonDown(int button)
	{
		return Input.GetMouseButtonDown(button);
	}
}
