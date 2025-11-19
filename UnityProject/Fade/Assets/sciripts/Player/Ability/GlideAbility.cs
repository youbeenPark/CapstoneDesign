using UnityEngine;

public class GlideAbility : MonoBehaviour
{
    [Header("Glide Settings")]
    public float glideGravity = 0.4f;     // 활공 중 중력
    public float normalGravity = 0f;      // 평소 중력 (Start에서 자동으로 설정)
    public float maxGlideTime = 3f;       // 활공 지속 시간

    private float glideTimer;
    private bool isGliding = false;
    private bool canGlide = true;

    private Rigidbody2D rb;
    private PlatformerPlayer playerMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlatformerPlayer>();

        // 플레이어가 사용 중인 실제 중력 자동 저장
        normalGravity = rb.gravityScale;

        glideTimer = maxGlideTime;
    }

    void Update()
    {
        HandleGlideInput();
        HandleGlideTimer();
        HandleGroundReset();
    }

    // ============================
    //     입력 처리
    // ============================
    void HandleGlideInput()
    {
        // 땅에 있으면 활공 X
        if (playerMove.IsGrounded)
            return;

        // T 키 누르면 활공 시작
        if (Input.GetKeyDown(KeyCode.T) && canGlide)
            StartGlide();

        // T 키 떼면 활공 중단
        if (Input.GetKeyUp(KeyCode.T) && isGliding)
            StopGlide();
    }

    // ============================
    //     활공 시작
    // ============================
    void StartGlide()
    {
        isGliding = true;
        canGlide = false;
        glideTimer = maxGlideTime;

        rb.gravityScale = glideGravity;

        // 너무 빠르게 떨어지는 중이면 감속
        if (rb.linearVelocity.y < -5f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -5f);
    }

    // ============================
    //     활공 종료
    // ============================
    void StopGlide()
    {
        isGliding = false;
        rb.gravityScale = normalGravity;
    }

    // ============================
    //     활공 시간 감소
    // ============================
    void HandleGlideTimer()
    {
        if (!isGliding)
            return;

        glideTimer -= Time.deltaTime;

        if (glideTimer <= 0)
            StopGlide();
    }

    // ============================
    //     땅 닿으면 리셋
    // ============================
    void HandleGroundReset()
    {
        if (!playerMove.IsGrounded)
            return;

        isGliding = false;
        canGlide = true;

        glideTimer = maxGlideTime;
        rb.gravityScale = normalGravity;
    }
}
