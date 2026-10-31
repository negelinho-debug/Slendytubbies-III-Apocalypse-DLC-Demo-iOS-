#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApocalypseIOSVisualFix7 : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        var go = new GameObject("Apocalypse iOS Visual Fix 7");
        DontDestroyOnLoad(go);
        go.AddComponent<ApocalypseIOSVisualFix7>();
    }

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void Start() { StartCoroutine(CheckCameras()); }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(CheckCameras());
    }

    private IEnumerator CheckCameras()
    {
        yield return null;
        yield return null;
        int active = 0;
        Camera spareMain = null;

        foreach (var camera in Resources.FindObjectsOfTypeAll<Camera>())
        {
            if (camera == null || !camera.gameObject.scene.IsValid()) continue;
            if (camera.isActiveAndEnabled) active++;
            if (camera.gameObject.CompareTag("MainCamera")) spareMain = camera;

            foreach (var behaviour in camera.GetComponents<MonoBehaviour>())
            {
                if (behaviour == null) continue;
                if (behaviour.GetType().Name.StartsWith(
                    "CameraFilterPack_", StringComparison.Ordinal))
                    behaviour.enabled = false;
            }
        }

        if (active == 0 && spareMain != null)
        {
            spareMain.gameObject.SetActive(true);
            spareMain.enabled = true;
            active = 1;
        }

        if (spareMain != null && spareMain.isActiveAndEnabled)
        {
            if (spareMain.cullingMask == 0) spareMain.cullingMask = ~0;
            if (spareMain.targetTexture != null) spareMain.targetTexture = null;
        }

        Debug.Log("[APOCALYPSE FIX7] Scene=" +
            SceneManager.GetActiveScene().name + "; active cameras=" + active);
        if (active == 0)
            Debug.LogError("[APOCALYPSE FIX7] No active camera in this scene!");
    }
}
#endif
