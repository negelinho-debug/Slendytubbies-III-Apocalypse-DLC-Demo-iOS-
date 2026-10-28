using System.Collections;
using UnityEngine;

namespace AC
{
	public class LoadingScreen : MonoBehaviour
	{
		public IEnumerator InnerLoad(string sceneName, int sceneNumber, string loadingSceneName, int loadingScene)
		{
			Object.DontDestroyOnLoad(gameObject);
			if (loadingSceneName != string.Empty)
			{
				Application.LoadLevel(loadingSceneName);
			}
			else
			{
				Application.LoadLevel(loadingScene);
			}
			yield return null;
			if (sceneName != string.Empty)
			{
				Application.LoadLevel(sceneName);
			}
			else
			{
				Application.LoadLevel(sceneNumber);
			}
			Object.Destroy(gameObject);
		}
	}
}
