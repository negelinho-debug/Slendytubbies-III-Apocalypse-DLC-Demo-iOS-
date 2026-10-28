using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/LensCircle")]
public class PP_LensCircle : PostProcessBase
{
	public float radiusX = 1f;

	public float radiusY;

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/LensCircle");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_RadiusX", radiusX);
		material.SetFloat("_RadiusY", radiusY);
		Graphics.Blit(source, destination, material);
	}
}
