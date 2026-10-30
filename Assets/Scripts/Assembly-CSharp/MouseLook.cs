using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
	public enum RotationAxes
	{
		MouseXAndY,
		MouseX,
		MouseY
	}

	public RotationAxes axes;

	private float sensitivityX = 3.5f;

	private float sensitivityY = 3.5f;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float minimumY = -60f;

	public float maximumY = 60f;

	private float rotationY;

	private float multiplyer;

	private void OnEnable()
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.x = 0f;
		transform.localEulerAngles = localEulerAngles;
	}

	private void Update()
	{
		if (Time.timeScale != 0f)
		{
			if (axes == RotationAxes.MouseXAndY)
			{
				float y = transform.localEulerAngles.y + Input.GetAxis("CursorHorizontal") * sensitivityX;
				rotationY += Input.GetAxis("CursorVertical") * sensitivityY;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
				transform.localEulerAngles = new Vector3(0f - rotationY, y, 0f);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0f, Input.GetAxis("CursorHorizontal") * sensitivityX, 0f);
			}
			else
			{
				rotationY += Input.GetAxis("CursorVertical") * sensitivityY;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
				transform.localEulerAngles = new Vector3(0f - rotationY, transform.localEulerAngles.y, 0f);
			}
		}
	}

	private void Start()
	{
		if ((bool)GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
		sensitivityX /= 2f;
		sensitivityY /= 2f;
		multiplyer = 1f * (float)PlayerPrefs.GetInt("Mouse");
		if (multiplyer == 0f)
		{
			multiplyer = 1f;
		}
		if (multiplyer < 0f)
		{
			multiplyer *= multiplyer;
			multiplyer = multiplyer / 2f / multiplyer;
		}
		sensitivityX *= multiplyer;
		sensitivityY *= multiplyer;
	}
}
