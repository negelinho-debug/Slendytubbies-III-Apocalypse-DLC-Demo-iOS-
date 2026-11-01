using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splashes : MonoBehaviour
{
	[Header("Splash Screen")]
	public bool useSplashScreen = true;

	public GameObject splashesContent;

	private List<Graphic> splashes = new List<Graphic>();

	public float splashStayDuration = 3f;

	public float splashCrossFadeTime = 1f;


    private void Start()
    {
        if (!useSplashScreen ||
            splashesContent == null)
        {
            Debug.LogWarning(
                "[FIX10] Missing splash content");
            GameObject overlay =
                GameObject.Find("Splashes");
            if (overlay != null &&
                overlay != gameObject)
                overlay.SetActive(false);
            enabled = false;
            return;
        }
        Graphic[] all =
            splashesContent.GetComponentsInChildren<Graphic>(
                true);
        if (all == null || all.Length == 0)
            return;
        foreach (Graphic graphic in all)
            if (graphic !=
                splashesContent.GetComponent<Graphic>())
                splashes.Add(graphic);
        StartCoroutine(PlayAllSplashes());
    }

	private IEnumerator PlayAllSplashes()
	{
		if (!splashesContent.activeSelf)
		{
			splashesContent.SetActive(true);
		}
		foreach (Graphic t in splashes)
		{
			t.gameObject.SetActive(true);
			t.canvasRenderer.SetAlpha(0f);
			t.CrossFadeAlpha(1f, splashCrossFadeTime, false);
			yield return new WaitForSeconds(splashStayDuration + splashCrossFadeTime);
			t.CrossFadeAlpha(0f, splashCrossFadeTime, false);
			yield return new WaitForSeconds(splashCrossFadeTime);
			t.gameObject.SetActive(false);
		}
		splashesContent.GetComponent<Graphic>().CrossFadeAlpha(0f, 0.5f, false);
		yield return new WaitForSeconds(0.5f);
		splashesContent.gameObject.SetActive(false);
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
