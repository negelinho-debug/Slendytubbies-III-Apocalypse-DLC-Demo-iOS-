using UnityEngine;

namespace AC
{
	public class RememberNPC : ConstantID
	{
		public AC_OnOff startState;

		public void Awake()
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			if ((bool)settingsManager && GetComponent<RememberHotspot>() == null && GameIsPlaying())
			{
				if (startState == AC_OnOff.On)
				{
					gameObject.layer = LayerMask.NameToLayer(settingsManager.hotspotLayer);
				}
				else
				{
					gameObject.layer = LayerMask.NameToLayer(settingsManager.deactivatedLayer);
				}
			}
		}

		public NPCData SaveData()
		{
			NPCData nPCData = new NPCData();
			nPCData.objectID = constantID;
			if (gameObject.layer == LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer))
			{
				nPCData.isOn = true;
			}
			else
			{
				nPCData.isOn = false;
			}
			nPCData.LocX = transform.position.x;
			nPCData.LocY = transform.position.y;
			nPCData.LocZ = transform.position.z;
			nPCData.RotX = transform.eulerAngles.x;
			nPCData.RotY = transform.eulerAngles.y;
			nPCData.RotZ = transform.eulerAngles.z;
			nPCData.ScaleX = transform.localScale.x;
			nPCData.ScaleY = transform.localScale.y;
			nPCData.ScaleZ = transform.localScale.z;
			if ((bool)GetComponent<NPC>())
			{
				NPC component = GetComponent<NPC>();
				if (component.animationEngine == AnimationEngine.Sprites2DToolkit || component.animationEngine == AnimationEngine.SpritesUnity)
				{
					nPCData.idleAnim = component.idleAnimSprite;
					nPCData.walkAnim = component.walkAnimSprite;
					nPCData.talkAnim = component.talkAnimSprite;
					nPCData.runAnim = component.runAnimSprite;
				}
				else if (component.animationEngine == AnimationEngine.Legacy)
				{
					nPCData.idleAnim = AssetLoader.GetAssetInstanceID(component.idleAnim).ToString();
					nPCData.walkAnim = AssetLoader.GetAssetInstanceID(component.walkAnim).ToString();
					nPCData.runAnim = AssetLoader.GetAssetInstanceID(component.runAnim).ToString();
					nPCData.talkAnim = AssetLoader.GetAssetInstanceID(component.talkAnim).ToString();
				}
				nPCData.walkSound = AssetLoader.GetAssetInstanceID(component.walkSound);
				nPCData.runSound = AssetLoader.GetAssetInstanceID(component.runSound);
				nPCData.speechLabel = component.speechLabel;
				nPCData.portraitGraphic = AssetLoader.GetAssetInstanceID(component.portraitIcon.texture);
				nPCData.walkSpeed = component.walkSpeedScale;
				nPCData.runSpeed = component.runSpeedScale;
				nPCData.lockDirection = component.lockDirection;
				nPCData.lockScale = component.lockScale;
				if ((bool)component.spriteChild && (bool)component.spriteChild.GetComponent<FollowSortingMap>())
				{
					nPCData.lockSorting = component.spriteChild.GetComponent<FollowSortingMap>().lockSorting;
				}
				else if ((bool)component.GetComponent<FollowSortingMap>())
				{
					nPCData.lockSorting = component.GetComponent<FollowSortingMap>().lockSorting;
				}
				else
				{
					nPCData.lockSorting = false;
				}
				nPCData.spriteDirection = component.spriteDirection;
				nPCData.spriteScale = component.spriteScale;
				if ((bool)component.spriteChild && (bool)component.spriteChild.GetComponent<Renderer>())
				{
					nPCData.sortingOrder = component.spriteChild.GetComponent<Renderer>().sortingOrder;
					nPCData.sortingLayer = component.spriteChild.GetComponent<Renderer>().sortingLayerName;
				}
				else if ((bool)component.GetComponent<Renderer>())
				{
					nPCData.sortingOrder = component.GetComponent<Renderer>().sortingOrder;
					nPCData.sortingLayer = component.GetComponent<Renderer>().sortingLayerName;
				}
				nPCData.pathID = 0;
				nPCData.lastPathID = 0;
				if ((bool)component.GetPath(true))
				{
					nPCData.targetNode = component.GetTargetNode(true);
					nPCData.prevNode = component.GetPrevNode(true);
					nPCData.isRunning = component.isRunning;
					if (component.GetPath(true) == GetComponent<Paths>())
					{
						nPCData.pathData = Serializer.CreatePathData(GetComponent<Paths>());
					}
					else if ((bool)component.GetPath(true).GetComponent<ConstantID>())
					{
						nPCData.pathID = component.GetPath(true).GetComponent<ConstantID>().constantID;
					}
					else
					{
						Debug.LogWarning("Want to save path data for " + name + " but path has no ID!");
					}
				}
				if ((bool)component.GetPath(false))
				{
					nPCData.lastTargetNode = component.GetTargetNode(false);
					nPCData.lastPrevNode = component.GetPrevNode(false);
					if ((bool)component.GetPath(false).GetComponent<ConstantID>())
					{
						nPCData.lastPathID = component.GetPath(false).GetComponent<ConstantID>().constantID;
					}
					else
					{
						Debug.LogWarning("Want to save previous path data for " + name + " but path has no ID!");
					}
				}
				if ((bool)component.followTarget)
				{
					if (!component.followTargetIsPlayer)
					{
						if ((bool)component.followTarget.GetComponent<ConstantID>())
						{
							nPCData.followTargetID = component.followTarget.GetComponent<ConstantID>().constantID;
							nPCData.followTargetIsPlayer = component.followTargetIsPlayer;
							nPCData.followFrequency = component.followFrequency;
							nPCData.followDistance = component.followDistance;
							nPCData.followDistanceMax = component.followDistanceMax;
						}
						else
						{
							Debug.LogWarning("Want to save follow data for " + name + " but " + component.followTarget.name + " has no ID!");
						}
					}
					else
					{
						nPCData.followTargetID = 0;
						nPCData.followTargetIsPlayer = component.followTargetIsPlayer;
						nPCData.followFrequency = component.followFrequency;
						nPCData.followDistance = component.followDistance;
						nPCData.followDistanceMax = component.followDistanceMax;
					}
				}
				else
				{
					nPCData.followTargetID = 0;
					nPCData.followTargetIsPlayer = false;
					nPCData.followFrequency = 0f;
					nPCData.followDistance = 0f;
					nPCData.followDistanceMax = 0f;
				}
				if (component.headFacing == HeadFacing.Manual)
				{
					nPCData.isHeadTurning = true;
					nPCData.headTargetX = component.headTurnTarget.x;
					nPCData.headTargetY = component.headTurnTarget.y;
					nPCData.headTargetZ = component.headTurnTarget.z;
				}
				else
				{
					nPCData.isHeadTurning = false;
					nPCData.headTargetX = 0f;
					nPCData.headTargetY = 0f;
					nPCData.headTargetZ = 0f;
				}
			}
			return nPCData;
		}

		public void LoadData(NPCData data)
		{
			if (data.isOn)
			{
				gameObject.layer = LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.hotspotLayer);
			}
			else
			{
				gameObject.layer = LayerMask.NameToLayer(AdvGame.GetReferences().settingsManager.deactivatedLayer);
			}
			transform.position = new Vector3(data.LocX, data.LocY, data.LocZ);
			transform.eulerAngles = new Vector3(data.RotX, data.RotY, data.RotZ);
			transform.localScale = new Vector3(data.ScaleX, data.ScaleY, data.ScaleZ);
			if (!GetComponent<NPC>())
			{
				return;
			}
			NPC component = GetComponent<NPC>();
			component.EndPath();
			if (component.animationEngine == AnimationEngine.Sprites2DToolkit || component.animationEngine == AnimationEngine.SpritesUnity)
			{
				component.idleAnimSprite = data.idleAnim;
				component.walkAnimSprite = data.walkAnim;
				component.talkAnimSprite = data.talkAnim;
				component.runAnimSprite = data.runAnim;
			}
			else if (component.animationEngine == AnimationEngine.Legacy)
			{
				int result = 0;
				int.TryParse(data.idleAnim, out result);
				component.idleAnim = AssetLoader.RetrieveAsset(component.idleAnim, result);
				int.TryParse(data.walkAnim, out result);
				component.walkAnim = AssetLoader.RetrieveAsset(component.walkAnim, result);
				int.TryParse(data.talkAnim, out result);
				component.runAnim = AssetLoader.RetrieveAsset(component.runAnim, result);
				int.TryParse(data.runAnim, out result);
				component.talkAnim = AssetLoader.RetrieveAsset(component.talkAnim, result);
			}
			component.walkSound = AssetLoader.RetrieveAsset(component.walkSound, data.walkSound);
			component.runSound = AssetLoader.RetrieveAsset(component.runSound, data.runSound);
			component.speechLabel = data.speechLabel;
			component.portraitIcon.texture = AssetLoader.RetrieveAsset(component.portraitIcon.texture, data.portraitGraphic);
			component.walkSpeedScale = data.walkSpeed;
			component.runSpeedScale = data.runSpeed;
			component.lockDirection = data.lockDirection;
			component.lockScale = data.lockScale;
			if ((bool)component.spriteChild && (bool)component.spriteChild.GetComponent<FollowSortingMap>())
			{
				component.spriteChild.GetComponent<FollowSortingMap>().lockSorting = data.lockSorting;
			}
			else if ((bool)component.GetComponent<FollowSortingMap>())
			{
				component.GetComponent<FollowSortingMap>().lockSorting = data.lockSorting;
			}
			else
			{
				component.ReleaseSorting();
			}
			if (data.lockDirection)
			{
				component.spriteDirection = data.spriteDirection;
			}
			if (data.lockScale)
			{
				component.spriteScale = data.spriteScale;
			}
			if (data.lockSorting)
			{
				if ((bool)component.spriteChild && (bool)component.spriteChild.GetComponent<Renderer>())
				{
					component.spriteChild.GetComponent<Renderer>().sortingOrder = data.sortingOrder;
					component.spriteChild.GetComponent<Renderer>().sortingLayerName = data.sortingLayer;
				}
				else if ((bool)component.GetComponent<Renderer>())
				{
					component.GetComponent<Renderer>().sortingOrder = data.sortingOrder;
					component.GetComponent<Renderer>().sortingLayerName = data.sortingLayer;
				}
			}
			Char followTarget = null;
			if (data.followTargetID != 0)
			{
				RememberNPC rememberNPC = Serializer.returnComponent<RememberNPC>(data.followTargetID);
				if ((bool)rememberNPC.GetComponent<Char>())
				{
					followTarget = rememberNPC.GetComponent<Char>();
				}
			}
			component.FollowAssign(followTarget, data.followTargetIsPlayer, data.followFrequency, data.followDistance, data.followDistanceMax);
			component.Halt();
			if (data.pathData != null && data.pathData != string.Empty && (bool)GetComponent<Paths>())
			{
				Paths component2 = GetComponent<Paths>();
				component2 = Serializer.RestorePathData(component2, data.pathData);
				component.SetPath(component2, data.targetNode, data.prevNode);
				component.isRunning = data.isRunning;
			}
			else if (data.pathID != 0)
			{
				Paths paths = Serializer.returnComponent<Paths>(data.pathID);
				if (paths != null)
				{
					component.SetPath(paths, data.targetNode, data.prevNode);
				}
				else
				{
					Debug.LogWarning("Trying to assign a path for NPC " + name + ", but the path was not found - was it deleted?");
				}
			}
			if (data.lastPathID != 0)
			{
				Paths paths2 = Serializer.returnComponent<Paths>(data.lastPathID);
				if (paths2 != null)
				{
					component.SetLastPath(paths2, data.lastTargetNode, data.lastPrevNode);
				}
				else
				{
					Debug.LogWarning("Trying to assign the previous path for NPC " + name + ", but the path was not found - was it deleted?");
				}
			}
			if (data.isHeadTurning)
			{
				component.SetHeadTurnTarget(new Vector3(data.headTargetX, data.headTargetY, data.headTargetZ), true);
			}
			else
			{
				component.ClearHeadTurnTarget(true);
			}
		}
	}
}
