using AC;
using UnityEngine;

public class ChangeDeath : MonoBehaviour
{
	public BossBattleMechanics bbm;

	public Cutscene newDeath;

	private void Start()
	{
		bbm.playerDeath = newDeath;
	}
}
