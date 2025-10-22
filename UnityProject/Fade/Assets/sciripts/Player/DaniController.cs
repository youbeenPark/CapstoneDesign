using UnityEngine;
// using System.Collections; // ⭐ 제거 (코루틴 사용 안 함) ⭐

public class DaniController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool hasLanded = false;

    [Header("Glide Settings")]
    [SerializeField] private float fallSpeed = -1.2f;      // 낙하 속도 (느릴수록 천천히, 음수 값)
    [SerializeField] private float horizontalRange = 0.5f;   // 좌우 이동 폭
    [SerializeField] private float horizontalSpeed = 1.2f;   // 좌우 이동 주기 (주파수)
    [SerializeField] private float verticalWaveStrength = 0.15f; // 상하 부드러운 흔들림 강도
    [SerializeField] private float rotateAngle = 10f;      // 회전 각도 (좌우 흔들릴 때 기울기)

    [Header("Landing Adjustment")]
    [SerializeField] private float horizontalOffsetStart = 3f;
    [SerializeField] private float horizontalGlideSpeed = 1f;

    // ⭐ 추가/복구된 변수 ⭐
    [Header("Animation Control")]
    [SerializeField] private float failAnimationSpeed = 0.5f; // DaniFail 재생 속도 (DaniFail 상태에 사용)

    [Header("Movement After Landing")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private Transform wallTarget;
    private bool isWalkingToWall = false;

    private Vector3 startPos;
    private float timer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f; // 직접 낙하 제어
        startPos = transform.position;
    }

    // ⭐ StateMachineBehaviour에서 호출하여 걷기 로직을 켜는 Setter 메서드 (필수 유지) ⭐
    public void SetIsWalkingToWall(bool isWalking)
    {
        isWalkingToWall = isWalking;

        // ⭐ WalkToWall 상태에 진입/탈출할 때 애니메이션 강제 전환 (StateMachineBehaviour 로직을 대체함) ⭐
        // 이 부분은 StartWalkOnEntry.cs가 DaniWalk 상태에 부착되어 Animator의 전환을 따르도록 합니다.
        // DaniWalk 클립이 재생되도록 animator.Play()를 사용할 필요는 없습니다.
        // 다만, 걷기 멈춤 시 Idle로 전환하는 로직은 여기에 추가해야 합니다.
        if (!isWalking)
        {
            // animator.Play("DaniIdle"); // 걷기 멈춤 시 Idle로 전환
        }
    }

    void Update()
    {
        // 1. 낙하 중일 때만 흔들림 로직 실행
        if (!hasLanded)
        {
            timer += Time.deltaTime;
            // ... (기존의 좌우 흔들림, 회전, 위치 계산 로직 유지) ...
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
        }

        // 2. 착지 후 걷는 동작 실행
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
                SetIsWalkingToWall(false);
            }
        }
    }

    // ⭐ OnCollisionEnter2D 메서드 복구 (Animator 전환 방식 사용) ⭐
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !hasLanded)
        {
            hasLanded = true;
            rb.gravityScale = 1f;

            // ⭐ animator.Play() 대신 isLanded 파라미터만 설정 (Animator 전환 사용) ⭐
            animator.SetBool("isLanded", true);

            // DaniFail 상태 진입 시 속도 설정 (LandingCleanup 스크립트에 이 로직이 들어가야 더 깔끔)
            // 임시로 여기에 둡니다.
            animator.speed = failAnimationSpeed;

            // 캐릭터 기울어짐 방지 및 오른쪽 방향 고정
            transform.rotation = Quaternion.identity;
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = false;
            }

            FindObjectOfType<LightController>()?.StartFadeIn();
        }
    }
}