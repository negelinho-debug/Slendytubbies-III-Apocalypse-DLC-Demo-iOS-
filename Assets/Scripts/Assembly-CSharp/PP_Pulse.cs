using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Pulse")]
public class PP_Pulse : PostProcessBase
{
	public float speed = 5f;

	public float distance = 0.1f;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Pulse");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Speed", speed);
		material.SetFloat("_Distance", distance);
		Graphics.Blit(source, destination, material);
	}
}
