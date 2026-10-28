using UnityEngine;
using UnityEngine.UI;

public class CheckMountains : MonoBehaviour
{
	public GameObject altChapter;

	private void Start()
	{
		if (PlayerPrefs.GetString("Mountains") == "1")
		{
			transform.parent.GetComponent<Button>().targetGraphic = altChapter.GetComponent<Text>();
			altChapter.SetActive(true);
			gameObject.SetActive(false);
			PlayerPrefs.SetString("CameraDamage", "1");
		}
		else
		{
			PlayerPrefs.SetString("CameraDamage", "0");
		}
	}
}
