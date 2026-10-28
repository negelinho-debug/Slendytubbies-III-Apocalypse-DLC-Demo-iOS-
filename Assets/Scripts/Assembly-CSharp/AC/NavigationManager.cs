using UnityEngine;

namespace AC
{
	public class NavigationManager : MonoBehaviour
	{
		[HideInInspector]
		public NavigationEngine navigationEngine;

		private void Awake()
		{
			navigationEngine = null;
			ResetEngine();
		}

		public void ResetEngine()
		{
			if ((bool)GetComponent<SceneSettings>())
			{
				string text = "NavigationEngine_" + GetComponent<SceneSettings>().navigationMethod;
				if (navigationEngine == null || !navigationEngine.ToString().Contains(text))
				{
					navigationEngine = (NavigationEngine)ScriptableObject.CreateInstance(text);
					navigationEngine.Awake();
				}
			}
		}
	}
}
