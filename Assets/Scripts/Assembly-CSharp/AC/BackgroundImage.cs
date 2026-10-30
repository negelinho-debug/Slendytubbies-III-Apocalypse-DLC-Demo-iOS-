using UnityEngine;

namespace AC
{
	public class BackgroundImage : MonoBehaviour
	{
		private float shakeDuration;

		private float startTime;

		private float startShakeIntensity;

		private float shakeIntensity;

		private Rect originalPixelInset;

		public void TurnOn()
		{
			if (LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.backgroundImageLayer) == -1)
			{
				Debug.LogWarning("No '" + AdvGame.GetReferences().settingsManager.backgroundImageLayer + "' layer exists - please define one in the Tags Manager.");
			}
			else
			{
				gameObject.layer = LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.backgroundImageLayer);
			}
			if ((bool)GetComponent<UnityEngine.UI.RawImage>())
			{
				GetComponent<UnityEngine.UI.RawImage>().enabled = true;
			}
			else
			{
				Debug.LogWarning(name + " has no GUITexture component");
			}
		}

		public void TurnOff()
		{
			gameObject.layer = LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.deactivatedLayer);
			if ((bool)GetComponent<UnityEngine.UI.RawImage>())
			{
				GetComponent<UnityEngine.UI.RawImage>().enabled = false;
			}
			else
			{
				Debug.LogWarning(name + " has no GUITexture component");
			}
		}

		public void Shake(float _shakeIntensity, float _duration)
		{
			if (shakeIntensity > 0f)
			{
				LegacyGuiRect.Set(gameObject, originalPixelInset);
			}
			originalPixelInset = LegacyGuiRect.Get(gameObject);
			shakeDuration = _duration;
			startTime = Time.time;
			shakeIntensity = _shakeIntensity;
			startShakeIntensity = shakeIntensity;
		}

		private void Update()
		{
			if ((bool)GetComponent<UnityEngine.UI.RawImage>())
			{
				if (shakeIntensity > 0f)
				{
					float num = Random.Range(0f, shakeIntensity) * 0.2f;
					LegacyGuiRect.Set(gameObject, new Rect(originalPixelInset.x - Random.Range(0f, shakeIntensity) * 0.1f, originalPixelInset.y - Random.Range(0f, shakeIntensity) * 0.1f, originalPixelInset.width + num, originalPixelInset.height + num));
					shakeIntensity = Mathf.Lerp(startShakeIntensity, 0f, AdvGame.Interpolate(startTime, shakeDuration, MoveMethod.Linear, null));
				}
				else if (shakeIntensity < 0f)
				{
					shakeIntensity = 0f;
					LegacyGuiRect.Set(gameObject, originalPixelInset);
				}
			}
		}
	}
}
