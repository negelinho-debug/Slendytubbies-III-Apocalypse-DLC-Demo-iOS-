using UnityEngine;

public class automaticDoorsTerminal : MonoBehaviour
{
	public automaticDoors doors;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			doors.Open();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			doors.Close();
		}
	}
}
