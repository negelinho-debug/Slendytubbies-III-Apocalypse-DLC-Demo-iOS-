using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/NightVision")]
public class PP_NightVision : PostProcessBase
{
	public Color color = Color.white;

	private float red = 1f;

	private float blue = 1.5f;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/NightVision");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Red", red);
		material.SetFloat("_Blue", blue);
		Graphics.Blit(source, destination, material);
	}
}
