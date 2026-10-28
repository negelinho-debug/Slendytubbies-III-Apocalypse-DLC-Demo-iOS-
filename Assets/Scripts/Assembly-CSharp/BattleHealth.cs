using UnityEngine;

public class BattleHealth : MonoBehaviour
{
	public int health = 300;

	private int maxHealth;

	public float stamina = 100f;

	public float maxStamina = 100f;

	public RectTransform bar;

	public RectTransform staminaBar;

	private float barWidth;

	private float staminaBarWidth;

	private void Awake()
	{
		barWidth = 1f;
		maxHealth = health;
		maxStamina = stamina;
	}

	private void Update()
	{
		barWidth = (float)health / (float)maxHealth;
		bar.localScale = new Vector3(barWidth, 1f, 1f);
		if (barWidth < 0f)
		{
			bar.localScale = new Vector3(0f, 1f, 1f);
		}
		if (staminaBar != null)
		{
			staminaBarWidth = stamina / maxStamina;
			staminaBar.localScale = new Vector3(staminaBarWidth, 1f, 1f);
			if (staminaBarWidth < 0f)
			{
				staminaBar.localScale = new Vector3(0f, 1f, 1f);
				stamina = 0f;
			}
		}
	}
}
