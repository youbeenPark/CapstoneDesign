using UnityEngine;

public class InteractionController : MonoBehaviour
{
    // 짧은 누름/길게 누름을 구분하는 기준 시간 (초)
    public float holdDuration = 0.5f;

    // 밀기 힘 설정
    public float pushForce = 10f;

    // G 키를 누르기 시작한 시간
    private float pressStartTime;

    // G 키를 누르고 있는 중인지 확인하는 플래그 (짧은 누름 감지용)
    private bool isHoldingKey = false;

    // G 키를 길게 눌러 밀기 상태가 활성화되었는지 확인하는 플래그 (FixedUpdate에서 사용)
    private bool isPushing = false;

    // 상호작용할 오브젝트 (Destructible 태그를 가진 오브젝트)
    private GameObject interactableObject = null;

    // 상호작용할 오브젝트의 Rigidbody2D (밀기 기능에 사용)
    private Rigidbody2D interactableRigidbody = null;

    void Update()
    {
        // 1. G 키 누름 시작 감지 (GetKeyDown)
        if (Input.GetKeyDown(KeyCode.G))
        {
            pressStartTime = Time.time; // 현재 시간 기록
            isHoldingKey = true;        // 누르고 있는 상태 시작
            isPushing = false;          // 밀기 상태 초기화
        }

        // 2. G 키를 누르고 있는 동안 (GetKey)
        if (Input.GetKey(KeyCode.G) && isHoldingKey)
        {
            float heldTime = Time.time - pressStartTime; // 누르고 있는 시간 계산

            // 2-1. 밀기 상태 활성화
            // 설정된 시간(holdDuration) 이상 눌렀고, Rigidbody가 있을 때
            if (heldTime >= holdDuration && interactableRigidbody != null)
            {
                // 밀기 상태 플래그만 설정
                isPushing = true;
            }
        }

        // 3. G 키 떼기 감지 (GetKeyUp)
        if (Input.GetKeyUp(KeyCode.G))
        {
            float heldTime = Time.time - pressStartTime; // 누르고 있던 총 시간
            isHoldingKey = false; // 누르고 있는 상태 종료
            isPushing = false;    // 밀기 상태 즉시 종료

            // 3-1. 오브젝트 부수기 (짧은 누름 기능)
            // 누르고 있던 시간이 holdDuration 미만이고, 상호작용 가능한 오브젝트가 있을 때
            if (heldTime < holdDuration && interactableObject != null)
            {
                BreakObject();
            }
        }
    }

    // ⭐️ 물리 관련 작업은 FixedUpdate에서 처리
    private void FixedUpdate()
    {
        // G 키가 길게 눌러져 밀기 상태가 활성화되었고, Rigidbody가 있을 때만 실행
        if (isPushing && interactableRigidbody != null)
        {
            PushObject();
        }
    }

    /// <summary>
    /// 짧게 눌렀을 때 실행되는 기능: 상호작용 오브젝트 파괴
    /// </summary>
    void BreakObject()
    {
        if (interactableObject != null)
        {
            Debug.Log(interactableObject.name + " 오브젝트를 부숩니다! (짧은 누름)");
            Destroy(interactableObject);

            // 파괴 후 상태 초기화
            interactableObject = null;
            interactableRigidbody = null;
        }
    }

    /// <summary>
    /// 길게 눌렀을 때 실행되는 기능: Rigidbody2D에 힘을 가해 밀기
    /// </summary>
    void PushObject()
    {
        // 밀어낼 방향 (예: 플레이어의 로컬 오른쪽 방향)
        Vector2 pushDirection = transform.right;

        // ForceMode2D.Force 사용 시 Time.deltaTime을 곱하지 않습니다. FixedUpdate는 고정된 시간 간격으로 실행됩니다.
        interactableRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Force);

        Debug.Log("오브젝트를 밀고 있습니다."); // 밀기 확인용 로그
    }

    // ... (OnTriggerEnter2D, OnTriggerExit2D는 동일)

    /// <summary>
    /// 플레이어 주변의 상호작용 오브젝트 감지
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 상호작용 오브젝트에 'Destructible' 태그가 지정되어 있어야 함
        if (other.CompareTag("Destructible"))
        {
            interactableObject = other.gameObject;
            interactableRigidbody = other.GetComponent<Rigidbody2D>();
        }
    }

    /// <summary>
    /// 상호작용 오브젝트 감지 해제
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == interactableObject)
        {
            interactableObject = null;
            interactableRigidbody = null;
            isPushing = false; // 영역을 벗어나면 밀기 상태도 해제
        }
    }
}