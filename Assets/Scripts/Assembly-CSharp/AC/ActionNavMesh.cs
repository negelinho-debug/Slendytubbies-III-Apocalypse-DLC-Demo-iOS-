using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionNavMesh : Action
	{
		public int constantID;

		public int parameterID = -1;

		public NavigationMesh newNavMesh;

		public SortingMap sortingMap;

		public PlayerStart playerStart;

		public Cutscene cutscene;

		public SceneSetting sceneSetting;

		public ChangeNavMeshMethod changeNavMeshMethod;

		public InvAction holeAction;

		public PolygonCollider2D hole;

		public ActionNavMesh()
		{
			isDisplayed = true;
			title = "Engine: Change scene setting";
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			SceneSettings component = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
			if (sceneSetting == SceneSetting.DefaultNavMesh)
			{
				if (component.navigationMethod == AC_NavigationMethod.PolygonCollider && changeNavMeshMethod == ChangeNavMeshMethod.ChangeNumberOfHoles)
				{
					hole = AssignFile(parameters, parameterID, constantID, hole);
				}
				else
				{
					newNavMesh = AssignFile(parameters, parameterID, constantID, newNavMesh);
				}
			}
			else if (sceneSetting == SceneSetting.DefaultPlayerStart)
			{
				playerStart = AssignFile(parameters, parameterID, constantID, playerStart);
			}
			else if (sceneSetting == SceneSetting.SortingMap)
			{
				sortingMap = AssignFile(parameters, parameterID, constantID, sortingMap);
			}
			else if (sceneSetting == SceneSetting.OnLoadCutscene || sceneSetting == SceneSetting.OnStartCutscene)
			{
				cutscene = AssignFile(parameters, parameterID, constantID, cutscene);
			}
		}

		public override float Run()
		{
			SceneSettings component = GameObject.FindWithTag("GameEngine").GetComponent<SceneSettings>();
			if (sceneSetting == SceneSetting.DefaultNavMesh)
			{
				if (component.navigationMethod == AC_NavigationMethod.PolygonCollider && changeNavMeshMethod == ChangeNavMeshMethod.ChangeNumberOfHoles && hole != null)
				{
					NavigationMesh navMesh = component.navMesh;
					if (holeAction == InvAction.Add)
					{
						navMesh.AddHole(hole);
					}
					else if (holeAction == InvAction.Remove)
					{
						navMesh.RemoveHole(hole);
					}
				}
				else if (newNavMesh != null)
				{
					NavigationMesh navMesh2 = component.navMesh;
					navMesh2.TurnOff();
					newNavMesh.TurnOn();
					component.navMesh = newNavMesh;
					if (newNavMesh.GetComponent<ConstantID>() == null)
					{
						Debug.LogWarning("Warning: Changing to new NavMesh with no ConstantID - change will not be recognised by saved games.");
					}
				}
			}
			else if (sceneSetting == SceneSetting.DefaultPlayerStart && (bool)playerStart)
			{
				component.defaultPlayerStart = playerStart;
				if (playerStart.GetComponent<ConstantID>() == null)
				{
					Debug.LogWarning("Warning: Changing to new default PlayerStart with no ConstantID - change will not be recognised by saved games.");
				}
			}
			else if (sceneSetting == SceneSetting.SortingMap && (bool)sortingMap)
			{
				component.sortingMap = sortingMap;
				FollowSortingMap[] array = UnityEngine.Object.FindObjectsOfType(typeof(FollowSortingMap)) as FollowSortingMap[];
				FollowSortingMap[] array2 = array;
				foreach (FollowSortingMap followSortingMap in array2)
				{
					followSortingMap.UpdateSortingMap();
				}
				if (sortingMap.GetComponent<ConstantID>() == null)
				{
					Debug.LogWarning("Warning: Changing to new SortingMap with no ConstantID - change will not be recognised by saved games.");
				}
			}
			else if (sceneSetting == SceneSetting.OnLoadCutscene)
			{
				component.cutsceneOnLoad = cutscene;
				if (cutscene.GetComponent<ConstantID>() == null)
				{
					Debug.LogWarning("Warning: Changing to Cutscene On Load with no ConstantID - change will not be recognised by saved games.");
				}
			}
			else if (sceneSetting == SceneSetting.OnStartCutscene)
			{
				component.cutsceneOnStart = cutscene;
				if (cutscene.GetComponent<ConstantID>() == null)
				{
					Debug.LogWarning("Warning: Changing to Cutscene On Start with no ConstantID - change will not be recognised by saved games.");
				}
			}
			return 0f;
		}
	}
}
