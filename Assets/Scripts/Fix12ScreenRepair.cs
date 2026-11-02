using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Fix12ScreenRepair : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Create()
    {
        if (FindObjectOfType<Fix12ScreenRepair>() != null) return;
        var obj = new GameObject("FIX12 Screen Repair");
        DontDestroyOnLoad(obj);
        obj.AddComponent<Fix12ScreenRepair>();
    }

    void OnEnable() { SceneManager.sceneLoaded += OnLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnLoaded; }
    void Start() { OnLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single); }

    void OnLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Chap4Menu") StartCoroutine(RepairMenu());
    }

    IEnumerator RepairMenu()
    {
        yield return null;
        yield return new WaitForEndOfFrame();
        foreach (Camera cam in Camera.allCameras)
        {
            if (cam == null || cam.targetTexture != null) continue;
            Debug.Log("[FIX12] Menu camera " + cam.name + " rect=" + cam.rect);
            if (cam.rect.width < 0.99f || cam.rect.height < 0.99f ||
                cam.rect.x > 0.01f || cam.rect.y > 0.01f)
            {
                cam.rect = new Rect(0f, 0f, 1f, 1f);
                cam.ResetAspect();
                Debug.Log("[FIX12] Full viewport: " + cam.name);
            }
        }
    }
}
