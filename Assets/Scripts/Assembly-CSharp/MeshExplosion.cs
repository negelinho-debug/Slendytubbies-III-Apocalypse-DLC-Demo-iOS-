using UnityEngine;

public class MeshExplosion : MonoBehaviour
{
	private MeshExploder.MeshExplosionPreparation preparation;

	private Mesh mesh;

	private Vector3[] vertices;

	private Vector3[] normals;

	private Vector4[] tangents;

	private Vector3[] triangleRotationAxes;

	private float[] triangleSpeeds;

	private float[] triangleRotationSpeeds;

	private Vector3[] triangleCurrentCentroids;

	private bool useGravity;

	private float explosionTime;

	private Transform thisTransform;

	public void Go(MeshExploder.MeshExplosionPreparation prep, float minSpeed, float maxSpeed, float minRotationSpeed, float maxRotationSpeed, bool useGravity, Vector3 scale)
	{
		preparation = prep;
		int totalFrontTriangles = prep.totalFrontTriangles;
		triangleRotationAxes = new Vector3[totalFrontTriangles];
		triangleSpeeds = new float[totalFrontTriangles];
		triangleRotationSpeeds = new float[totalFrontTriangles];
		float num = maxSpeed - minSpeed;
		float num2 = maxRotationSpeed - minRotationSpeed;
		bool flag = minSpeed == maxSpeed;
		bool flag2 = minRotationSpeed == maxRotationSpeed;
		this.useGravity = useGravity;
		explosionTime = 0f;
		thisTransform = transform;
		for (int i = 0; i < totalFrontTriangles; i++)
		{
			triangleRotationAxes[i] = Random.onUnitSphere;
			triangleSpeeds[i] = ((!flag) ? (minSpeed + Random.value * num) : minSpeed);
			triangleRotationSpeeds[i] = ((!flag2) ? (minRotationSpeed + Random.value * num2) : minRotationSpeed);
		}
		GetComponent<MeshFilter>().mesh = (mesh = Object.Instantiate(prep.startMesh));
		triangleCurrentCentroids = (Vector3[])prep.triangleCentroids.Clone();
		vertices = mesh.vertices;
		normals = mesh.normals;
		tangents = mesh.tangents;
		if (scale != Vector3.one)
		{
			int num3 = vertices.Length;
			for (int j = 0; j < num3; j++)
			{
				vertices[j] = Vector3.Scale(vertices[j], scale);
			}
			int num4 = triangleCurrentCentroids.Length;
			for (int k = 0; k < num4; k++)
			{
				triangleCurrentCentroids[k] = Vector3.Scale(triangleCurrentCentroids[k], scale);
			}
		}
		Update();
	}

	private void Update()
	{
		if (vertices == null)
		{
			string text = GetType().Name;
			Debug.LogError("The " + text + " component should not be used directly. Add the " + typeof(MeshExploder).Name + " component to your object and it will take care of creating the explosion object and adding the " + text + " component.");
			enabled = false;
			return;
		}
		float deltaTime = Time.deltaTime;
		explosionTime += deltaTime;
		if (tangents != null && tangents.Length == 0)
		{
			tangents = null;
		}
		Vector3[] triangleNormals = preparation.triangleNormals;
		Vector3 vector = ((!useGravity) ? default(Vector3) : thisTransform.InverseTransformDirection(Physics.gravity));
		int num = vertices.Length / 3 / 2;
		int num2 = 0;
		int num3 = 0;
		while (num3 < num)
		{
			float num4 = triangleSpeeds[num3] * deltaTime;
			float angle = triangleRotationSpeeds[num3] * deltaTime;
			Vector3 vector2 = triangleNormals[num3];
			Vector3 vector3 = vector2 * num4;
			if (useGravity)
			{
				vector3 += explosionTime * vector * deltaTime;
			}
			Quaternion quaternion = Quaternion.AngleAxis(angle, triangleRotationAxes[num3]);
			Vector3 vector4 = triangleCurrentCentroids[num3];
			Vector3 vector5 = vector4 + vector3;
			for (int i = 0; i < 3; i++)
			{
				int num5 = num2 + i;
				vertices[num5] = quaternion * (vertices[num5] - vector4) + vector5;
				if (normals != null)
				{
					normals[num5] = quaternion * normals[num5];
				}
				if (tangents != null)
				{
					tangents[num5] = quaternion * tangents[num5];
				}
			}
			triangleCurrentCentroids[num3] = vector5;
			num3++;
			num2 += 6;
		}
		SetBackTriangleVertices(vertices, normals, tangents, preparation.totalFrontTriangles);
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.tangents = tangents;
		mesh.RecalculateBounds();
	}

	public static void SetBackTriangleVertices(Vector3[] vertices, Vector3[] normals, Vector4[] tangents, int totalFrontTriangles)
	{
		int num = 0;
		for (int i = 0; i < totalFrontTriangles; i++)
		{
			int num2 = num;
			num += 3;
			int num3 = 0;
			while (num3 < 3)
			{
				int num4 = 2 - num3 + num2;
				vertices[num] = vertices[num4];
				if (normals != null)
				{
					normals[num] = -normals[num4];
				}
				if (tangents != null)
				{
					tangents[num] = -tangents[num4];
				}
				num3++;
				num++;
			}
		}
	}
}
