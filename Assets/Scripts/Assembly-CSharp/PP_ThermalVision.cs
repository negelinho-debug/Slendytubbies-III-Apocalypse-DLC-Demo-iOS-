using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/ThermalVision")]
public class PP_ThermalVision : PostProcessBase
{
	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/ThermalVision");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}
}
