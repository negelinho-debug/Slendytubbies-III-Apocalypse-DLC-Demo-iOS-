using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/TilesXY")]
public class PP_TilesXY : PostProcessBase
{
	public float numTilesX = 32f;

	public float numTilesY = 32f;

	public float threshold = 0.16f;

	public Color edgeColor = Color.grey;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/TilesXY");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("numTilesX", numTilesX);
		material.SetFloat("numTilesY", numTilesY);
		material.SetFloat("threshold", threshold);
		material.SetColor("edgeColor", edgeColor);
		Graphics.Blit(source, destination, material);
	}
}
