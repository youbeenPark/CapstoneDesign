using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public float breakThreshold = 0.5f;

    private float pressStartTime;
    private bool isHoldingKey = false;

    private GameObject interactableObject = null;

    private Collider2D interactionTrigger;

    private Rigidbody2D rb;
    private RigidbodyType2D originalBodyType;

    void Start()
    {
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
            Debug.LogWarning("Trigger Collider 없음!");
            return;
        }

        // 평소에는 OFF
        interactionTrigger.enabled = false;

        // Rigidbody 저장
        rb = GetComponent<Rigidbody2D>();
        originalBodyType = rb.bodyType;
    }

    void Update()
    {
        // G 키 누름
        if (Input.GetKeyDown(KeyCode.G))
        {
            pressStartTime = Time.time;
            isHoldingKey = true;

            interactionTrigger.enabled = true;

            // 🔥 밀림 완전 방지: G 누르는 동안 Kinematic
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // G 키 떼기
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (isHoldingKey)
            {
                float held = Time.time - pressStartTime;
                if (held < breakThreshold)
                    BreakObject();
            }

            isHoldingKey = false;

            interactionTrigger.enabled = false;

            // 🔥 원래 물리 상태로 복귀
            rb.bodyType = originalBodyType;

            interactableObject = null;
        }
    }

    private void BreakObject()
    {
        if (interactableObject != null)
        {
            Destroy(interactableObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!interactionTrigger.enabled) return;

        if (other.CompareTag("Destructible"))
        {
            interactableObject = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == interactableObject)
        {
            interactableObject = null;
        }
    }
}
