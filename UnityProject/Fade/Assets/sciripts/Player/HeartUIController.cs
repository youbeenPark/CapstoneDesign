using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartUIController : MonoBehaviour
{
    [Header("하트 패널 UI 오브젝트")]
    public GameObject heartPanel;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        // 스테이지 씬에서만 하트 표시
        if (sceneName.Contains("_Stage"))
            heartPanel.SetActive(true);
        else
            heartPanel.SetActive(false);
    }
}
