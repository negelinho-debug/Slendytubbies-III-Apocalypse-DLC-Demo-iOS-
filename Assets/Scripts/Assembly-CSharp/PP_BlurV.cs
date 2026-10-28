using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/BlurV")]
public class PP_BlurV : PostProcessBase
{
	public float blurMultiplier = 1f;

	private void Awake()
	{
		material.SetFloat("_BlurMulti", blurMultiplier);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/BlurV");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_BlurMulti", blurMultiplier);
		Graphics.Blit(source, destination, material);
	}
}
