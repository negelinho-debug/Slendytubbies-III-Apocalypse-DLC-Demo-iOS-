using UnityEngine;

public static class TransformExtension
{
	public static Transform FindChildByRecursive(this Transform aParent, string aName)
	{
		Transform transform = aParent.Find(aName);
		if (transform != null)
		{
			return transform;
		}
		foreach (Transform item in aParent)
		{
			transform = item.FindChildByRecursive(aName);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}
}
