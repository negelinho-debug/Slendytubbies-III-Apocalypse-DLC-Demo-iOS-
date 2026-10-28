using UnityEngine;

public class ChangeRenderSettings : MonoBehaviour
{
	public Color fogcolor;

	public Color skyboxcolor;

	public Color lighting;

	public bool changedrlight;

	public GameObject directionallight;

	public Color drcolor;

	public float fogdens = 0.01f;

	public Material newSkybox;

	public bool changeLightToColor = true;

	public bool enablePhotoFilter;

	private void Start()
	{
		RenderSettings.fogColor = fogcolor;
		RenderSettings.ambientLight = lighting;
		GameObject.Find("MainCamera").GetComponent<Camera>().backgroundColor = skyboxcolor;
		if (changedrlight)
		{
			directionallight.GetComponent<Light>().color = drcolor;
		}
		RenderSettings.fogDensity = fogdens;
		if (newSkybox != null)
		{
			RenderSettings.skybox = newSkybox;
		}
		else
		{
			RenderSettings.skybox = null;
		}
		if (changeLightToColor)
		{
		}
		if (enablePhotoFilter)
		{
			GameObject.Find("MainCamera").GetComponent<CameraFilterPack_Colors_Adjust_PreFilters>().enabled = true;
		}
	}
}
