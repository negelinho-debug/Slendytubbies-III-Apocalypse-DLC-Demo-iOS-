using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/BloomSimple")]
public class PP_BloomSimple : PostProcessBase
{
	public float strength = 0.5f;

	private void Awake()
	{
		material.SetFloat("_Strength", strength);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/BloomSimple");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Strength", strength);
		Graphics.Blit(source, destination, material);
	}
}
