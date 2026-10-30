using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
	public bool fadein;

	public bool fadeout;

	private float fadetime;

	public int speed = 40;

	public bool newGUI;

	private float maxFade = 255f;

	private bool isFlash;

	private bool beginFadeOut;

	private void OnEnable()
	{
		if (!newGUI)
		{
			maxFade = 155f;
		}
		if (fadein)
		{
			fadetime = 0f;
		}
		if (fadeout)
		{
			fadetime = maxFade;
		}
	}

	private void Update()
	{
		if (fadein)
		{
			if (fadetime < maxFade)
			{
				fadetime += Time.deltaTime * (float)speed;
			}
			else
			{
				fadein = false;
			}
		}
		if (fadeout)
		{
			if (fadetime > 1f)
			{
				fadetime -= Time.deltaTime * (float)speed;
			}
			else
			{
				fadeout = false;
			}
		}
		if (newGUI)
		{
			GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, fadetime / 255f);
		}
		else
		{
			GetComponent<UnityEngine.UI.Graphic>().color = new Color(GetComponent<UnityEngine.UI.Graphic>().color.r, GetComponent<UnityEngine.UI.Graphic>().color.g, GetComponent<UnityEngine.UI.Graphic>().color.b, fadetime / 255f);
		}
		if (isFlash && !fadein)
		{
			if (!beginFadeOut)
			{
				fadeout = true;
				FadeOut();
				beginFadeOut = true;
			}
			if (!fadeout)
			{
				isFlash = false;
			}
		}
	}

	public void FadeIn()
	{
		if (newGUI)
		{
			GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 0f);
		}
		else
		{
			GetComponent<UnityEngine.UI.Graphic>().color = new Color(GetComponent<UnityEngine.UI.Graphic>().color.r, GetComponent<UnityEngine.UI.Graphic>().color.g, GetComponent<UnityEngine.UI.Graphic>().color.b, 0f);
		}
		fadetime = 0f;
		fadeout = false;
		fadein = true;
	}

	public void FadeOut()
	{
		if (newGUI)
		{
			GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, maxFade);
		}
		else
		{
			GetComponent<UnityEngine.UI.Graphic>().color = new Color(GetComponent<UnityEngine.UI.Graphic>().color.r, GetComponent<UnityEngine.UI.Graphic>().color.g, GetComponent<UnityEngine.UI.Graphic>().color.b, maxFade);
		}
		fadetime = maxFade;
		fadeout = true;
		fadein = false;
	}

	public void Flash()
	{
		beginFadeOut = false;
		fadeout = false;
		FadeIn();
		isFlash = true;
	}
}
