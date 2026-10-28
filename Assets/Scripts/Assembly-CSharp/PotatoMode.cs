using UnityEngine;

public class PotatoMode : MonoBehaviour
{
	private void Start()
	{
		if (QualitySettings.GetQualityLevel() == 0)
		{
			PotatoNow();
		}
	}

	private void PotatoNow()
	{
		MeshRenderer[] array = Object.FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
		MeshRenderer[] array2 = array;
		foreach (Renderer renderer in array2)
		{
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				material.shader = Shader.Find("Diffuse");
			}
		}
		SkinnedMeshRenderer[] array3 = Object.FindObjectsOfType(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer[];
		SkinnedMeshRenderer[] array4 = array3;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array4)
		{
			Material[] materials2 = skinnedMeshRenderer.materials;
			foreach (Material material2 in materials2)
			{
				material2.shader = Shader.Find("Diffuse");
			}
		}
	}
}
