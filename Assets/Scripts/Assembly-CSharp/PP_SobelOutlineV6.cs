using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/SobelOutlineV6")]
public class PP_SobelOutlineV6 : PostProcessBase
{
	public float threshold = 0.7f;

	private void Awake()
	{
		material.SetFloat("_Threshold", threshold);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/SobelOutlineV6");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Threshold", threshold);
		Graphics.Blit(source, destination, material);
	}
}
