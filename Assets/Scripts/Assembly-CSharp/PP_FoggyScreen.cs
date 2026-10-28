using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/FoggyScreen")]
public class PP_FoggyScreen : PostProcessBase
{
	public Color fogColor = Color.gray;

	public float fogThickness = 1f;

	private void Awake()
	{
		material.SetVector("_FogColor", fogColor);
		material.SetFloat("_FogThickness", fogThickness);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/FoggyScreen");
		GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetVector("_FogColor", fogColor);
		material.SetFloat("_FogThickness", fogThickness);
		Graphics.Blit(source, destination, material);
	}
}
