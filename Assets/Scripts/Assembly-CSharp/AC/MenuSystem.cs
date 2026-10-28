using UnityEngine;

namespace AC
{
	public class MenuSystem : MonoBehaviour
	{
		public static void OnMenuEnable(Menu _menu)
		{
			if (_menu.title == "Pause")
			{
				MenuElement elementWithName = _menu.GetElementWithName("SaveButton");
				if ((bool)elementWithName)
				{
					elementWithName.isVisible = !PlayerMenus.IsSavingLocked();
				}
				_menu.Recalculate();
			}
		}

		public static void OnElementClick(Menu _menu, MenuElement _element, int _slot, int _buttonPressed)
		{
			if (_element.title == "QuitButton")
			{
				MonoBehaviour.print("Quit Game");
				Application.Quit();
			}
		}
	}
}
