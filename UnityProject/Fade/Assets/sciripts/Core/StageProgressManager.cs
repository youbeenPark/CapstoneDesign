using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 각 스테이지(Scene)의 진행 상태를 관리.
/// 기억조각을 먹으면 클리어 처리 + 다음 스테이지 해금 + 마지막 스테이지면 에피소드 클리어.
/// </summary>
public class StageProgressManager : MonoBehaviour
{
    [Header("Stage Info")]
    [SerializeField] private string worldName;     // 예: "TUTO"
    [SerializeField] private int stageIndex;       // 예: 1, 2, 3, 4
    [SerializeField] private string memoryItemTag = "MemoryPiece"; // 기억조각 태그명

    private bool cleared = false;

    private void Start()
    {
        if (IsStageCleared())
            Debug.Log($"{worldName}_Stage{stageIndex} 이미 클리어됨");
    }

    private void Update()
    {
        // 테스트용 강제 클리어 키
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("테스트: 강제로 클리어 처리");
            ClearStage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(memoryItemTag))
        {
            Destroy(other.gameObject);
            ClearStage();
        }
    }

    /// <summary>
    /// 현재 스테이지 클리어 여부 확인
    /// </summary>
    public bool IsStageCleared()
    {
        return PlayerPrefs.GetInt($"{worldName}_Stage{stageIndex}_Cleared", 0) == 1;
    }

    /// <summary>
    /// 현재 스테이지 클리어 처리
    /// </summary>
    public void ClearStage()
    {
        if (cleared) return;
        cleared = true;

        string stageKey = $"{worldName}_Stage{stageIndex}_Cleared";
        PlayerPrefs.SetInt(stageKey, 1);

        Debug.Log($"스테이지 클리어: {worldName}_Stage{stageIndex}");

        UnlockNextStage();
        PlayerPrefs.Save();

        if (IsLastStage())
        {
            GameProgressManager.Instance.MarkWorldCleared(worldName);
            Debug.Log($"{worldName} 에피소드 클리어!");
        }

        // 일정 시간 후 에피소드 맵으로 복귀
        Invoke(nameof(ReturnToEpisodeMap), 1.5f);
    }

    /// <summary>
    /// 다음 스테이지 해금
    /// </summary>
    private void UnlockNextStage()
    {
        int nextIndex = stageIndex + 1;
        string nextKey = $"{worldName}_Stage{nextIndex}_Unlocked";

        if (PlayerPrefs.GetInt(nextKey, 0) == 0)
        {
            PlayerPrefs.SetInt(nextKey, 1);
            Debug.Log($"다음 스테이지 해금: {worldName}_Stage{nextIndex}");
        }
    }

    /// <summary>
    /// 현재 스테이지가 마지막인지 확인
    /// </summary>
    private bool IsLastStage()
    {
        // 스테이지 수는 프로젝트마다 다를 수 있으니, 기본은 4개 기준
        return stageIndex >= 4;
    }

    /// <summary>
    /// 에피소드 맵으로 돌아가기
    /// </summary>
    private void ReturnToEpisodeMap()
    {
        string episodeSceneName = $"{worldName}_EpisodeMap";
        Debug.Log($"에피소드 맵으로 복귀: {episodeSceneName}");
        SceneManager.LoadScene(episodeSceneName);
    }
}
