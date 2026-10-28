using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/ToneMap")]
public class PP_ToneMap : PostProcessBase
{
	public float exposure = 0.1f;

	public float gamma = 1f;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/ToneMap");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Exposure", exposure);
		material.SetFloat("_Gamma", gamma);
		Graphics.Blit(source, destination, material);
	}
}
