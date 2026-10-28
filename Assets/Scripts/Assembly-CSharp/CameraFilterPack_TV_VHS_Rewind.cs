using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/VHS/VHS_Rewind")]
public class CameraFilterPack_TV_VHS_Rewind : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(0f, 1f)]
	public float Cryptage = 1f;

	[Range(-20f, 20f)]
	public float Parasite = 9f;

	[Range(-20f, 20f)]
	public float Parasite2 = 12f;

	[Range(0f, 1f)]
	private float WhiteParasite = 1f;

	public static float ChangeValue;

	public static float ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;

	private Material material
	{
		get
		{
			if (SCMaterial == null)
			{
				SCMaterial = new Material(SCShader);
				SCMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return SCMaterial;
		}
	}

	private void Start()
	{
		ChangeValue = Cryptage;
		ChangeValue2 = Parasite;
		ChangeValue3 = Parasite2;
		ChangeValue4 = WhiteParasite;
		SCShader = Shader.Find("CameraFilterPack/TV_VHS_Rewind");
		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
		}
	}

	private void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
	{
		if (SCShader != null)
		{
			TimeX += Time.deltaTime;
			if (TimeX > 100f)
			{
				TimeX = 0f;
			}
			material.SetFloat("_TimeX", TimeX);
			material.SetFloat("_Value", Cryptage);
			material.SetFloat("_Value2", Parasite);
			material.SetFloat("_Value3", Parasite2);
			material.SetFloat("_Value4", WhiteParasite);
			material.SetVector("_ScreenResolution", new Vector4(sourceTexture.width, sourceTexture.height, 0f, 0f));
			Graphics.Blit(sourceTexture, destTexture, material);
		}
		else
		{
			Graphics.Blit(sourceTexture, destTexture);
		}
	}

	private void OnValidate()
	{
		ChangeValue = Cryptage;
		ChangeValue2 = Parasite;
		ChangeValue3 = Parasite2;
		ChangeValue4 = WhiteParasite;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			Cryptage = ChangeValue;
			Parasite = ChangeValue2;
			Parasite2 = ChangeValue3;
			WhiteParasite = ChangeValue4;
		}
	}

	private void OnDisable()
	{
		if ((bool)SCMaterial)
		{
			Object.DestroyImmediate(SCMaterial);
		}
	}
}
