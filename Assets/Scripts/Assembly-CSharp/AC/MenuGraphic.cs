using UnityEngine;

namespace AC
{
	public class MenuGraphic : MenuElement
	{
		public CursorIconBase graphic;

		public override void Declare()
		{
			isVisible = true;
			isClickable = false;
			graphic = new CursorIconBase();
			numSlots = 1;
			SetSize(new Vector2(10f, 5f));
			base.Declare();
		}

		public override MenuElement DuplicateSelf()
		{
			MenuGraphic menuGraphic = ScriptableObject.CreateInstance<MenuGraphic>();
			menuGraphic.Declare();
			menuGraphic.CopyGraphic(this);
			return menuGraphic;
		}

		public void CopyGraphic(MenuGraphic _element)
		{
			graphic = _element.graphic;
			base.Copy(_element);
		}

		public override void Display(GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			base.Display(_style, _slot, zoom, isActive);
			graphic.DrawAsInteraction(ZoomRect(relativeRect, zoom), true);
		}

		public override void RecalculateSize()
		{
			graphic.Reset();
			base.RecalculateSize();
		}

		protected override void AutoSize()
		{
			if (graphic.texture != null)
			{
				GUIContent content = new GUIContent(graphic.texture);
				AutoSize(content);
			}
		}
	}
}
