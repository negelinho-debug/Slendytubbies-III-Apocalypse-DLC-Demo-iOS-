using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/4Bit")]
public class PP_4Bit : PostProcessBase
{
	public int bitDepth = 1;

	public float contrast = 1f;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/4Bit");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_BitDepth", bitDepth);
		material.SetFloat("_Contrast", contrast);
		Graphics.Blit(source, destination, material);
	}
}
