using UnityEngine;

public class HealPlayer : MonoBehaviour
{
	public int healAmount = 20;

	public Transform healAudio;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<PlayerBattleMechanics>().playerHealth.health += healAmount;
			if (other.GetComponent<PlayerBattleMechanics>().playerHealth.health > 100)
			{
				other.GetComponent<PlayerBattleMechanics>().playerHealth.health = 100;
			}
			if (healAudio != null)
			{
				Object.Instantiate(healAudio, transform.position, transform.rotation);
			}
			Object.Destroy(gameObject);
		}
	}
}
