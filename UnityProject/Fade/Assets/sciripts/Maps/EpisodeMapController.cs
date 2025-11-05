using UnityEngine;
using UnityEngine.SceneManagement;

public class EpisodeMapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private float moveSpeed = 3f;

    [Header("World Info")]
    [SerializeField] private string currentWorld = ""; // 예: "TUTO"

    private string currentStageName = null;

    private void Start()
    {
        // ✅ 첫 번째 스테이지 자동 해금
        string firstStageKey = $"{currentWorld}_Stage1_Unlocked";
        if (PlayerPrefs.GetInt(firstStageKey, 0) == 0)
        {
            PlayerPrefs.SetInt(firstStageKey, 1);
            PlayerPrefs.Save();
            Debug.Log($"기본 해금 설정됨: {firstStageKey}");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleStageEnter();

        Collider2D hit = Physics2D.OverlapCircle(player.position, 0.2f, LayerMask.GetMask("StageSpot"));
        if (hit != null)
            Debug.Log($"Overlap 감지됨: {hit.name}");
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
        Debug.Log($"현재 currentStageName: {currentStageName}");

        if (Input.GetKeyDown(KeyCode.UpArrow) && currentStageName != null)
        {
            string unlockKey = $"{currentStageName}_Unlocked";
            bool unlocked = PlayerPrefs.GetInt(unlockKey, 0) == 1 || currentStageName.EndsWith("Stage1");

            Debug.Log($"스테이지 확인 → {currentStageName}, 해금 여부: {unlocked}");

            if (!unlocked)
            {
                Debug.Log("이 스테이지는 아직 잠겨 있습니다!");
                return;
            }

            Debug.Log($"스테이지 진입: {currentStageName}");
            SceneManager.LoadScene(currentStageName);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"충돌 감지됨: {other.gameObject.name}, 태그: {other.tag}");

        if (other.CompareTag("StageSpot"))
        {
            currentStageName = other.gameObject.name;
            Debug.Log($"{currentStageName} 위에 있음");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("StageSpot") && currentStageName == other.gameObject.name)
        {
            currentStageName = null;
        }
    }
}
