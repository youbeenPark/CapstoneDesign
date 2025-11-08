using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePortal : MonoBehaviour
{
    [Header("이 포탈이 여는 스테이지 이름")]
    public string stageName; // 예: "TUTO_Stage2"

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (StageProgressManager.IsStageUnlocked(stageName))
        {
            Debug.Log($"🚪 {stageName} 입장!");
            SceneManager.LoadScene(stageName);
        }
        else
        {
            Debug.Log($"🔒 {stageName}은 아직 잠겨 있습니다!");
        }
    }
}
