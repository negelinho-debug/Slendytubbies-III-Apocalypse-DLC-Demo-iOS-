using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionParent : Action
	{
		public enum ParentAction
		{
			SetParent,
			ClearParent
		}

		public int parentTransformID;

		public int parentTransformParameterID = -1;

		public int obToAffectID;

		public int obToAffectParameterID = -1;

		public ParentAction parentAction;

		public Transform parentTransform;

		public GameObject obToAffect;

		public bool isPlayer;

		public bool setPosition;

		public Vector3 newPosition;

		public bool setRotation;

		public Vector3 newRotation;

		public ActionParent()
		{
			isDisplayed = true;
			title = "Object: Set parent";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			parentTransform = AssignFile(parameters, parentTransformParameterID, parentTransformID, parentTransform);
			obToAffect = AssignFile(parameters, obToAffectParameterID, obToAffectID, obToAffect);
			if (isPlayer && (bool)KickStarter.player)
			{
				obToAffect = KickStarter.player.gameObject;
			}
		}

		public override float Run()
		{
			if (parentAction == ParentAction.SetParent && (bool)parentTransform)
			{
				obToAffect.transform.parent = parentTransform;
				if (setPosition)
				{
					obToAffect.transform.localPosition = newPosition;
				}
				if (setRotation)
				{
					obToAffect.transform.localRotation = Quaternion.LookRotation(newRotation);
				}
			}
			else if (parentAction == ParentAction.ClearParent)
			{
				obToAffect.transform.parent = null;
			}
			return 0f;
		}
	}
}
