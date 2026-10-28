using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Tiles")]
public class PP_Tiles : PostProcessBase
{
	public float numTiles = 32f;

	public float threshold = 0.16f;

	public Color edgeColor = Color.grey;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Tiles");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("numTiles", numTiles);
		material.SetFloat("threshold", threshold);
		material.SetColor("edgeColor", edgeColor);
		Graphics.Blit(source, destination, material);
	}
}
