using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Scanlines")]
public class PP_Scanlines : PostProcessBase
{
	public float intense1 = 0.9f;

	public float intense2 = 0.5f;

	public float count = 15f;

	private void Awake()
	{
		material.SetFloat("_Intense1", intense1);
		material.SetFloat("_Intense2", intense2);
		material.SetFloat("_Count", count);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Scanlines");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Intense1", intense1);
		material.SetFloat("_Intense2", intense2);
		material.SetFloat("_Count", count);
		Graphics.Blit(source, destination, material);
	}
}
