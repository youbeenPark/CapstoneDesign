using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject); // 씬 넘어가도 유지
    }

    // 임시: 게임 시작 시 Stage1로 자동 이동
    void Start()
    {
        LoadScene("OR_01"); // 씬 이름은 네가 만든 이름과 동일해야 함
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
