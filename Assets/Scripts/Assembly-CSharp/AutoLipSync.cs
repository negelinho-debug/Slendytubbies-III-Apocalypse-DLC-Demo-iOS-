using UnityEngine;

public class AutoLipSync : MonoBehaviour
{
	public enum Coord
	{
		W,
		X,
		Y,
		Z
	}

	public Transform jawBone;

	public Coord coordinateToAffect;

	public float rotationFactor = 10f;

	private float volume;

	private float bin = 0.04f;

	private int width = 64;

	private float output;

	private float[] array;

	private Quaternion jawRotation;

	private void Awake()
	{
		array = new float[width];
	}

	private void FixedUpdate()
	{
		if (GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().GetOutputData(array, 0);
			float num = 0f;
			for (int i = 0; i < width; i++)
			{
				float num2 = Mathf.Abs(array[i]);
				num += num2;
			}
			num /= (float)width;
			if (Mathf.Abs(num - volume) > bin)
			{
				volume = num;
			}
			volume = Mathf.Clamp01(volume * 2f);
			volume *= 0.3f;
			output = Mathf.Lerp(output, volume, Time.deltaTime * Mathf.Abs(rotationFactor));
		}
		else
		{
			output = 0f;
		}
	}

	private void LateUpdate()
	{
		jawRotation = jawBone.localRotation;
		if (coordinateToAffect == Coord.W)
		{
			if (rotationFactor < 0f)
			{
				jawRotation.w += output;
			}
			else
			{
				jawRotation.w -= output;
			}
		}
		else if (coordinateToAffect == Coord.X)
		{
			if (rotationFactor < 0f)
			{
				jawRotation.x += output;
			}
			else
			{
				jawRotation.x -= output;
			}
		}
		else if (coordinateToAffect == Coord.Y)
		{
			if (rotationFactor < 0f)
			{
				jawRotation.y += output;
			}
			else
			{
				jawRotation.y -= output;
			}
		}
		else if (coordinateToAffect == Coord.Z)
		{
			if (rotationFactor < 0f)
			{
				jawRotation.z += output;
			}
			else
			{
				jawRotation.z -= output;
			}
		}
		jawBone.localRotation = jawRotation;
	}
}
