using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeshTK
{
	public class CombineTools : ScriptableObject
	{
		public static void CombineChildren(GameObject parent)
		{
			List<Material> list = new List<Material>();
			ArrayList arrayList = new ArrayList();
			MeshFilter[] componentsInChildren = parent.GetComponentsInChildren<MeshFilter>();
			MeshFilter[] array = componentsInChildren;
			foreach (MeshFilter meshFilter in array)
			{
				MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();
				if (meshRenderer != null)
				{
					for (int s = 0; s < meshFilter.sharedMesh.subMeshCount; s++)
					{
						if (meshFilter.sharedMesh.GetTriangles(s).Length == 0)
						{
							continue;
						}
						int num = list.FindIndex((Material p) => p != null && p.name == meshRenderer.sharedMaterials[s].name);
						if (num == -1)
						{
							if (meshRenderer.sharedMaterials.Length > s)
							{
								list.Add(meshRenderer.sharedMaterials[s]);
							}
							else
							{
								list.Add(null);
							}
							num = list.Count - 1;
						}
						arrayList.Add(new ArrayList());
						CombineInstance combineInstance = new CombineInstance
						{
							transform = meshFilter.transform.localToWorldMatrix * parent.transform.worldToLocalMatrix,
							subMeshIndex = s,
							mesh = meshFilter.sharedMesh
						};
						(arrayList[num] as ArrayList).Add(combineInstance);
					}
				}
				meshFilter.gameObject.SetActive(false);
			}
			MeshFilter meshFilter2 = parent.GetComponent<MeshFilter>();
			if (!meshFilter2)
			{
				meshFilter2 = parent.AddComponent<MeshFilter>();
			}
			Mesh[] array2 = new Mesh[list.Count];
			CombineInstance[] array3 = new CombineInstance[list.Count];
			for (int num2 = 0; num2 < list.Count; num2++)
			{
				CombineInstance[] combine = (arrayList[num2] as ArrayList).ToArray(typeof(CombineInstance)) as CombineInstance[];
				array2[num2] = new Mesh();
				array2[num2].CombineMeshes(combine, true, true);
				array3[num2] = default(CombineInstance);
				array3[num2].mesh = array2[num2];
				array3[num2].subMeshIndex = 0;
			}
			meshFilter2.sharedMesh = new Mesh();
			meshFilter2.sharedMesh.CombineMeshes(array3, false, false);
			Mesh[] array4 = array2;
			foreach (Mesh mesh in array4)
			{
				mesh.Clear();
				Object.DestroyImmediate(mesh);
			}
			MeshRenderer meshRenderer2 = parent.GetComponent<MeshRenderer>();
			if (!meshRenderer2)
			{
				meshRenderer2 = parent.AddComponent<MeshRenderer>();
			}
			Material[] materials = list.ToArray();
			meshRenderer2.materials = materials;
			parent.SetActive(true);
		}
	}
}
