using AC;
using UnityEngine;

public class playerFPS : MonoBehaviour
{
	public Transform model;

	public Transform playerHead;

	private float speed;

	public float walkSpeed = 2f;

	public float runSpeed = 4.5f;

	public AudioClip nvOn;

	public AudioClip nvOff;

	public float playerSize = 2f;

	public Transform nvCam;

	public bool enableNVOnStart;

	private Vector3 moveDir;

	public bool isCrouching;

	private float crouchTimer = 1f;

	private void LateUpdate()
	{
		if (KickStarter.player.isCutscene())
		{
			return;
		}
		if (Input.GetButton("Run"))
		{
			speed = runSpeed;
		}
		else
		{
			speed = walkSpeed;
		}
		if (!isCrouching)
		{
			gameObject.transform.localScale = new Vector3(playerSize, crouchTimer, playerSize);
			if (crouchTimer < playerSize)
			{
				crouchTimer += 4f * Time.deltaTime;
			}
			else
			{
				crouchTimer = playerSize;
			}
		}
		else
		{
			if (crouchTimer > playerSize / 2f)
			{
				crouchTimer -= 4f * Time.deltaTime;
			}
			else
			{
				crouchTimer = playerSize / 2f;
			}
			gameObject.transform.localScale = new Vector3(playerSize, crouchTimer, playerSize);
			speed = walkSpeed / 2f;
		}
		if (Input.GetButton("Crouch"))
		{
			isCrouching = true;
		}
		else if (!Physics.BoxCast(transform.position, new Vector3(0.21f, 1f, 0.21f), Vector3.up, transform.rotation, playerSize / 1.5f))
		{
			isCrouching = false;
		}
		if (Time.timeScale != 0f)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		moveDir.x = Input.GetAxis("Horizontal");
		moveDir.z = Input.GetAxis("Vertical");
		if (moveDir.z < 0f)
		{
			moveDir.z /= 2f;
		}
		transform.Translate(moveDir * Time.deltaTime * speed);
		if (moveDir.x != 0f || moveDir.z != 0f)
		{
			if (!GetComponent<Player>().soundChild.GetComponent<AudioSource>().isPlaying)
			{
				if (Input.GetButton("Run") && !Input.GetButton("Crouch") && !isCrouching)
				{
					GetComponent<Player>().soundChild.GetComponent<AudioSource>().clip = GetComponent<Player>().runSound;
				}
				else
				{
					GetComponent<Player>().soundChild.GetComponent<AudioSource>().clip = GetComponent<Player>().walkSound;
				}
				GetComponent<Player>().soundChild.GetComponent<AudioSource>().Play();
			}
		}
		else if (GetComponent<Player>().soundChild.GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<Player>().soundChild.GetComponent<AudioSource>().Stop();
		}
		if (Input.GetButtonDown("NightVision"))
		{
			GameObject.FindWithTag("MainCamera").GetComponent<CameraGlitch>().enabled = !GameObject.FindWithTag("MainCamera").GetComponent<CameraGlitch>().enabled;
			if (GameObject.FindWithTag("MainCamera").GetComponent<CameraGlitch>().enabled)
			{
				transform.Find("FPS/FPS_Camera").GetComponent<AudioSource>().clip = nvOn;
				transform.Find("FPS/FPS_Camera").GetComponent<AudioSource>().Play();
			}
			else
			{
				transform.Find("FPS/FPS_Camera").GetComponent<AudioSource>().clip = nvOff;
				transform.Find("FPS/FPS_Camera").GetComponent<AudioSource>().Play();
			}
		}
	}

	private bool IsGrounded()
	{
		return Physics.Raycast(transform.position, -Vector3.up, 0.1f);
	}

	private void OnEnable()
	{
		model.gameObject.SetActive(false);
		playerHead.gameObject.SetActive(false);
		GetComponent<Player>().enabled = false;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		GetComponent<MouseLook>().enabled = true;
		if (enableNVOnStart)
		{
			GameObject.FindWithTag("MainCamera").GetComponent<CameraGlitch>().enabled = true;
		}
		if (GameObject.FindWithTag("MainCamera").GetComponent<CameraGlitch>().enabled)
		{
			transform.Find("FPS/FPS_Camera").GetComponent<AudioSource>().clip = nvOn;
			transform.Find("FPS/FPS_Camera").GetComponent<AudioSource>().Play();
		}
		if (nvCam != null)
		{
			nvCam.gameObject.SetActive(false);
		}
		if (isCrouching)
		{
			crouchTimer = playerSize / 2f;
			gameObject.transform.localScale = new Vector3(playerSize, playerSize / 2f, playerSize);
			speed = walkSpeed / 2f;
		}
	}

	private void OnDisable()
	{
		model.gameObject.SetActive(true);
		playerHead.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		GetComponent<Player>().enabled = true;
		GetComponent<MouseLook>().enabled = false;
		Camera.main.GetComponent<CameraGlitch>().enabled = false;
		gameObject.transform.localScale = new Vector3(playerSize, playerSize, playerSize);
		if (nvCam != null)
		{
			nvCam.gameObject.SetActive(true);
		}
	}

	private void Awake()
	{
		playerSize = transform.localScale.y;
		crouchTimer = playerSize;
		Application.targetFrameRate = 60;
	}
}
