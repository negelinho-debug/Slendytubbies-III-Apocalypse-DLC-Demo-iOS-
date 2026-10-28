using UnityEngine;

public class FurScript : MonoBehaviour
{
	[Tooltip("Attach the material with the furshader here.")]
	public Material furMaterial;

	[Space(10f)]
	[Tooltip("Enable to simulate fur movement when the object is moving. Requires object to have a RigidBody.")]
	public bool simulateMovement;

	[Tooltip("Enable to fix fur being culled at the edge of screen. This does affect performance, only use if neccessary.")]
	public bool cullCorrection;

	[Tooltip("Adjust to a higher value if your fur is removing other transparent objects behind it. If you have a lot of transparent objects you will need to intelligently layer them with this.")]
	[Range(0f, 10f)]
	public int transparentDepth;

	private Rigidbody rb;

	private bool hasRB;

	private Camera camera;

	private Bounds originalBounds;

	private Mesh mesh;

	private void Start()
	{
		if (!GetComponent<Rigidbody>().Equals(null))
		{
			rb = GetComponent<Rigidbody>();
		}
		else if (GetComponent<Rigidbody>().Equals(null) && simulateMovement)
		{
			Debug.Log("FurShader: Simulate Movement option enabled on GameObject: '" + gameObject.name + "', but this object does not have a RigidBody. Attach a RigidBody Component to simulate movement.");
		}
		furMaterial.renderQueue = 3000 + transparentDepth;
		camera = Camera.main;
		mesh = GetComponent<MeshFilter>().mesh;
		originalBounds = mesh.bounds;
	}

	private void Update()
	{
		if (!GetComponent<Rigidbody>().Equals(null) && simulateMovement)
		{
			furMaterial.SetVector("_Velocity", transform.InverseTransformDirection(rb.velocity));
		}
		else
		{
			furMaterial.SetVector("_Velocity", new Vector3(0f, 0f, 0f));
		}
		if (cullCorrection)
		{
			Vector3 position = camera.transform.position;
			Vector3 vector = Vector3.Normalize(camera.transform.forward);
			float num = (camera.farClipPlane - camera.nearClipPlane) / 2f + camera.nearClipPlane;
			Vector3 position2 = position + vector * num;
			Vector3 center = transform.InverseTransformPoint(position2);
			mesh.bounds = new Bounds(center, Vector3.one);
		}
		else
		{
			mesh.bounds = originalBounds;
		}
	}
}
