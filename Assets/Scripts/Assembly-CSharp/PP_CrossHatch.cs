using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/CrossHatch")]
public class PP_CrossHatch : PostProcessBase
{
	public Color lineColor = Color.red;

	public float lineWidth = 0.005f;

	private void Awake()
	{
		material.SetVector("_LineColor", lineColor);
		material.SetFloat("_LineWidth", lineWidth);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/CrossHatch");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetVector("_LineColor", lineColor);
		material.SetFloat("_LineWidth", lineWidth);
		Graphics.Blit(source, destination, material);
	}
}
