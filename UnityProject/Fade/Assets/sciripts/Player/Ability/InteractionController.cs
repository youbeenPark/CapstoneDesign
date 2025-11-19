using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [Header("짧게 누를 때 부서지는 시간 기준")]
    public float breakThreshold = 0.5f;

    private float pressStartTime;
    private bool isHoldingKey = false;

    private GameObject interactableObject = null;

    private Collider2D interactionTrigger;

    private Rigidbody2D rb;
    private RigidbodyType2D originalBodyType;

    private Animator anim;

    void Start()
    {
        // 플레이어 Animator 가져오기
        anim = GetComponent<Animator>();

        // 감지용 Trigger 찾기
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (var c in cols)
        {
            if (c.isTrigger)
            {
                interactionTrigger = c;
                break;
            }
        }

        if (interactionTrigger == null)
        {
            Debug.LogWarning("⚠ Trigger Collider 없음! 플레이어에 Trigger 콜라이더 추가해줘.");
            return;
        }

        // 평소에는 Trigger 꺼두기
        interactionTrigger.enabled = false;

        // 플레이어 Rigidbody 정보 저장
        rb = GetComponent<Rigidbody2D>();
        originalBodyType = rb.bodyType;
    }

    void Update()
    {
        // 🔥 G 키 누르기 시작
        if (Input.GetKeyDown(KeyCode.G))
        {
            pressStartTime = Time.time;
            isHoldingKey = true;

            // 상호작용 감지 ON
            interactionTrigger.enabled = true;

            // 🔥 G 누르는 동안 플레이어를 밀리지 않도록 고정
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // 🔥 G 키 떼기
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (isHoldingKey)
            {
                float held = Time.time - pressStartTime;

                // 짧은 누름 → 부수기
                if (held < breakThreshold)
                    BreakObject();
            }

            isHoldingKey = false;

            // 감지 Trigger 끄기
            interactionTrigger.enabled = false;

            // 플레이어 물리 원래대로
            rb.bodyType = originalBodyType;

            interactableObject = null;
        }
    }

    // ⭐ 오브젝트 부수기 + Break 애니메이션 실행
    private void BreakObject()
    {
        if (interactableObject != null)
        {
            // Break 애니메이션 실행
            anim.SetTrigger("Break");

            // 실제 오브젝트 파괴
            Destroy(interactableObject);
        }
    }

    // Trigger 안으로 들어옴
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!interactionTrigger.enabled) return;

        if (other.CompareTag("Destructible"))
        {
            interactableObject = other.gameObject;
        }
    }

    // Trigger 밖으로 나감
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == interactableObject)
        {
            interactableObject = null;
        }
    }
}
