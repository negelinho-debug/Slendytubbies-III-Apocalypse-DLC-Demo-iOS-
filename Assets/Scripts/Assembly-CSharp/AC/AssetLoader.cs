using UnityEngine;

namespace AC
{
	public static class AssetLoader
	{
		private static Object[] textureAssets;

		private static Object[] audioAssets;

		private static Object[] animationAssets;

		public static int GetAssetInstanceID<T>(T originalFile) where T : Object
		{
			if (originalFile != null)
			{
				return originalFile.GetInstanceID();
			}
			return 0;
		}

		public static T RetrieveAsset<T>(T originalFile, int _ID) where T : Object
		{
			if (_ID == 0)
			{
				return originalFile;
			}
			Object[] array = null;
			if (originalFile is Texture2D)
			{
				if (textureAssets == null)
				{
					textureAssets = Resources.LoadAll(string.Empty, typeof(T));
				}
				array = textureAssets;
			}
			else if (originalFile is AudioClip)
			{
				if (audioAssets == null)
				{
					audioAssets = Resources.LoadAll(string.Empty, typeof(T));
				}
				array = audioAssets;
			}
			if (originalFile is AnimationClip)
			{
				if (animationAssets == null)
				{
					animationAssets = Resources.LoadAll(string.Empty, typeof(T));
				}
				array = animationAssets;
			}
			Object[] array2 = array;
			foreach (Object obj in array2)
			{
				if (obj.GetInstanceID() == _ID)
				{
					return (T)obj;
				}
			}
			return originalFile;
		}

		public static void UnloadAssets()
		{
			textureAssets = null;
			audioAssets = null;
			animationAssets = null;
			Resources.UnloadUnusedAssets();
		}
	}
}
