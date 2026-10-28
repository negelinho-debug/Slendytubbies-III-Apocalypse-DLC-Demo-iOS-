using System;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class MenuElement : ScriptableObject
	{
		public int ID;

		public bool isEditing;

		public string title = "Element";

		public Vector2 slotSize;

		public AC_SizeType sizeType;

		public AC_PositionType2 positionType;

		public float slotSpacing;

		public int lineID = -1;

		public Font font;

		public float fontScaleFactor = 60f;

		public Color fontColor = Color.white;

		public Color fontHighlightColor = Color.white;

		public Texture2D highlightTexture;

		public bool isVisible;

		public bool isClickable;

		public ElementOrientation orientation = ElementOrientation.Vertical;

		public int gridWidth = 3;

		public Texture2D backgroundTexture;

		public AudioClip hoverSound;

		public AudioClip clickSound;

		protected int offset;

		private Vector2 dragOffset;

		[SerializeField]
		protected Rect relativeRect;

		[SerializeField]
		protected Vector2 relativePosition;

		[SerializeField]
		protected int numSlots;

		public virtual void Declare()
		{
			fontScaleFactor = 2f;
			fontColor = Color.white;
			fontHighlightColor = Color.white;
			highlightTexture = null;
			orientation = ElementOrientation.Vertical;
			positionType = AC_PositionType2.Aligned;
			sizeType = AC_SizeType.Automatic;
			gridWidth = 3;
			lineID = -1;
			hoverSound = null;
			clickSound = null;
			dragOffset = Vector2.zero;
		}

		public virtual MenuElement DuplicateSelf()
		{
			return null;
		}

		public virtual void Copy(MenuElement _element)
		{
			ID = _element.ID;
			isEditing = false;
			title = _element.title;
			slotSize = _element.slotSize;
			sizeType = _element.sizeType;
			positionType = _element.positionType;
			relativeRect = _element.relativeRect;
			numSlots = _element.numSlots;
			lineID = _element.lineID;
			slotSpacing = _element.slotSpacing;
			font = _element.font;
			fontScaleFactor = _element.fontScaleFactor;
			fontColor = _element.fontColor;
			fontHighlightColor = _element.fontHighlightColor;
			highlightTexture = _element.highlightTexture;
			isVisible = _element.isVisible;
			isClickable = _element.isClickable;
			orientation = _element.orientation;
			gridWidth = _element.gridWidth;
			backgroundTexture = _element.backgroundTexture;
			hoverSound = _element.hoverSound;
			clickSound = _element.clickSound;
			relativePosition = _element.relativePosition;
			dragOffset = Vector2.zero;
		}

		public void UpdateID(int[] idArray)
		{
			foreach (int num in idArray)
			{
				if (ID == num)
				{
					ID++;
				}
			}
		}

		protected string TranslateLabel(string label)
		{
			if (Options.GetLanguage() > 0 && lineID > -1)
			{
				return SpeechManager.GetTranslation(lineID, Options.GetLanguage());
			}
			return label;
		}

		public virtual string GetLabel(int slot)
		{
			return string.Empty;
		}

		public virtual void Display(GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			if ((bool)backgroundTexture && _slot == 0)
			{
				GUI.DrawTexture(ZoomRect(relativeRect, zoom), backgroundTexture, ScaleMode.StretchToFill, true, 0f);
			}
		}

		public virtual void DrawOutline(bool isSelected, Menu _menu)
		{
			Color color = Color.yellow;
			if (isSelected)
			{
				color = Color.red;
			}
			for (int i = 0; i < GetNumSlots(); i++)
			{
				if (i > 0)
				{
					color = Color.blue;
				}
				DrawStraightLine.DrawBox(_menu.GetRectAbsolute(GetSlotRectRelative(i)), color, 1f, false, 1);
			}
		}

		protected Rect ZoomRect(Rect rect, float zoom)
		{
			if (zoom == 1f)
			{
				if (!Application.isPlaying)
				{
					dragOffset = Vector2.zero;
				}
				if (dragOffset != Vector2.zero)
				{
					rect.x += dragOffset.x;
					rect.y += dragOffset.y;
				}
				return rect;
			}
			return new Rect(rect.x * zoom, rect.y * zoom, rect.width * zoom, rect.height * zoom);
		}

		protected void LimitOffset(int maxValue)
		{
			if (offset > 0 && numSlots + offset > maxValue)
			{
				offset = maxValue - numSlots;
			}
			if (offset < 0)
			{
				offset = 0;
			}
		}

		protected void Shift(AC_ShiftInventory shiftType, int maxSlots, int arraySize, int amount)
		{
			switch (shiftType)
			{
			case AC_ShiftInventory.ShiftRight:
				offset += amount;
				if (maxSlots + offset >= arraySize)
				{
					offset = arraySize - maxSlots;
				}
				break;
			case AC_ShiftInventory.ShiftLeft:
				if (offset > 0)
				{
					offset -= amount;
					if (offset < 0)
					{
						offset = 0;
					}
				}
				break;
			}
		}

		public virtual void Shift(AC_ShiftInventory shiftType, int amount)
		{
			Debug.LogWarning("The MenuElement " + title + " cannot be 'Shifted'");
		}

		public virtual bool CanBeShifted(AC_ShiftInventory shiftType)
		{
			return true;
		}

		public Vector2 GetSize()
		{
			return new Vector2(relativeRect.width, relativeRect.height);
		}

		public Vector2 GetSizeFromCorner()
		{
			return new Vector2(relativeRect.width + relativeRect.x, relativeRect.height + relativeRect.y);
		}

		public void SetSize(Vector2 _size)
		{
			slotSize = new Vector2(_size.x, _size.y);
		}

		protected void SetAbsoluteSize(Vector2 _size)
		{
			slotSize = new Vector2(_size.x * 100f / AdvGame.GetMainGameViewSize().x, _size.y * 100f / AdvGame.GetMainGameViewSize().y);
		}

		public int GetNumSlots()
		{
			return numSlots;
		}

		public Rect GetSlotRectRelative(int _slot)
		{
			Vector2 vector = Vector2.one;
			if (sizeType != AC_SizeType.AbsolutePixels)
			{
				vector = new Vector2(AdvGame.GetMainGameViewSize().x / 100f, AdvGame.GetMainGameViewSize().y / 100f);
			}
			Rect result = relativeRect;
			result.width = slotSize.x * vector.x;
			result.height = slotSize.y * vector.y;
			if (_slot > numSlots)
			{
				_slot = numSlots;
			}
			if (orientation == ElementOrientation.Horizontal)
			{
				result.x += (slotSize.x + slotSpacing) * (float)_slot * vector.x;
			}
			else if (orientation == ElementOrientation.Vertical)
			{
				result.y += (slotSize.y + slotSpacing) * (float)_slot * vector.y;
			}
			else if (orientation == ElementOrientation.Grid)
			{
				int num = _slot + 1;
				float num2 = Mathf.CeilToInt((float)num / (float)gridWidth) - 1;
				while (num > gridWidth)
				{
					num -= gridWidth;
				}
				num--;
				result.x += (slotSize.x + slotSpacing) * (float)num * vector.x;
				result.y += (slotSize.y + slotSpacing) * num2 * vector.y;
			}
			return result;
		}

		public virtual void RecalculateSize()
		{
			dragOffset = Vector2.zero;
			Vector2 vector = Vector2.one;
			if (sizeType == AC_SizeType.Automatic)
			{
				AutoSize();
			}
			if (sizeType != AC_SizeType.AbsolutePixels)
			{
				vector = new Vector2(AdvGame.GetMainGameViewSize().x / 100f, AdvGame.GetMainGameViewSize().y / 100f);
			}
			if (orientation == ElementOrientation.Horizontal)
			{
				relativeRect.width = slotSize.x * vector.x * (float)numSlots;
				relativeRect.height = slotSize.y * vector.y;
				if (numSlots > 1)
				{
					relativeRect.width += slotSpacing * vector.x * (float)(numSlots - 1);
				}
			}
			else if (orientation == ElementOrientation.Vertical)
			{
				relativeRect.width = slotSize.x * vector.x;
				relativeRect.height = slotSize.y * vector.y * (float)numSlots;
				if (numSlots > 1)
				{
					relativeRect.height += slotSpacing * vector.y * (float)(numSlots - 1);
				}
			}
			else
			{
				if (orientation != ElementOrientation.Grid)
				{
					return;
				}
				if (numSlots < gridWidth)
				{
					relativeRect.width = (slotSize.x + slotSpacing) * vector.x * (float)numSlots;
					relativeRect.height = slotSize.y * vector.y;
					return;
				}
				float num = Mathf.CeilToInt((float)numSlots / (float)gridWidth);
				relativeRect.width = slotSize.x * vector.x * (float)gridWidth;
				relativeRect.height = slotSize.y * vector.y * num;
				if (numSlots > 1)
				{
					relativeRect.width += slotSpacing * vector.x * (float)(gridWidth - 1);
					relativeRect.height += slotSpacing * vector.y * (num - 1f);
				}
			}
		}

		public int GetFontSize()
		{
			if (sizeType == AC_SizeType.AbsolutePixels)
			{
				return (int)(fontScaleFactor * 10f);
			}
			return (int)(AdvGame.GetMainGameViewSize().x * fontScaleFactor / 100f);
		}

		protected void AutoSize(GUIContent content)
		{
			GUIStyle gUIStyle = new GUIStyle();
			gUIStyle.font = font;
			gUIStyle.fontSize = GetFontSize();
			Vector2 size = GetSize();
			size = gUIStyle.CalcSize(content);
			SetAbsoluteSize(size);
		}

		protected virtual void AutoSize()
		{
			GUIContent content = new GUIContent(backgroundTexture);
			AutoSize(content);
		}

		public void SetPosition(Vector2 _position)
		{
			relativeRect.x = _position.x;
			relativeRect.y = _position.y;
		}

		public void SetRelativePosition(Vector2 _size)
		{
			relativeRect.x = relativePosition.x * _size.x;
			relativeRect.y = relativePosition.y * _size.y;
		}

		public void ResetDragOffset()
		{
			dragOffset = Vector2.zero;
		}

		public void SetDragOffset(Vector2 pos, Rect dragRect)
		{
			if (pos.x < dragRect.x)
			{
				pos.x = dragRect.x;
			}
			else if (pos.x > dragRect.x + dragRect.width - relativeRect.width)
			{
				pos.x = dragRect.x + dragRect.width - relativeRect.width;
			}
			if (pos.y < dragRect.y)
			{
				pos.y = dragRect.y;
			}
			else if (pos.y > dragRect.y + dragRect.height - relativeRect.height)
			{
				pos.y = dragRect.y + dragRect.height - relativeRect.height;
			}
			dragOffset = pos;
		}

		public Vector2 GetDragStart()
		{
			return new Vector2(0f - dragOffset.x, dragOffset.y);
		}

		public void AutoSetVisibility()
		{
			if (numSlots == 0)
			{
				isVisible = false;
			}
			else
			{
				isVisible = true;
			}
		}
	}
}
