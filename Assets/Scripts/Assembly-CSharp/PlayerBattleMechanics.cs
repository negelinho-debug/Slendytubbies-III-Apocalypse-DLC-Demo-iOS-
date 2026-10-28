using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleMechanics : MonoBehaviour
{
	[Serializable]
	public class AttackMove
	{
		public string animName;

		public float[] hitTimes;

		public int damage;

		public float finishDelay;

		public AudioSource sound;

		public float joltAmount = 1f;
	}

	public Camera cam;

	public MecanimControl mc;

	public float walkSpeed = 3f;

	public float runSpeed = 5f;

	public GameObject target;

	public BattleHealth playerHealth;

	public Transform blockImpact;

	public Transform blockPos;

	private float speed;

	private int curAttack;

	public List<AttackMove> attacks;

	public float attackDistance = 2f;

	private bool isAttacking;

	private bool isBlocking;

	private bool isRolling;

	private bool isfalling;

	private Vector3 direction;

	private bool rollVert;

	private bool rollHorz;

	private bool combo;

	private bool delay;

	private bool hurt;

	private void Start()
	{
	}

	private void OnDisable()
	{
		StopAllCoroutines();
		isAttacking = false;
		isBlocking = false;
		isRolling = false;
		hurt = false;
	}

	private void Update()
	{
		if (target == null)
		{
			target = GameObject.FindGameObjectWithTag("Monster");
		}
		else
		{
			target = GameObject.FindGameObjectWithTag("Monster");
		}
		if (!isAttacking)
		{
			if (Input.GetButton("Run") && playerHealth.stamina > 0f)
			{
				speed = runSpeed;
			}
			else
			{
				speed = walkSpeed;
			}
		}
		else
		{
			speed = 0.5f;
		}
		if (!isRolling)
		{
			float axis = Input.GetAxis("Vertical");
			float axis2 = Input.GetAxis("Horizontal");
			direction = transform.forward * axis;
			direction += transform.right * axis2;
			if (!isBlocking && !hurt)
			{
				transform.position += direction * Time.deltaTime * speed;
			}
		}
		else
		{
			transform.position += direction * Time.deltaTime * 8f;
		}
		checkRoll();
		Vector3 position = target.transform.position;
		position.y = transform.position.y;
		Quaternion b = Quaternion.LookRotation(position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, b, 1f * Time.deltaTime);
		if (direction != Vector3.zero)
		{
			if (!isBlocking)
			{
				mc.transform.rotation = Quaternion.Slerp(mc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 4f);
			}
			else
			{
				mc.transform.rotation = Quaternion.Slerp(mc.transform.rotation, Quaternion.LookRotation(transform.forward), Time.deltaTime * 6f);
			}
			if (!isAttacking && !isBlocking && !hurt && !isRolling)
			{
				if (!mc.IsPlaying("Run") && speed == runSpeed)
				{
					mc.CrossFade("Run", 0.2f);
				}
				if (!mc.IsPlaying("Walk") && speed == walkSpeed)
				{
					mc.CrossFade("Walk", 0.2f);
				}
			}
		}
		else
		{
			if (!mc.IsPlaying("Idle") && !isAttacking && !isBlocking && !hurt && !isRolling)
			{
				mc.CrossFade("Idle", 0.2f);
			}
			if (isBlocking)
			{
				mc.transform.rotation = Quaternion.Slerp(mc.transform.rotation, Quaternion.LookRotation(transform.forward), Time.deltaTime * 6f);
			}
		}
		if (!isAttacking && !isBlocking && !hurt && !isRolling && Input.GetButtonDown("Attack"))
		{
			StartCoroutine(AttackNow());
		}
		if (!hurt && playerHealth.stamina > 1f && !isRolling)
		{
			isBlocking = Input.GetButton("Block");
		}
		else
		{
			isBlocking = false;
		}
		if (isBlocking)
		{
			StopCoroutine(AttackNow());
			if (!mc.IsPlaying("Block"))
			{
				mc.Play("Block");
			}
		}
		if (!Input.GetButton("Block") && !isBlocking && !Input.GetButton("Run") && !isAttacking && !isRolling)
		{
			if (playerHealth.stamina < playerHealth.maxStamina)
			{
				playerHealth.stamina += Time.deltaTime * 15f;
			}
			else
			{
				playerHealth.stamina = playerHealth.maxStamina;
			}
		}
		if (Input.GetButton("Run") && !isAttacking && direction != Vector3.zero && !isBlocking)
		{
			playerHealth.stamina -= Time.deltaTime * 10f;
		}
	}

	private void checkRoll()
	{
		float axisRaw = Input.GetAxisRaw("Vertical");
		if (axisRaw != 0f && !rollVert)
		{
			if (!isRolling && playerHealth.stamina >= 10f && Input.GetButton("Block"))
			{
				StartCoroutine(Rolling());
			}
			rollVert = true;
		}
		else if (axisRaw == 0f && rollVert)
		{
			rollVert = false;
		}
		axisRaw = Input.GetAxisRaw("Horizontal");
		if (axisRaw != 0f && !rollHorz)
		{
			if (!isRolling && playerHealth.stamina >= 10f && Input.GetButton("Block"))
			{
				StartCoroutine(Rolling());
			}
			rollHorz = true;
		}
		else if (axisRaw == 0f && rollHorz)
		{
			rollHorz = false;
		}
		if (!isfalling)
		{
		}
	}

	private IEnumerator AttackNow()
	{
		curAttack = 0;
		isAttacking = true;
		combo = false;
		while (isAttacking)
		{
			AttackMove attack = attacks[curAttack];
			mc.CrossFade(attack.animName, 0.2f);
			attack.sound.Play();
			for (int i = 0; i < attack.hitTimes.Length; i++)
			{
				yield return new WaitForSeconds(attack.hitTimes[i]);
				target.SendMessage("DoPlayerDamage", attack.damage, SendMessageOptions.DontRequireReceiver);
				if (i == 0 && !isBlocking)
				{
					GetComponent<Rigidbody>().AddForce(mc.transform.forward * (100f * attack.joltAmount), ForceMode.Force);
				}
			}
			yield return new WaitForSeconds(attack.finishDelay);
			StartCoroutine(DelayTime(0.3f));
			delay = true;
			combo = false;
			while (delay)
			{
				if (Input.GetButtonDown("Attack"))
				{
					combo = true;
				}
				yield return null;
			}
			if (combo && curAttack < attacks.Count - 1)
			{
				isAttacking = true;
				curAttack++;
			}
			else
			{
				isAttacking = false;
				curAttack = 0;
			}
		}
		yield return new WaitForEndOfFrame();
		isAttacking = false;
	}

	private IEnumerator DelayTime(float delayTime)
	{
		delay = true;
		yield return new WaitForSecondsRealtime(delayTime);
		delay = false;
	}

	private void DoDamage(int damage)
	{
		GetComponent<Rigidbody>().AddForce(-transform.forward * 100f, ForceMode.Force);
		if (isBlocking && playerHealth.stamina > (float)damage)
		{
			UnityEngine.Object.Instantiate(blockImpact, blockPos.position, blockPos.rotation);
			playerHealth.stamina -= damage;
			return;
		}
		if (isBlocking)
		{
			if (playerHealth.stamina >= 0f)
			{
				damage -= (int)playerHealth.stamina;
			}
			playerHealth.stamina = 0f;
		}
		playerHealth.health -= damage;
		StartCoroutine(HurtNow());
	}

	private IEnumerator HurtNow()
	{
		hurt = true;
		mc.Play("Hurt");
		StopCoroutine(AttackNow());
		yield return new WaitForSeconds(0.5f);
		hurt = false;
	}

	private IEnumerator Rolling()
	{
		isRolling = true;
		float vertical = Input.GetAxisRaw("Vertical");
		float horizontal = Input.GetAxisRaw("Horizontal");
		direction = transform.forward * vertical;
		direction += transform.right * horizontal;
		playerHealth.stamina -= 15f;
		mc.Play("Roll");
		while (mc.IsPlaying("Roll"))
		{
			yield return new WaitForEndOfFrame();
		}
		isRolling = false;
	}
}
