using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class Highlight : MonoBehaviour
	{
		public bool brightenMaterials = true;

		private float maxHighlight = 2f;

		private float highlight = 1f;

		private int direction = 1;

		private float fadeStartTime;

		private float fadeTime = 0.3f;

		private HighlightState highlightState;

		private List<Color> originalColors = new List<Color>();

		public float GetHighlightIntensity()
		{
			return (highlight - 1f) / maxHighlight;
		}

		private void Awake()
		{
			if ((bool)GetComponent<Renderer>())
			{
				Material[] materials = GetComponent<Renderer>().materials;
				foreach (Material material in materials)
				{
					if (material.HasProperty("_Color"))
					{
						originalColors.Add(material.color);
					}
				}
			}
			Component[] componentsInChildren = GetComponentsInChildren<Renderer>();
			Component[] array = componentsInChildren;
			for (int j = 0; j < array.Length; j++)
			{
				Renderer renderer = (Renderer)array[j];
				Material[] materials2 = renderer.materials;
				foreach (Material material2 in materials2)
				{
					if (material2.HasProperty("_Color"))
					{
						originalColors.Add(material2.color);
					}
				}
			}
			if ((bool)GetComponent<UnityEngine.UI.Graphic>())
			{
				originalColors.Add(GetComponent<UnityEngine.UI.Graphic>().color);
			}
		}

		private void FixedUpdate()
		{
			if (highlightState == HighlightState.None)
			{
				return;
			}
			if (direction == 1)
			{
				highlight = Mathf.Lerp(1f, maxHighlight, AdvGame.Interpolate(fadeStartTime, fadeTime, MoveMethod.Linear, null));
				if (highlight >= maxHighlight)
				{
					highlight = maxHighlight;
					if (highlightState == HighlightState.Flash || highlightState == HighlightState.Pulse)
					{
						direction = -1;
						fadeStartTime = Time.time;
					}
					else
					{
						highlightState = HighlightState.None;
					}
				}
			}
			else
			{
				highlight = Mathf.Lerp(maxHighlight, 1f, AdvGame.Interpolate(fadeStartTime, fadeTime, MoveMethod.Linear, null));
				if (highlight <= 1f)
				{
					highlight = 1f;
					if (highlightState == HighlightState.Pulse)
					{
						direction = 1;
						fadeStartTime = Time.time;
					}
					else
					{
						highlightState = HighlightState.None;
					}
				}
			}
			if (brightenMaterials)
			{
				UpdateMaterials();
			}
		}

		public void HighlightOn()
		{
			highlightState = HighlightState.Normal;
			direction = 1;
			fadeStartTime = Time.time;
			if (highlight > 1f)
			{
				fadeStartTime -= (highlight - 1f) / (maxHighlight - 1f) * fadeTime;
			}
			else
			{
				highlight = 1f;
			}
		}

		public void HighlightOnInstant()
		{
			highlightState = HighlightState.None;
			highlight = maxHighlight;
			UpdateMaterials();
		}

		public void HighlightOff()
		{
			highlightState = HighlightState.Normal;
			direction = -1;
			fadeStartTime = Time.time;
			if (highlight < maxHighlight)
			{
				fadeStartTime -= (maxHighlight - highlight) / (maxHighlight - 1f) * fadeTime;
			}
			else
			{
				highlight = maxHighlight;
			}
		}

		public void Flash()
		{
			if (highlightState != HighlightState.Flash && (highlightState == HighlightState.None || direction == -1))
			{
				highlightState = HighlightState.Flash;
				highlight = 1f;
				direction = 1;
				fadeStartTime = Time.time;
			}
		}

		public float GetFlashTime()
		{
			return fadeTime * 2f;
		}

		public float GetFadeTime()
		{
			return fadeTime;
		}

		public void Pulse()
		{
			highlightState = HighlightState.Pulse;
			highlight = 1f;
			direction = 1;
			fadeStartTime = Time.time;
		}

		public float GetHighlightAlpha()
		{
			return highlight - 1f;
		}

		public void HighlightOffInstant()
		{
			highlightState = HighlightState.None;
			highlight = 1f;
			UpdateMaterials();
		}

		private void UpdateMaterials()
		{
			int num = 0;
			if ((bool)GetComponent<Renderer>())
			{
				Material[] materials = GetComponent<Renderer>().materials;
				foreach (Material material in materials)
				{
					if (material.HasProperty("_Color"))
					{
						float a = material.color.a;
						Color color = originalColors[num] * highlight;
						color.a = a;
						material.color = color;
						num++;
					}
				}
			}
			Component[] componentsInChildren = GetComponentsInChildren<Renderer>();
			Component[] array = componentsInChildren;
			for (int j = 0; j < array.Length; j++)
			{
				Renderer renderer = (Renderer)array[j];
				Material[] materials2 = renderer.materials;
				foreach (Material material2 in materials2)
				{
					if (originalColors.Count <= num)
					{
						break;
					}
					if (material2.HasProperty("_Color"))
					{
						float a = material2.color.a;
						Color color2 = originalColors[num] * highlight;
						color2.a = a;
						material2.color = color2;
						num++;
					}
				}
			}
			if ((bool)GetComponent<UnityEngine.UI.Graphic>())
			{
				float a = Mathf.Lerp(0.2f, 1f, highlight - 1f);
				Color color3 = originalColors[num];
				color3.a = a;
				GetComponent<UnityEngine.UI.Graphic>().color = color3;
			}
		}
	}
}
