using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Charcoal")]
public class PP_Charcoal : PostProcessBase
{
	public Color lineColor = Color.black;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Charcoal");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetVector("_LineColor", lineColor);
		Graphics.Blit(source, destination, material);
	}
}
