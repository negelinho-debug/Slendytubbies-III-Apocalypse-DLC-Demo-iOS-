using UnityEngine;

namespace MagicalFX
{
	public class FX_ParticlePreview : MonoBehaviour
	{
		public GameObject[] Particles;

		public float RotationSpeed = 3f;

		public int Index;

		public Texture2D logo;

		private void Start()
		{
		}

		public void AddParticle(Vector3 position)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				Index++;
				if (Index >= Particles.Length || Index < 0)
				{
					Index = 0;
				}
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				Index--;
				if (Index < 0)
				{
					Index = Particles.Length - 1;
				}
			}
			if (Index >= Particles.Length || Index < 0)
			{
				Index = 0;
			}
			if (Index >= 0 && Index < Particles.Length && Particles.Length > 0)
			{
				Object.Instantiate(Particles[Index], position, Particles[Index].transform.rotation);
			}
		}

		private void Update()
		{
			transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
			RaycastHit hitInfo = default(RaycastHit);
			if (Input.GetButtonDown("Fire1"))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hitInfo, 1000f))
				{
					AddParticle(hitInfo.point + Vector3.up);
				}
			}
		}

		private void OnGUI()
		{
			string empty = string.Empty;
			if (Index >= 0 && Index < Particles.Length && Particles.Length > 0)
			{
				empty = Particles[Index].name;
			}
			GUI.Label(new Rect(30f, 30f, Screen.width, 100f), "Change FX : Key Up / Down \nCurrent FX " + empty);
			if (GUI.Button(new Rect(30f, 90f, 200f, 30f), "Next"))
			{
				Index++;
				AddParticle(Vector3.up);
			}
			if (GUI.Button(new Rect(30f, 130f, 200f, 30f), "Prev"))
			{
				Index--;
				AddParticle(Vector3.up);
			}
			if ((bool)logo)
			{
				GUI.DrawTexture(new Rect(Screen.width - logo.width - 30, 30f, logo.width, logo.height), logo);
			}
		}
	}
}
