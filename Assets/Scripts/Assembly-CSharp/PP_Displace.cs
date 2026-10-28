using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Displacement")]
public class PP_Displace : PostProcessBase
{
	public Texture bumpTexture;

	public float bumpAmount = 0.5f;

	private void Awake()
	{
		if ((bool)bumpTexture)
		{
			material.SetTexture("_BumpTex", bumpTexture);
		}
		material.SetFloat("_Amount", bumpAmount);
	}

	private void OnEnable()
	{
		if (!bumpTexture)
		{
			Debug.LogWarning("You must set the bumpTexture Texture");
		}
		shader = Shader.Find("Hidden/Aubergine/Displacement");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetTexture("_BumpTex", bumpTexture);
		material.SetFloat("_Amount", bumpAmount);
		Graphics.Blit(source, destination, material);
	}
}
