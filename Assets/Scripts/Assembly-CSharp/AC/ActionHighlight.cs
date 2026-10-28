using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionHighlight : Action
	{
		public enum WhatToHighlight
		{
			SceneObject,
			InventoryItem
		}

		public int parameterID = -1;

		public int constantID;

		public WhatToHighlight whatToHighlight;

		public HighlightType highlightType;

		public bool isInstant;

		public Highlight highlightObject;

		public int invID;

		private int invNumber;

		private InventoryManager inventoryManager;

		public ActionHighlight()
		{
			isDisplayed = true;
			title = "Object: Highlight";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			if (whatToHighlight == WhatToHighlight.SceneObject)
			{
				highlightObject = AssignFile(parameters, parameterID, constantID, highlightObject);
			}
			else
			{
				invID = AssignInvItemID(parameters, parameterID, invID);
			}
		}

		public override float Run()
		{
			if (whatToHighlight == WhatToHighlight.SceneObject && highlightObject == null)
			{
				return 0f;
			}
			if (whatToHighlight == WhatToHighlight.SceneObject)
			{
				if (highlightType == HighlightType.Enable)
				{
					if (isInstant)
					{
						highlightObject.HighlightOnInstant();
					}
					else
					{
						highlightObject.HighlightOn();
					}
				}
				else if (highlightType == HighlightType.Disable)
				{
					if (isInstant)
					{
						highlightObject.HighlightOffInstant();
					}
					else
					{
						highlightObject.HighlightOff();
					}
				}
				else if (highlightType == HighlightType.PulseOnce)
				{
					highlightObject.Flash();
				}
				else if (highlightType == HighlightType.PulseContinually)
				{
					highlightObject.Pulse();
				}
			}
			else
			{
				RuntimeInventory component = GameObject.FindWithTag("PersistentEngine").GetComponent<RuntimeInventory>();
				if ((bool)component)
				{
					if (highlightType == HighlightType.Enable && isInstant)
					{
						component.HighlightItemOnInstant(invID);
						return 0f;
					}
					if (highlightType == HighlightType.Disable && isInstant)
					{
						component.HighlightItemOffInstant();
						return 0f;
					}
					component.HighlightItem(invID, highlightType);
				}
			}
			return 0f;
		}
	}
}
