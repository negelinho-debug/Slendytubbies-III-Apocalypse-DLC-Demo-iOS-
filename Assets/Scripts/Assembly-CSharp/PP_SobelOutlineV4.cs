using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/SobelOutlineV4")]
public class PP_SobelOutlineV4 : PostProcessBase
{
	public float threshold = 0.7f;

	private void Awake()
	{
		material.SetFloat("_Threshold", threshold);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/SobelOutlineV4");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Threshold", threshold);
		Graphics.Blit(source, destination, material);
	}
}
