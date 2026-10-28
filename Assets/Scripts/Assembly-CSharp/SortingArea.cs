using System;
using UnityEngine;

[Serializable]
public class SortingArea
{
	public float z;

	public int order;

	public string layer;

	public Color color;

	public int scale = 100;

	public SortingArea(SortingArea lastArea)
	{
		z = lastArea.z + 1f;
		order = lastArea.order + 1;
		layer = string.Empty;
		scale = lastArea.scale;
		color = GetRandomColor();
	}

	public SortingArea(SortingArea area1, SortingArea area2)
	{
		z = (area1.z + area2.z) / 2f;
		float num = (float)area1.order + (float)area2.order;
		order = (int)(num / 2f);
		float num2 = (float)area1.scale + (float)area2.scale;
		scale = (int)(num2 / 2f);
		layer = string.Empty;
		color = GetRandomColor();
	}

	public SortingArea(float _z, int _order)
	{
		z = _z;
		order = _order;
		layer = string.Empty;
		scale = 100;
		color = GetRandomColor();
	}

	private Color GetRandomColor()
	{
		return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
	}
}
