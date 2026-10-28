using UnityEngine;

public class BattleDamage : MonoBehaviour
{
	public GameObject player;

	public bool constantCheck;

	public int damage = 10;

	public bool lookAtPlayer;

	public Vector3 lookOffset;

	public Transform impactObj;

	public bool knockDown;

	public float delayAgain;

	private float timer;

	private void Awake()
	{
		if (player == null)
		{
			player = GameObject.Find("Player");
		}
		if (lookAtPlayer)
		{
			transform.LookAt(player.transform);
			transform.localEulerAngles += lookOffset;
		}
		timer = delayAgain;
	}

	private void Start()
	{
		if (!constantCheck && player != null && Vector3.Distance(transform.position, player.transform.position) < GetComponent<BoxCollider>().size.x)
		{
			player.transform.SendMessage("DoDamage", damage, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void Update()
	{
		if (!constantCheck || !(player != null))
		{
			return;
		}
		if (Vector3.Distance(transform.position, player.transform.position) < GetComponent<BoxCollider>().size.x && timer >= delayAgain)
		{
			player.transform.SendMessage("DoDamage", damage, SendMessageOptions.DontRequireReceiver);
			if (impactObj != null)
			{
				Object.Instantiate(impactObj, player.transform.position, player.transform.rotation);
			}
			if (delayAgain == 0f)
			{
				Object.Destroy(gameObject);
			}
			timer = 0f;
		}
		timer += 1f * Time.deltaTime;
	}
}
