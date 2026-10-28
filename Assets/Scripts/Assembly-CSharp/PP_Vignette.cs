using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Vignette")]
public class PP_Vignette : PostProcessBase
{
	public float radius = 3f;

	public float darkness = 0.5f;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Vignette");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Radius", radius);
		material.SetFloat("_Darkness", darkness);
		Graphics.Blit(source, destination, material);
	}
}
