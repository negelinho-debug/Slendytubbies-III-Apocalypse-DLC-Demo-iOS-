using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/StereoAnaglyph_AmberBlue")]
public class PP_StereoAnaglyph_AmberBlue : PostProcessBase
{
	public float distance = 0.005f;

	private void Awake()
	{
		material.SetFloat("_Distance", distance);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/StereoAnaglyph_AmberBlue");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Distance", distance);
		material.SetFloat("_TexSizeX", source.width);
		material.SetFloat("_TexSizeY", source.height);
		Graphics.Blit(source, destination, material);
	}
}
