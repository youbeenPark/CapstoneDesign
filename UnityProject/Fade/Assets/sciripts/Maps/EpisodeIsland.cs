//using System.Collections;
//using UnityEngine;

///// <summary>
///// 월드맵 섬의 Sprite를 스테이지 해금 상태에 따라 자동 전환.
///// </summary>
//[RequireComponent(typeof(SpriteRenderer))]
//public class EpisodeIsland : MonoBehaviour
//{
//    [Header("에피소드 기본 설정")]
//    public string episodeName;        // 예: "TUTO", "GR", "YL" 등
//    public string lastStageName;      // 예: "TUTO_Stage2"

//    [Header("이미지 설정")]
//    public Sprite lockedSprite;       // 회색 버전
//    public Sprite clearedSprite;      // 컬러 버전

//    private SpriteRenderer spriteRenderer;

//    private void Start()
//    {
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        UpdateIslandSprite();
//    }

//    /// <summary>
//    /// 에피소드 클리어 여부에 따라 섬 이미지를 업데이트
//    /// </summary>
//    public void UpdateIslandSprite()
//    {
//        bool cleared = StageProgressManager.IsStageUnlocked(lastStageName);
//        spriteRenderer.sprite = cleared ? clearedSprite : lockedSprite;

//        if (cleared) StartCoroutine(GlowEffect());


//        Debug.Log($"🏝️ [{episodeName}] 섬 이미지 갱신됨 → {(cleared ? "컬러버전" : "회색버전")}");
//    }
//    private IEnumerator GlowEffect()
//    {
//        float duration = 1.5f;
//        float timer = 0f;
//        Color baseColor = spriteRenderer.color;

//        while (timer < duration)
//        {
//            float t = Mathf.Sin(timer * Mathf.PI * 2f) * 0.5f + 0.5f;
//            spriteRenderer.color = Color.Lerp(baseColor, Color.white, t);
//            timer += Time.deltaTime;
//            yield return null;
//        }

//        spriteRenderer.color = Color.white;
//    }

//}

using UnityEngine;

public class EpisodeIsland : MonoBehaviour
{
    [Header("Island Info")]
    public string episodeName; // 예: "TUTO", "GR", "YL", "RD"
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void UpdateIslandSprite()
    {
        int unlocked = PlayerPrefs.GetInt("Unlocked_" + episodeName, episodeName == "TUTO" ? 1 : 0);
        sr.sprite = (unlocked == 1) ? unlockedSprite : lockedSprite;

        // ✅ 투명도(및 색상) 초기화 추가
        // 혹시 이전 SpriteRenderer가 반투명 상태였거나 색상이 바뀌어 있었을 경우 대비
        sr.color = Color.white;

        Debug.Log($"[{episodeName}] 스프라이트 갱신 완료 (Unlocked: {unlocked})");
    }
}

