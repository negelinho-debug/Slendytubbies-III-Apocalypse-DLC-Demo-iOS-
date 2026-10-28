using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionListAsset : ScriptableObject
	{
		public bool isSkippable = true;

		public ActionListType actionListType;

		public bool useParameters;

		public bool unfreezePauseMenus = true;

		public List<ActionParameter> parameters = new List<ActionParameter>();

		public List<Action> actions = new List<Action>();
	}
}
