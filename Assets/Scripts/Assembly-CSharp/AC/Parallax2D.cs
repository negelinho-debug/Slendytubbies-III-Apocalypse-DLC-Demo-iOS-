using UnityEngine;

namespace AC
{
	public class Parallax2D : MonoBehaviour
	{
		public GameCamera2D camera2D;

		public float depth;

		public bool xScroll;

		public bool yScroll;

		public float xOffset;

		public float yOffset;

		private float xStart;

		private float yStart;

		private float xDesired;

		private float yDesired;

		private Vector2 perspectiveOffset;

		private bool isUnity2D;

		private void Awake()
		{
			xStart = transform.localPosition.x;
			yStart = transform.localPosition.y;
			xDesired = xStart;
			yDesired = yStart;
			if ((bool)AdvGame.GetReferences().settingsManager)
			{
				isUnity2D = AdvGame.GetReferences().settingsManager.IsUnity2D();
			}
		}

		public void UpdateOffset()
		{
			if (isUnity2D)
			{
				perspectiveOffset = new Vector2(KickStarter.mainCamera.transform.position.x, KickStarter.mainCamera.transform.position.y);
			}
			else
			{
				perspectiveOffset = KickStarter.mainCamera.perspectiveOffset;
			}
			xDesired = xStart;
			if (xScroll)
			{
				xDesired += perspectiveOffset.x * depth;
				xDesired += xOffset;
			}
			yDesired = yStart;
			if (yScroll)
			{
				yDesired += perspectiveOffset.y * depth;
				yDesired += yOffset;
			}
			transform.localPosition = new Vector3(xDesired, yDesired, transform.localPosition.z);
		}
	}
}
