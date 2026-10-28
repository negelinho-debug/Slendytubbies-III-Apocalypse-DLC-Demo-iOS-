using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/NightVisionV2")]
public class PP_NightVisionV2 : PostProcessBase
{
	public Texture noiseTex;

	public float noiseAmount = 0.9f;

	public float lumThreshold = 0.2f;

	public float brightenFactor = 2f;

	public Color visionColor = Color.green;

	private void Awake()
	{
		if ((bool)noiseTex)
		{
			material.SetTexture("_NoiseTex", noiseTex);
		}
		material.SetFloat("_NoiseAmount", noiseAmount);
		material.SetFloat("_LumThreshold", lumThreshold);
		material.SetFloat("_BrightenFactor", brightenFactor);
		material.SetVector("_VisionColor", visionColor);
	}

	private void OnEnable()
	{
		if (!noiseTex)
		{
			Debug.LogWarning("You must set the noiseTex Texture");
		}
		shader = Shader.Find("Hidden/Aubergine/NightVisionV2");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetTexture("_NoiseTex", noiseTex);
		material.SetFloat("_NoiseAmount", noiseAmount);
		material.SetFloat("_LumThreshold", lumThreshold);
		material.SetFloat("_BrightenFactor", brightenFactor);
		material.SetVector("_VisionColor", visionColor);
		Graphics.Blit(source, destination, material);
	}
}
