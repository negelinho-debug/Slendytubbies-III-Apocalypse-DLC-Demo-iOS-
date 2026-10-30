using TranslatorSystem;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	public string[] thetext;

	public int language;

	public int ID = 1;

	public Rect rect = new Rect(0f, 0f, 300f, 20f);

	private void Awake()
	{
		language = 0;
		Translator translator = new Translator();
		if (PlayerPrefs.GetInt("Language") > 0)
		{
			thetext[language] = thetext[language];
		}
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		GUI.depth = 2;
		float num = 0f;
		Vector3 vector = Camera.main.WorldToScreenPoint(transform.position);
		if (vector.z * 3f < 50f)
		{
			num = vector.z * 3f;
		}
		else
		{
			num = 50f;
		}
		if (vector.z > 0f)
		{
			GUI.Label(new Rect(vector.x - 30f, (float)Screen.height - vector.y - 1f - 21f, 200f, 30f), "<color=yellow><b>" + thetext[language] + "</b></color>");
		}
	}
}
