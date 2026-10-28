using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
	public Material[] materials;

	public float waitTime;

	public float fadeTime = 4f;

	public bool replaceShaders = true;

	private static Dictionary<Shader, Shader> replacementShaders = new Dictionary<Shader, Shader>();

	private static string[] colorPropertyNameCandidates = new string[2] { "_Color", "_TintColor" };

	public static Shader GetReplacementFor(Shader original)
	{
		Shader value;
		if (replacementShaders.TryGetValue(original, out value))
		{
			return value;
		}
		string text = original.name;
		if (text.StartsWith("Mobile/"))
		{
			text = text.Substring("Mobile/".Length);
		}
		if (!text.StartsWith("Transparent/"))
		{
			value = Shader.Find("Transparent/" + text);
		}
		replacementShaders[original] = value;
		return value;
	}

	private IEnumerator Start()
	{
		Material[] mat = materials;
		if (mat == null || mat.Length == 0)
		{
			Fade fade = this;
			Material[] array;
			mat = (array = GetComponent<Renderer>().materials);
			fade.materials = array;
		}
		if (waitTime > 0f)
		{
			yield return new WaitForSeconds(waitTime);
		}
		if (replaceShaders)
		{
			Material[] array2 = mat;
			foreach (Material material in array2)
			{
				Shader replacementFor = GetReplacementFor(material.shader);
				if (replacementFor != null)
				{
					material.shader = replacementFor;
				}
			}
		}
		int materialCount = mat.Length;
		List<string> colorPropertyNames = new List<string>(materialCount);
		Material[] array3 = mat;
		foreach (Material material2 in array3)
		{
			bool flag = false;
			string[] array4 = colorPropertyNameCandidates;
			foreach (string text in array4)
			{
				flag = material2.HasProperty(text);
				if (flag)
				{
					colorPropertyNames.Add(text);
					break;
				}
			}
			if (!flag)
			{
				colorPropertyNames.Add(null);
			}
		}
		for (float t = 0f; t < fadeTime; t += Time.deltaTime)
		{
			for (int l = 0; l < materialCount; l++)
			{
				Material material3 = mat[l];
				string text2 = colorPropertyNames[l];
				if (text2 != null)
				{
					Color color = material3.GetColor(text2);
					color.a = 1f - t / fadeTime;
					material3.SetColor(text2, color);
				}
			}
			yield return null;
		}
		SendMessage("FadeCompleted", SendMessageOptions.DontRequireReceiver);
	}
}
