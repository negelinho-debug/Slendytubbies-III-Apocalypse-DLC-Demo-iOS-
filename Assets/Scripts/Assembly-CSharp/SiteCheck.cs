using UnityEngine;

public class SiteCheck : MonoBehaviour
{
	public string theurl;

	private void Start()
	{
		Application.ExternalEval("if(document.domain != 'gameshed.com') { document.location='http://www.gameshed.com/Scary-Games/Slendytubbies-3--Campaign-Demo/'; }");
		if (!Application.dataPath.Contains("gameshed.com") || !Application.absoluteURL.Contains("gameshed.com"))
		{
			Application.LoadLevel(4);
		}
	}

	private void OnGUI()
	{
	}
}
