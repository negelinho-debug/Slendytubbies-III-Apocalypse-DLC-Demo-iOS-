using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/BlurH")]
public class PP_BlurH : PostProcessBase
{
	public float blurMultiplier = 1f;

	private void Awake()
	{
		material.SetFloat("_BlurMulti", blurMultiplier);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/BlurH");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_BlurMulti", blurMultiplier);
		Graphics.Blit(source, destination, material);
	}
}
