using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Amnesia")]
public class PP_Amnesia : PostProcessBase
{
	public float density = 1f;

	public float speed = 3f;

	private void Awake()
	{
		material.SetFloat("_Density", density);
		material.SetFloat("_Speed", speed);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/Amnesia");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Density", density);
		material.SetFloat("_Speed", speed);
		Graphics.Blit(source, destination, material);
	}
}
