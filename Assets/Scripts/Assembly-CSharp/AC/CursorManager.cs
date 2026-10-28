using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class CursorManager : ScriptableObject
	{
		public CursorRendering cursorRendering;

		public CursorDisplay cursorDisplay;

		public bool allowMainCursor;

		public bool addHotspotPrefix;

		public bool allowInteractionCursor;

		public bool allowInteractionCursorForInventory;

		public bool cycleCursors;

		public bool leftClickExamine;

		public bool onlyWalkWhenOverNavMesh;

		public float inventoryCursorSize = 0.06f;

		public CursorIconBase waitIcon = new CursorIcon();

		public CursorIconBase pointerIcon = new CursorIcon();

		public CursorIconBase walkIcon = new CursorIcon();

		public CursorIconBase mouseOverIcon = new CursorIcon();

		public InventoryHandling inventoryHandling;

		public HotspotPrefix hotspotPrefix1 = new HotspotPrefix("Use");

		public HotspotPrefix hotspotPrefix2 = new HotspotPrefix("on");

		public List<CursorIcon> cursorIcons = new List<CursorIcon>();

		public List<ActionListAsset> unhandledCursorInteractions = new List<ActionListAsset>();

		public LookUseCursorAction lookUseCursorAction;

		public int lookCursor_ID;

		public int lookCursor_int;

		private SettingsManager settingsManager;

		public string[] GetLabelsArray(int requestedInt)
		{
			List<string> list = new List<string>();
			foreach (CursorIcon cursorIcon in cursorIcons)
			{
				list.Add(cursorIcon.label);
			}
			return list.ToArray();
		}

		public string GetLabelFromID(int _ID)
		{
			foreach (CursorIcon cursorIcon in cursorIcons)
			{
				if (cursorIcon.id == _ID)
				{
					if (Options.GetLanguage() > 0)
					{
						return SpeechManager.GetTranslation(cursorIcon.lineID, Options.GetLanguage()) + " ";
					}
					return cursorIcon.label + " ";
				}
			}
			return string.Empty;
		}

		public CursorIcon GetCursorIconFromID(int _ID)
		{
			foreach (CursorIcon cursorIcon in cursorIcons)
			{
				if (cursorIcon.id == _ID)
				{
					return cursorIcon;
				}
			}
			return null;
		}

		public int GetIntFromID(int _ID)
		{
			int num = 0;
			int num2 = -1;
			foreach (CursorIcon cursorIcon in cursorIcons)
			{
				if (cursorIcon.id == _ID)
				{
					num2 = num;
				}
				num++;
			}
			if (num2 == -1)
			{
				num2 = 0;
			}
			return num2;
		}

		public ActionListAsset GetUnhandledInteraction(int _ID)
		{
			foreach (CursorIcon cursorIcon in cursorIcons)
			{
				if (cursorIcon.id == _ID)
				{
					int num = cursorIcons.IndexOf(cursorIcon);
					if (unhandledCursorInteractions.Count > num)
					{
						return unhandledCursorInteractions[num];
					}
					return null;
				}
			}
			return null;
		}

		private int[] GetIDArray()
		{
			List<int> list = new List<int>();
			foreach (CursorIcon cursorIcon in cursorIcons)
			{
				list.Add(cursorIcon.id);
			}
			list.Sort();
			return list.ToArray();
		}
	}
}
