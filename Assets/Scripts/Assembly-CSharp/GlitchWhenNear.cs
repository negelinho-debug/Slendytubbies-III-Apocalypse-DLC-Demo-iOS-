using UnityEngine;

public class GlitchWhenNear : MonoBehaviour
{
	public string searchTag = "Monster";

	public GameObject[] taggedGameObjects;

	public float curdis;

	public float newdis;

	public float scanFrequency = 1f;

	public Transform target;

	public float MinDis = 5f;

	public float distance;

	private void Start()
	{
		InvokeRepeating("ScanForTarget", 0f, scanFrequency);
	}

	private void Update()
	{
		if (target == null && GameObject.FindWithTag("Monster") != null)
		{
			ScanForTarget();
			GetComponent<CameraGlitch>().Intensity = 0f;
		}
		if (target != null)
		{
			distance = Vector3.Distance(transform.position, target.position);
			if (distance < MinDis)
			{
				GetComponent<CameraGlitch>().Intensity = MinDis - distance;
			}
			else
			{
				GetComponent<CameraGlitch>().Intensity = 0f;
			}
		}
	}

	private void ScanForTarget()
	{
		if (GameObject.FindWithTag("Monster") != null)
		{
			target = GetNearestTaggedObject().transform;
		}
	}

	private GameObject GetNearestTaggedObject()
	{
		float num = float.PositiveInfinity;
		taggedGameObjects = GameObject.FindGameObjectsWithTag(searchTag);
		GameObject result = null;
		GameObject[] array = taggedGameObjects;
		foreach (GameObject gameObject in array)
		{
			Vector3 position = gameObject.transform.position;
			float sqrMagnitude = (position - transform.position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				result = gameObject;
				num = sqrMagnitude;
			}
		}
		return result;
	}
}
