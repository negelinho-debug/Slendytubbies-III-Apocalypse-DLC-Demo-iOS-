using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class MenuManager : ScriptableObject
	{
		public List<Menu> menus = new List<Menu>();

		public bool drawOutlines = true;

		public bool drawInEditor;

		public int globalDepth;

		public Texture2D pauseTexture;
	}
}
