using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Pencil")]
public class PP_Pencil : PostProcessBase
{
	public Texture pencilTexture;

	public float effectStrength = 0.5f;

	public float brightness = 0.5f;

	private void Awake()
	{
		if ((bool)pencilTexture)
		{
			material.SetTexture("_PencilTex", pencilTexture);
		}
		material.SetFloat("_Amount", effectStrength);
		material.SetFloat("_Brightness", brightness);
	}

	private void OnEnable()
	{
		if (!pencilTexture)
		{
			Debug.LogWarning("You must set the pencilTexture Texture");
		}
		shader = Shader.Find("Hidden/Aubergine/Pencil");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetTexture("_PencilTex", pencilTexture);
		material.SetFloat("_Amount", effectStrength);
		material.SetFloat("_Brightness", brightness);
		Graphics.Blit(source, destination, material);
	}
}
