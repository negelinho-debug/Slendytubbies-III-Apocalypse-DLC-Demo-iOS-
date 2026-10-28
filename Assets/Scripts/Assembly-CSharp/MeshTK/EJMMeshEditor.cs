using UnityEngine;

namespace MeshTK
{
	public abstract class EJMMeshEditor
	{
		public EJMMesh ejmmesh;

		public Color DefaultColor = new Color(0f, 0.2f, 1f, 0.95f);

		public Color SelectionColor = new Color(1f, 0.25f, 0.25f, 0.9f);

		public abstract void DrawSceneGUI();

		public abstract void ReloadData();
	}
}
