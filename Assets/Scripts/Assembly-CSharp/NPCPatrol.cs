using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
	public GameObject[] waypoints;

	public int curPoint;

	public Transform target;

	public NavMeshAgent nma;

	public bool skipAnimFrame;

	public Animation model;

	public bool hideModel;

	public bool teleportBack;

	public float maxDistance = 1f;

	private Vector3 orgPos;

	private void Awake()
	{
		if (hideModel)
		{
			model.gameObject.SetActive(false);
		}
	}

	private void Start()
	{
		if (skipAnimFrame)
		{
			float time = Random.Range(0f, model.clip.length);
			model["skintubby_moving"].time = time;
		}
		orgPos = transform.position;
	}

	private void Update()
	{
		if (Vector3.Distance(transform.position, waypoints[curPoint].transform.position) < maxDistance)
		{
			if (curPoint != waypoints.Length - 1)
			{
				if (!teleportBack)
				{
					curPoint++;
				}
				else
				{
					transform.position = orgPos;
				}
			}
			else if (!teleportBack)
			{
				curPoint = 0;
			}
			else
			{
				transform.position = orgPos;
			}
		}
		nma.SetDestination(waypoints[curPoint].transform.position);
		if (hideModel)
		{
			enabled = false;
		}
	}
}
