using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Dream_Grey")]
public class PP_Dream_Grey : PostProcessBase
{
	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Dream_Grey");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}
}
