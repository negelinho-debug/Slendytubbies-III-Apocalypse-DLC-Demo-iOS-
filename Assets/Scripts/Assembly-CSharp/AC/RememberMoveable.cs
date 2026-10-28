using UnityEngine;

namespace AC
{
	public class RememberMoveable : ConstantID
	{
		public AC_OnOff startState;

		public void Awake()
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if (!settingsManager || !GameIsPlaying())
			{
				return;
			}
			if ((bool)GetComponent<DragBase>())
			{
				if (startState == AC_OnOff.On)
				{
					GetComponent<DragBase>().TurnOn();
				}
				else
				{
					GetComponent<DragBase>().TurnOff();
				}
			}
			if (startState == AC_OnOff.On)
			{
				gameObject.layer = LayerMask.NameToLayer(settingsManager.hotspotLayer);
			}
			else
			{
				gameObject.layer = LayerMask.NameToLayer(settingsManager.deactivatedLayer);
			}
		}

		public MoveableData SaveData()
		{
			MoveableData moveableData = new MoveableData();
			moveableData.objectID = constantID;
			if (gameObject.layer == LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer))
			{
				moveableData.isOn = true;
			}
			else
			{
				moveableData.isOn = false;
			}
			if ((bool)GetComponent<Moveable_Drag>())
			{
				Moveable_Drag component = GetComponent<Moveable_Drag>();
				moveableData.trackValue = component.trackValue;
				moveableData.revolutions = component.revolutions;
			}
			moveableData.LocX = transform.position.x;
			moveableData.LocY = transform.position.y;
			moveableData.LocZ = transform.position.z;
			moveableData.RotX = transform.eulerAngles.x;
			moveableData.RotY = transform.eulerAngles.y;
			moveableData.RotZ = transform.eulerAngles.z;
			moveableData.ScaleX = transform.localScale.x;
			moveableData.ScaleY = transform.localScale.y;
			moveableData.ScaleZ = transform.localScale.z;
			return moveableData;
		}

		public void LoadData(MoveableData data)
		{
			if ((bool)GetComponent<DragBase>())
			{
				if (data.isOn)
				{
					GetComponent<DragBase>().TurnOn();
				}
				else
				{
					GetComponent<DragBase>().TurnOff();
				}
			}
			if (data.isOn)
			{
				gameObject.layer = LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer);
			}
			else
			{
				gameObject.layer = LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.deactivatedLayer);
			}
			transform.position = new Vector3(data.LocX, data.LocY, data.LocZ);
			transform.eulerAngles = new Vector3(data.RotX, data.RotY, data.RotZ);
			transform.localScale = new Vector3(data.ScaleX, data.ScaleY, data.ScaleZ);
			if ((bool)GetComponent<Moveable_Drag>())
			{
				Moveable_Drag component = GetComponent<Moveable_Drag>();
				component.isHeld = false;
				if (component.dragMode == DragMode.LockToTrack && component.track != null)
				{
					component.trackValue = data.trackValue;
					component.revolutions = data.revolutions;
					component.StopAutoMove();
					component.track.SetPositionAlong(data.trackValue, component);
				}
			}
		}
	}
}
