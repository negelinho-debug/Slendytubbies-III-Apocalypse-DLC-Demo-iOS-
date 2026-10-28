using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AC
{
	public class Conversation : MonoBehaviour
	{
		public ActionListSource interactionSource;

		public List<ButtonDialog> options = new List<ButtonDialog>();

		public ButtonDialog selectedOption;

		public bool isTimed;

		public bool autoPlay;

		public float timer = 5f;

		public int defaultOption;

		private float startTime;

		private PlayerInput playerInput;

		private StateHandler stateHandler;

		private void Awake()
		{
			playerInput = GameObject.FindWithTag("GameEngine").GetComponent<PlayerInput>();
			Upgrade();
		}

		private void Start()
		{
			stateHandler = GameObject.FindWithTag("PersistentEngine").GetComponent<StateHandler>();
		}

		public void Interact()
		{
			CancelInvoke("RunDefault");
			int num = 0;
			foreach (ButtonDialog option in options)
			{
				if (option.isOn)
				{
					num++;
				}
			}
			if ((bool)playerInput)
			{
				if (num == 1 && autoPlay)
				{
					foreach (ButtonDialog option2 in options)
					{
						if (option2.isOn)
						{
							RunOption(option2);
							return;
						}
					}
				}
				else if (num > 0)
				{
					playerInput.activeConversation = this;
					stateHandler.gameState = GameState.DialogOptions;
				}
				else
				{
					playerInput.activeConversation = null;
				}
			}
			if (isTimed)
			{
				startTime = Time.time;
				Invoke("RunDefault", timer);
			}
		}

		private void RunOption(ButtonDialog _option)
		{
			Conversation conversation;
			if (_option.conversationAction != ConversationAction.ReturnToConversation)
			{
				conversation = ((_option.conversationAction != ConversationAction.RunOtherConversation || !(_option.newConversation != null)) ? null : _option.newConversation);
			}
			else
			{
				conversation = this;
			}
			if (interactionSource == ActionListSource.AssetFile && (bool)_option.assetFile)
			{
				AdvGame.RunActionListAsset(_option.assetFile, conversation);
			}
			else if (interactionSource == ActionListSource.InScene && (bool)_option.dialogueOption)
			{
				_option.dialogueOption.conversation = conversation;
				_option.dialogueOption.Interact();
			}
			else
			{
				Debug.Log("No Interaction object found!");
				stateHandler.gameState = GameState.Normal;
			}
		}

		public void TurnOn()
		{
			Interact();
		}

		public void TurnOff()
		{
			if ((bool)playerInput)
			{
				CancelInvoke("RunDefault");
				playerInput.activeConversation = null;
			}
		}

		private void RunDefault()
		{
			if ((bool)playerInput && playerInput.activeConversation != null && options.Count > defaultOption && defaultOption > -1)
			{
				playerInput.activeConversation = null;
				RunOption(options[defaultOption]);
			}
		}

		private IEnumerator RunOptionCo(int i)
		{
			yield return new WaitForSeconds(0.3f);
			RunOption(options[i]);
		}

		public void RunOption(int slot)
		{
			CancelInvoke("RunDefault");
			int i = ConvertSlotToOption(slot);
			if ((bool)playerInput)
			{
				playerInput.activeConversation = null;
			}
			StartCoroutine(RunOptionCo(i));
		}

		public float GetTimeRemaining()
		{
			return (startTime + timer - Time.time) / timer;
		}

		private int ConvertSlotToOption(int slot)
		{
			int num = 0;
			for (int i = 0; i < options.Count; i++)
			{
				if (options[i].isOn)
				{
					num++;
					if (num == slot + 1)
					{
						return i;
					}
				}
			}
			return 0;
		}

		public string GetOptionName(int slot)
		{
			int index = ConvertSlotToOption(slot);
			if (Options.GetLanguage() > 0)
			{
				return SpeechManager.GetTranslation(options[index].lineID, Options.GetLanguage());
			}
			return options[index].label;
		}

		public Texture2D GetOptionIcon(int slot)
		{
			int index = ConvertSlotToOption(slot);
			return options[index].icon;
		}

		public void SetOption(int id, bool flag, bool isLocked)
		{
			foreach (ButtonDialog option in options)
			{
				if (option.ID == id)
				{
					if (!option.isLocked)
					{
						option.isLocked = isLocked;
						option.isOn = flag;
					}
					break;
				}
			}
		}

		public int GetCount()
		{
			int num = 0;
			foreach (ButtonDialog option in options)
			{
				if (option.isOn)
				{
					num++;
				}
			}
			return num;
		}

		public List<bool> GetOptionStates()
		{
			List<bool> list = new List<bool>();
			foreach (ButtonDialog option in options)
			{
				list.Add(option.isOn);
			}
			return list;
		}

		public List<bool> GetOptionLocks()
		{
			List<bool> list = new List<bool>();
			foreach (ButtonDialog option in options)
			{
				list.Add(option.isLocked);
			}
			return list;
		}

		public void SetOptionStates(List<bool> states)
		{
			int num = 0;
			foreach (ButtonDialog option in options)
			{
				option.isOn = states[num];
				num++;
			}
		}

		public void SetOptionLocks(List<bool> locks)
		{
			int num = 0;
			foreach (ButtonDialog option in options)
			{
				option.isLocked = locks[num];
				num++;
			}
		}

		public void Upgrade()
		{
			if (options.Count > 0 && options[0].ID == 0)
			{
				for (int i = 0; i < options.Count; i++)
				{
					options[i].ID = i + 1;
				}
			}
		}

		public int[] GetIDArray()
		{
			List<int> list = new List<int>();
			foreach (ButtonDialog option in options)
			{
				list.Add(option.ID);
			}
			list.Sort();
			return list.ToArray();
		}

		private void OnDestroy()
		{
			playerInput = null;
			stateHandler = null;
		}
	}
}
