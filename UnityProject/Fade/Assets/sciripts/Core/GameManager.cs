using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    // ⭐ 씬 이름 변수들: Inspector에서 설정 ⭐
    [Header("Scene Names")]
    [Tooltip("게임을 처음 시작할 때 로드할 씬 이름 (튜토리얼)")]
    public string tutorialSceneName = "Tutorial_World";
    [Tooltip("튜토리얼 완료 후 로드할 큰 맵 씬 이름")]
    public string mainGameSceneName = "Main_World";

    // PlayerPrefs에 저장할 키
    private const string TUTORIAL_COMPLETED_KEY = "TutorialCompleted";

    void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    // 씬 이름으로 로드하는 기존 함수
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // ⭐ '시작하기' 버튼에 연결될 조건부 씬 로드 함수 ⭐
    public void StartGame()
    {
        // PlayerPrefs에서 튜토리얼 완료 기록(1) 또는 기록 없음(0)을 확인합니다.
        int tutorialCompleted = PlayerPrefs.GetInt(TUTORIAL_COMPLETED_KEY, 0);

        if (tutorialCompleted == 0)
        {
            // 1. 기록 없음 (처음 플레이): Inspector에서 설정된 튜토리얼 씬 로드
            Debug.Log("GameManager: 첫 플레이 감지. 튜토리얼 씬 로드.");
            LoadScene(tutorialSceneName);
        }
        else
        {
            // 2. 기록 있음 (이전에 플레이함): Inspector에서 설정된 메인 맵 씬 로드
            Debug.Log("GameManager: 기록 존재. 메인 맵 씬 로드.");
            LoadScene(mainGameSceneName);
        }
    }

    // 🔔 튜토리얼 완료 시 호출해야 하는 함수 (기록 남기기)
    // 튜토리얼 씬의 끝 지점(예: 씬 전환 직전)에서 이 함수를 호출해야 합니다.
    public void CompleteTutorial()
    {
        // 튜토리얼 완료 기록을 1로 저장
        PlayerPrefs.SetInt(TUTORIAL_COMPLETED_KEY, 1);
        PlayerPrefs.Save();
        Debug.Log("Tutorial progress saved.");
    }

    // 게임 종료 함수
    public void QuitGame()
    {
        Debug.Log("GameManager: 게임 종료 요청됨.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}