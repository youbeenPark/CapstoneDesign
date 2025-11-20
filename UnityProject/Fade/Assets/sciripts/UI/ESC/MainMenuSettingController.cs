using UnityEngine;

public class MainMenuSettingController : MonoBehaviour
{
    public GameObject pauseMenuMinimal;
    public GameObject panelPauseWindow;
    public GameObject panelSetting;
    public GameObject panelHelp;

    public void OpenSetting()
    {
        Debug.Log("설정창 켜기!");

        pauseMenuMinimal.SetActive(true);   // ESC 전체 UI 켜기
        panelPauseWindow.SetActive(true);   // 기본 윈도우 켜기 (중요!!)
        panelSetting.SetActive(true);       // Setting 패널 켜기
        panelHelp.SetActive(false);         // 도움말 끔
    }
}