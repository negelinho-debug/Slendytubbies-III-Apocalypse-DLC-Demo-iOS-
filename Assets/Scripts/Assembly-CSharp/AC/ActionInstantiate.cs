using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionInstantiate : Action
	{
		public GameObject gameObject;

		public int parameterID = -1;

		public int constantID;

		public InvAction invAction;

		public PositionRelativeTo positionRelativeTo;

		public ActionInstantiate()
		{
			isDisplayed = true;
			title = "Object: Add or remove";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			gameObject = AssignFile(parameters, parameterID, constantID, gameObject);
		}

		public override float Run()
		{
			if (this.gameObject == null)
			{
				return 0f;
			}
			if (invAction == InvAction.Add)
			{
				if (this.gameObject.activeInHierarchy)
				{
					Debug.Log(this.gameObject.name + " won't be instantiated, as it is already present in the scene.");
					return 0f;
				}
				Vector3 position = this.gameObject.transform.position;
				Quaternion rotation = this.gameObject.transform.rotation;
				if (positionRelativeTo != PositionRelativeTo.Nothing)
				{
					float z = this.gameObject.transform.position.z;
					float x = this.gameObject.transform.position.x;
					float y = this.gameObject.transform.position.y;
					if (positionRelativeTo == PositionRelativeTo.RelativeToActiveCamera)
					{
						Transform transform = KickStarter.mainCamera.transform;
						position = transform.position + transform.forward * z + transform.right * x + transform.up * y;
						rotation.eulerAngles += transform.transform.rotation.eulerAngles;
					}
					else if (positionRelativeTo == PositionRelativeTo.RelativeToPlayer && (bool)KickStarter.player)
					{
						Transform transform2 = KickStarter.player.transform;
						position = transform2.position + transform2.forward * z + transform2.right * x + transform2.up * y;
						rotation.eulerAngles += transform2.rotation.eulerAngles;
					}
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(this.gameObject, position, rotation);
				gameObject.name = this.gameObject.name;
			}
			else
			{
				UnityEngine.Object.Destroy(this.gameObject);
			}
			return 0f;
		}
	}
}
