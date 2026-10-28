using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/HDR")]
public class PP_HDR : PostProcessBase
{
	public float amount = 0.045f;

	public float multiplier = 1f;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/HDR");
		if (Application.loadedLevelName.Contains("(Classic") || Application.loadedLevelName.Contains("TubbyCraft"))
		{
			enabled = false;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Amount", amount);
		material.SetFloat("_Multiplier", multiplier);
		Graphics.Blit(source, destination, material);
	}
}
