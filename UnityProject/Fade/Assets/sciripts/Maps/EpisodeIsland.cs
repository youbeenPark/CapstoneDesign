
//using UnityEngine;

//public class EpisodeIsland : MonoBehaviour
//{
//    [Header("Island Info")]
//    public string episodeName; // 예: "TUTO", "GR", "YL", "RD"
//    [SerializeField] private Sprite lockedSprite;
//    [SerializeField] private Sprite unlockedSprite;
//    private SpriteRenderer sr;

//    private void Awake()
//    {
//        sr = GetComponent<SpriteRenderer>();
//    }

//    public void UpdateIslandSprite()
//    {
//        int unlocked = PlayerPrefs.GetInt("Unlocked_" + episodeName, episodeName == "TUTO" ? 1 : 0);
//        sr.sprite = (unlocked == 1) ? unlockedSprite : lockedSprite;

//        // ✅ 투명도(및 색상) 초기화 추가
//        // 혹시 이전 SpriteRenderer가 반투명 상태였거나 색상이 바뀌어 있었을 경우 대비
//        sr.color = Color.white;

//        Debug.Log($"[{episodeName}] 스프라이트 갱신 완료 (Unlocked: {unlocked})");
//    }
//}

using UnityEngine;

public class EpisodeIsland : MonoBehaviour
{
    [Header("Island Info")]
    public string episodeName; // 예: "TUTO", "GR", "YL", "RD"

    [Header("Sprites")]
    [SerializeField] private Sprite lockedSprite;    // 해금 전: 흑백
    [SerializeField] private Sprite unlockedSprite;  // 해금 후: 무채색 (깜빡임용)
    [SerializeField] private Sprite clearedSprite;   // 클리어 후: 컬러

    [Header("Blink Settings (해금 후 깜빡임)")]
    [SerializeField] private float blinkSpeed = 2f;       // 깜빡임 속도
    [SerializeField] private float blinkIntensity = 0.4f; // 밝기 변화 강도 (0~1)

    private SpriteRenderer sr;
    private bool isUnlocked;
    private bool isCleared;
    private float blinkTimer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        UpdateIslandSprite();
    }

    /// <summary>
    /// PlayerPrefs 기반으로 섬 상태 갱신
    /// </summary>
    public void UpdateIslandSprite()
    {
        // PlayerPrefs 값 불러오기
        isUnlocked = PlayerPrefs.GetInt("Unlocked_" + episodeName, episodeName == "TUTO" ? 1 : 0) == 1;
        isCleared = PlayerPrefs.GetInt("Cleared_" + episodeName, 0) == 1;

        // 🔹 Cleared 상태를 가장 먼저 검사해야 깜빡임이 덮어씌워지지 않음
        if (isCleared)
        {
            // ✅ ③ 클리어 후: 컬러맵 표시 + 깜빡임 완전 종료
            sr.sprite = clearedSprite != null ? clearedSprite : unlockedSprite;
            sr.color = Color.white;
        }
        else if (isUnlocked)
        {
            // ✅ ② 해금 후: 무채색 + 깜빡임용 (Update에서 처리)
            sr.sprite = unlockedSprite != null ? unlockedSprite : lockedSprite;
            sr.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        }
        else
        {
            // ✅ ① 해금 전: 어두운 흑백
            sr.sprite = lockedSprite;
            sr.color = new Color(0.4f, 0.4f, 0.4f, 1f);
        }

        Debug.Log($"[{episodeName}] 상태 갱신됨 (Unlocked: {isUnlocked}, Cleared: {isCleared})");
    }

    private void Update()
    {
        // ✅ 깜빡임은 “해금 후지만 클리어되지 않은 경우”에만 실행
        if (isUnlocked && !isCleared)
        {
            blinkTimer += Time.deltaTime * blinkSpeed;

            // PingPong으로 0~1 반복 → 밝기 변화
            float blinkValue = Mathf.PingPong(blinkTimer, 1f);
            float brightness = Mathf.Lerp(0.7f, 1.0f, blinkValue * blinkIntensity);

            sr.color = new Color(brightness, brightness, brightness, 1f);
        }
        else if (isCleared)
        {
            // ✅ 혹시 이전 프레임에 깜빡임 색이 남았을 경우 대비
            sr.color = Color.white;
        }
    }
}
