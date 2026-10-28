using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EffectDemo : MonoBehaviour
{
	public const string EFFECT_ASSET_PATH = "Assets/Prefab/";

	public List<GameObject> m_EffectPrefabList = new List<GameObject>();

	public bool m_LookAtEffect = true;

	private GameObject m_NowShowEffect;

	private int m_NowIndex;

	private string m_NowEffectName;

	private void Awake()
	{
		if (Application.isPlaying)
		{
			m_NowIndex = 1;
			GenPrevEffect();
		}
	}

	private void OnDestroy()
	{
		Object.DestroyImmediate(m_NowShowEffect);
	}

	private void LateUpdate()
	{
		if (Application.isPlaying && m_LookAtEffect && (bool)m_NowShowEffect)
		{
			transform.LookAt(m_NowShowEffect.transform.position);
		}
	}

	private void OnGUI()
	{
		if (Application.isPlaying)
		{
			if (GUI.Button(new Rect(0f, 25f, 80f, 50f), "Prev"))
			{
				GenPrevEffect();
			}
			if (GUI.Button(new Rect(90f, 25f, 80f, 50f), "Next"))
			{
				GenNextEffect();
			}
			GUI.Label(new Rect(5f, 0f, 350f, 50f), m_NowEffectName);
		}
	}

	private void GenPrevEffect()
	{
		m_NowIndex--;
		if (m_NowIndex < 0)
		{
			m_NowIndex = 0;
			return;
		}
		if (m_NowShowEffect != null)
		{
			Object.Destroy(m_NowShowEffect);
		}
		m_NowShowEffect = Object.Instantiate(m_EffectPrefabList[m_NowIndex]);
		m_NowEffectName = m_NowShowEffect.name;
	}

	private void GenNextEffect()
	{
		m_NowIndex++;
		if (m_NowIndex >= m_EffectPrefabList.Count)
		{
			m_NowIndex = m_EffectPrefabList.Count - 1;
			return;
		}
		if (m_NowShowEffect != null)
		{
			Object.Destroy(m_NowShowEffect);
		}
		m_NowShowEffect = Object.Instantiate(m_EffectPrefabList[m_NowIndex]);
		m_NowEffectName = m_NowShowEffect.name;
	}
}
