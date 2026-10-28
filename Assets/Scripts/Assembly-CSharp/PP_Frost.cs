using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Frost")]
public class PP_Frost : PostProcessBase
{
	public Texture noiseMap;

	public float frequenzy = 1f;

	private void Awake()
	{
		if ((bool)noiseMap)
		{
			material.SetTexture("_NoiseMap", noiseMap);
		}
	}

	private void OnEnable()
	{
		if (!noiseMap)
		{
			Debug.LogWarning("You must set the noiseMap Texture");
		}
		shader = Shader.Find("Hidden/Aubergine/Frost");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetTexture("_NoiseMap", noiseMap);
		material.SetFloat("_Amount", frequenzy);
		Graphics.Blit(source, destination, material);
	}
}
