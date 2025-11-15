//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;

//public class PauseMenuController : MonoBehaviour
//{
//    [Header("Panels")]
//    public GameObject panelBackground;   // 어두운 배경
//    public GameObject panelWindow;       // ESC 누르면 뜨는 기본 설정창
//    public GameObject panelSetting;      // 옵션창
//    public GameObject panelHelp;         // 도움말창

//    private bool isPaused = false;

//    void Update()
//    {
//        // UI 클릭 중일 때 ESC 중복 입력 방지
//        if (EventSystem.current.IsPointerOverGameObject())
//            return;

//        if (Keyboard.current.escapeKey.wasPressedThisFrame)
//        {
//            HandleEsc();
//        }
//    }

//    // ========================================================
//    // ESC 동작 처리 (너가 원하는 정확한 구조로 구현됨)
//    // ========================================================
//    private void HandleEsc()
//    {
//        // 1) 옵션창 열려있으면 → 옵션창 닫고 설정창 다시 띄우기
//        if (panelSetting.activeSelf)
//        {
//            panelSetting.SetActive(false);
//            panelWindow.SetActive(true);
//            Debug.Log("[ESC] 옵션창 → 설정창으로 돌아감");
//            return;
//        }

//        // 2) 도움말창 열려있으면 → 도움말창 닫고 설정창 다시 띄우기
//        if (panelHelp.activeSelf)
//        {
//            panelHelp.SetActive(false);
//            panelWindow.SetActive(true);
//            Debug.Log("[ESC] 도움말창 → 설정창으로 돌아감");
//            return;
//        }

//        // 3) 설정창이 열려있다면 → 닫고 게임 재개
//        if (isPaused)
//        {
//            TogglePause(false);
//            Debug.Log("[ESC] 설정창 닫힘 → 게임 재개");
//            return;
//        }

//        // 4) 아무 패널도 없으면 → ESC 열기
//        TogglePause(true);
//        Debug.Log("[ESC] 설정창 열림");
//    }

//    // ========================================================
//    // Pause 메뉴 켜고 끄기
//    // ========================================================
//    private void TogglePause(bool pause)
//    {
//        isPaused = pause;
//        Time.timeScale = pause ? 0 : 1;

//        panelBackground.SetActive(pause);
//        panelWindow.SetActive(pause);

//        // ESC로 닫을 때 모든 서브창 닫기
//        if (!pause)
//        {
//            panelSetting.SetActive(false);
//            panelHelp.SetActive(false);
//        }
//    }

//    // ========================================================
//    // Button Functions
//    // ========================================================

//    // 게임 다시 시작
//    public void RestartStage()
//    {
//        Debug.Log("[Button] 다시 시작");
//        Time.timeScale = 1;
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }

//    // 옵션 열기
//    public void OpenSetting()
//    {
//        Debug.Log("[Button] 옵션 열기");
//        panelWindow.SetActive(false);
//        panelSetting.SetActive(true);
//    }

//    // 도움말 열기
//    public void OpenHelp()
//    {
//        Debug.Log("[Button] 도움말 열기");
//        panelWindow.SetActive(false);
//        panelHelp.SetActive(true);
//    }

//    // 메인 화면으로 이동
//    public void ReturnToMainMenu()
//    {
//        Debug.Log("[Button] 메인화면으로 이동");
//        Time.timeScale = 1;
//        SceneManager.LoadScene("MainMenu"); // ← 네 Main Menu 씬 이름으로 바꾸기
//    }

//    // 게임 종료
//    public void QuitGame()
//    {
//        Debug.Log("[Button] 게임 종료");
//        Application.Quit();
//    }
//}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelBackground;   // 어두운 배경
    public GameObject panelWindow;       // ESC 누르면 뜨는 기본 설정창
    public GameObject panelSetting;      // 옵션창
    public GameObject panelHelp;         // 도움말창

    private bool isPaused = false;

    void Update()
    {
        // UI 클릭 중일 때 ESC 중복 입력 방지
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleEsc();
        }
    }

    // ========================================================
    // ESC 동작 처리
    // ========================================================
    private void HandleEsc()
    {
        // 1) 옵션창 열려있으면 → 옵션창 닫고 설정창 띄우기
        if (panelSetting.activeSelf)
        {
            CloseSetting();
            return;
        }

        // 2) 도움말창 열려있으면 → 도움말창 닫고 설정창 띄우기
        if (panelHelp.activeSelf)
        {
            CloseHelp();
            return;
        }

        // 3) 설정창이 열려있다면 → 닫고 게임 재개
        if (isPaused)
        {
            TogglePause(false);
            Debug.Log("[ESC] 설정창 닫힘 → 게임 재개");
            return;
        }

        // 4) 아무 패널도 없으면 → ESC로 설정창 열기
        TogglePause(true);
        Debug.Log("[ESC] 설정창 열림");
    }

    // ========================================================
    // Pause 메뉴 열기/닫기
    // ========================================================
    private void TogglePause(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0 : 1;

        panelBackground.SetActive(pause);
        panelWindow.SetActive(pause);

        // ESC로 닫을 때 서브창 전부 닫기
        if (!pause)
        {
            panelSetting.SetActive(false);
            panelHelp.SetActive(false);
        }
    }

    // ========================================================
    // Button Functions
    // ========================================================

    // 게임 다시 시작
    public void RestartStage()
    {
        Debug.Log("[Button] 다시 시작");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 옵션 열기
    public void OpenSetting()
    {
        Debug.Log("[Button] 옵션 열기");
        panelWindow.SetActive(false);
        panelSetting.SetActive(true);
    }

    // 옵션 닫기  ← ★ 버튼/ESC 둘 다 사용 가능
    public void CloseSetting()
    {
        Debug.Log("[Button] 옵션 창 닫기");
        panelSetting.SetActive(false);
        panelWindow.SetActive(true);
    }

    // 도움말 열기
    public void OpenHelp()
    {
        Debug.Log("[Button] 도움말 열기");
        panelWindow.SetActive(false);
        panelHelp.SetActive(true);
    }

    // 도움말 닫기  ← ★ 버튼/ESC 둘 다 사용 가능
    public void CloseHelp()
    {
        Debug.Log("[Button] 도움말 창 닫기");
        panelHelp.SetActive(false);
        panelWindow.SetActive(true);
    }

    // 메인화면으로 이동
    public void ReturnToMainMenu()
    {
        Debug.Log("[Button] 메인화면으로 이동");
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");   // ← 씬 이름 맞게 변경하기
    }

    // 게임 종료
    public void QuitGame()
    {
        Debug.Log("[Button] 게임 종료");
        Application.Quit();
    }
}
