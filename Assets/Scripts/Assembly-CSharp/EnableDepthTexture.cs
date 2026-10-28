using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class EnableDepthTexture : MonoBehaviour
{
	public bool EnableInEditor = true;

	private void OnEnable()
	{
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}

	private void OnDisable()
	{
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
	}

	private void Update()
	{
	}
}
