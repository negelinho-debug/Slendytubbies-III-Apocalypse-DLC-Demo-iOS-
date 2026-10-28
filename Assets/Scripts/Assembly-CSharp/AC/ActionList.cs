using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	[Serializable]
	public class ActionList : MonoBehaviour
	{
		[HideInInspector]
		public bool isSkippable = true;

		[HideInInspector]
		public float triggerTime;

		[HideInInspector]
		public bool autosaveAfter;

		[HideInInspector]
		public ActionListType actionListType;

		[HideInInspector]
		public List<Action> actions = new List<Action>();

		[HideInInspector]
		public Conversation conversation;

		[HideInInspector]
		public ActionListAsset assetFile;

		[HideInInspector]
		public ActionListSource source;

		[HideInInspector]
		public bool useParameters;

		[HideInInspector]
		public bool unfreezePauseMenus = true;

		[HideInInspector]
		public List<ActionParameter> parameters = new List<ActionParameter>();

		protected bool isSkipping;

		protected int nextActionNumber = -1;

		protected LayerMask LayerHotspot;

		protected LayerMask LayerOff;

		protected StateHandler stateHandler;

		protected ActionListManager actionListManager;

		private void Awake()
		{
			SettingsManager settingsManager = AdvGame.GetReferences().settingsManager;
			LayerHotspot = LayerMask.NameToLayer(settingsManager.hotspotLayer);
			LayerOff = LayerMask.NameToLayer(settingsManager.deactivatedLayer);
			if ((bool)GameObject.FindWithTag("GameEngine") && (bool)GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>())
			{
				actionListManager = GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>();
			}
			if (source == ActionListSource.AssetFile)
			{
				actions.Clear();
				if (assetFile != null && assetFile.actions.Count > 0)
				{
					foreach (Action action in assetFile.actions)
					{
						actions.Add(action);
					}
					useParameters = assetFile.useParameters;
					parameters = assetFile.parameters;
				}
			}
			if (!useParameters)
			{
				return;
			}
			foreach (ActionParameter parameter in parameters)
			{
				parameter.Reset();
			}
		}

		private void Start()
		{
			if ((bool)GameObject.FindWithTag("PersistentEngine") && (bool)GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>())
			{
				stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
			}
		}

		public virtual void Interact()
		{
			if (actions.Count > 0)
			{
				if (triggerTime > 0f)
				{
					StartCoroutine("PauseUntilStart");
					return;
				}
				ResetSkips();
				BeginActionList(0);
			}
		}

		public void Interact(int i)
		{
			if (actions.Count > 0 && actions.Count > i)
			{
				ResetSkips();
				BeginActionList(i);
			}
		}

		private IEnumerator PauseUntilStart()
		{
			if (triggerTime > 0f)
			{
				yield return new WaitForSeconds(triggerTime);
			}
			ResetSkips();
			BeginActionList(0);
		}

		private void ResetSkips()
		{
			foreach (Action action in actions)
			{
				if (action != null)
				{
					action.lastResult = -10;
				}
			}
		}

		private void BeginActionList(int i)
		{
			if (actionListManager == null)
			{
				actionListManager = GameObject.FindWithTag("GameEngine").GetComponent<ActionListManager>();
			}
			actionListManager.AddToList(this);
			nextActionNumber = i;
			ProcessAction(i);
		}

		private void ProcessAction(int thisActionNumber)
		{
			if (nextActionNumber > -1 && nextActionNumber < actions.Count && (object)actions[thisActionNumber] != null)
			{
				if (!actions[thisActionNumber].isEnabled)
				{
					if (actions.Count > thisActionNumber + 1)
					{
						ProcessAction(thisActionNumber + 1);
					}
					else
					{
						EndCutscene();
					}
				}
				else
				{
					nextActionNumber = thisActionNumber + 1;
					StartCoroutine("RunAction", actions[thisActionNumber]);
				}
			}
			else
			{
				EndCutscene();
			}
		}

		private IEnumerator RunAction(Action action)
		{
			if (useParameters)
			{
				action.AssignValues(parameters);
			}
			else
			{
				action.AssignValues(null);
			}
			if (isSkipping)
			{
				action.Skip();
			}
			else
			{
				action.isRunning = false;
				float waitTime = action.Run();
				if (action is ActionCheck || action is ActionCheckMultiple)
				{
					yield return new WaitForFixedUpdate();
				}
				else if (waitTime > 0f)
				{
					while (action.isRunning)
					{
						if (this is RuntimeActionList && actionListType == ActionListType.PauseGameplay && !unfreezePauseMenus)
						{
							float endTime = Time.realtimeSinceStartup + waitTime;
							while (Time.realtimeSinceStartup < endTime)
							{
								yield return null;
							}
						}
						else
						{
							yield return new WaitForSeconds(waitTime);
						}
						waitTime = action.Run();
					}
				}
			}
			EndAction(action);
		}

		private void EndAction(Action action)
		{
			action.isRunning = false;
			int num = 0;
			num = ((isSkipping && action.lastResult != -10 && (action is ActionCheck || action is ActionCheckMultiple)) ? action.lastResult : (action.lastResult = action.End(actions)));
			if (num >= 0)
			{
				nextActionNumber = num;
			}
			if (action.endAction == ResultAction.RunCutscene)
			{
				if (action.isAssetFile && action.linkedAsset != null)
				{
					AdvGame.RunActionListAsset(action.linkedAsset);
				}
				else if (!action.isAssetFile && action.linkedCutscene != null && action.linkedCutscene != this)
				{
					action.linkedCutscene.SendMessage("Interact");
				}
			}
			if (num == -1 || num == -2)
			{
				EndCutscene();
			}
			else if (nextActionNumber >= 0)
			{
				ProcessAction(nextActionNumber);
			}
			if (action.endAction == ResultAction.RunCutscene && !action.isAssetFile && action.linkedCutscene != null && action.linkedCutscene == this)
			{
				action.linkedCutscene.SendMessage("Interact");
			}
		}

		protected virtual void EndCutscene()
		{
			actionListManager.EndList(this);
		}

		private void TurnOn()
		{
			gameObject.layer = LayerHotspot;
		}

		private void TurnOff()
		{
			gameObject.layer = LayerOff;
		}

		public void Reset()
		{
			isSkipping = false;
			nextActionNumber = -1;
			StopCoroutine("RunAction");
			StopCoroutine("InteractCoroutine");
		}

		public void Kill()
		{
			StopCoroutine("PauseUntilStart");
			actionListManager.EndList(this);
		}

		public void Skip()
		{
			isSkipping = true;
			StopCoroutine("RunAction");
			BeginActionList(0);
		}

		private void OnDestroy()
		{
			actionListManager = null;
			stateHandler = null;
		}
	}
}
