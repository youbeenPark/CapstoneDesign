//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System.Collections;

//[RequireComponent(typeof(SpriteRenderer))]
//public class StagePortal : MonoBehaviour
//{
//    [Header("이 포탈이 여는 스테이지 이름")]
//    public string stageName;

//    [Header("시각효과 설정")]
//    public Color unlockedColor = Color.white;
//    public Color lockedColor = new Color(0.4f, 0.4f, 0.4f, 0.6f);
//    public float glowIntensity = 1.2f;

//    private bool isUnlocked;
//    private bool isPlayerInRange;
//    private bool isNextStage;
//    private SpriteRenderer spriteRenderer;
//    private float glowTimer = 0f;

//    private void Start()
//    {
//        spriteRenderer = GetComponent<SpriteRenderer>() ?? GetComponentInChildren<SpriteRenderer>();
//        StartCoroutine(InitAfterDelay());
//    }

//    private IEnumerator InitAfterDelay()
//    {
//        yield return new WaitForSeconds(0.3f);
//        RefreshVisualState();
//    }

//    public void RefreshVisualState()
//    {
//        isUnlocked = StageProgressManager.IsStageUnlocked(stageName);

//        // TUTO_Stage1 전용 처리
//        if (stageName == "TUTO_Stage1")
//        {
//            bool nextUnlocked = StageProgressManager.IsStageUnlocked("TUTO_Stage2");
//            isNextStage = !nextUnlocked; // Stage2가 해금되면 반짝 멈춤

//            spriteRenderer.color = isNextStage ? unlockedColor : unlockedColor * 0.9f;
//            Debug.Log($"[{stageName}] → {(isNextStage ? "반짝" : "고정")} 상태");
//            return;
//        }

//        // 이전 스테이지 클리어 여부
//        string prevStage = GetPreviousStageName(stageName);
//        bool prevCleared = false;
//        if (!string.IsNullOrEmpty(prevStage))
//            prevCleared = StageProgressManager.IsStageUnlocked(prevStage);

//        // 다음 스테이지 후보 여부 (아직 잠겨있고 이전 스테이지 클리어 시)
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
//        // 반짝 효과
//        if (isNextStage)
//        {
//            glowTimer += Time.deltaTime * 2f;
//            float glow = (Mathf.Sin(glowTimer) + 1f) / 2f;
//            spriteRenderer.color = Color.Lerp(unlockedColor * 0.8f, unlockedColor * glowIntensity, glow);
//        }

//        // 실시간 해금 감지 (TUTO 전용)
//        if (stageName == "TUTO_Stage1")
//        {
//            bool nextUnlocked = StageProgressManager.IsStageUnlocked("TUTO_Stage2");
//            if (nextUnlocked && isNextStage)
//            {
//                isNextStage = false;
//                spriteRenderer.color = unlockedColor * 0.9f;
//                Debug.Log($"[{stageName}] Stage2 해금 감지 → 반짝 종료");
//            }
//        }

//        // 🚪 스테이지 입장
//        if (isPlayerInRange && Input.GetKeyDown(KeyCode.UpArrow))
//        {
//            if (isUnlocked)
//            {
//                Debug.Log($"🚪 {stageName} 입장!");
//                SceneManager.LoadScene(stageName);
//            }
//            else
//            {
//                Debug.Log($"🔒 {stageName}은 아직 잠겨 있습니다!");
//            }
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player")) isPlayerInRange = true;
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Player")) isPlayerInRange = false;
//    }

//    private string GetPreviousStageName(string stage)
//    {
//        int idx = stage.LastIndexOf("Stage");
//        if (idx < 0) return null;

//        string prefix = stage.Substring(0, idx + 5);
//        string numberPart = stage.Substring(idx + 5);
//        if (int.TryParse(numberPart, out int stageNum) && stageNum > 1)
//            return $"{prefix}{stageNum - 1}";
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
    public string stageName; // 예: "TUTO_Stage1", "GR_Stage2"

    [Header("시각 효과 설정")]
    public Color unlockedColor = Color.white;                     // 클리어 후 컬러
    public Color lockedColor = new Color(0.4f, 0.4f, 0.4f, 0.6f); // 잠김 상태
    public float glowIntensity = 1.2f;                            // 빛 강도

    [Header("Glow (테두리 불빛)용 Renderer")]
    [SerializeField] private SpriteRenderer glowRenderer; // 따로 추가한 Glow SpriteRenderer

    private bool isUnlocked;   // 해금 여부
    private bool isCleared;    // 클리어 여부
    private bool isNextStage;  // 다음 스테이지 여부
    private bool isPlayerInRange;

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

    // 시각 상태 갱신
    public void RefreshVisualState()
    {
        // PlayerPrefs 기반으로 상태 불러오기
        isUnlocked = StageProgressManager.IsStageUnlocked(stageName);
        isCleared = StageProgressManager.IsStageCleared(stageName);

        // 이전 스테이지 클리어 여부 확인
        string prevStage = GetPreviousStageName(stageName);
        bool prevCleared = false;
        if (!string.IsNullOrEmpty(prevStage))
            prevCleared = StageProgressManager.IsStageCleared(prevStage);

        // 다음 스테이지 (이전 클리어 + 아직 클리어 안됨)
        isNextStage = prevCleared && !isCleared;

        // 상태별 표시
        if (isCleared)
        {
            // ✅ 클리어 → 컬러 맵 + Glow 끔
            spriteRenderer.color = unlockedColor;
            if (glowRenderer != null) glowRenderer.enabled = false;
        }
        else if (isNextStage)
        {
            // ✅ 해금만 됨 → 흑백 맵 + Glow 켬
            spriteRenderer.color = lockedColor;
            if (glowRenderer != null) glowRenderer.enabled = true;
        }
        else
        {
            // 🔒 잠김 → 흑백 + Glow 없음
            spriteRenderer.color = lockedColor;
            if (glowRenderer != null) glowRenderer.enabled = false;
        }
    }

    private void Update()
    {
        // ✨ 해금된 포탈의 테두리 반짝임 (GlowRenderer 전용)
        if (isNextStage && glowRenderer != null && glowRenderer.enabled)
        {
            glowTimer += Time.deltaTime * 2f;
            float glow = (Mathf.Sin(glowTimer) + 1f) / 2f;
            glowRenderer.color = Color.Lerp(unlockedColor * 0.5f, unlockedColor * glowIntensity, glow);
        }

        // 🚪 스테이지 입장
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isUnlocked || isCleared)
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
        if (other.CompareTag("Player"))
            isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRange = false;
    }

    // 🔹 이전 스테이지 이름 계산 (예: GR_Stage2 → GR_Stage1)
    private string GetPreviousStageName(string stage)
    {
        int idx = stage.LastIndexOf("Stage");
        if (idx < 0) return null;

        string prefix = stage.Substring(0, idx + 5); // "GR_Stage"
        string numberPart = stage.Substring(idx + 5);
        if (int.TryParse(numberPart, out int stageNum) && stageNum > 1)
            return $"{prefix}{stageNum - 1}";
        return null;
    }
}
