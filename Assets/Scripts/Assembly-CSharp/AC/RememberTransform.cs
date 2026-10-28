using UnityEngine;

namespace AC
{
	public class RememberTransform : ConstantID
	{
		public bool saveParent;

		public bool saveScenePresence;

		public TransformData SaveData()
		{
			TransformData transformData = new TransformData();
			transformData.objectID = constantID;
			transformData.LocX = transform.position.x;
			transformData.LocY = transform.position.y;
			transformData.LocZ = transform.position.z;
			transformData.RotX = transform.eulerAngles.x;
			transformData.RotY = transform.eulerAngles.y;
			transformData.RotZ = transform.eulerAngles.z;
			transformData.ScaleX = transform.localScale.x;
			transformData.ScaleY = transform.localScale.y;
			transformData.ScaleZ = transform.localScale.z;
			transformData.bringBack = saveScenePresence;
			if (saveParent)
			{
				Transform parent = transform.parent;
				if (parent == null)
				{
					transformData.parentID = 0;
					return transformData;
				}
				while (parent.parent != null)
				{
					parent = parent.parent;
					if ((bool)parent.GetComponent<Char>())
					{
						Char component = parent.GetComponent<Char>();
						if ((component is Player || ((bool)component.GetComponent<ConstantID>() && component.GetComponent<ConstantID>().constantID != 0)) && (transform.parent == component.leftHandBone || transform.parent == component.rightHandBone))
						{
							if (component is Player)
							{
								transformData.parentIsPlayer = true;
								transformData.parentIsNPC = false;
								transformData.parentID = 0;
							}
							else
							{
								transformData.parentIsPlayer = false;
								transformData.parentIsNPC = true;
								transformData.parentID = component.GetComponent<ConstantID>().constantID;
							}
							if (transform.parent == component.leftHandBone)
							{
								transformData.heldHand = ActionCharHold.Hand.Left;
							}
							else
							{
								transformData.heldHand = ActionCharHold.Hand.Right;
							}
							return transformData;
						}
						break;
					}
				}
				if ((bool)transform.parent.GetComponent<ConstantID>() && transform.parent.GetComponent<ConstantID>().constantID != 0)
				{
					transformData.parentID = transform.parent.GetComponent<ConstantID>().constantID;
				}
				else
				{
					transformData.parentID = 0;
					Debug.LogWarning("Could not save " + name + "'s parent since it has no Constant ID");
				}
			}
			return transformData;
		}

		public void LoadData(TransformData data)
		{
			if (data.parentIsPlayer)
			{
				if ((bool)KickStarter.player)
				{
					if (data.heldHand == ActionCharHold.Hand.Left)
					{
						transform.parent = KickStarter.player.leftHandBone;
					}
					else
					{
						transform.parent = KickStarter.player.rightHandBone;
					}
				}
			}
			else if (data.parentID != 0)
			{
				ConstantID constantID = Serializer.returnComponent<ConstantID>(data.parentID);
				if (constantID != null)
				{
					if (data.parentIsNPC && (bool)constantID.GetComponent<NPC>())
					{
						if (data.heldHand == ActionCharHold.Hand.Left)
						{
							transform.parent = constantID.GetComponent<NPC>().leftHandBone;
						}
						else
						{
							transform.parent = constantID.GetComponent<NPC>().rightHandBone;
						}
					}
					else
					{
						transform.parent = constantID.gameObject.transform;
					}
				}
			}
			else if (data.parentID == 0 && saveParent)
			{
				transform.parent = null;
			}
			transform.position = new Vector3(data.LocX, data.LocY, data.LocZ);
			transform.eulerAngles = new Vector3(data.RotX, data.RotY, data.RotZ);
			transform.localScale = new Vector3(data.ScaleX, data.ScaleY, data.ScaleZ);
		}
	}
}
