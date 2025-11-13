using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panelBackground;
    [SerializeField] private GameObject panelWindow;

    private bool isPaused = false;

    void Start()
    {
        // 시작 시 메뉴 꺼두기
        panelBackground.SetActive(false);
        panelWindow.SetActive(false);
    }

    void Update()
    {
        // ESC 키 입력 감지 (Input System)
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        panelBackground.SetActive(isPaused);
        panelWindow.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    // 버튼에서 "Resume" 연결용
    public void ResumeGame()
    {
        isPaused = false;
        panelBackground.SetActive(false);
        panelWindow.SetActive(false);
        Time.timeScale = 1f;
    }
}
