using AC;
using UnityEngine;

public class LeverPuzzle : MonoBehaviour
{
	public string puzzle = "01001";

	public string solution = "00000";

	public string solution2 = "11111";

	public Animation[] levers;

	public string[] animNum = new string[2];

	public Interaction resultInteraction;

	public bool puzzleSolved;

	private void Awake()
	{
		for (int i = 0; i < levers.Length; i++)
		{
		}
	}

	private void OnEnable()
	{
		puzzleSolved = false;
	}

	private void Update()
	{
		if (puzzleSolved)
		{
			return;
		}
		string text = string.Empty;
		for (int i = 0; i < levers.Length; i++)
		{
			text += levers[i].name;
			if (!levers[i].IsPlaying(animNum[int.Parse(levers[i].name)]))
			{
				levers[i].CrossFade(animNum[int.Parse(levers[i].name)], 0.2f);
			}
		}
		if (text == solution || text == solution2)
		{
			resultInteraction.SendMessage("Interact");
			puzzleSolved = true;
		}
	}
}
