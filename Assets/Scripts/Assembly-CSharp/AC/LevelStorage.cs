using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class LevelStorage : MonoBehaviour
	{
		[HideInInspector]
		public List<SingleLevelData> allLevelData = new List<SingleLevelData>();

		private void Awake()
		{
			allLevelData = new List<SingleLevelData>();
		}

		public void ClearAllLevelData()
		{
			allLevelData.Clear();
			allLevelData = new List<SingleLevelData>();
		}

		public void ClearCurrentLevelData()
		{
			foreach (SingleLevelData allLevelDatum in allLevelData)
			{
				if (allLevelDatum.sceneNumber == Application.loadedLevel)
				{
					allLevelData.Remove(allLevelDatum);
					break;
				}
			}
		}

		public void ReturnCurrentLevelData()
		{
			foreach (SingleLevelData allLevelDatum in allLevelData)
			{
				if (allLevelDatum.sceneNumber == Application.loadedLevel)
				{
					UnloadTransformData(allLevelDatum.transforms);
					UnloadColliderData(allLevelDatum.colliders);
					UnloadConversationData(allLevelDatum.conversations);
					UnloadContainerData(allLevelDatum.containers);
					UnloadNavMesh2DData(allLevelDatum.navMesh2Ds);
					UnloadCutsceneOnLoad(allLevelDatum.onLoadCutscene);
					UnloadCutsceneOnStart(allLevelDatum.onStartCutscene);
					UnloadShapeableData(allLevelDatum.shapeables);
					UnloadMoveableData(allLevelDatum.moveables);
					UnloadHotspotData(allLevelDatum.hotspots);
					UnloadTriggerData(allLevelDatum.triggers);
					UnloadNameData(allLevelDatum.names);
					UnloadNavMesh(allLevelDatum.navMesh);
					UnloadNPCData(allLevelDatum.npcs);
					UnloadPlayerStart(allLevelDatum.playerStart);
					UnloadSortingMap(allLevelDatum.sortingMap);
					UnloadVisibilityData(allLevelDatum.visibilitys);
					UnloadVariablesData(allLevelDatum.localVariablesData);
					break;
				}
			}
			AssetLoader.UnloadAssets();
		}

		public void StoreCurrentLevelData()
		{
			List<ColliderData> colliders = PopulateColliderData();
			List<ConversationData> conversations = PopulateConversationData();
			List<ContainerData> containers = PopulateContainerData();
			List<NavMesh2DData> navMesh2Ds = PopulateNavMesh2DData();
			List<ShapeableData> shapeables = PopulateShapeableData();
			List<MoveableData> moveables = PopulateMoveableData();
			List<HotspotData> hotspots = PopulateHotspotData();
			List<TriggerData> triggers = PopulateTriggerData();
			List<NameData> names = PopulateNameData();
			List<NPCData> npcs = PopulateNPCData();
			List<TransformData> transforms = PopulateTransformData();
			List<VisibilityData> visibilitys = PopulateVisibilityData();
			SingleLevelData singleLevelData = new SingleLevelData();
			singleLevelData.sceneNumber = Application.loadedLevel;
			SceneSettings component = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
			if ((bool)component)
			{
				if ((bool)component.navMesh && (bool)component.navMesh.GetComponent<ConstantID>())
				{
					singleLevelData.navMesh = Serializer.GetConstantID(component.navMesh.gameObject);
				}
				if ((bool)component.defaultPlayerStart && (bool)component.defaultPlayerStart.GetComponent<ConstantID>())
				{
					singleLevelData.playerStart = Serializer.GetConstantID(component.defaultPlayerStart.gameObject);
				}
				if ((bool)component.sortingMap && (bool)component.sortingMap.GetComponent<ConstantID>())
				{
					singleLevelData.sortingMap = Serializer.GetConstantID(component.sortingMap.gameObject);
				}
				if ((bool)component.cutsceneOnLoad && (bool)component.cutsceneOnLoad.GetComponent<ConstantID>())
				{
					singleLevelData.onLoadCutscene = Serializer.GetConstantID(component.cutsceneOnLoad.gameObject);
				}
				if ((bool)component.cutsceneOnStart && (bool)component.cutsceneOnStart.GetComponent<ConstantID>())
				{
					singleLevelData.onStartCutscene = Serializer.GetConstantID(component.cutsceneOnStart.gameObject);
				}
			}
			singleLevelData.localVariablesData = SaveSystem.CreateVariablesData(GameObject.FindWithTag("GameEngine").GetComponent<LocalVariables>().localVars, false, VariableLocation.Local);
			singleLevelData.conversations = conversations;
			singleLevelData.visibilitys = visibilitys;
			singleLevelData.transforms = transforms;
			singleLevelData.containers = containers;
			singleLevelData.navMesh2Ds = navMesh2Ds;
			singleLevelData.colliders = colliders;
			singleLevelData.shapeables = shapeables;
			singleLevelData.moveables = moveables;
			singleLevelData.hotspots = hotspots;
			singleLevelData.triggers = triggers;
			singleLevelData.names = names;
			singleLevelData.npcs = npcs;
			bool flag = false;
			for (int i = 0; i < allLevelData.Count; i++)
			{
				if (allLevelData[i].sceneNumber == Application.loadedLevel)
				{
					allLevelData[i] = singleLevelData;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				allLevelData.Add(singleLevelData);
			}
		}

		private void UnloadNavMesh(int navMeshInt)
		{
			NavigationMesh navigationMesh = Serializer.returnComponent<NavigationMesh>(navMeshInt);
			SceneSettings component = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
			if ((bool)navigationMesh && (bool)component && component.navigationMethod != AC_NavigationMethod.UnityNavigation)
			{
				if ((bool)component.navMesh)
				{
					NavigationMesh navMesh = component.navMesh;
					navMesh.TurnOff();
				}
				navigationMesh.TurnOn();
				component.navMesh = navigationMesh;
			}
		}

		private void UnloadPlayerStart(int playerStartInt)
		{
			PlayerStart playerStart = Serializer.returnComponent<PlayerStart>(playerStartInt);
			SceneSettings component = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
			if ((bool)playerStart && (bool)component)
			{
				component.defaultPlayerStart = playerStart;
			}
		}

		private void UnloadSortingMap(int sortingMapInt)
		{
			SortingMap sortingMap = Serializer.returnComponent<SortingMap>(sortingMapInt);
			SceneSettings component = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
			if ((bool)sortingMap && (bool)component)
			{
				component.sortingMap = sortingMap;
				FollowSortingMap[] array = Object.FindObjectsOfType(typeof(FollowSortingMap)) as FollowSortingMap[];
				FollowSortingMap[] array2 = array;
				foreach (FollowSortingMap followSortingMap in array2)
				{
					followSortingMap.UpdateSortingMap();
				}
			}
		}

		private void UnloadCutsceneOnLoad(int cutsceneInt)
		{
			Cutscene cutscene = Serializer.returnComponent<Cutscene>(cutsceneInt);
			SceneSettings component = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
			if ((bool)cutscene && (bool)component)
			{
				component.cutsceneOnLoad = cutscene;
			}
		}

		private void UnloadCutsceneOnStart(int cutsceneInt)
		{
			Cutscene cutscene = Serializer.returnComponent<Cutscene>(cutsceneInt);
			SceneSettings component = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
			if ((bool)cutscene && (bool)component)
			{
				component.cutsceneOnStart = cutscene;
			}
		}

		private void UnloadNameData(List<NameData> _names)
		{
			foreach (NameData _name in _names)
			{
				RememberName rememberName = Serializer.returnComponent<RememberName>(_name.objectID);
				if (rememberName != null)
				{
					rememberName.LoadData(_name);
				}
			}
		}

		private void UnloadColliderData(List<ColliderData> _colliders)
		{
			foreach (ColliderData _collider in _colliders)
			{
				RememberCollider rememberCollider = Serializer.returnComponent<RememberCollider>(_collider.objectID);
				if (rememberCollider != null)
				{
					rememberCollider.LoadData(_collider);
				}
			}
		}

		private void UnloadTransformData(List<TransformData> _transforms)
		{
			RememberTransform[] array = Object.FindObjectsOfType(typeof(RememberTransform)) as RememberTransform[];
			RememberTransform[] array2 = array;
			foreach (RememberTransform rememberTransform in array2)
			{
				if (!rememberTransform.saveScenePresence)
				{
					continue;
				}
				bool flag = false;
				foreach (TransformData _transform in _transforms)
				{
					if (_transform.objectID == rememberTransform.constantID)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					Object.Destroy(rememberTransform.gameObject);
				}
			}
			foreach (TransformData _transform2 in _transforms)
			{
				RememberTransform rememberTransform2 = Serializer.returnComponent<RememberTransform>(_transform2.objectID);
				if (rememberTransform2 == null && _transform2.bringBack)
				{
					Object[] array3 = Resources.LoadAll(string.Empty, typeof(GameObject));
					Object[] array4 = array3;
					foreach (Object obj in array4)
					{
						if (obj is GameObject)
						{
							GameObject gameObject = (GameObject)obj;
							if ((bool)gameObject.GetComponent<RememberTransform>() && gameObject.GetComponent<RememberTransform>().constantID == _transform2.objectID)
							{
								GameObject gameObject2 = Object.Instantiate(gameObject.gameObject);
								gameObject2.name = gameObject.name;
								rememberTransform2 = gameObject2.GetComponent<RememberTransform>();
							}
						}
					}
					Resources.UnloadUnusedAssets();
				}
				if (rememberTransform2 != null)
				{
					rememberTransform2.LoadData(_transform2);
				}
			}
		}

		private void UnloadNPCData(List<NPCData> _npcs)
		{
			foreach (NPCData _npc in _npcs)
			{
				RememberNPC rememberNPC = Serializer.returnComponent<RememberNPC>(_npc.objectID);
				if (rememberNPC != null)
				{
					rememberNPC.LoadData(_npc);
				}
			}
		}

		private void UnloadShapeableData(List<ShapeableData> _shapeables)
		{
			foreach (ShapeableData _shapeable in _shapeables)
			{
				RememberShapeable rememberShapeable = Serializer.returnComponent<RememberShapeable>(_shapeable.objectID);
				if (rememberShapeable != null)
				{
					rememberShapeable.LoadData(_shapeable);
				}
			}
		}

		private void UnloadMoveableData(List<MoveableData> _moveables)
		{
			foreach (MoveableData _moveable in _moveables)
			{
				RememberMoveable rememberMoveable = Serializer.returnComponent<RememberMoveable>(_moveable.objectID);
				if (rememberMoveable != null)
				{
					rememberMoveable.LoadData(_moveable);
				}
			}
		}

		private void UnloadHotspotData(List<HotspotData> _hotspots)
		{
			foreach (HotspotData _hotspot in _hotspots)
			{
				RememberHotspot rememberHotspot = Serializer.returnComponent<RememberHotspot>(_hotspot.objectID);
				if (rememberHotspot != null)
				{
					rememberHotspot.LoadData(_hotspot);
				}
			}
		}

		private void UnloadTriggerData(List<TriggerData> _triggers)
		{
			foreach (TriggerData _trigger in _triggers)
			{
				RememberTrigger rememberTrigger = Serializer.returnComponent<RememberTrigger>(_trigger.objectID);
				if (rememberTrigger != null)
				{
					rememberTrigger.LoadData(_trigger);
				}
			}
		}

		private void UnloadVisibilityData(List<VisibilityData> _visibilitys)
		{
			foreach (VisibilityData _visibility in _visibilitys)
			{
				RememberVisibility rememberVisibility = Serializer.returnComponent<RememberVisibility>(_visibility.objectID);
				if (rememberVisibility != null)
				{
					rememberVisibility.LoadData(_visibility);
				}
			}
		}

		private void UnloadConversationData(List<ConversationData> _conversations)
		{
			foreach (ConversationData _conversation in _conversations)
			{
				RememberConversation rememberConversation = Serializer.returnComponent<RememberConversation>(_conversation.objectID);
				if (rememberConversation != null)
				{
					rememberConversation.LoadData(_conversation);
				}
			}
		}

		private void UnloadContainerData(List<ContainerData> _containers)
		{
			foreach (ContainerData _container in _containers)
			{
				RememberContainer rememberContainer = Serializer.returnComponent<RememberContainer>(_container.objectID);
				if (rememberContainer != null)
				{
					rememberContainer.LoadData(_container);
				}
			}
		}

		private void UnloadNavMesh2DData(List<NavMesh2DData> _navMesh2Ds)
		{
			foreach (NavMesh2DData _navMesh2D in _navMesh2Ds)
			{
				RememberNavMesh2D rememberNavMesh2D = Serializer.returnComponent<RememberNavMesh2D>(_navMesh2D.objectID);
				if (rememberNavMesh2D != null)
				{
					rememberNavMesh2D.LoadData(_navMesh2D);
				}
			}
		}

		private List<TransformData> PopulateTransformData()
		{
			List<TransformData> list = new List<TransformData>();
			RememberTransform[] array = Object.FindObjectsOfType(typeof(RememberTransform)) as RememberTransform[];
			RememberTransform[] array2 = array;
			foreach (RememberTransform rememberTransform in array2)
			{
				if (rememberTransform.constantID != 0)
				{
					list.Add(rememberTransform.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberTransform.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private List<ColliderData> PopulateColliderData()
		{
			List<ColliderData> list = new List<ColliderData>();
			RememberCollider[] array = Object.FindObjectsOfType(typeof(RememberCollider)) as RememberCollider[];
			RememberCollider[] array2 = array;
			foreach (RememberCollider rememberCollider in array2)
			{
				if (rememberCollider.constantID != 0)
				{
					list.Add(rememberCollider.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberCollider.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private List<ShapeableData> PopulateShapeableData()
		{
			List<ShapeableData> list = new List<ShapeableData>();
			RememberShapeable[] array = Object.FindObjectsOfType(typeof(RememberShapeable)) as RememberShapeable[];
			RememberShapeable[] array2 = array;
			foreach (RememberShapeable rememberShapeable in array2)
			{
				if (rememberShapeable.constantID != 0)
				{
					list.Add(rememberShapeable.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberShapeable.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private List<MoveableData> PopulateMoveableData()
		{
			List<MoveableData> list = new List<MoveableData>();
			RememberMoveable[] array = Object.FindObjectsOfType(typeof(RememberMoveable)) as RememberMoveable[];
			RememberMoveable[] array2 = array;
			foreach (RememberMoveable rememberMoveable in array2)
			{
				if (rememberMoveable.constantID != 0)
				{
					list.Add(rememberMoveable.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberMoveable.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private List<NameData> PopulateNameData()
		{
			List<NameData> list = new List<NameData>();
			RememberName[] array = Object.FindObjectsOfType(typeof(RememberName)) as RememberName[];
			RememberName[] array2 = array;
			foreach (RememberName rememberName in array2)
			{
				if (rememberName.constantID != 0)
				{
					list.Add(rememberName.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberName.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private List<NPCData> PopulateNPCData()
		{
			List<NPCData> list = new List<NPCData>();
			RememberNPC[] array = Object.FindObjectsOfType(typeof(RememberNPC)) as RememberNPC[];
			RememberNPC[] array2 = array;
			foreach (RememberNPC rememberNPC in array2)
			{
				if (rememberNPC.constantID != 0)
				{
					list.Add(rememberNPC.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberNPC.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private List<VisibilityData> PopulateVisibilityData()
		{
			List<VisibilityData> list = new List<VisibilityData>();
			RememberVisibility[] array = Object.FindObjectsOfType(typeof(RememberVisibility)) as RememberVisibility[];
			RememberVisibility[] array2 = array;
			foreach (RememberVisibility rememberVisibility in array2)
			{
				if (rememberVisibility.constantID != 0)
				{
					list.Add(rememberVisibility.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberVisibility.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private List<TriggerData> PopulateTriggerData()
		{
			List<TriggerData> list = new List<TriggerData>();
			RememberTrigger[] array = Object.FindObjectsOfType(typeof(RememberTrigger)) as RememberTrigger[];
			RememberTrigger[] array2 = array;
			foreach (RememberTrigger rememberTrigger in array2)
			{
				if (rememberTrigger.constantID != 0)
				{
					list.Add(rememberTrigger.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberTrigger.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private List<HotspotData> PopulateHotspotData()
		{
			List<HotspotData> list = new List<HotspotData>();
			RememberHotspot[] array = Object.FindObjectsOfType(typeof(RememberHotspot)) as RememberHotspot[];
			RememberHotspot[] array2 = array;
			foreach (RememberHotspot rememberHotspot in array2)
			{
				if (rememberHotspot.constantID != 0)
				{
					list.Add(rememberHotspot.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberHotspot.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private List<ConversationData> PopulateConversationData()
		{
			List<ConversationData> list = new List<ConversationData>();
			RememberConversation[] array = Object.FindObjectsOfType(typeof(RememberConversation)) as RememberConversation[];
			RememberConversation[] array2 = array;
			foreach (RememberConversation rememberConversation in array2)
			{
				if (rememberConversation.constantID != 0)
				{
					list.Add(rememberConversation.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberConversation.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private List<ContainerData> PopulateContainerData()
		{
			List<ContainerData> list = new List<ContainerData>();
			RememberContainer[] array = Object.FindObjectsOfType(typeof(RememberContainer)) as RememberContainer[];
			RememberContainer[] array2 = array;
			foreach (RememberContainer rememberContainer in array2)
			{
				if (rememberContainer.constantID != 0)
				{
					list.Add(rememberContainer.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberContainer.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private List<NavMesh2DData> PopulateNavMesh2DData()
		{
			List<NavMesh2DData> list = new List<NavMesh2DData>();
			RememberNavMesh2D[] array = Object.FindObjectsOfType(typeof(RememberNavMesh2D)) as RememberNavMesh2D[];
			RememberNavMesh2D[] array2 = array;
			foreach (RememberNavMesh2D rememberNavMesh2D in array2)
			{
				if (rememberNavMesh2D.constantID != 0)
				{
					list.Add(rememberNavMesh2D.SaveData());
				}
				else
				{
					Debug.LogWarning("GameObject " + rememberNavMesh2D.name + " was not saved because it's ConstantID has not been set!");
				}
			}
			return list;
		}

		private void AssignMenuLocks(List<Menu> menus, string menuLockData)
		{
			if (menuLockData.Length == 0)
			{
				return;
			}
			string[] array = menuLockData.Split("|"[0]);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(":"[0]);
				int result = 0;
				int.TryParse(array3[0], out result);
				bool result2 = false;
				bool.TryParse(array3[1], out result2);
				foreach (Menu menu in menus)
				{
					if (menu.id == result)
					{
						menu.isLocked = result2;
						break;
					}
				}
			}
		}

		private void UnloadVariablesData(string data)
		{
			if (data == null || data.Length <= 0)
			{
				return;
			}
			string[] array = data.Split("|"[0]);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string[] array3 = text.Split(":"[0]);
				int result = 0;
				int.TryParse(array3[0], out result);
				GVar variable = LocalVariables.GetVariable(result);
				if (variable.type == VariableType.String)
				{
					string value = array3[1];
					variable.SetValue(value);
				}
				else if (variable.type == VariableType.Float)
				{
					float result2 = 0f;
					float.TryParse(array3[1], out result2);
					variable.SetValue(result2, SetVarMethod.SetValue);
				}
				else
				{
					int result3 = 0;
					int.TryParse(array3[1], out result3);
					variable.SetValue(result3, SetVarMethod.SetValue);
				}
			}
		}
	}
}
