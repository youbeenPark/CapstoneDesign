using UnityEngine;

public class DaniController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool hasLanded = false;

    [Header("Glide Settings")]
    [SerializeField] private float fallSpeed = -1.2f;
    [SerializeField] private float horizontalRange = 0.5f;
    [SerializeField] private float horizontalSpeed = 1.2f;
    [SerializeField] private float verticalWaveStrength = 0.15f;
    [SerializeField] private float rotateAngle = 10f;

    [Header("Landing Adjustment")]
    [SerializeField] private float horizontalOffsetStart = 1.5f;
    [SerializeField] private float horizontalGlideSpeed = 1f;

    [Header("Animation Control")]
    [SerializeField] private float failAnimationSpeed = 0.5f;

    [Header("Movement After Landing")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private Transform wallTarget;
    private bool isWalkingToWall = false;

    // ⭐ FindObjectOfType 대신 Inspector에서 연결할 필드로 변경 ⭐
    [SerializeField] private LightController lightController;

    // DaniWalk 상태 및 Trigger Hash ID
    private int daniWalkShortHash;
    private int turnBackHash;


    private Vector3 startPos;
    private float timer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
        startPos = transform.position;

        // FindObjectOfType 로직 제거됨. lightController는 Inspector에서 연결됨.

        // Animator Hash ID 미리 저장
        daniWalkShortHash = Animator.StringToHash("DaniWalk");
        turnBackHash = Animator.StringToHash("TurnBack");
    }

    // Animator State Machine Behaviour 없이 외부에서 호출되어 이동 시작/정지 신호만 받습니다.
    public void SetIsWalkingToWall(bool isWalking)
    {
        isWalkingToWall = isWalking;

        if (animator != null)
        {
            // DaniWalk -> DaniBackIdle 전환 조건 중 isWalking=false 파라미터를 제어
            animator.SetBool("isWalking", isWalking);
        }
    }

    void Update()
    {
        // 1. 낙하 중일 때만 흔들림 로직 실행
        if (!hasLanded)
        {
            timer += Time.deltaTime;

            float horizontalWobble = Mathf.Sin(timer * horizontalSpeed) * horizontalRange;
            float totalHorizontalMovement = horizontalGlideSpeed * timer;
            float currentHorizontalDirection = Mathf.Cos(timer * horizontalSpeed);

            if (spriteRenderer != null)
            {
                if (currentHorizontalDirection > 0.01f) spriteRenderer.flipX = false;
                else if (currentHorizontalDirection < -0.01f) spriteRenderer.flipX = true;
            }

            float newX = startPos.x + horizontalOffsetStart + horizontalWobble + totalHorizontalMovement;
            float totalFallDistance = fallSpeed * timer;
            float verticalWobble = Mathf.Sin(timer * horizontalSpeed * 2f) * verticalWaveStrength;
            float newY = startPos.y + totalFallDistance + verticalWobble;

            transform.position = new Vector3(newX, newY, transform.position.z);
            float rotationZ = Mathf.Sin(timer * horizontalSpeed) * rotateAngle;
            transform.rotation = Quaternion.Euler(0, 0, rotationZ);

            return;
        }

        // 2. 걷기 이동 로직 시작 (State Machine Behaviour 대체)
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // DaniWalk 상태에 진입했고, 아직 이동을 시작하지 않았다면 이동을 켬
        if (!isWalkingToWall && stateInfo.shortNameHash == daniWalkShortHash)
        {
            isWalkingToWall = true;
            // DaniWalk -> DaniBackIdle 전환 조건 중 하나인 isWalking 파라미터를 true로 설정
            animator.SetBool("isWalking", true);
        }

        // 3. 착지 후 걷는 동작 실행
        if (isWalkingToWall && wallTarget != null)
        {
            Vector3 targetPos = wallTarget.position;
            Vector3 currentPos = transform.position;

            if (Vector3.Distance(currentPos, targetPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(currentPos, targetPos, walkSpeed * Time.deltaTime);

                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = false; // 오른쪽 방향 고정
                }
            }
            else
            {
                // ⭐ 목표 지점에 도착하면 걷기를 멈추고 뒷모습으로 전환 ⭐

                // 1. 걷기를 멈추라는 신호를 보냅니다. (DaniWalk -> DaniBackIdle 전환 조건 1)
                SetIsWalkingToWall(false);

                // 2. 뒷모습으로 전환하라는 Trigger를 호출합니다. (DaniWalk -> DaniBackIdle 전환 조건 2)
                if (animator != null)
                {
                    animator.SetTrigger(turnBackHash);
                }

                // 걷기 로직이 완전히 종료되었음을 알립니다.
                isWalkingToWall = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !hasLanded)
        {
            hasLanded = true;
            rb.gravityScale = 1f;

            animator.SetBool("isLanded", true);
            animator.speed = failAnimationSpeed;

            transform.rotation = Quaternion.identity;
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = false;
            }

            if (MainMenuUIController.Instance != null)
            {
                MainMenuUIController.Instance.ShowUIOnLanding();
            }

            // LightController 호출 (배경 페이드인)
            // lightController가 Inspector에서 연결되어 있다면 호출됨
            if (lightController != null)
            {
                lightController.StartFadeIn();
            }
        }
    }
}