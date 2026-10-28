using System;
using UnityEngine;

[Serializable]
public class EffectData
{
	public bool m_bFoldoutOpen = true;

	public float m_fTimeSec;

	public GameObject m_goEffect;

	public bool m_bTransformFoldout = true;

	public Vector3 m_goPos = new Vector3(0f, 0f, 0f);

	public Vector3 m_goRotation = new Vector3(0f, 0f, 0f);

	public Vector3 m_goScale = new Vector3(1f, 1f, 1f);

	public bool m_bSortingFoldout = true;

	public int m_SortingLayerID;

	public int m_SortingOrder;
}
