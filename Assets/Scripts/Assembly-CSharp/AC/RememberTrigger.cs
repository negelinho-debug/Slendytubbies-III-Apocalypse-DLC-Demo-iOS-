using UnityEngine;

namespace AC
{
	public class RememberTrigger : ConstantID
	{
		public AC_OnOff startState;

		public void Awake()
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if ((bool)settingsManager && GameIsPlaying() && (bool)GetComponent<AC_Trigger>())
			{
				if (startState == AC_OnOff.On)
				{
					GetComponent<AC_Trigger>().TurnOn();
				}
				else
				{
					GetComponent<AC_Trigger>().TurnOff();
				}
			}
		}

		public TriggerData SaveData()
		{
			TriggerData triggerData = new TriggerData();
			triggerData.objectID = constantID;
			if ((bool)GetComponent<Collider>())
			{
				triggerData.isOn = GetComponent<Collider>().enabled;
			}
			else if ((bool)GetComponent<Collider2D>())
			{
				triggerData.isOn = GetComponent<Collider2D>().enabled;
			}
			else
			{
				triggerData.isOn = false;
			}
			return triggerData;
		}

		public void LoadData(TriggerData data)
		{
			if ((bool)GetComponent<Collider>())
			{
				GetComponent<Collider>().enabled = data.isOn;
			}
			else if ((bool)GetComponent<Collider2D>())
			{
				GetComponent<Collider2D>().enabled = data.isOn;
			}
		}
	}
}
