using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Posterize")]
public class PP_Posterize : PostProcessBase
{
	public float colors = 4f;

	public float gamma = 1f;

	private void Awake()
	{
		material.SetFloat("_Colors", colors);
		material.SetFloat("_Gamma", gamma);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Posterize");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Colors", colors);
		material.SetFloat("_Gamma", gamma);
		Graphics.Blit(source, destination, material);
	}
}
