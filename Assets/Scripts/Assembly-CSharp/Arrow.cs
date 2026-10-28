using System;
using AC;
using UnityEngine;

[Serializable]
public class Arrow
{
	public bool isPresent;

	public Texture2D texture;

	public Cutscene linkedCutscene;

	public Rect rect;

	public Arrow()
	{
		isPresent = false;
	}

	public void Run()
	{
		if ((bool)linkedCutscene)
		{
			linkedCutscene.SendMessage("Interact");
		}
	}

	public void Draw()
	{
		if ((bool)texture)
		{
			GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill, true);
		}
	}
}
