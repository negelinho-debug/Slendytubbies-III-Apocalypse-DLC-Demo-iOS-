using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Desaturate")]
public class PP_Desaturate : PostProcessBase
{
	public float desaturate = 0.5f;

	private void Awake()
	{
		material.SetFloat("_Amount", desaturate);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Desaturate");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Amount", desaturate);
		Graphics.Blit(source, destination, material);
	}
}
