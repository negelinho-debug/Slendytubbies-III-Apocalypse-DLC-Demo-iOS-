using UnityEngine;

namespace MagicalFX
{
	public class RaisingWall : MonoBehaviour
	{
		public GameObject Skill;

		public float Offset = -7f;

		public float Distance = 2f;

		private void Start()
		{
			if (Skill != null)
			{
				FX_SpawnDirection component = Skill.GetComponent<FX_SpawnDirection>();
				if ((bool)component)
				{
					Offset = -(int)((float)component.Number / 2f);
				}
			}
			Raising();
		}

		private void Raising()
		{
			if (Skill != null)
			{
				GameObject gameObject = Object.Instantiate(Skill, transform.position + transform.forward * Distance + transform.right * Offset, Skill.transform.rotation);
				gameObject.transform.forward = transform.right;
			}
		}
	}
}
