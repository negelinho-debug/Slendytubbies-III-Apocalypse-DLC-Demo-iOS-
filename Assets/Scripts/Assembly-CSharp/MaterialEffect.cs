using System;
using UnityEngine;

[Serializable]
public class MaterialEffect
{
	public Material m_EffectMaterial;

	public bool m_EnableAlphaAnimation;

	public float m_AlphaAnimationTimeScale = 1f;

	public AnimationCurve m_AlphaCurve = new AnimationCurve();

	public Texture m_MainTexture;

	public Texture m_MaskTexutre;

	public TextureWrapMode m_MainTexWrapMode;

	public TextureWrapMode m_MaskTexWrapMode;

	public bool m_EnableUVScroll;

	public Vector2 m_UVScrollMainTex;

	public Vector2 m_UVScrollCutTex;

	public MaterialEffect(Material material)
	{
	}

	public void ReInitMaterial(Material material)
	{
		if (!(material == null))
		{
			m_EffectMaterial = material;
			if (material.HasProperty("_MainTex"))
			{
				m_MainTexture = material.GetTexture("_MainTex");
			}
			if (material.HasProperty("_CutTex"))
			{
				m_MaskTexutre = material.GetTexture("_CutTex");
			}
		}
	}

	public void UpdateEffect(float execueTime)
	{
		if (m_MainTexture != null && m_MainTexWrapMode != m_MainTexture.wrapMode)
		{
			m_MainTexture.wrapMode = m_MainTexWrapMode;
		}
		if (m_MaskTexutre != null && m_MaskTexWrapMode != m_MaskTexutre.wrapMode)
		{
			m_MaskTexutre.wrapMode = m_MaskTexWrapMode;
		}
		if (m_EnableUVScroll)
		{
			if ((bool)m_MainTexture)
			{
				m_EffectMaterial.SetTextureOffset("_MainTex", m_UVScrollMainTex * execueTime);
			}
			if ((bool)m_MaskTexutre)
			{
				m_EffectMaterial.SetTextureOffset("_CutTex", m_UVScrollCutTex * execueTime);
			}
		}
	}

	private void SetAlpha(float value)
	{
		Color color = m_EffectMaterial.color;
		color.a = value;
		m_EffectMaterial.color = color;
	}
}
