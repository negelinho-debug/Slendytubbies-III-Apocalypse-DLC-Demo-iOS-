using UnityEngine;

namespace AC
{
	public class RememberVisibility : ConstantID
	{
		public AC_OnOff startState;

		public bool affectChildren;

		public void Awake()
		{
			if (!GameIsPlaying())
			{
				return;
			}
			bool flag = false;
			if (startState == AC_OnOff.On)
			{
				flag = true;
			}
			if ((bool)GetComponent<Renderer>())
			{
				GetComponent<Renderer>().enabled = flag;
			}
			if (!affectChildren)
			{
				return;
			}
			foreach (Transform item in base.transform)
			{
				if ((bool)item.gameObject.GetComponent<Renderer>())
				{
					item.gameObject.GetComponent<Renderer>().enabled = flag;
				}
			}
		}

		public VisibilityData SaveData()
		{
			VisibilityData visibilityData = new VisibilityData();
			visibilityData.objectID = constantID;
			if ((bool)GetComponent<Renderer>())
			{
				visibilityData.isOn = GetComponent<Renderer>().enabled;
			}
			else if (affectChildren)
			{
				foreach (Transform item in base.transform)
				{
					if ((bool)item.gameObject.GetComponent<Renderer>())
					{
						visibilityData.isOn = item.gameObject.GetComponent<Renderer>().enabled;
						break;
					}
				}
			}
			return visibilityData;
		}

		public void LoadData(VisibilityData data)
		{
			if ((bool)GetComponent<Renderer>())
			{
				GetComponent<Renderer>().enabled = data.isOn;
			}
			if (!affectChildren)
			{
				return;
			}
			foreach (Transform item in base.transform)
			{
				if ((bool)item.gameObject.GetComponent<Renderer>())
				{
					item.gameObject.GetComponent<Renderer>().enabled = data.isOn;
				}
			}
		}
	}
}
