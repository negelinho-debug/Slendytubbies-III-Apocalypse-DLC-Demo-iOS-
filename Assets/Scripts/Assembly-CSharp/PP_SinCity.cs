using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/SinCity")]
public class PP_SinCity : PostProcessBase
{
	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/SinCity");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}
}
