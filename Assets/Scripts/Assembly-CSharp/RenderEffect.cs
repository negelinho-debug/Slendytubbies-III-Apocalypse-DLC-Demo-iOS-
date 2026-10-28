using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderEffect : MonoBehaviour
{
	public RenderBillBoardType m_BillBoardType;

	private Camera m_ReferenceCamera;

	public bool m_EnableBillBoard;

	public bool m_EnableSetSortLayer = true;

	public Renderer m_Render;

	public List<MaterialEffect> m_MaterialEffects = new List<MaterialEffect>();

	private float m_TimeLine;

	[HideInInspector]
	public int m_SortingLayerID;

	[HideInInspector]
	public int m_SortingOrder;

	private void Awake()
	{
		m_ReferenceCamera = Camera.main;
		m_Render = GetComponent<Renderer>();
		if (!(m_Render == null))
		{
		}
	}

	private void OnEnable()
	{
		RefreshMaterial();
	}

	public void UpdateRenderLayer()
	{
		if (m_EnableSetSortLayer)
		{
			m_Render.sortingLayerID = m_SortingLayerID;
			m_Render.sortingOrder = m_SortingOrder;
		}
	}

	public void RefreshMaterial()
	{
		if (m_Render == null)
		{
			m_Render = GetComponent<Renderer>();
			if (m_Render == null)
			{
				return;
			}
		}
		int num = 0;
		for (num = 0; num < m_Render.sharedMaterials.Length; num++)
		{
			if (m_MaterialEffects.Count <= num)
			{
				MaterialEffect item = new MaterialEffect(m_Render.sharedMaterials[num]);
				m_MaterialEffects.Add(item);
			}
			else
			{
				m_MaterialEffects[num].ReInitMaterial(m_Render.sharedMaterials[num]);
			}
		}
		int num2 = m_MaterialEffects.Count - 1;
		while (num <= num2)
		{
			m_MaterialEffects.RemoveAt(num2);
			num2--;
		}
		UpdateRenderLayer();
	}

	private void UpdateBillBoard()
	{
		if (m_EnableBillBoard)
		{
			if (m_ReferenceCamera == null)
			{
				m_ReferenceCamera = Camera.main;
			}
			if (m_BillBoardType == RenderBillBoardType.Normal)
			{
				Vector3 worldPosition = transform.position + m_ReferenceCamera.transform.rotation * Vector3.forward;
				Vector3 worldUp = m_ReferenceCamera.transform.rotation * Vector3.up;
				transform.LookAt(worldPosition, worldUp);
			}
			else if (m_BillBoardType == RenderBillBoardType.Vertical)
			{
				Vector3 forward = m_ReferenceCamera.transform.forward;
				forward.y = 0f;
				transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
			}
			else if (m_BillBoardType == RenderBillBoardType.Horizontal)
			{
				Vector3 worldPosition2 = transform.position + m_ReferenceCamera.transform.rotation * Vector3.down;
				Vector3 worldUp2 = m_ReferenceCamera.transform.rotation * Vector3.up;
				transform.LookAt(worldPosition2, worldUp2);
				Vector3 eulerAngles = transform.rotation.eulerAngles;
				eulerAngles.x = 90f;
				transform.rotation = Quaternion.Euler(eulerAngles);
			}
		}
	}

	private void Update()
	{
		m_TimeLine += Time.deltaTime;
		foreach (MaterialEffect materialEffect in m_MaterialEffects)
		{
			materialEffect.UpdateEffect(m_TimeLine);
		}
	}

	private void LateUpdate()
	{
		UpdateBillBoard();
	}

	public void Sim(float timer)
	{
		UpdateBillBoard();
		foreach (MaterialEffect materialEffect in m_MaterialEffects)
		{
			materialEffect.UpdateEffect(timer);
		}
	}
}
