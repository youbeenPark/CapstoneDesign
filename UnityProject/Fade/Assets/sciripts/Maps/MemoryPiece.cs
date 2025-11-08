using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryFragment : MonoBehaviour
{
    [Header("다음 스테이지 이름")]
    public string nextStageName;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;
        Debug.Log("✨ 기억 조각 획득!");

        // 다음 스테이지 해금
        if (!string.IsNullOrEmpty(nextStageName))
        {
            StageProgressManager.UnlockStage(nextStageName);
        }

        // 현재 스테이지 클리어 처리
        StageClear();
    }

    private void StageClear()
    {
        Debug.Log("🌈 스테이지 클리어!");
        if (!string.IsNullOrEmpty(nextStageName))
            SceneManager.LoadScene(nextStageName);
        else
            Debug.LogWarning("⚠️ 다음 스테이지 이름이 설정되지 않았습니다.");
    }
}
