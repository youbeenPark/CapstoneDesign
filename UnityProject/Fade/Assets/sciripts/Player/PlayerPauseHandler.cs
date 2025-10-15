using UnityEngine;

public class PlayerPauseHandler : MonoBehaviour
{
    private PlayerControls controls;
    private bool isPaused = false;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Pause.performed += _ => TogglePause();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        Debug.Log(isPaused ? "Game Paused" : "Game Resumed");
    }
}
