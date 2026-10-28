using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/SecurityCamera")]
public class PP_SecurityCamera : PostProcessBase
{
	public Color color = Color.white;

	public float speed = 2f;

	public float thickness = 3f;

	public float luminance = 0.5f;

	public float darkness = 0.75f;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/SecurityCamera");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetColor("_MainTex", color);
		material.SetFloat("_Speed", speed);
		material.SetFloat("_Thickness", thickness);
		material.SetFloat("_Luminance", luminance);
		material.SetFloat("_Darkness", darkness);
		Graphics.Blit(source, destination, material);
	}
}
