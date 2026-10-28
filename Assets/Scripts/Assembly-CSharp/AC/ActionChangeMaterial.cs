using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionChangeMaterial : Action
	{
		public int constantID;

		public int parameterID = -1;

		public bool isPlayer;

		public GameObject obToAffect;

		public int materialIndex;

		public Material newMaterial;

		public ActionChangeMaterial()
		{
			isDisplayed = true;
			title = "Object: Change material";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			if (isPlayer)
			{
				obToAffect = KickStarter.player.gameObject;
				if ((bool)KickStarter.player && (bool)KickStarter.player.spriteChild && (bool)KickStarter.player.spriteChild.GetComponent<Renderer>())
				{
					obToAffect = KickStarter.player.spriteChild.gameObject;
				}
			}
			else
			{
				obToAffect = AssignFile(parameters, parameterID, constantID, obToAffect);
			}
		}

		public override float Run()
		{
			if ((bool)obToAffect && (bool)obToAffect.GetComponent<Renderer>() && (bool)newMaterial)
			{
				Material[] materials = obToAffect.GetComponent<Renderer>().materials;
				materials[materialIndex] = newMaterial;
				obToAffect.GetComponent<Renderer>().materials = materials;
			}
			return 0f;
		}
	}
}
