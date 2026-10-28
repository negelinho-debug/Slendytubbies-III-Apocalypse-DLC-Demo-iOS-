using UnityEngine;

public class CustardCounter : MonoBehaviour
{
	public Texture2D custardIcon;

	public int maxCustards;

	public int totalCustards;

	private void Update()
	{
		totalCustards = maxCustards - GameObject.FindGameObjectsWithTag("Custard").Length;
	}

	private void OnGUI()
	{
		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		GUI.DrawTexture(new Rect(Screen.width / 2 - 60, 2f, 50f, 50f), custardIcon);
		GUI.Label(new Rect(Screen.width / 2 - 12, 15f, 50f, 50f), "<size=20>" + totalCustards + "/" + maxCustards + "</size>");
	}
}
