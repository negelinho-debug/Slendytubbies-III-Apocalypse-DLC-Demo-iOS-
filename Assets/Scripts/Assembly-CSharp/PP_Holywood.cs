using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Holywood")]
public class PP_Holywood : PostProcessBase
{
	public float lumThreshold = 0.13f;

	private void Awake()
	{
		material.SetFloat("_LumThreshold", lumThreshold);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Holywood");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_LumThreshold", lumThreshold);
		Graphics.Blit(source, destination, material);
	}
}
