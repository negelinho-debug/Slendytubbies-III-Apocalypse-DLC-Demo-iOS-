using System.Collections.Generic;
using UnityEngine;

public class WaterPlusScript : MonoBehaviour
{
	public WaterMovementType movementType;

	public Vector2 velocity;

	public float speed;

	public Transform target;

	public bool animatedNormalmaps = true;

	private Texture2D[] normalmapAnimation;

	private Texture2D[] dudvfoamAnimation;

	private float animationValue;

	private Vector3 waterCenter;

	private Material waterMaterial;

	private Vector3 projectedLightDir;

	private Vector2 anisoDirAnimationOffset;

	private int causticsAnimationFrame;

	private float causticsAnimationTime;

	private int normalmapAnimationFrame;

	private float normalmapAnimationTime;

	private void Reset()
	{
		speed = 3f;
		velocity = new Vector2(0.7f, 0f);
	}

	private Light FindTheBrightestDirectionalLight()
	{
		Light light = null;
		Light[] array = Object.FindObjectsOfType(typeof(Light)) as Light[];
		List<Light> list = new List<Light>();
		Light[] array2 = array;
		foreach (Light light2 in array2)
		{
			if (light2.type == LightType.Directional)
			{
				list.Add(light2);
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		light = list[0];
		foreach (Light item in list)
		{
			if (item.intensity > light.intensity)
			{
				light = item;
			}
		}
		return light;
	}

	private void Start()
	{
		waterCenter = GetComponent<Renderer>().bounds.center;
		if (movementType == WaterMovementType.directional)
		{
			speed = velocity.magnitude;
		}
		float num = GetComponent<Renderer>().bounds.size.x / gameObject.GetComponent<Renderer>().material.GetTextureScale("_MainTex").x;
		speed /= num;
		waterMaterial = GetComponent<Renderer>().material;
		Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
		Shader.EnableKeyword("WATER_EDGEBLEND_ON");
		if (movementType == WaterMovementType.flowmap)
		{
			Shader.DisableKeyword("FLOWMAP_ANIMATION_OFF");
			Shader.EnableKeyword("FLOWMAP_ANIMATION_ON");
			FlowmapAnimator flowmapAnimator = gameObject.AddComponent<FlowmapAnimator>();
			flowmapAnimator.flowSpeed = speed;
		}
		else
		{
			Shader.DisableKeyword("FLOWMAP_ANIMATION_ON");
			Shader.EnableKeyword("FLOWMAP_ANIMATION_OFF");
		}
		Light light = FindTheBrightestDirectionalLight();
		projectedLightDir = light.transform.forward - transform.up * Vector3.Dot(transform.up, light.transform.forward);
		projectedLightDir.Normalize();
		anisoDirAnimationOffset = Vector2.zero;
		causticsAnimationFrame = 0;
		if (!animatedNormalmaps)
		{
			return;
		}
		normalmapAnimation = new Texture2D[60];
		dudvfoamAnimation = new Texture2D[60];
		for (int i = 0; i < 60; i++)
		{
			string text = string.Empty;
			if (i < 10)
			{
				text = "0";
			}
			text += i;
			normalmapAnimation[i] = Resources.Load("water_hm" + text, typeof(Texture2D)) as Texture2D;
			dudvfoamAnimation[i] = Resources.Load("dudv_foam" + text, typeof(Texture2D)) as Texture2D;
			if (null == normalmapAnimation[i])
			{
				Debug.LogError("unable to find normalmap animation file 'water_normal_" + text + "'. Aborting.");
				animatedNormalmaps = false;
				break;
			}
			if (null == dudvfoamAnimation[i])
			{
				Debug.LogError("unable to find dudv animation file 'dudv_foam" + text + "'. Aborting.");
				animatedNormalmaps = false;
				break;
			}
		}
		normalmapAnimationFrame = 0;
	}

	private void OnDestroy()
	{
		Shader.DisableKeyword("WATER_EDGEBLEND_ON");
		Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
	}

	private void Update()
	{
		switch (movementType)
		{
		case WaterMovementType.island:
		{
			Vector3 vector = waterCenter - target.position;
			velocity.x = vector.x;
			velocity.y = vector.z;
			velocity = velocity.normalized * speed;
			break;
		}
		case WaterMovementType.still:
			velocity = Vector3.zero;
			break;
		}
		if ((movementType == WaterMovementType.directional) | (movementType == WaterMovementType.island))
		{
			Vector2 textureOffset = waterMaterial.GetTextureOffset("_MainTex");
			Vector2 value = textureOffset + velocity * Time.deltaTime;
			if ((velocity * Time.deltaTime).sqrMagnitude > 1f)
			{
				Vector2 vector2 = velocity * Time.deltaTime;
				Vector2 normalized = vector2.normalized;
				while (vector2.sqrMagnitude > 1f)
				{
					vector2 -= normalized;
				}
				value = textureOffset + vector2;
			}
			waterMaterial.SetTextureOffset("_MainTex", value);
			waterMaterial.SetTextureOffset("_Normalmap", value);
		}
		anisoDirAnimationOffset += new Vector2(projectedLightDir.x, projectedLightDir.z) * Time.deltaTime * 0.01f;
		Vector4 value2 = new Vector4(anisoDirAnimationOffset.x, anisoDirAnimationOffset.y, 0f, 0f);
		waterMaterial.SetVector("anisoDirAnimationOffset", value2);
		int num = causticsAnimationFrame / 16;
		float y = (float)(causticsAnimationFrame % 16 / 4) * 0.25f;
		float x = (float)(causticsAnimationFrame % 16 % 4) * 0.25f;
		Vector4 value3 = new Vector4(x, y, 0.25f, 0.25f);
		Vector4 value4;
		switch (num)
		{
		default:
			value4 = new Vector4(1f, 0f, 0f, 0f);
			break;
		case 1:
			value4 = new Vector4(0f, 1f, 0f, 0f);
			break;
		case 2:
			value4 = new Vector4(0f, 0f, 1f, 0f);
			break;
		}
		waterMaterial.SetVector("causticsOffsetAndScale", value3);
		waterMaterial.SetVector("causticsAnimationColorChannel", value4);
		causticsAnimationTime += Time.deltaTime;
		if (causticsAnimationTime >= 0.04f)
		{
			causticsAnimationFrame++;
			causticsAnimationTime = 0f;
			if (causticsAnimationFrame >= 48)
			{
				causticsAnimationFrame = 0;
			}
		}
		if (!animatedNormalmaps)
		{
			return;
		}
		normalmapAnimationTime += Time.deltaTime;
		if (normalmapAnimationTime >= 0.04f)
		{
			normalmapAnimationFrame++;
			normalmapAnimationTime = 0f;
			if (normalmapAnimationFrame >= 60)
			{
				normalmapAnimationFrame = 0;
			}
			waterMaterial.SetTexture("_NormalMap", normalmapAnimation[normalmapAnimationFrame]);
			waterMaterial.SetTexture("_DUDVFoamMap", dudvfoamAnimation[normalmapAnimationFrame]);
		}
	}
}
