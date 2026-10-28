using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
	public float speed;

	public float walkspeed;

	public float runSpeed;

	public float jumpSpeed;

	public float gravity;

	public bool pistol;

	private Vector3 moveDirection = Vector3.zero;

	public Vector3 movementSpeed = Vector3.zero;

	private CharacterController controller;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
	}

	private void Update()
	{
		transform.Rotate(0f, Input.GetAxis("Mouse X"), 0f);
		Pistol();
		if (controller.isGrounded)
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
			movementSpeed = moveDirection;
			if (Input.GetButton("Sprint"))
			{
				speed = Mathf.Lerp(speed, runSpeed, Time.deltaTime * 2f);
			}
			else
			{
				speed = Mathf.Lerp(speed, walkspeed, Time.deltaTime * 2f);
			}
			movementSpeed *= speed;
			animator.SetFloat("MovementX", movementSpeed.x);
			animator.SetFloat("MovementZ", movementSpeed.z);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
		}
		Jump();
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}

	private void Jump()
	{
		if (controller.isGrounded)
		{
			if (Input.GetAxis("Horizontal") == 0f && animator.GetFloat("MovementZ") > 1.3f)
			{
				if (Input.GetButton("Jump"))
				{
					animator.SetBool("Jump", true);
				}
				if (animator.GetFloat("JumpCurve") == 1f)
				{
					moveDirection.y = jumpSpeed;
				}
			}
			else
			{
				animator.SetBool("Jump", false);
			}
		}
		if (animator.GetFloat("Curve") > 0.3f)
		{
			animator.SetBool("Jump", false);
		}
	}

	private void Pistol()
	{
		if (Input.GetButtonDown("Pistol"))
		{
			pistol = true;
		}
		else if (Input.GetButtonDown("Unarmed"))
		{
			pistol = false;
		}
		if (pistol)
		{
			if (animator.GetLayerWeight(1) < 0.999999f)
			{
				animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 3f));
			}
		}
		else if (animator.GetLayerWeight(1) > 1E-06f)
		{
			animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 3f));
		}
	}
}
