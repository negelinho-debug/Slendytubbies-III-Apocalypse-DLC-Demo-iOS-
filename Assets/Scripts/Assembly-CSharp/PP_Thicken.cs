using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Thicken")]
public class PP_Thicken : PostProcessBase
{
	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Thicken");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}
}
