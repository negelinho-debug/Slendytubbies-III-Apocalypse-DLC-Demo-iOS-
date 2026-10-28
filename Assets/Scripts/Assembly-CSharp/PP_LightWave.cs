using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/LightWave")]
public class PP_LightWave : PostProcessBase
{
	public float red = 4f;

	public float green = 4f;

	public float blue = 4f;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/LightWave");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Red", red);
		material.SetFloat("_Green", green);
		material.SetFloat("_Blue", blue);
		Graphics.Blit(source, destination, material);
	}
}
