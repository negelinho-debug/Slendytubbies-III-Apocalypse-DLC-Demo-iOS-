using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/FakeSSAO")]
public class PP_FakeSSAO : PostProcessBase
{
	public int baseC = 4;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/FakeSSAO");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_BaseC", baseC);
		Graphics.Blit(source, destination, material);
	}
}
