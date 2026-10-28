using UnityEngine;

namespace AC
{
	public class MenuLink : MonoBehaviour
	{
		public string menuName = string.Empty;

		public string elementName = string.Empty;

		public int slot;

		public bool setTextLabels;

		private Menu menu;

		private MenuElement element;

		private void Start()
		{
			if (menuName == string.Empty || elementName == string.Empty)
			{
				return;
			}
			try
			{
				menu = PlayerMenus.GetMenuWithName(menuName);
				element = PlayerMenus.GetElementWithName(menuName, elementName);
			}
			catch
			{
				Debug.LogWarning("Cannot find Menu Element with name: " + elementName + " on Menu: " + menuName);
			}
		}

		private void FixedUpdate()
		{
			if ((bool)element && setTextLabels)
			{
				if ((bool)GetComponent<GUIText>())
				{
					GetComponent<GUIText>().text = GetLabel();
				}
				if ((bool)GetComponent<TextMesh>())
				{
					GetComponent<TextMesh>().text = GetLabel();
				}
			}
		}

		public string GetLabel()
		{
			if ((bool)element)
			{
				return element.GetLabel(slot);
			}
			return string.Empty;
		}

		public bool IsVisible()
		{
			if ((bool)element && (bool)menu)
			{
				if (!menu.IsVisible())
				{
					return false;
				}
				return element.isVisible;
			}
			return false;
		}

		public void Interact()
		{
			if ((bool)element)
			{
				if (!element.isClickable)
				{
					Debug.Log("Cannot click on " + elementName);
				}
				PlayerMenus.SimulateClick(menuName, element, slot);
			}
		}

		private void OnDestroy()
		{
			element = null;
			menu = null;
		}
	}
}
