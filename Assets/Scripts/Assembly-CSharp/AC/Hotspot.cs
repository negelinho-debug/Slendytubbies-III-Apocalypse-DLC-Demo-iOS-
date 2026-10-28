using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class Hotspot : MonoBehaviour
	{
		public bool showInEditor = true;

		public InteractionSource interactionSource;

		public string hotspotName;

		public Highlight highlight;

		public bool playUseAnim;

		public Marker walkToMarker;

		public int lineID = -1;

		public bool provideUseInteraction;

		public Button useButton = new Button();

		public List<Button> useButtons = new List<Button>();

		public bool oneClick;

		public bool provideLookInteraction;

		public Button lookButton = new Button();

		public bool provideInvInteraction;

		public List<Button> invButtons = new List<Button>();

		private float iconAlpha;

		public Collider _collider;

		public Collider2D _collider2D;

		public bool drawGizmos = true;

		public int lastInteractionIndex;

		private SettingsManager settingsManager;

		private CursorManager cursorManager;

		private void Awake()
		{
			if ((bool)AdvGame.GetReferences() && (bool)AdvGame.GetReferences().settingsManager && (bool)AdvGame.GetReferences().cursorManager)
			{
				settingsManager = AdvGame.GetReferences().settingsManager;
				cursorManager = AdvGame.GetReferences().cursorManager;
			}
			if ((bool)settingsManager && settingsManager.interactionMethod == AC_InteractionMethod.ContextSensitive)
			{
				UpgradeSelf();
			}
			if ((bool)GetComponent<Collider>())
			{
				_collider = GetComponent<Collider>();
			}
			else if ((bool)GetComponent<Collider2D>())
			{
				_collider2D = GetComponent<Collider2D>();
			}
		}

		public bool UpgradeSelf()
		{
			if (useButton.IsButtonModified())
			{
				Button button = new Button();
				button.CopyButton(useButton);
				useButtons.Add(button);
				useButton = new Button();
				provideUseInteraction = true;
				if (Application.isPlaying)
				{
					Debug.Log("Hotspot '" + gameObject.name + "' has been temporarily upgraded - please view it's Inspector when the game ends and save the scene.");
				}
				else
				{
					Debug.Log("Upgraded Hotspot '" + gameObject.name + "', please save the scene.");
				}
				return true;
			}
			return false;
		}

		public void DrawHotspotIcon()
		{
			if (IsOn() && Camera.main.WorldToScreenPoint(transform.position).z > 0f)
			{
				iconAlpha = 1f;
				if (settingsManager.hotspotIconDisplay == HotspotIconDisplay.OnlyWhenHighlighting)
				{
					if ((bool)highlight)
					{
						iconAlpha = highlight.GetHighlightAlpha();
					}
					else
					{
						Debug.LogWarning("Cannot display correct Hotspot Icon alpha on " + name + " because it has no associated Highlight object.");
					}
				}
			}
			else
			{
				iconAlpha = 0f;
			}
			if (iconAlpha > 0f)
			{
				Color color = GUI.color;
				Color color2 = color;
				color.a = iconAlpha;
				GUI.color = color;
				if (settingsManager.hotspotIcon == HotspotIcon.UseIcon)
				{
					GetMainIcon()?.Draw(GetIconScreenPosition());
				}
				else if (settingsManager.hotspotIconTexture != null)
				{
					GUI.DrawTexture(AdvGame.GUIBox(GetIconScreenPosition(), settingsManager.hotspotIconSize), settingsManager.hotspotIconTexture, ScaleMode.ScaleToFit, true, 0f);
				}
				GUI.color = color2;
			}
		}

		public Button GetFirstUseButton()
		{
			foreach (Button useButton in useButtons)
			{
				if (useButton != null && !useButton.isDisabled)
				{
					return useButton;
				}
			}
			return null;
		}

		public void TurnOn()
		{
			gameObject.layer = LayerMask.NameToLayer(settingsManager.hotspotLayer);
		}

		public void TurnOff()
		{
			gameObject.layer = LayerMask.NameToLayer(settingsManager.deactivatedLayer);
		}

		public bool IsOn()
		{
			if (gameObject.layer == LayerMask.NameToLayer(settingsManager.deactivatedLayer))
			{
				return false;
			}
			return true;
		}

		public void Select()
		{
			if ((bool)highlight)
			{
				highlight.HighlightOn();
			}
		}

		public void Deselect()
		{
			if ((bool)highlight)
			{
				highlight.HighlightOff();
			}
		}

		public bool IsSingleInteraction()
		{
			if (oneClick && provideUseInteraction && useButtons != null && GetFirstUseButton() != null)
			{
				return true;
			}
			return false;
		}

		public void DeselectInstant()
		{
			if ((bool)highlight)
			{
				highlight.HighlightOffInstant();
			}
		}

		private void OnDrawGizmos()
		{
			if (showInEditor)
			{
				DrawGizmos();
			}
		}

		private void OnDrawGizmosSelected()
		{
			DrawGizmos();
		}

		private void DrawGizmos()
		{
			if (GetComponent<Char>() == null && drawGizmos)
			{
				if ((bool)GetComponent<PolygonCollider2D>())
				{
					AdvGame.DrawPolygonCollider(transform, GetComponent<PolygonCollider2D>(), new Color(1f, 1f, 0f, 0.6f));
				}
				else
				{
					AdvGame.DrawCubeCollider(transform, new Color(1f, 1f, 0f, 0.6f));
				}
			}
		}

		public Vector2 GetIconScreenPosition()
		{
			Vector3 vector = Camera.main.WorldToScreenPoint(GetIconPosition());
			return new Vector3(vector.x, vector.y);
		}

		public Vector3 GetIconPosition()
		{
			Vector3 position = transform.position;
			if (_collider != null)
			{
				if (_collider is BoxCollider)
				{
					BoxCollider boxCollider = (BoxCollider)_collider;
					position += boxCollider.center;
				}
				else if (_collider is CapsuleCollider)
				{
					CapsuleCollider capsuleCollider = (CapsuleCollider)_collider;
					position += capsuleCollider.center;
				}
			}
			else if (_collider2D != null && _collider2D is BoxCollider2D)
			{
				BoxCollider2D boxCollider2D = (BoxCollider2D)_collider2D;
				position += new Vector3(boxCollider2D.offset.x, boxCollider2D.offset.y * transform.localScale.y, 0f);
			}
			return position;
		}

		private CursorIconBase GetMainIcon()
		{
			if (cursorManager == null)
			{
				return null;
			}
			if (provideUseInteraction && useButton != null && useButton.iconID >= 0 && !useButton.isDisabled)
			{
				return cursorManager.GetCursorIconFromID(useButton.iconID);
			}
			if (provideLookInteraction && lookButton != null && lookButton.iconID >= 0 && !lookButton.isDisabled)
			{
				return cursorManager.GetCursorIconFromID(lookButton.iconID);
			}
			if (provideUseInteraction && useButtons != null && useButtons.Count > 0 && !useButtons[0].isDisabled)
			{
				return cursorManager.GetCursorIconFromID(useButtons[0].iconID);
			}
			return null;
		}

		public bool HasContextUse()
		{
			if ((oneClick || settingsManager.interactionMethod == AC_InteractionMethod.ContextSensitive) && provideUseInteraction && useButtons != null && GetFirstUseButton() != null)
			{
				return true;
			}
			return false;
		}

		public bool HasContextLook()
		{
			if (provideLookInteraction && lookButton != null && !lookButton.isDisabled)
			{
				return true;
			}
			return false;
		}

		public int GetNextInteraction(int i, int numInvInteractions)
		{
			if (i < useButtons.Count)
			{
				i++;
				while (i < useButtons.Count && useButtons[i].isDisabled)
				{
					i++;
				}
				if (i >= useButtons.Count + numInvInteractions)
				{
					return FindFirstEnabledInteraction();
				}
				return i;
			}
			if (i == useButtons.Count - 1 + numInvInteractions)
			{
				return FindFirstEnabledInteraction();
			}
			return i + 1;
		}

		public int FindFirstEnabledInteraction()
		{
			if (useButtons != null && useButtons.Count > 0)
			{
				for (int i = 0; i < useButtons.Count; i++)
				{
					if (!useButtons[i].isDisabled)
					{
						return i;
					}
				}
			}
			return 0;
		}

		private int FindLastEnabledInteraction()
		{
			if (useButtons != null && useButtons.Count > 0)
			{
				for (int num = useButtons.Count - 1; num >= 0; num--)
				{
					if (!useButtons[num].isDisabled)
					{
						return num;
					}
				}
			}
			return 0;
		}

		public int GetPreviousInteraction(int i, int numInvInteractions)
		{
			if (i > useButtons.Count && numInvInteractions > 0)
			{
				return i - 1;
			}
			if (i == 0)
			{
				return FindLastEnabledInteraction();
			}
			if (i <= useButtons.Count)
			{
				i--;
				while (i > 0 && useButtons[i].isDisabled)
				{
					i--;
				}
				if (i < 0)
				{
					return FindLastEnabledInteraction();
				}
				if (i == 0 && useButtons.Count > 0 && useButtons[0].isDisabled)
				{
					return FindLastEnabledInteraction();
				}
				return i;
			}
			return i - 1;
		}

		private int GetNumInteractions(int numInvInteractions)
		{
			int num = 0;
			foreach (Button useButton in useButtons)
			{
				if (!useButton.isDisabled)
				{
					num++;
				}
			}
			return num + numInvInteractions;
		}

		private void OnDestroy()
		{
			settingsManager = null;
			cursorManager = null;
		}
	}
}
