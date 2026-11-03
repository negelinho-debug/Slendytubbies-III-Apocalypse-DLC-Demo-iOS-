using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(10000)]
public sealed class ApocalypseFix13CampaignMove : MonoBehaviour
{
    AC.Player player;
    Vector2 move;
    Vector3 positionBefore;
    float nextReport;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Install()
    {
        if (FindObjectOfType<ApocalypseFix13CampaignMove>() != null) return;
        GameObject obj = new GameObject("FIX13 Campaign Touch Diagnostic");
        DontDestroyOnLoad(obj);
        obj.AddComponent<ApocalypseFix13CampaignMove>();
    }

    void Update()
    {
        move = Vector2.zero;
        if (SceneManager.GetActiveScene().name != "DEMOLEVEL") return;
        if (player == null) player = FindObjectOfType<AC.Player>();
        if (player == null) { Report("player=MISSING"); return; }
        if (player.isCutscene()) { Report("cutscene=true"); return; }
        positionBefore = player.transform.position;

        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) continue;
            Vector2 p = new Vector2(t.position.x, Screen.height - t.position.y);
            if (p.x < Screen.width * .40f && p.y > Screen.height * .48f)
            {
                Vector2 center = new Vector2(Screen.width * .16f, Screen.height * .79f);
                move = Vector2.ClampMagnitude((p - center) / (Screen.height * .13f), 1f);
                break;
            }
        }
        Report("player=" + player.name + " move=" + move + " touches=" + Input.touchCount);
    }

    void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name != "DEMOLEVEL" || player == null) return;
        if (player.isCutscene() || move.sqrMagnitude < .02f) return;

        if (Vector3.Distance(positionBefore, player.transform.position) > .003f) return;

        Vector3 delta = new Vector3(move.x, 0f, -move.y) * (2.3f * Time.deltaTime);
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null && cc.enabled) cc.Move(delta);
        else
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic) rb.MovePosition(rb.position + delta);
            else player.transform.position += delta;
        }

        // No free camera rotation in campaign.
    }

    void Report(string value)
    {
        if (Time.unscaledTime < nextReport) return;
        Debug.Log("[FIX13] " + value);
        nextReport = Time.unscaledTime + 3f;
    }
}
