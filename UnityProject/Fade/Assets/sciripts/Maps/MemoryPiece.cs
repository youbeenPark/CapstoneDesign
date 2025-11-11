

//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System.Collections;

//public class MemoryPiece : MonoBehaviour
//{
//    [Header("다음 스테이지 이름 (같은 에피소드 내)")]
//    public string nextStageName; // 예: "TUTO_Stage2"

//    [Header("다음 에피소드 첫 스테이지 (다음 섬의 Stage1)")]
//    public string nextEpisodeFirstStage; // 예: "RED_Stage1"

//    [Header("클리어 후 이동할 에피소드 맵 이름")]
//    public string episodeMapName; // 예: "Episode_TUTO"

//    private bool collected = false;

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (collected) return;
//        if (!other.CompareTag("Player")) return;

//        collected = true;
//        Debug.Log("✨ 기억 조각 획득!");

//        // 1️⃣ 같은 에피소드 내 다음 스테이지 해금
//        if (!string.IsNullOrEmpty(nextStageName))
//        {
//            StageProgressManager.UnlockStage(nextStageName);
//            Debug.Log($"🔓 {nextStageName} 해금 완료!");
//        }

//        // 2️⃣ 다음 에피소드 첫 스테이지 해금
//        if (!string.IsNullOrEmpty(nextEpisodeFirstStage))
//        {
//            StageProgressManager.UnlockStage(nextEpisodeFirstStage);
//            Debug.Log($"🌈 다음 에피소드 스테이지 해금 완료: {nextEpisodeFirstStage}");
//        }

//        // 3️⃣ 저장 보장
//        PlayerPrefs.Save();

//        // 4️⃣ 딜레이 후 에피소드 맵으로 이동
//        StartCoroutine(GoToEpisodeMapAfterDelay(0.2f));
//    }

//    private IEnumerator GoToEpisodeMapAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        if (!string.IsNullOrEmpty(episodeMapName))
//        {
//            SceneManager.LoadScene(episodeMapName);
//        }
//        else
//        {
//            Debug.LogWarning("⚠️ episodeMapName이 비어 있습니다!");
//        }
//    }
//}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MemoryPiece : MonoBehaviour
{
    [Header("다음 스테이지 이름 (같은 에피소드 내)")]
    public string nextStageName; // 예: "TUTO_Stage2"

    [Header("다음 에피소드 첫 스테이지 (다음 섬의 Stage1)")]
    public string nextEpisodeFirstStage; // 예: "GR_Stage1"

    [Header("클리어 후 이동할 에피소드 맵 이름")]
    public string episodeMapName; // 예: "Episode_TUTO"

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.CompareTag("Player")) return;
        collected = true;

        Debug.Log("✨ 기억 조각 획득!");

        // 1️⃣ 같은 에피소드 내 다음 스테이지 해금
        if (!string.IsNullOrEmpty(nextStageName))
        {
            StageProgressManager.UnlockStage(nextStageName);
            Debug.Log($"🔓 {nextStageName} 해금 완료!");
        }

        // 2️⃣ 다음 에피소드 첫 스테이지 해금
        if (!string.IsNullOrEmpty(nextEpisodeFirstStage))
        {
            StageProgressManager.UnlockStage(nextEpisodeFirstStage);
            Debug.Log($"🌈 다음 에피소드 스테이지 해금 완료: {nextEpisodeFirstStage}");
        }

        // 3️⃣ 전체 에피소드 클리어 여부 확인
        var controller = FindObjectOfType<EpisodeMapController>();
        if (controller != null)
        {
            string currentStage = SceneManager.GetActiveScene().name;
            controller.OnStageCleared(currentStage);
        }
        else
        {
            Debug.LogWarning("⚠️ EpisodeMapController를 찾을 수 없습니다!");
        }

        PlayerPrefs.Save();

        // 4️⃣ 딜레이 후 에피소드 맵으로 복귀
        StartCoroutine(GoToEpisodeMapAfterDelay(0.3f));
    }

    private IEnumerator GoToEpisodeMapAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!string.IsNullOrEmpty(episodeMapName))
        {
            SceneManager.LoadScene(episodeMapName);
        }
        else
        {
            Debug.LogWarning("⚠️ episodeMapName이 비어 있습니다!");
        }
    }
}
