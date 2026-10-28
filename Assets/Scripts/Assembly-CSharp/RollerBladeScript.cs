using UnityEngine;

public class RollerBladeScript : MonoBehaviour
{
	private Animator animator;

	private CharacterController controller;

	public float speed = 6f;

	public float jumpSpeed = 8f;

	public float gravity = 20f;

	public float slowdownSpeed = 0.05f;

	public float brakeSpeed = 0.005f;

	public float forwardVelocity;

	private Vector3 moveDirection = Vector3.zero;

	private void Start()
	{
		animator = GetComponent<Animator>();
		controller = GetComponent<CharacterController>();
	}

	private void Update()
	{
		if (controller.isGrounded)
		{
			animator.SetFloat("Turn", Input.GetAxis("Horizontal"));
			animator.SetFloat("SkateForward", Input.GetAxis("Vertical"));
			animator.SetFloat("ForwardVelocity", forwardVelocity);
			if (Input.GetAxis("Vertical") < 0f && Input.GetButton("Vertical"))
			{
				animator.SetBool("Brake", true);
			}
			else
			{
				animator.SetBool("Brake", false);
			}
			if (Input.GetButtonDown("Vertical") || forwardVelocity < speed)
			{
				if (Input.GetAxis("Vertical") > 0f)
				{
					forwardVelocity += Input.GetAxis("Vertical");
				}
				else if (Input.GetAxis("Vertical") < 0f)
				{
					forwardVelocity += Input.GetAxis("Vertical") * brakeSpeed;
				}
				if (forwardVelocity > speed)
				{
					forwardVelocity = speed;
				}
				if (forwardVelocity < 0f)
				{
					forwardVelocity = 0f;
				}
			}
			if (!Input.GetButtonDown("Vertical"))
			{
				forwardVelocity -= slowdownSpeed;
				if (forwardVelocity < 0f)
				{
					forwardVelocity = 0f;
				}
			}
			moveDirection = new Vector3(0f, 0f, forwardVelocity);
			moveDirection = transform.TransformDirection(moveDirection);
			controller.transform.Rotate(0f, Input.GetAxis("Horizontal"), 0f);
			if (Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
}
