using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Camera Filter Pack/EXTRA/SHOWFPS")]
public class CameraFilterPack_EXTRA_SHOWFPS : MonoBehaviour
{
	public Shader SCShader;

	private float TimeX = 1f;

	private Vector4 ScreenResolution;

	private Material SCMaterial;

	[Range(8f, 42f)]
	public float Size = 12f;

	[Range(0f, 100f)]
	private int FPS = 1;

	[Range(0f, 10f)]
	private float Value3 = 1f;

	[Range(0f, 10f)]
	private float Value4 = 1f;

	private float deltaTime;

	public static float ChangeValue;

	public static int ChangeValue2;

	public static float ChangeValue3;

	public static float ChangeValue4;

	private float accum;

	private int frames;

	public float frequency = 0.5f;

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
		FPS = 0;
		StartCoroutine(FPSX());
		ChangeValue = Size;
		ChangeValue2 = FPS;
		ChangeValue3 = Value3;
		ChangeValue4 = Value4;
		SCShader = Shader.Find("CameraFilterPack/EXTRA_SHOWFPS");
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
			material.SetFloat("_Value", Size);
			material.SetFloat("_Value2", FPS);
			material.SetFloat("_Value3", Value3);
			material.SetFloat("_Value4", Value4);
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
		ChangeValue = Size;
		ChangeValue2 = FPS;
		ChangeValue3 = Value3;
		ChangeValue4 = Value4;
	}

	private IEnumerator FPSX()
	{
		while (true)
		{
			float fps = accum / (float)frames;
			FPS = (int)fps;
			ChangeValue2 = (int)fps;
			accum = 0f;
			frames = 0;
			yield return new WaitForSeconds(frequency);
		}
	}

	private void Update()
	{
		accum += Time.timeScale / Time.deltaTime;
		frames++;
		if (Application.isPlaying)
		{
			Size = ChangeValue;
			FPS = ChangeValue2;
			Value3 = ChangeValue3;
			Value4 = ChangeValue4;
		}
		else
		{
			FPS = 9999;
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
