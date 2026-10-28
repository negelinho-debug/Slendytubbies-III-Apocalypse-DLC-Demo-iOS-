using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionSendMessage : Action
	{
		public enum MessageToSend
		{
			TurnOn,
			TurnOff,
			Interact,
			Kill,
			Custom
		}

		public int constantID;

		public int parameterID = -1;

		public GameObject linkedObject;

		public bool affectChildren;

		public MessageToSend messageToSend;

		public string customMessage;

		public bool sendValue;

		public int customValue;

		public bool ignoreWhenSkipping;

		public ActionSendMessage()
		{
			isDisplayed = true;
			title = "Object: Send message";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			linkedObject = AssignFile(parameters, parameterID, constantID, linkedObject);
		}

		public override float Run()
		{
			if ((bool)linkedObject)
			{
				if (messageToSend == MessageToSend.Custom)
				{
					if (affectChildren)
					{
						if (!sendValue)
						{
							linkedObject.BroadcastMessage(customMessage, SendMessageOptions.DontRequireReceiver);
						}
						else
						{
							linkedObject.BroadcastMessage(customMessage, customValue, SendMessageOptions.DontRequireReceiver);
						}
					}
					else if (!sendValue)
					{
						linkedObject.SendMessage(customMessage);
					}
					else
					{
						linkedObject.SendMessage(customMessage, customValue);
					}
				}
				else if (affectChildren)
				{
					linkedObject.BroadcastMessage(messageToSend.ToString(), SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					linkedObject.SendMessage(messageToSend.ToString());
				}
			}
			return 0f;
		}

		public override void Skip()
		{
			if (!ignoreWhenSkipping)
			{
				Run();
			}
		}

		public override int End(List<Action> actions)
		{
			if ((bool)linkedObject && messageToSend == MessageToSend.Interact && (bool)linkedObject.GetComponent<Cutscene>())
			{
				Cutscene component = linkedObject.GetComponent<Cutscene>();
				if (component.triggerTime == 0f)
				{
					return -2;
				}
			}
			return base.End(actions);
		}
	}
}
