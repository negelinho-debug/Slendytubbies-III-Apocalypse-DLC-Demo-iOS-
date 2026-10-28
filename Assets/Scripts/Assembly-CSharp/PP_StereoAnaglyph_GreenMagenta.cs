using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/StereoAnaglyph_GreenMagenta")]
public class PP_StereoAnaglyph_GreenMagenta : PostProcessBase
{
	public float distance = 0.001f;

	private void Awake()
	{
		material.SetFloat("_Distance", distance);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/StereoAnaglyph_GreenMagenta");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Distance", distance);
		material.SetFloat("_TexSizeX", source.width);
		material.SetFloat("_TexSizeY", source.height);
		Graphics.Blit(source, destination, material);
	}
}
