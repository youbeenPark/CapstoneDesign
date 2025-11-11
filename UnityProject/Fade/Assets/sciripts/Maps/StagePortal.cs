//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System.Collections;

//[RequireComponent(typeof(SpriteRenderer))]
//public class StagePortal : MonoBehaviour
//{
//    [Header("이 포탈이 여는 스테이지 이름")]
//    public string stageName; // 예: "TUTO_Stage1", "GR_Stage2"

//    [Header("시각효과 설정")]
//    public Color unlockedColor = Color.white;
//    public Color lockedColor = new Color(0.4f, 0.4f, 0.4f, 0.6f);
//    public float glowIntensity = 1.2f;

//    private bool isUnlocked = false;
//    private bool isPlayerInRange = false;
//    private bool isNextStage = false;
//    private SpriteRenderer spriteRenderer;
//    private float glowTimer = 0f;

//    private void Start()
//    {
//        // 🔹 자식 오브젝트에 SpriteRenderer 있을 경우도 커버
//        spriteRenderer = GetComponent<SpriteRenderer>() ?? GetComponentInChildren<SpriteRenderer>();
//        StartCoroutine(InitAfterDelay());
//    }

//    private IEnumerator InitAfterDelay()
//    {
//        yield return new WaitForSeconds(0.3f);
//        RefreshVisualState();
//    }


//    private void RefreshVisualState()
//    {
//        isUnlocked = StageProgressManager.IsStageUnlocked(stageName);

//        // 🔹 튜토리얼 첫 스테이지의 반짝임 제어
//        if (stageName == "TUTO_Stage1")
//        {
//            bool nextUnlocked = StageProgressManager.IsStageUnlocked("TUTO_Stage2");

//            // Stage2가 해금되면 반짝임 멈추고 고정 불빛 유지
//            if (nextUnlocked)
//            {
//                isNextStage = false;
//                spriteRenderer.color = unlockedColor * 0.9f;
//                Debug.Log("[TUTO_Stage1] Stage2 해금됨 → 고정 불빛 유지");
//            }
//            else
//            {
//                isNextStage = true;
//                spriteRenderer.color = unlockedColor;
//                Debug.Log("[TUTO_Stage1] 아직 클리어 전 → 반짝임 유지");
//            }

//            return; // 🔥 아래 로직 덮어쓰지 않게 차단
//        }

//        // 🔹 나머지 스테이지는 기존 로직 그대로
//        string prevStageName = GetPreviousStageName(stageName);
//        bool prevCleared = false;
//        if (!string.IsNullOrEmpty(prevStageName))
//            prevCleared = StageProgressManager.IsStageUnlocked(prevStageName);

//        isNextStage = prevCleared && !isUnlocked;

//        if (!isUnlocked)
//            spriteRenderer.color = lockedColor;
//        else if (isNextStage)
//            spriteRenderer.color = unlockedColor;
//        else
//            spriteRenderer.color = unlockedColor * 0.9f;
//    }



//    private void Update()
//    {
//        // ✨ 반짝이는 효과 (isNextStage == true일 때만)
//        if (isNextStage)
//        {
//            glowTimer += Time.deltaTime * 2f;
//            float glow = (Mathf.Sin(glowTimer) + 1f) / 2f;
//            spriteRenderer.color = Color.Lerp(unlockedColor * 0.8f, unlockedColor * glowIntensity, glow);
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//            isPlayerInRange = true;
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//            isPlayerInRange = false;
//    }

//    // 🔹 예: "GR_Stage2" → "GR_Stage1"
//    private string GetPreviousStageName(string stage)
//    {
//        int idx = stage.LastIndexOf("Stage");
//        if (idx < 0) return null;

//        string prefix = stage.Substring(0, idx + 5); // "GR_Stage"
//        string numberPart = stage.Substring(idx + 5);
//        if (int.TryParse(numberPart, out int stageNum) && stageNum > 1)
//            return $"{prefix}{stageNum - 1}";
//        return null;
//    }

//    // 🔹 예: "GR_Stage1" → "TUTO_Stage2"
//    private string GetPreviousEpisodeLastStage(string stage)
//    {
//        string prefix = stage.Split('_')[0];
//        string prevEpisode = prefix switch
//        {
//            "GR" => "TUTO",
//            "YL" => "GR",
//            "BL" => "YL",
//            "OR" => "BL",
//            "RD" => "OR",
//            "SK" => "RD",
//            "PR" => "SK",
//            "BOSE" => "PR",
//            "RAINBOW" => "BOSE",
//            _ => null
//        };

//        if (string.IsNullOrEmpty(prevEpisode))
//            return null;

//        // 🔸 각 에피소드의 마지막 스테이지 번호 (필요시 수정)
//        return $"{prevEpisode}_Stage2";
//    }

//    // 🔹 다음 스테이지 이름 계산 (예: GR_Stage1 → GR_Stage2)
//    private string GetNextStageName(string stage)
//    {
//        int idx = stage.LastIndexOf("Stage");
//        if (idx < 0) return null;

//        string prefix = stage.Substring(0, idx + 5); // "GR_Stage"
//        string numberPart = stage.Substring(idx + 5);
//        if (int.TryParse(numberPart, out int stageNum))
//            return $"{prefix}{stageNum + 1}";
//        return null;
//    }
//}
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class StagePortal : MonoBehaviour
{
    [Header("이 포탈이 여는 스테이지 이름")]
    public string stageName;

    [Header("시각효과 설정")]
    public Color unlockedColor = Color.white;
    public Color lockedColor = new Color(0.4f, 0.4f, 0.4f, 0.6f);
    public float glowIntensity = 1.2f;

    private bool isUnlocked;
    private bool isPlayerInRange;
    private bool isNextStage;
    private SpriteRenderer spriteRenderer;
    private float glowTimer = 0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>() ?? GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(InitAfterDelay());
    }

    private IEnumerator InitAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);
        RefreshVisualState();
    }

    public void RefreshVisualState()
    {
        isUnlocked = StageProgressManager.IsStageUnlocked(stageName);

        // TUTO_Stage1 전용 처리
        if (stageName == "TUTO_Stage1")
        {
            bool nextUnlocked = StageProgressManager.IsStageUnlocked("TUTO_Stage2");
            isNextStage = !nextUnlocked; // Stage2가 해금되면 반짝 멈춤

            spriteRenderer.color = isNextStage ? unlockedColor : unlockedColor * 0.9f;
            Debug.Log($"[{stageName}] → {(isNextStage ? "반짝" : "고정")} 상태");
            return;
        }

        // 이전 스테이지 클리어 여부
        string prevStage = GetPreviousStageName(stageName);
        bool prevCleared = false;
        if (!string.IsNullOrEmpty(prevStage))
            prevCleared = StageProgressManager.IsStageUnlocked(prevStage);

        // 다음 스테이지 후보 여부 (아직 잠겨있고 이전 스테이지 클리어 시)
        isNextStage = prevCleared && !isUnlocked;

        if (!isUnlocked)
            spriteRenderer.color = lockedColor;
        else if (isNextStage)
            spriteRenderer.color = unlockedColor;
        else
            spriteRenderer.color = unlockedColor * 0.9f;
    }

    private void Update()
    {
        // 반짝 효과
        if (isNextStage)
        {
            glowTimer += Time.deltaTime * 2f;
            float glow = (Mathf.Sin(glowTimer) + 1f) / 2f;
            spriteRenderer.color = Color.Lerp(unlockedColor * 0.8f, unlockedColor * glowIntensity, glow);
        }

        // 실시간 해금 감지 (TUTO 전용)
        if (stageName == "TUTO_Stage1")
        {
            bool nextUnlocked = StageProgressManager.IsStageUnlocked("TUTO_Stage2");
            if (nextUnlocked && isNextStage)
            {
                isNextStage = false;
                spriteRenderer.color = unlockedColor * 0.9f;
                Debug.Log($"[{stageName}] Stage2 해금 감지 → 반짝 종료");
            }
        }

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isUnlocked)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerInRange = false;
    }

    private string GetPreviousStageName(string stage)
    {
        int idx = stage.LastIndexOf("Stage");
        if (idx < 0) return null;

        string prefix = stage.Substring(0, idx + 5);
        string numberPart = stage.Substring(idx + 5);
        if (int.TryParse(numberPart, out int stageNum) && stageNum > 1)
            return $"{prefix}{stageNum - 1}";
        return null;
    }
}
