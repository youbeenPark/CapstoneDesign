

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

//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System.Collections;

//public class MemoryPiece : MonoBehaviour
//{
//    [Header("다음 스테이지 이름 (같은 에피소드 내)")]
//    public string nextStageName; // 예: "TUTO_Stage2"

//    [Header("다음 에피소드 첫 스테이지 (다음 섬의 Stage1)")]
//    public string nextEpisodeFirstStage; // 예: "GR_Stage1"

//    [Header("클리어 후 이동할 에피소드 맵 이름")]
//    public string episodeMapName; // 예: "Episode_TUTO"

//    private bool collected = false;

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (collected || !other.CompareTag("Player")) return;
//        collected = true;

//        Debug.Log("✨ 기억 조각 획득!");

//        // 1️⃣ 같은 에피소드 내 다음 스테이지 해금
//        if (!string.IsNullOrEmpty(nextStageName))
//        {
//            StageProgressManager.UnlockStage(nextStageName);
//            Debug.Log($"🔓 {nextStageName} 해금 완료!");
//        }

//        // 2️⃣ 다음 에피소드 첫 스테이지 해금 + 에피소드 자체 해금
//        if (!string.IsNullOrEmpty(nextEpisodeFirstStage))
//        {
//            StageProgressManager.UnlockStage(nextEpisodeFirstStage);
//            Debug.Log($"🌈 다음 에피소드 스테이지 해금 완료: {nextEpisodeFirstStage}");

//            // ✅ 에피소드 단위 해금 추가
//            string nextWorld = nextEpisodeFirstStage.Split('_')[0]; // "GR"
//            PlayerPrefs.SetInt("Unlocked_" + nextWorld, 1);
//            PlayerPrefs.Save();
//            Debug.Log($"🏝️ {nextWorld} 에피소드 해금 완료!");
//        }

//        // 3️⃣ 스테이지 클리어 처리 (EpisodeMapController 연동)
//        var controller = FindObjectOfType<EpisodeMapController>();

//        if (controller != null)
//        {
//            string currentStage = SceneManager.GetActiveScene().name;
//            controller.OnStageCleared(currentStage);
//        }
//        else
//        {
//            Debug.LogWarning("⚠️ EpisodeMapController를 찾을 수 없습니다!");
//        }

//        PlayerPrefs.Save();

//        // 4️⃣ 딜레이 후 에피소드 맵으로 복귀
//        StartCoroutine(GoToEpisodeMapAfterDelay(0.3f));
//    }

//    private IEnumerator GoToEpisodeMapAfterDelay(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        if (!string.IsNullOrEmpty(episodeMapName))
//            SceneManager.LoadScene(episodeMapName);
//        else
//            Debug.LogWarning("⚠️ episodeMapName이 비어 있습니다!");
//    }
//}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryPiece : MonoBehaviour
{
    [Header("다음 스테이지 이름")]
    public string nextStageName;

    [Header("다음 에피소드 첫 스테이지 이름 (예: GR_Stage1)")]
    public string nextEpisodeFirstStage;

    [Header("다음 에피소드 월드 이름 (예: GR, YL 등)")]
    public string nextWorld;

    [Header("이동할 에피소드 맵 씬 이름 (예: EpisodeMap_GR)")]
    public string episodeMapName;

    [Header("획득 이펙트")]
    [SerializeField] private GameObject collectEffect;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;

        // ✅ 획득 이펙트 생성
        if (collectEffect != null)
            Instantiate(collectEffect, transform.position, Quaternion.identity);

        string currentStage = SceneManager.GetActiveScene().name;
        Debug.Log($"💎 기억 조각 획득! 현재 스테이지: {currentStage}");

        // 1️⃣ 다음 스테이지 해금
        if (!string.IsNullOrEmpty(nextStageName))
        {
            StageProgressManager.UnlockStage(nextStageName);
            Debug.Log($"🔓 다음 스테이지 해금: {nextStageName}");
        }

        // 2️⃣ 다음 에피소드 첫 스테이지 해금
        if (!string.IsNullOrEmpty(nextEpisodeFirstStage))
        {
            StageProgressManager.UnlockStage(nextEpisodeFirstStage);
            Debug.Log($"🔓 다음 에피소드 첫 스테이지 해금: {nextEpisodeFirstStage}");
        }

        // 3️⃣ 다음 월드 해금
        if (!string.IsNullOrEmpty(nextWorld))
        {
            PlayerPrefs.SetInt("Unlocked_" + nextWorld, 1);
            PlayerPrefs.Save();
            Debug.Log($"🌈 다음 월드 해금됨: {nextWorld}");
        }

        // 4️⃣ 현재 스테이지 클리어 처리
        var controller = FindObjectOfType<EpisodeMapController>();
        if (controller != null)
        {
            controller.OnStageCleared(currentStage);
            Debug.Log($"✅ EpisodeMapController에 클리어 전달: {currentStage}");
        }
        else
        {
            Debug.LogWarning("⚠️ EpisodeMapController를 찾을 수 없습니다. StageProgressManager로만 처리합니다.");
        }

        // ✅ StageProgressManager에도 저장
        StageProgressManager.ClearStage(currentStage);

        // ✅ 에피소드 단위로도 클리어 처리 (월드맵용)
        string episodePrefix = currentStage.Split('_')[0]; // 예: TUTO_Stage1 → TUTO
        PlayerPrefs.SetInt("Cleared_" + episodePrefix, 1);
        Debug.Log($"🎯 에피소드 단위 클리어 저장: Cleared_{episodePrefix} = 1");

        // ✅ 강제 저장
        PlayerPrefs.Save();

        // 5️⃣ 이동
        if (!string.IsNullOrEmpty(episodeMapName))
        {
            Debug.Log($"🚪 에피소드 맵으로 이동: {episodeMapName}");
            SceneManager.LoadScene(episodeMapName);
        }

        // 6️⃣ 자기 자신 제거
        Destroy(gameObject);
    }
}
