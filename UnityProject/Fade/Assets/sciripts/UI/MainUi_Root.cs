using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyRoot : MonoBehaviour
{
    [Header("UI 전체(Canvas) 오브젝트")]
    public GameObject canvasRoot;   // UI_Root > Canvas

    [Header("하트 패널(HeartPanel) 오브젝트")]
    public GameObject heartPanel;   // UI_Root > Canvas > HeartPanel

    void Awake()
    {
        // UI_Root 전체를 씬 이동해도 유지
        DontDestroyOnLoad(gameObject);
    }

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
        string name = scene.name;

        // ★ 스테이지 씬에서만 UI 켜기
        if (name.Contains("Stage"))
        {
            if (canvasRoot != null) canvasRoot.SetActive(true);
            if (heartPanel != null) heartPanel.SetActive(true);
        }
        else
        {
            // ★ 에피소드맵, 메인메뉴 등 스테이지가 아닐 경우 UI 끄기
            if (canvasRoot != null) canvasRoot.SetActive(false);
        }
    }
}
