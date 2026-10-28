using UnityEngine;

namespace MagicalFX
{
	public class Wizard : MonoBehaviour
	{
		public GameObject[] Skills;

		private Vector3 positionLook;

		public int Index;

		public bool Showtime;

		public float Delay = 1f;

		public float RandomSize = 10f;

		public bool RandomSkill;

		private float timeTemp;

		private void Start()
		{
			timeTemp = Time.time;
		}

		private void Update()
		{
			if (Showtime)
			{
				if (Time.time >= timeTemp + Delay)
				{
					Ray ray = new Ray(transform.position + new Vector3(Random.Range(0f - RandomSize, RandomSize), 0f, Random.Range(0f - RandomSize, RandomSize)), -Vector3.up);
					RaycastHit hitInfo;
					if (Physics.Raycast(ray, out hitInfo, 100f))
					{
						positionLook = hitInfo.point;
					}
					Quaternion rotation = Quaternion.LookRotation((positionLook - transform.position).normalized);
					rotation.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
					transform.rotation = rotation;
					if (RandomSkill)
					{
						Index = Random.Range(0, Skills.Length);
					}
					else
					{
						Index++;
					}
					Deploy();
					timeTemp = Time.time;
				}
			}
			else
			{
				Aim();
				if (Input.GetButtonDown("Fire1"))
				{
					Deploy();
				}
			}
		}

		private void Deploy()
		{
			if (Index >= Skills.Length || Index < 0)
			{
				Index = 0;
			}
			FX_Position component = Skills[Index].GetComponent<FX_Position>();
			if ((bool)component)
			{
				if (component.Mode == SpawnMode.Static)
				{
					Place(Skills[Index]);
				}
				if (component.Mode == SpawnMode.OnDirection)
				{
					PlaceDirection(Skills[Index]);
				}
			}
			else
			{
				Shoot(Skills[Index]);
			}
		}

		private void Aim()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 100f))
			{
				positionLook = hitInfo.point;
			}
			Quaternion b = Quaternion.LookRotation((positionLook - transform.position).normalized);
			b.eulerAngles = new Vector3(0f, b.eulerAngles.y, 0f);
			transform.rotation = Quaternion.Lerp(transform.rotation, b, 0.5f);
		}

		private void Shoot(GameObject skill)
		{
			GameObject gameObject = Object.Instantiate(skill, transform.position + Vector3.up * 0.5f + transform.forward, skill.transform.rotation);
			gameObject.transform.forward = (positionLook - transform.position).normalized;
		}

		private void Place(GameObject skill)
		{
			GameObject gameObject = Object.Instantiate(skill, positionLook, skill.transform.rotation);
		}

		private void PlaceDirection(GameObject skill)
		{
			GameObject gameObject = Object.Instantiate(skill, transform.position + transform.forward, skill.transform.rotation);
			FX_Position component = gameObject.GetComponent<FX_Position>();
			if (component.Mode == SpawnMode.OnDirection)
			{
				component.transform.forward = transform.forward;
			}
		}

		private void OnGUI()
		{
			string empty = string.Empty;
			if (Index >= 0 && Index < Skills.Length && Skills.Length > 0)
			{
				empty = Skills[Index].name;
			}
			GUI.Label(new Rect(30f, 30f, Screen.width, 100f), string.Empty + empty);
			if (GUI.Button(new Rect(30f, Screen.height - 40, 100f, 30f), "Prev"))
			{
				Index--;
			}
			if (GUI.Button(new Rect(140f, Screen.height - 40, 100f, 30f), "Next"))
			{
				Index++;
			}
			if (GUI.Button(new Rect(250f, Screen.height - 40, 100f, 30f), "Show time"))
			{
				Showtime = !Showtime;
			}
			if (Index < 0)
			{
				Index = Skills.Length - 1;
			}
		}
	}
}
