using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class VariablesManager : ScriptableObject
	{
		public List<GVar> vars = new List<GVar>();

		public bool updateRuntime;
	}
}
