using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/BlackAndWhite")]
public class PP_BlackAndWhite : PostProcessBase
{
	public float threshold = 0.5f;

	private void Awake()
	{
		material.SetFloat("_Threshold", threshold);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/BlackAndWhite");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Threshold", threshold);
		Graphics.Blit(source, destination, material);
	}
}
