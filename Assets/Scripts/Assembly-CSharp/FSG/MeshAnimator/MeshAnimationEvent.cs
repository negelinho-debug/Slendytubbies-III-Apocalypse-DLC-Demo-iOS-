using System;
using UnityEngine;

namespace FSG.MeshAnimator
{
	[Serializable]
	public class MeshAnimationEvent
	{
		public enum Mode : byte
		{
			Data,
			String,
			Float
		}

		public string methodName;

		public int frame;

		public Mode eventType;

		public string stringValue;

		public float floatValue;

		public UnityEngine.Object data;

		public void FireEvent(GameObject eventReciever)
		{
			if ((bool)eventReciever)
			{
				if (eventType == Mode.Data)
				{
					eventReciever.SendMessage(methodName, data, SendMessageOptions.DontRequireReceiver);
				}
				else if (eventType == Mode.Float)
				{
					eventReciever.SendMessage(methodName, floatValue, SendMessageOptions.DontRequireReceiver);
				}
				else if (eventType == Mode.String)
				{
					eventReciever.SendMessage(methodName, stringValue, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}
