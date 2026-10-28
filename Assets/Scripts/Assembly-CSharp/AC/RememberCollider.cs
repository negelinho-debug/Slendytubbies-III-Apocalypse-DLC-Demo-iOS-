using UnityEngine;

namespace AC
{
	public class RememberCollider : ConstantID
	{
		public AC_OnOff startState;

		public void Awake()
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if ((bool)settingsManager && GameIsPlaying())
			{
				bool flag = false;
				if (startState == AC_OnOff.On)
				{
					flag = true;
				}
				if ((bool)GetComponent<Collider>())
				{
					GetComponent<Collider>().enabled = flag;
				}
				else if ((bool)GetComponent<Collider2D>())
				{
					GetComponent<Collider2D>().enabled = flag;
				}
			}
		}

		public ColliderData SaveData()
		{
			ColliderData colliderData = new ColliderData();
			colliderData.objectID = constantID;
			colliderData.isOn = false;
			if ((bool)GetComponent<Collider>())
			{
				colliderData.isOn = GetComponent<Collider>().enabled;
			}
			else if ((bool)GetComponent<Collider2D>())
			{
				colliderData.isOn = GetComponent<Collider2D>().enabled;
			}
			return colliderData;
		}

		public void LoadData(ColliderData data)
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
