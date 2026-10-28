using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMechanics : MonoBehaviour
{
	[Serializable]
	public class AttackMove
	{
		public string animName;

		public float[] hitTimes;

		public int damage;

		public float minDistance;

		public bool setParent;

		public Transform spawnObj;

		public Transform spawnPos;

		public float delayBeforeAttack;

		public string delayAnim;

		public bool lookAtTarget;

		public float hitDistance = 2.5f;

		public float joltAmount;

		public AudioClip audioClip;
	}

	[Serializable]
	public class MoveToPoint
	{
		public Transform point;

		public int healthRequired;

		public int attackMove = -1;

		public AttackMove customAttackMove;

		public bool hasMovedTo;
	}

	public Transform target;

	public MecanimControl mc;

	public BattleHealth bossHealth;

	public float walkSpeed;

	public float runSpeed;

	public bool turnWhenMoving;

	public int curAttack = -1;

	public float delayBetweenAttacks = 2f;

	public int attackChance = 2;

	public Transform[] hurtObj;

	public Transform hitAlert;

	public Transform hitAlertPos;

	public List<AttackMove> attacks;

	private bool isMovingToPoint;

	public List<MoveToPoint> movePoints;

	public Transform lookBone;

	public Vector3 lookAtRots;

	private bool attemptAttack;

	private bool isAttacking;

	private bool finishedAttacking = true;

	private bool checkMoving;

	private Vector3 randomDirection;

	private Vector3 movementDirection;

	private string randomAnim = "Idle";

	private float randomlyTimer;

	private int random;

	public bool disableBack;

	private float minimumDistance = 2f;

	public float damageResistance = 1f;

	private bool hurt;

	private void Awake()
	{
	}

	private void Update()
	{
		if (!attemptAttack && !isMovingToPoint)
		{
			StartCoroutine(Attack());
		}
		if (!isAttacking && !hurt && !isMovingToPoint)
		{
			MoveRandomly();
		}
		else if (turnWhenMoving)
		{
			mc.transform.rotation = Quaternion.Slerp(mc.transform.rotation, Quaternion.LookRotation(transform.forward), Time.deltaTime * 3f);
		}
		if (!isMovingToPoint && !checkMoving)
		{
			StartCoroutine(CheckMoveToPoint());
		}
	}

	private IEnumerator CheckMoveToPoint()
	{
		checkMoving = true;
		for (int i = 0; i < movePoints.Count; i++)
		{
			if (bossHealth.health >= movePoints[i].healthRequired || movePoints[i].hasMovedTo)
			{
				continue;
			}
			isMovingToPoint = true;
			while (attemptAttack || hurt)
			{
				yield return new WaitForEndOfFrame();
			}
			while (Vector3.Distance(transform.position, movePoints[i].point.position) > 1f)
			{
				Vector3 targetPosition = movePoints[i].point.position;
				targetPosition.y = transform.position.y;
				SmoothLookAt(targetPosition);
				if (!mc.IsPlaying("Run"))
				{
					mc.CrossFade("Run", 0.2f);
				}
				transform.position += transform.forward * Time.deltaTime * (runSpeed + 2f);
				movementDirection = transform.forward;
				yield return new WaitForEndOfFrame();
			}
			if (movePoints[i].attackMove > -1)
			{
				StartCoroutine(Attack(attacks[movePoints[i].attackMove]));
				yield return new WaitForEndOfFrame();
			}
			else
			{
				StartCoroutine(Attack(movePoints[i].customAttackMove));
				yield return new WaitForEndOfFrame();
			}
			movePoints[i].hasMovedTo = true;
		}
		isMovingToPoint = false;
		yield return new WaitForEndOfFrame();
		checkMoving = false;
	}

	private void LateUpdate()
	{
		if (lookBone != null)
		{
			Vector3 localEulerAngles = lookBone.localEulerAngles;
			lookBone.LookAt(target);
			Vector3 localEulerAngles2 = lookBone.localEulerAngles;
			Vector3 localEulerAngles3 = localEulerAngles;
			if (lookAtRots.x != 0f)
			{
				localEulerAngles3.x = localEulerAngles2.x * lookAtRots.x;
			}
			if (lookAtRots.y != 0f)
			{
				localEulerAngles3.y = localEulerAngles2.y * lookAtRots.y;
			}
			if (lookAtRots.z != 0f)
			{
				localEulerAngles3.z = localEulerAngles2.z * lookAtRots.z;
			}
			lookBone.localEulerAngles = localEulerAngles3;
		}
	}

	private void MoveRandomly()
	{
		if (!turnWhenMoving)
		{
			Vector3 position = target.position;
			position.y = transform.position.y;
			SmoothLookAt(position);
		}
		randomlyTimer += Time.deltaTime;
		if (randomlyTimer > 1f)
		{
			random = UnityEngine.Random.Range(0, 5);
			randomlyTimer = 0f;
		}
		if (random == 1 || random == 4)
		{
			if (!disableBack)
			{
				randomDirection = -transform.forward;
				randomAnim = "WalkBack";
				movementDirection = transform.forward;
			}
			else
			{
				random = 0;
			}
		}
		if (random == 2)
		{
			randomDirection = -transform.right;
			randomAnim = "Left";
			movementDirection = randomDirection;
		}
		if (random == 3)
		{
			randomDirection = transform.right;
			randomAnim = "Right";
			movementDirection = randomDirection;
		}
		if (random == 0 || random > 4)
		{
			randomDirection = Vector3.zero;
			randomAnim = "Idle";
			movementDirection = Vector3.zero;
		}
		if (!mc.IsPlaying(randomAnim))
		{
			mc.Play(randomAnim);
		}
		transform.position += randomDirection * Time.deltaTime * walkSpeed;
		if (turnWhenMoving && movementDirection != Vector3.zero)
		{
			mc.transform.rotation = Quaternion.Slerp(mc.transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * 3f);
		}
		float maxDistance = GetComponent<CapsuleCollider>().radius + 0.1f;
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, randomDirection, out hitInfo, maxDistance))
		{
			random = 0;
		}
	}

	private IEnumerator Attack(AttackMove hasMove = null)
	{
		attemptAttack = true;
		finishedAttacking = false;
		int chance = 0;
		if (hasMove == null)
		{
			yield return new WaitForSeconds(delayBetweenAttacks);
			chance = UnityEngine.Random.Range(0, attackChance);
		}
		if (chance == 0)
		{
			if (hitAlert != null)
			{
				Transform transform = UnityEngine.Object.Instantiate(hitAlert, hitAlertPos.position, hitAlertPos.rotation);
				transform.parent = hitAlertPos;
			}
			isAttacking = true;
			AttackMove move = ((hasMove != null) ? hasMove : attacks[UnityEngine.Random.Range(0, attacks.Count)]);
			finishedAttacking = false;
			StartCoroutine(Move(move));
			while (!finishedAttacking)
			{
				if (move.lookAtTarget)
				{
					Vector3 position = target.position;
					position.y = base.transform.position.y;
					SmoothLookAt(position);
				}
				yield return new WaitForEndOfFrame();
			}
		}
		attemptAttack = false;
		isAttacking = false;
	}

	private IEnumerator Move(AttackMove move)
	{
		while (Vector3.Distance(base.transform.position, target.position) > move.minDistance)
		{
			Vector3 targetPosition = target.position;
			targetPosition.y = base.transform.position.y;
			SmoothLookAt(targetPosition);
			if (!mc.IsPlaying("Run"))
			{
				mc.CrossFade("Run", 0.2f);
			}
			base.transform.position += base.transform.forward * Time.deltaTime * runSpeed;
			movementDirection = base.transform.forward;
			yield return new WaitForEndOfFrame();
		}
		GetComponent<Rigidbody>().AddForce(mc.transform.forward * (100f * move.joltAmount), ForceMode.Force);
		bool[] hasChecked = new bool[move.hitTimes.Length];
		if (move.delayBeforeAttack > 0f)
		{
			if (move.delayAnim != string.Empty)
			{
				mc.CrossFade(move.delayAnim, 0.2f);
			}
			yield return new WaitForSecondsRealtime(move.delayBeforeAttack);
		}
		mc.CrossFade(move.animName, 0.2f);
		while (mc.IsPlaying(move.animName))
		{
			for (int i = 0; i < move.hitTimes.Length; i++)
			{
				if (hasChecked[i] || !(mc.GetCurrentClipTime() > move.hitTimes[i]))
				{
					continue;
				}
				if (move.spawnObj != null)
				{
					Transform transform = UnityEngine.Object.Instantiate(move.spawnObj, move.spawnPos.position, move.spawnPos.rotation);
					if (move.setParent)
					{
						transform.parent = base.transform;
					}
				}
				if (Vector3.Distance(target.transform.position, base.transform.position) <= move.hitDistance && IsLookingAtObject(base.transform, target))
				{
					target.SendMessage("DoDamage", move.damage, SendMessageOptions.DontRequireReceiver);
				}
				if (move.audioClip != null && GetComponent<AudioSource>() != null)
				{
					GetComponent<AudioSource>().clip = move.audioClip;
					GetComponent<AudioSource>().Play();
				}
				hasChecked[i] = true;
				if (move.hitTimes.Length > 1 && i < move.hitTimes.Length - 1 && hitAlert != null)
				{
					Transform transform2 = UnityEngine.Object.Instantiate(hitAlert, hitAlertPos.position, hitAlertPos.rotation);
					transform2.parent = hitAlertPos;
				}
			}
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForEndOfFrame();
		finishedAttacking = true;
		mc.CrossFade("Idle", 0.2f);
	}

	private void DoPlayerDamage(int damage)
	{
		Vector3 position = GameObject.FindGameObjectWithTag("Player").transform.position;
		position.y += 0.5f;
		SkinnedMeshRenderer[] componentsInChildren = mc.GetComponentsInChildren<SkinnedMeshRenderer>();
		Transform[] bones = componentsInChildren[0].bones;
		Transform transform = null;
		for (int i = 0; i < bones.Length; i++)
		{
			if (Vector3.Distance(position, bones[i].position) < minimumDistance && transform == null)
			{
				transform = bones[i];
			}
		}
		if (transform != null)
		{
			bossHealth.health -= (int)((float)damage / damageResistance);
			StartCoroutine(HurtNow());
			UnityEngine.Object.Instantiate(hurtObj[UnityEngine.Random.Range(0, hurtObj.Length)], transform.position, transform.rotation);
		}
	}

	private void DoDamage(int damage)
	{
		bossHealth.health -= (int)((float)damage / damageResistance);
		StartCoroutine(HurtNow());
		UnityEngine.Object.Instantiate(hurtObj[UnityEngine.Random.Range(0, hurtObj.Length)], transform.position, transform.rotation);
	}

	private IEnumerator HurtNow()
	{
		hurt = true;
		if (!isAttacking && !finishedAttacking)
		{
			mc.CrossFade("Hurt", 0.2f);
			GetComponent<Rigidbody>().AddForce(-transform.forward * 100f, ForceMode.Force);
		}
		yield return new WaitForSeconds(0.5f);
		hurt = false;
	}

	private void SmoothLookAt(Vector3 targetPosition)
	{
		Quaternion b = Quaternion.LookRotation(targetPosition - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, b, 5f * Time.deltaTime);
	}

	private bool IsLookingAtObject(Transform looker, Transform targetPos)
	{
		Vector3 normalized = (targetPos.position - looker.position).normalized;
		float num = Vector3.Dot(normalized, looker.forward);
		if ((double)num > 0.75)
		{
			return true;
		}
		return false;
	}
}
