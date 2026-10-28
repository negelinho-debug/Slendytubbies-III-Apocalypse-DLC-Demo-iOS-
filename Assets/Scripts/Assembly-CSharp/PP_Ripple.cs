using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Ripple")]
public class PP_Ripple : PostProcessBase
{
	public float speed = 4f;

	public float amount = 16f;

	public float strength = 0.009f;

	public float offsetX;

	public float offsetY;

	private void Awake()
	{
		material.SetFloat("_Speed", speed);
		material.SetFloat("_Amount", amount);
		material.SetFloat("_Strength", strength);
		material.SetFloat("_OffsetX", offsetX);
		material.SetFloat("_OffsetY", offsetY);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Ripple");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Speed", speed);
		material.SetFloat("_Amount", amount);
		material.SetFloat("_Strength", strength);
		material.SetFloat("_OffsetX", offsetX);
		material.SetFloat("_OffsetY", offsetY);
		Graphics.Blit(source, destination, material);
	}
}
