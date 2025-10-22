using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager I;

    void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;

        // 씬 전환 시 파괴되지 않도록 설정 (MainMenu 씬에서 다음 씬으로 넘어갈 때 유지)
        DontDestroyOnLoad(gameObject);
    }

    // ⭐ 시작하기 버튼에 연결될 씬 로드 함수 ⭐
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // ⭐ 새로 추가된 게임 종료 함수 ⭐
    public void QuitGame()
    {
        Debug.Log("GameManager: 게임 종료 요청됨.");

#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 때만
        // 유니티 에디터를 종료하는 코드는 UnityEditor 네임스페이스에 있으므로,
        // 이 코드는 빌드 시 포함되지 않습니다.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 게임(PC, 모바일 등)에서 실행 중일 때만
        Application.Quit();
#endif
    }
}