using System.Text;
using UnityEngine;

namespace AC
{
	public class RememberHotspot : ConstantID
	{
		public AC_OnOff startState;

		public void Awake()
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if ((bool)settingsManager && GameIsPlaying())
			{
				if (startState == AC_OnOff.On)
				{
					gameObject.layer = LayerMask.NameToLayer(settingsManager.hotspotLayer);
				}
				else
				{
					gameObject.layer = LayerMask.NameToLayer(settingsManager.deactivatedLayer);
				}
			}
		}

		public HotspotData SaveData()
		{
			HotspotData hotspotData = new HotspotData();
			hotspotData.objectID = constantID;
			if (gameObject.layer == LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer))
			{
				hotspotData.isOn = true;
			}
			else
			{
				hotspotData.isOn = false;
			}
			if ((bool)GetComponent<Hotspot>())
			{
				hotspotData.buttonStates = ButtonStatesToString(GetComponent<Hotspot>());
			}
			return hotspotData;
		}

		public void LoadData(HotspotData data)
		{
			if (data.isOn)
			{
				gameObject.layer = LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer);
			}
			else
			{
				gameObject.layer = LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.deactivatedLayer);
			}
			if ((bool)GetComponent<Hotspot>())
			{
				StringToButtonStates(GetComponent<Hotspot>(), data.buttonStates);
			}
		}

		private void StringToButtonStates(Hotspot hotspot, string stateString)
		{
			if (stateString.Length == 0)
			{
				return;
			}
			string[] array = stateString.Split("|"[0]);
			if ((AdvGame.GetReferences().settingsManager == null || AdvGame.GetReferences().settingsManager.interactionMethod == AC_InteractionMethod.ContextSensitive) && hotspot.provideLookInteraction && hotspot.lookButton != null)
			{
				hotspot.lookButton.isDisabled = SetButtonDisabledValue(array[0]);
			}
			if (hotspot.provideUseInteraction)
			{
				string[] array2 = array[1].Split(","[0]);
				for (int i = 0; i < array2.Length && hotspot.useButtons.Count >= i + 1; i++)
				{
					hotspot.useButtons[i].isDisabled = SetButtonDisabledValue(array2[i]);
				}
			}
			if (hotspot.provideInvInteraction && array.Length > 2)
			{
				string[] array3 = array[2].Split(","[0]);
				for (int j = 0; j < array3.Length && hotspot.invButtons.Count >= j + 1; j++)
				{
					hotspot.invButtons[j].isDisabled = SetButtonDisabledValue(array3[j]);
				}
			}
		}

		private string ButtonStatesToString(Hotspot hotspot)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (AdvGame.GetReferences().settingsManager == null || AdvGame.GetReferences().settingsManager.interactionMethod == AC_InteractionMethod.ContextSensitive)
			{
				if (hotspot.provideLookInteraction)
				{
					stringBuilder.Append(GetButtonDisabledValue(hotspot.lookButton));
				}
				else
				{
					stringBuilder.Append("0");
				}
			}
			stringBuilder.Append("|");
			if (hotspot.provideUseInteraction)
			{
				foreach (Button useButton in hotspot.useButtons)
				{
					stringBuilder.Append(GetButtonDisabledValue(useButton));
					if (hotspot.useButtons.IndexOf(useButton) < hotspot.useButtons.Count - 1)
					{
						stringBuilder.Append(",");
					}
				}
			}
			stringBuilder.Append("|");
			if (hotspot.provideInvInteraction)
			{
				foreach (Button invButton in hotspot.invButtons)
				{
					stringBuilder.Append(GetButtonDisabledValue(invButton));
					if (hotspot.invButtons.IndexOf(invButton) < hotspot.invButtons.Count - 1)
					{
						stringBuilder.Append(",");
					}
				}
			}
			return stringBuilder.ToString();
		}

		private string GetButtonDisabledValue(Button button)
		{
			if (button != null && !button.isDisabled)
			{
				return "1";
			}
			return "0";
		}

		private bool SetButtonDisabledValue(string text)
		{
			if (text == "1")
			{
				return false;
			}
			return true;
		}
	}
}
