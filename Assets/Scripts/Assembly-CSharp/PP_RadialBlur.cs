using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/RadialBlur")]
public class PP_RadialBlur : PostProcessBase
{
	public float centerX = 0.5f;

	public float centerY = 0.5f;

	private void Awake()
	{
		material.SetFloat("_CenterX", centerX);
		material.SetFloat("_CenterY", centerY);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/RadialBlur");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_CenterX", centerX);
		material.SetFloat("_CenterY", centerY);
		Graphics.Blit(source, destination, material);
	}
}
