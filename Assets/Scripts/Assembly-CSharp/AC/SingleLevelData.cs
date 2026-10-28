using System;
using System.Collections.Generic;

namespace AC
{
	[Serializable]
	public class SingleLevelData
	{
		public List<ConversationData> conversations = new List<ConversationData>();

		public List<VisibilityData> visibilitys = new List<VisibilityData>();

		public List<NavMesh2DData> navMesh2Ds = new List<NavMesh2DData>();

		public List<ContainerData> containers = new List<ContainerData>();

		public List<TransformData> transforms = new List<TransformData>();

		public List<ShapeableData> shapeables = new List<ShapeableData>();

		public List<MoveableData> moveables = new List<MoveableData>();

		public List<ColliderData> colliders = new List<ColliderData>();

		public List<HotspotData> hotspots = new List<HotspotData>();

		public List<TriggerData> triggers = new List<TriggerData>();

		public List<NameData> names = new List<NameData>();

		public List<NPCData> npcs = new List<NPCData>();

		public int sceneNumber;

		public int navMesh;

		public int playerStart;

		public int sortingMap;

		public int onLoadCutscene;

		public int onStartCutscene;

		public string localVariablesData;
	}
}
