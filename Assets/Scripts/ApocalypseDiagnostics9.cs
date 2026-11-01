#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApocalypseDiagnostics9 : MonoBehaviour
{
    static ApocalypseDiagnostics9 current;
    static readonly object sync = new object();
    string folder, logfile;
    int errors;
    string sceneName = "starting";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        if (current != null) return;
        var obj = new GameObject("APOCALYPSE FIX 9 DIAGNOSTICS");
        DontDestroyOnLoad(obj);
        current = obj.AddComponent<ApocalypseDiagnostics9>();
    }

    void Awake()
    {
        folder = Path.Combine(Application.persistentDataPath, "Apocalypse-DIAG9");
        logfile = Path.Combine(folder, "launch-log.txt");

        try
        {
            Directory.CreateDirectory(folder);
            File.WriteAllText(
                Path.Combine(folder, "BUILD-VERSION.txt"),
                "APOCALYPSE DIAG 9\nSource: unity2021-compile-fix9\n");
        }
        catch (Exception) { }

        Application.logMessageReceivedThreaded += OnLog;
        SceneManager.sceneLoaded += SceneLoaded;

        Record("BOOT | Unity=" + Application.unityVersion +
            " | GPU=" + SystemInfo.graphicsDeviceName +
            " | graphics=" + SystemInfo.graphicsDeviceType);

        StartCoroutine(Snapshot());
    }

    void OnLog(string message, string trace, LogType type)
    {
        if (type == LogType.Exception ||
            type == LogType.Error ||
            type == LogType.Assert)
            System.Threading.Interlocked.Increment(ref errors);

        Record("[" + type + "] " + message + "\n" + trace);
    }

    void Record(string message)
    {
        try
        {
            lock (sync)
                File.AppendAllText(
                    logfile,
                    DateTime.UtcNow.ToString("HH:mm:ss") +
                    " " + message + "\n");
        }
        catch (Exception) { }
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneName = scene.name;
        Record("SCENE LOADED: " + sceneName);
        StartCoroutine(Snapshot());
    }

    IEnumerator Snapshot()
    {
        yield return new WaitForSecondsRealtime(10f);

        var cams = Camera.allCameras;
        string report =
            "Scene=" + SceneManager.GetActiveScene().name +
            " | cameras=" + cams.Length +
            " | orientation=" + Screen.orientation +
            " | errors=" + errors + "\n";

        foreach (var cam in cams)
        {
            report += cam.name +
                " depth=" + cam.depth +
                " mask=" + cam.cullingMask +
                " target=" +
                (cam.targetTexture ? cam.targetTexture.name : "screen") +
                "\n";
        }

        Record("CAMERAS\n" + report);

        try
        {
            File.WriteAllText(
                Path.Combine(folder, "camera-report.txt"), report);

            ScreenCapture.CaptureScreenshot(
                Path.Combine(folder, "screen-10s.png"));
        }
        catch (Exception ex)
        {
            Record("SCREENSHOT ERROR " + ex);
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 225, 55),
            "APOCALYPSE DIAG 9\n" +
            sceneName + " | errors: " + errors);
    }

    void OnDestroy()
    {
        Application.logMessageReceivedThreaded -= OnLog;
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
#endif
