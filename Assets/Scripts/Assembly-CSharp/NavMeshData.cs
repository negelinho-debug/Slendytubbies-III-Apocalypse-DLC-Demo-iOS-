using UnityEngine;

public class NavMeshData
{
	public Vector3 vertex;

	public float distance;

	public NavMeshData(Vector3 _vertex, Vector3 _target, Transform navObject)
	{
		vertex = navObject.TransformPoint(_vertex);
		distance = Vector3.Distance(vertex, _target);
	}
}
