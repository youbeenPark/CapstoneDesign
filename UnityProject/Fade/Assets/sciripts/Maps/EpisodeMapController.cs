//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System.Collections;

//public class EpisodeMapController : MonoBehaviour
//{
//    [Header("References")]
//    [SerializeField] private Rigidbody2D player;
//    [SerializeField] private float moveSpeed = 3f;

//    [Header("World Info")]
//    [SerializeField] private string currentWorld = ""; // 예: "TUTO"

//    private string currentStageName = null;

//    private void Start()
//    {
//        // ✅ 첫 번째 스테이지 자동 해금
//        string firstStageKey = $"Unlocked_{currentWorld}_Stage1";
//        PlayerPrefs.SetInt(firstStageKey, 1);
//        PlayerPrefs.Save();
//        Debug.Log($"기본 해금 설정됨: {firstStageKey}");

//        // ✅ 씬 시작 후 잠시 대기 → 모든 StagePortal 상태 새로고침
//        StartCoroutine(DelayedPortalRefresh());
//    }

//    private IEnumerator DelayedPortalRefresh()
//    {
//        yield return new WaitForSeconds(0.3f); // PlayerPrefs 저장/불러오기 완료 대기
//        foreach (var portal in FindObjectsOfType<StagePortal>())
//        {
//            portal.SendMessage("RefreshUnlockState", SendMessageOptions.DontRequireReceiver);
//        }
//        Debug.Log("🔄 모든 StagePortal 상태 새로고침 완료");
//    }

//    private void Update()
//    {
//        HandleMovement();
//        HandleStageEnter();

//        // 디버그용 — 근처 스테이지 감지 확인
//        Collider2D hit = Physics2D.OverlapCircle(player.position, 0.2f, LayerMask.GetMask("StageSpot"));
//        if (hit != null)
//            Debug.Log($"Overlap 감지됨: {hit.name}");
//    }

//    private void HandleMovement()
//    {
//        float moveX = Input.GetAxisRaw("Horizontal");
//        float moveY = Input.GetAxisRaw("Vertical");
//        Vector2 dir = new Vector2(moveX, moveY).normalized;
//        player.linearVelocity = dir * moveSpeed;
//    }

//    private void HandleStageEnter()
//    {
//        Debug.Log($"현재 currentStageName: {currentStageName}");

//        if (Input.GetKeyDown(KeyCode.UpArrow) && currentStageName != null)
//        {
//            // ✅ PlayerPrefs 키 통일 (Unlocked_ 접두사로 저장)
//            string unlockKey = $"Unlocked_{currentStageName}";
//            bool unlocked = PlayerPrefs.GetInt(unlockKey, 0) == 1
//                            || currentStageName.EndsWith("Stage1"); // 첫 스테이지만 예외 허용

//            Debug.Log($"스테이지 확인 → {currentStageName}, 해금 여부: {unlocked}");

//            if (!unlocked)
//            {
//                Debug.Log("🔒 이 스테이지는 아직 잠겨 있습니다!");
//                return;
//            }

//            Debug.Log($"🚪 스테이지 진입: {currentStageName}");
//            SceneManager.LoadScene(currentStageName);
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        Debug.Log($"충돌 감지됨: {other.gameObject.name}, 태그: {other.tag}");

//        if (other.CompareTag("StageSpot"))
//        {
//            currentStageName = other.gameObject.name;
//            Debug.Log($"{currentStageName} 위에 있음");
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("StageSpot") && currentStageName == other.gameObject.name)
//        {
//            currentStageName = null;
//        }
//    }
//}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EpisodeMapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private float moveSpeed = 3f;

    [Header("World Info")]
    [SerializeField] private string currentWorld; // 예: "TUTO"
    [SerializeField] private string nextWorld;    // 예: "GR"
    [SerializeField] private int totalStages = 2; // 해당 에피소드 내 스테이지 개수

    private string currentStageName = null;

    private void Start()
    {
        // 첫 번째 스테이지 자동 해금
        string firstStageKey = $"Unlocked_{currentWorld}_Stage1";
        PlayerPrefs.SetInt(firstStageKey, 1);
        PlayerPrefs.Save();
        Debug.Log($"기본 해금 설정됨: {firstStageKey}");

        StartCoroutine(DelayedPortalRefresh());
    }

    private IEnumerator DelayedPortalRefresh()
    {
        yield return new WaitForSeconds(0.3f);
        foreach (var portal in FindObjectsOfType<StagePortal>())
            portal.SendMessage("RefreshVisualState", SendMessageOptions.DontRequireReceiver);
        Debug.Log("🔄 모든 StagePortal 상태 새로고침 완료");
    }

    private void Update()
    {
        HandleMovement();
        HandleStageEnter();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(moveX, moveY).normalized;
        player.linearVelocity = dir * moveSpeed;
    }

    private void HandleStageEnter()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentStageName != null)
        {
            string unlockKey = $"Unlocked_{currentStageName}";
            bool unlocked = PlayerPrefs.GetInt(unlockKey, 0) == 1
                            || currentStageName.EndsWith("Stage1");

            if (!unlocked)
            {
                Debug.Log("🔒 이 스테이지는 아직 잠겨 있습니다!");
                return;
            }

            Debug.Log($"🚪 스테이지 진입: {currentStageName}");
            SceneManager.LoadScene(currentStageName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("StageSpot"))
            currentStageName = other.gameObject.name;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("StageSpot") && currentStageName == other.gameObject.name)
            currentStageName = null;
    }

    // ✅ 스테이지 클리어 시 호출
    public void OnStageCleared(string clearedStageName)
    {
        Debug.Log($"[EpisodeMapController] {clearedStageName} 클리어 감지!");

        PlayerPrefs.SetInt($"Cleared_{clearedStageName}", 1);

        int clearedCount = 0;
        for (int i = 1; i <= totalStages; i++)
        {
            string key = $"Cleared_{currentWorld}_Stage{i}";
            if (PlayerPrefs.GetInt(key, 0) == 1)
                clearedCount++;
        }

        // ✅ 모든 스테이지 클리어 → 다음 월드 해금
        if (clearedCount >= totalStages)
        {
            PlayerPrefs.SetInt($"Unlocked_{nextWorld}", 1);
            PlayerPrefs.Save();
            Debug.Log($"🎉 {currentWorld}의 모든 스테이지 클리어 → {nextWorld} 해금 완료!");
        }
    }
}
