using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Aubergine/LightShafts")]
public class PP_LightShafts : PostProcessBase
{
	public Transform lightSource;

	public float density = 1f;

	public float weight = 1f;

	public float decay = 1f;

	public float exposure = 1f;

	private Vector3 lightSPos;

	private void Awake()
	{
		material.SetFloat("_Density", density);
		material.SetFloat("_Weight", weight);
		material.SetFloat("_Decay", decay);
		material.SetFloat("_Exposure", exposure);
		lightSPos = GetComponent<Camera>().WorldToViewportPoint(lightSource.position);
		material.SetVector("_LightSPos", lightSPos);
	}

	private void OnEnable()
	{
		shader = Shader.Find("Hidden/Aubergine/LightShafts");
	}

	private void Update()
	{
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		lightSPos = GetComponent<Camera>().WorldToViewportPoint(lightSource.position);
		if (lightSPos.x < 0f || lightSPos.x > 1f || lightSPos.y < 0f || lightSPos.y > 1f || lightSPos.z < 0f)
		{
			material.SetVector("_LightSPos", new Vector3(0.5f, 0.5f, 0f));
			material.SetFloat("_Density", density - density + 0.1f);
			material.SetFloat("_Weight", weight);
			material.SetFloat("_Decay", decay);
			material.SetFloat("_Exposure", exposure);
		}
		else
		{
			material.SetVector("_LightSPos", lightSPos);
			material.SetFloat("_Density", density);
			material.SetFloat("_Weight", weight);
			material.SetFloat("_Decay", decay);
			material.SetFloat("_Exposure", exposure);
		}
		Debug.Log(GetComponent<Camera>().WorldToViewportPoint(lightSource.position));
		Graphics.Blit(source, destination, material);
	}
}
