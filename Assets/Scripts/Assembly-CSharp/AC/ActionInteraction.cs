using System;

namespace AC
{
	[Serializable]
	public class ActionInteraction : Action
	{
		public int constantID;

		public Hotspot hotspot;

		public InteractionType interactionType;

		public ChangeType changeType;

		public int number;

		public ActionInteraction()
		{
			isDisplayed = true;
			title = "Hotspot: Change interaction";
		}

		public override void AssignValues()
		{
			hotspot = AssignFile(constantID, hotspot);
		}

		public override float Run()
		{
			if (hotspot == null)
			{
				return 0f;
			}
			if (interactionType == InteractionType.Use)
			{
				if (hotspot.useButtons.Count > number)
				{
					ChangeButton(hotspot.useButtons[number]);
				}
			}
			else if (interactionType == InteractionType.Examine)
			{
				ChangeButton(hotspot.lookButton);
			}
			else if (interactionType == InteractionType.Inventory && hotspot.invButtons.Count > number)
			{
				ChangeButton(hotspot.invButtons[number]);
			}
			return 0f;
		}

		private void ChangeButton(Button button)
		{
			if (button != null)
			{
				if (changeType == ChangeType.Enable)
				{
					button.isDisabled = false;
				}
				else if (changeType == ChangeType.Disable)
				{
					button.isDisabled = true;
				}
			}
		}
	}
}
