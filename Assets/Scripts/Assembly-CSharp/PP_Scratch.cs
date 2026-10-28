using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/Scratch")]
public class PP_Scratch : PostProcessBase
{
	public Texture noiseTexture;

	public float speed1 = 0.03f;

	public float speed2 = 0.01f;

	public float intensity = 0.5f;

	public float scratchWidth = 0.01f;

	private void Awake()
	{
		if ((bool)noiseTexture)
		{
			material.SetTexture("_Noise", noiseTexture);
		}
		material.SetTexture("_Noise", noiseTexture);
		material.SetFloat("_Speed1", speed1);
		material.SetFloat("_Speed2", speed2);
		material.SetFloat("_Intensity", intensity);
		material.SetFloat("_ScratchWidth", scratchWidth);
	}

	private void OnEnable()
	{
		if (!noiseTexture)
		{
			Debug.LogWarning("You must set the noiseTexture Texture");
		}
		shader = Shader.Find("Hidden/Aubergine/Scratch");
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetTexture("_Noise", noiseTexture);
		material.SetFloat("_Speed1", speed1);
		material.SetFloat("_Speed2", speed2);
		material.SetFloat("_Intensity", intensity);
		material.SetFloat("_ScratchWidth", scratchWidth);
		Graphics.Blit(source, destination, material);
	}
}
