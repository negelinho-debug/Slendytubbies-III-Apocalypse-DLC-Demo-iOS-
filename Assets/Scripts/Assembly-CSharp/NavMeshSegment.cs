using AC;
using UnityEngine;

public class NavMeshSegment : MonoBehaviour
{
	private SceneSettings sceneSettings;

	private void Awake()
	{
		Hide();
		if (!GameObject.FindWithTag("GameEngine") || !GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>())
		{
			return;
		}
		sceneSettings = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
		if (sceneSettings.navigationMethod == AC_NavigationMethod.UnityNavigation)
		{
			if (LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.navMeshLayer) == -1)
			{
				Debug.LogWarning("No 'NavMesh' layer exists - please define one in the Tags Manager.");
			}
			else
			{
				gameObject.layer = LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.navMeshLayer);
			}
		}
	}

	public void Hide()
	{
		if ((bool)GetComponent<MeshRenderer>())
		{
			GetComponent<MeshRenderer>().enabled = false;
		}
	}

	public void Show()
	{
		if ((bool)GetComponent<MeshRenderer>() && (bool)GetComponent<MeshFilter>() && (bool)GetComponent<MeshCollider>() && (bool)GetComponent<MeshCollider>().sharedMesh)
		{
			GetComponent<MeshFilter>().mesh = GetComponent<MeshCollider>().sharedMesh;
			GetComponent<MeshRenderer>().enabled = true;
			GetComponent<MeshRenderer>().castShadows = false;
			GetComponent<MeshRenderer>().receiveShadows = false;
		}
	}
}
