using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Camera Glitch")]
[ExecuteInEditMode]
public class CameraGlitch : MonoBehaviour
{
	public Texture GlitchTexture;

	public Texture OverlayTexture;

	public float Intensity = 1f;

	public bool ShowOverlay;

	public bool CanShowCrack;

	private float glitchUp;

	private float glitchDown;

	private float flicker;

	private float glitchUpTime = 0.05f;

	private float glitchDownTime = 0.05f;

	private float flickerTime = 0.5f;

	private static Shader shader;

	private Material _material;

	private Material material
	{
		get
		{
			if (_material == null)
			{
				_material = new Material(shader);
				_material.SetTexture("_GlitchTex", (!GlitchTexture) ? (Resources.Load("GlitchTexture") as Texture) : GlitchTexture);
				_material.SetTextureScale("_GlitchTex", new Vector2((float)Screen.width / ((!GlitchTexture) ? 512f : ((float)GlitchTexture.width)), (float)Screen.height / ((!GlitchTexture) ? 512f : ((float)GlitchTexture.height))));
				_material.SetTexture("_OverlayTex", (!OverlayTexture) ? (Resources.Load("OverlayTexture") as Texture) : OverlayTexture);
				_material.hideFlags = HideFlags.HideAndDontSave;
			}
			return _material;
		}
	}

	private void Start()
	{
		if (!shader)
		{
			shader = Resources.Load("CameraGlitch") as Shader;
		}
		if (!SystemInfo.supportsImageEffects || !shader || !shader.isSupported)
		{
			enabled = false;
		}
	}

	private void OnDisable()
	{
		if ((bool)_material)
		{
			Object.DestroyImmediate(_material);
		}
		GetComponent<PP_SecurityCamera>().enabled = false;
		GetComponent<PP_NightVision>().enabled = false;
		if (CanShowCrack && PlayerPrefs.GetString("Mountains") == "1")
		{
			GetComponent<CameraFilterPack_TV_BrokenGlass>().enabled = false;
		}
		GetComponent<Light>().enabled = false;
	}

	private void OnEnable()
	{
		GetComponent<PP_SecurityCamera>().enabled = true;
		GetComponent<PP_NightVision>().enabled = true;
		if (CanShowCrack && PlayerPrefs.GetString("Mountains") == "1")
		{
			GetComponent<CameraFilterPack_TV_BrokenGlass>().enabled = true;
		}
		GetComponent<Light>().enabled = true;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_Intensity", Intensity);
		material.SetFloat("_ShowOverlay", ShowOverlay ? 1 : 0);
		if (Intensity == 0f)
		{
			material.SetFloat("filterRadius", 0f);
		}
		glitchUp += Time.deltaTime * Intensity;
		glitchDown += Time.deltaTime * Intensity;
		flicker += Time.deltaTime * Intensity;
		if (flicker > flickerTime)
		{
			material.SetFloat("filterRadius", Random.Range(-3f, 3f) * Intensity);
			material.SetTextureOffset("_GlitchTex", new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
			flicker = 0f;
			flickerTime = Random.value;
		}
		if (glitchUp > glitchUpTime)
		{
			if (Random.value < 0.1f * Intensity)
			{
				material.SetFloat("flipUp", Random.Range(0f, 1f) * Intensity);
			}
			else
			{
				material.SetFloat("flipUp", 0f);
			}
			glitchUp = 0f;
			glitchUpTime = Random.value / 10f;
		}
		if (glitchDown > glitchDownTime)
		{
			if (Random.value < 0.1f * Intensity)
			{
				material.SetFloat("flipDown", 1f - Random.Range(0f, 1f) * Intensity);
			}
			else
			{
				material.SetFloat("flipDown", 1f);
			}
			glitchDown = 0f;
			glitchDownTime = Random.value / 10f;
		}
		if (Random.value < 0.05f * Intensity)
		{
			material.SetFloat("displace", Random.value * Intensity);
			material.SetFloat("scale", 1f - Random.value * Intensity);
		}
		else
		{
			material.SetFloat("displace", 0f);
		}
		Graphics.Blit(source, destination, material);
	}
}
