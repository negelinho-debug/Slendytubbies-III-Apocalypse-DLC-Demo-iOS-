using UnityEngine;

namespace AC
{
	public class PlayerStart : Marker
	{
		public ChooseSceneBy chooseSceneBy;

		public int previousScene;

		public string previousSceneName;

		public bool fadeInOnStart;

		public float fadeSpeed = 0.5f;

		public _Camera cameraOnStart;

		private GameObject playerOb;

		public void SetPlayerStart()
		{
			if (!KickStarter.mainCamera)
			{
				return;
			}
			if (fadeInOnStart)
			{
				KickStarter.mainCamera.FadeIn(fadeSpeed);
			}
			if (!AdvGame.GetReferences() || !AdvGame.GetReferences().settingsManager)
			{
				return;
			}
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if ((bool)KickStarter.player)
			{
				playerOb = KickStarter.player.gameObject;
				playerOb.GetComponent<Player>().Teleport(transform.position);
				playerOb.GetComponent<Player>().SetLookDirection(transform.forward, true);
				if (settingsManager.ActInScreenSpace() && !settingsManager.IsUnity2D())
				{
					playerOb.transform.position = AdvGame.GetScreenNavMesh(playerOb.transform.position);
				}
			}
			if (settingsManager.IsInFirstPerson())
			{
				KickStarter.mainCamera.SetFirstPerson();
			}
			else if ((bool)cameraOnStart)
			{
				KickStarter.mainCamera.SetGameCamera(cameraOnStart);
				KickStarter.mainCamera.lastNavCamera = cameraOnStart;
				cameraOnStart.MoveCameraInstant();
				KickStarter.mainCamera.SetGameCamera(cameraOnStart);
				KickStarter.mainCamera.SnapToAttached();
			}
			else if (cameraOnStart == null)
			{
				Debug.LogWarning(name + " has no Camera On Start");
			}
		}
	}
}
