using UnityEngine;
using UnityEngine.AI;

public class LaaLaaMechanics : MonoBehaviour
{
	public Transform player;

	public NavMeshAgent nma;

	public Animation model;

	private Vector3 oldPos;

	private bool isWander;

	private void Start()
	{
	}

	private void Update()
	{
		if (player == null)
		{
			player = GameObject.FindWithTag("Player").transform;
		}
		else if (player.position != oldPos)
		{
			nma.SetDestination(player.position);
			oldPos = player.position;
			isWander = false;
		}
		else
		{
			if (!isWander)
			{
				nma.SetDestination(ChooseRandomDestination());
			}
			isWander = true;
		}
		if (nma.hasPath)
		{
			model.CrossFade("walk");
		}
		else
		{
			model.CrossFade("idle");
		}
	}

	private Vector3 ChooseRandomDestination()
	{
		float num = 30f;
		Vector3 sourcePosition = Random.insideUnitSphere * num;
		sourcePosition += transform.position;
		NavMeshHit hit;
		NavMesh.SamplePosition(sourcePosition, out hit, num, -1);
		return hit.position;
	}
}
