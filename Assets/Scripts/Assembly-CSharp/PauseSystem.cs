using UnityEngine;

public class PauseSystem : MonoBehaviour
{
	public RectTransform pauseMenu;

	private bool curVisibleMouse;

	private CursorLockMode curLockMouse;

	private bool isPaused;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!isPaused)
			{
				PauseGame();
			}
			else
			{
				UnpauseGame();
			}
		}
	}

	private void PauseGame()
	{
		curVisibleMouse = Cursor.visible;
		curLockMouse = Cursor.lockState;
		Time.timeScale = 0f;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		pauseMenu.gameObject.SetActive(true);
		isPaused = true;
	}

	private void UnpauseGame()
	{
		Cursor.lockState = curLockMouse;
		Cursor.visible = curVisibleMouse;
		Time.timeScale = 1f;
		pauseMenu.gameObject.SetActive(false);
		isPaused = false;
	}
}
