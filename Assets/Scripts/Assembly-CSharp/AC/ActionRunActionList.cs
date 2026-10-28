using System;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionRunActionList : Action
	{
		public enum ListSource
		{
			InScene,
			AssetFile
		}

		public ListSource listSource;

		public ActionList actionList;

		public ActionListAsset invActionList;

		public int constantID;

		public int parameterID = -1;

		public bool runFromStart = true;

		public int jumpToAction;

		public Action jumpToActionActual;

		public bool runInParallel;

		public List<ActionParameter> localParameters = new List<ActionParameter>();

		private RuntimeActionList runtimeActionList;

		public ActionRunActionList()
		{
			isDisplayed = true;
			title = "Engine: Run ActionList";
			numSockets = 0;
		}

		public override void AssignValues(List<ActionParameter> parameters)
		{
			if (listSource == ListSource.InScene)
			{
				actionList = AssignFile(parameters, parameterID, constantID, actionList);
			}
		}

		public override float Run()
		{
			ActionListManager component = GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>();
			if (!isRunning)
			{
				isRunning = true;
				runtimeActionList = null;
				if (listSource == ListSource.InScene)
				{
					if (actionList != null)
					{
						component.EndList(actionList);
						if (actionList.useParameters)
						{
							SendParameters(actionList.parameters, false);
						}
						if (runFromStart)
						{
							actionList.Interact();
						}
						else
						{
							actionList.Interact(GetSkipIndex(actionList.actions));
						}
					}
				}
				else if (listSource == ListSource.AssetFile && invActionList != null)
				{
					if (invActionList.useParameters)
					{
						SendParameters(invActionList.parameters, true);
					}
					if (runFromStart)
					{
						runtimeActionList = AdvGame.RunActionListAsset(invActionList);
					}
					else
					{
						runtimeActionList = AdvGame.RunActionListAsset(invActionList, GetSkipIndex(invActionList.actions));
					}
				}
				if (!runInParallel || (runInParallel && willWait))
				{
					return defaultPauseTime;
				}
			}
			else if (listSource == ListSource.InScene && actionList != null)
			{
				if (component.IsListRunning(actionList))
				{
					return defaultPauseTime;
				}
				isRunning = false;
			}
			else if (listSource == ListSource.AssetFile && invActionList != null)
			{
				if (component.IsListRunning(runtimeActionList))
				{
					return defaultPauseTime;
				}
				isRunning = false;
			}
			return 0f;
		}

		public override void Skip()
		{
			ActionListManager component = GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>();
			if (listSource == ListSource.InScene && actionList != null)
			{
				component.EndList(actionList);
				if (actionList.useParameters)
				{
					SendParameters(actionList.parameters, false);
				}
				if (runFromStart)
				{
					actionList.Interact();
				}
				else
				{
					actionList.Interact(GetSkipIndex(actionList.actions));
				}
			}
			else if (listSource == ListSource.AssetFile && invActionList != null)
			{
				component.EndAssetList(invActionList.name);
				if (invActionList.useParameters)
				{
					SendParameters(invActionList.parameters, true);
				}
				if (runFromStart)
				{
					runtimeActionList = AdvGame.RunActionListAsset(invActionList);
				}
				else
				{
					runtimeActionList = AdvGame.RunActionListAsset(invActionList, GetSkipIndex(invActionList.actions));
				}
			}
		}

		private int GetSkipIndex(List<Action> _actions)
		{
			int result = jumpToAction;
			if ((bool)jumpToActionActual && _actions.IndexOf(jumpToActionActual) > 0)
			{
				result = _actions.IndexOf(jumpToActionActual);
			}
			return result;
		}

		public override int End(List<Action> actions)
		{
			if (runInParallel)
			{
				return base.End(actions);
			}
			return -1;
		}

		private void SendParameters(List<ActionParameter> externalParameters, bool sendingToAsset)
		{
			for (int i = 0; i < externalParameters.Count; i++)
			{
				if (localParameters.Count <= i)
				{
					continue;
				}
				if (externalParameters[i].parameterType == ParameterType.String)
				{
					externalParameters[i].SetValue(localParameters[i].stringValue);
				}
				else if (externalParameters[i].parameterType == ParameterType.Float)
				{
					externalParameters[i].SetValue(localParameters[i].floatValue);
				}
				else if (externalParameters[i].parameterType != ParameterType.GameObject)
				{
					externalParameters[i].SetValue(localParameters[i].intValue);
				}
				else if (sendingToAsset)
				{
					if (isAssetFile)
					{
						externalParameters[i].SetValue(localParameters[i].intValue);
					}
					else if (localParameters[i].gameObject != null)
					{
						int value = 0;
						if ((bool)localParameters[i].gameObject && (bool)localParameters[i].gameObject.GetComponent<ConstantID>())
						{
							value = localParameters[i].gameObject.GetComponent<ConstantID>().constantID;
						}
						else
						{
							Debug.LogWarning(localParameters[i].gameObject.name + " requires a ConstantID script component!");
						}
						externalParameters[i].SetValue(value);
					}
					else
					{
						externalParameters[i].SetValue(localParameters[i].intValue);
					}
				}
				else if (localParameters[i].gameObject != null)
				{
					externalParameters[i].SetValue(localParameters[i].gameObject);
				}
				else
				{
					externalParameters[i].SetValue(localParameters[i].intValue);
				}
			}
		}
	}
}
