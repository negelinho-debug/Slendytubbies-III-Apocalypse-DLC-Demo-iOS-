using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
	public int m_nNumOfEffects;

	public bool m_bLockNums;

	public List<EffectData> m_kEffectGenList = new List<EffectData>();

	private int m_nNowIndex;

	private void Awake()
	{
		for (int i = 0; i < m_kEffectGenList.Count; i++)
		{
			Invoke("GenEffect", m_kEffectGenList[i].m_fTimeSec);
		}
		Comp comparer = new Comp();
		m_kEffectGenList.Sort(comparer);
	}

	private void Update()
	{
		CheckTransfromUpdate();
	}

	private void GenEffect()
	{
		EffectData effectData = m_kEffectGenList[m_nNowIndex];
		if (effectData != null)
		{
			if (effectData.m_goEffect != null)
			{
				GameObject gameObject = Object.Instantiate(effectData.m_goEffect);
				gameObject.transform.parent = transform;
				gameObject.name = m_nNowIndex.ToString();
				UpdateEffectTransformByIndex(m_nNowIndex);
				UPdateRenderLayerByIndex(m_nNowIndex);
			}
			m_nNowIndex++;
		}
	}

	private void CheckTransfromUpdate()
	{
		foreach (Transform item in base.transform)
		{
			int index = int.Parse(item.name);
			EffectData effectData = m_kEffectGenList[index];
			if (effectData == null)
			{
				break;
			}
			if (item.position != effectData.m_goPos)
			{
				effectData.m_goPos = item.position;
			}
			if (item.localRotation.eulerAngles != effectData.m_goRotation)
			{
				effectData.m_goRotation = item.localRotation.eulerAngles;
			}
			if (item.localScale != effectData.m_goScale)
			{
				effectData.m_goScale = item.localScale;
			}
		}
	}

	public void UpdateEffectTransformByIndex(int nIndex)
	{
		Transform transform = base.transform.Find(nIndex.ToString());
		if (!(transform == null))
		{
			EffectData effectData = m_kEffectGenList[nIndex];
			if (effectData != null)
			{
				transform.position = effectData.m_goPos;
				transform.localRotation = new Quaternion
				{
					eulerAngles = effectData.m_goRotation
				};
				transform.localScale = effectData.m_goScale;
			}
		}
	}

	public ParticleSystem CheckHasParticleSystem(int nIndex)
	{
		Transform transform = base.transform.Find(nIndex.ToString());
		if (transform == null)
		{
			return null;
		}
		return transform.gameObject.GetComponent<ParticleSystem>();
	}

	public RenderEffect CheckHasRenderEffectScript(int nIndex)
	{
		Transform transform = base.transform.Find(nIndex.ToString());
		if (transform == null)
		{
			return null;
		}
		return transform.gameObject.GetComponent<RenderEffect>();
	}

	public void UPdateRenderLayerByIndex(int nIndex)
	{
		Transform transform = base.transform.Find(nIndex.ToString());
		if (!(transform == null))
		{
			EffectData effectData = m_kEffectGenList[nIndex];
			if (effectData != null)
			{
				Renderer component = transform.gameObject.GetComponent<Renderer>();
				component.sortingLayerID = effectData.m_SortingLayerID;
				component.sortingOrder = effectData.m_SortingOrder;
			}
		}
	}
}
