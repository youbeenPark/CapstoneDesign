
//using UnityEngine;
//using UnityEngine.InputSystem;

//[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
//public class PlatformerPlayer : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    [SerializeField] private float speed = 5f;
//    [SerializeField] private float jumpForce = 10f;

//    [Header("Ground Check Settings")]
//    [SerializeField] private Transform groundCheck;
//    [SerializeField] private LayerMask groundLayer;
//    [SerializeField] private float groundRadius = 0.1f;

//    [Header("Down Jump Settings")]
//    [SerializeField] private float dropDownDisableTime = 0.25f;

//    // Clamp ON/OFF
//    [HideInInspector] public bool allowClamp = true;

//    private Rigidbody2D rb;
//    private SpriteRenderer sr;
//    private Animator anim;
//    private PlayerControls controls;
//    private Collider2D playerCollider;

//    private Vector2 moveInput;
//    private bool isGrounded;
//    private bool isJumping;
//    private bool isDropping = false;

//    // ⭐ 더블 점프
//    private int jumpCount = 0;
//    private int maxJumps = 2;

//    [Header("Clamp Bounds")]
//    [SerializeField] private float minX;
//    [SerializeField] private float maxX;
//    [SerializeField] private float minY;
//    [SerializeField] private float maxY;


//    void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        sr = GetComponent<SpriteRenderer>();
//        anim = GetComponent<Animator>();
//        playerCollider = GetComponent<Collider2D>();

//        controls = new PlayerControls();
//    }

//    void OnEnable()
//    {
//        if (controls == null)
//            controls = new PlayerControls();
//        controls.Enable();
//    }

//    void OnDisable()
//    {
//        if (controls != null)
//            controls.Disable();
//    }

//    void Start()
//    {
//        rb.gravityScale = 3f;
//        rb.freezeRotation = true;

//        anim.SetBool("isJumping", false);
//        anim.SetFloat("Speed", 0);

//        // ⭐ 플레이어 마찰 없애기
//        PhysicsMaterial2D mat = new PhysicsMaterial2D("PlayerFriction");
//        mat.friction = 0f;
//        mat.bounciness = 0f;
//        playerCollider.sharedMaterial = mat;
//    }


//    void Update()
//    {
//        moveInput = controls.Player.Move.ReadValue<Vector2>();

//        // ⭐ ↓ + 점프 → 발판 아래로 떨어지기
//        if (controls.Player.Jump.triggered && !isDropping)
//        {
//            if (moveInput.y < -0.5f)
//            {
//                StartCoroutine(DropDownFromPlatform());
//                return;
//            }
//        }

//        // ⭐ 점프 및 더블 점프
//        if (controls.Player.Jump.triggered)
//        {
//            TryJump();
//        }
//    }


//    void FixedUpdate()
//    {
//        Move();
//        CheckGround();
//    }


//    // ----------------------------------------------------------
//    // ⭐ 이동: 물리 기반 플랫폼 이동(AAA 방식)
//    // ----------------------------------------------------------
//    //private void Move()
//    //{
//    //    Vector2 v = rb.linearVelocity;

//    //    // 1) 플레이어 자신의 속도
//    //    float playerSpeed = moveInput.x * speed;

//    //    // 2) 플랫폼 속도
//    //    float platformSpeed = 0f;

//    //    // 3) 발 밑으로 Raycast 날려서 발판 탐지
//    //    RaycastHit2D hit = Physics2D.Raycast(
//    //        transform.position,
//    //        Vector2.down,
//    //        1.2f,
//    //        groundLayer
//    //    );

//    //    if (hit.collider != null)
//    //    {
//    //        PlayerFollowPlatform follow = hit.collider.GetComponentInParent<PlayerFollowPlatform>();
//    //        if (follow != null)
//    //        {
//    //            platformSpeed = follow.GetPlatformVelocity().x;
//    //        }
//    //    }

//    //    // ⭐ 4) 핵심: 플레이어 속도 + 플랫폼 이동 속도 합산
//    //    v.x = playerSpeed + platformSpeed;

//    //    rb.linearVelocity = v;

//    //    // ======================================================
//    //    // 애니메이션
//    //    // ======================================================
//    //    anim.SetFloat("Speed", Mathf.Abs(playerSpeed));

//    //    // 좌우 반전
//    //    if (Mathf.Abs(moveInput.x) > 0.01f)
//    //        sr.flipX = moveInput.x < 0f;
//    //}

//    private void Move()
//    {
//        Vector2 v = rb.linearVelocity;

//        float playerSpeed = moveInput.x * speed;
//        float platformSpeed = 0f;

//        // !!! 반드시 BoxCast로 감지!
//        RaycastHit2D hit = Physics2D.BoxCast(
//            playerCollider.bounds.center,
//            new Vector2(playerCollider.bounds.size.x * 0.9f, 0.1f),
//            0f,
//            Vector2.down,
//            0.4f,    // 기존 0.2f → 0.4f 또는 0.5f 추천
//            groundLayer
//        );

//        if (hit.collider != null)
//        {
//            PlayerFollowPlatform follow = hit.collider.GetComponentInParent<PlayerFollowPlatform>();
//            if (follow != null)
//            {
//                platformSpeed = follow.GetPlatformVelocity().x;
//            }
//        }

//        v.x = playerSpeed + platformSpeed;

//        rb.linearVelocity = v;

//        // 애니메이션
//        anim.SetFloat("Speed", Mathf.Abs(playerSpeed));

//        // 좌우 반전
//        if (Mathf.Abs(moveInput.x) > 0.01f)
//            sr.flipX = moveInput.x < 0f;
//    }


//    // ----------------------------------------------------------
//    // 더블 점프 처리
//    // ----------------------------------------------------------
//    private void TryJump()
//    {
//        if (isGrounded)
//            jumpCount = 0;

//        if (jumpCount < maxJumps)
//        {
//            jumpCount++;
//            Jump();
//        }
//    }

//    private void Jump()
//    {
//        Vector2 v = rb.linearVelocity;
//        v.y = 0;
//        rb.linearVelocity = v;

//        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

//        isJumping = true;
//        anim.SetBool("isJumping", true);
//    }


//    // ----------------------------------------------------------
//    // 바닥 체크
//    // ----------------------------------------------------------
//    private void CheckGround()
//    {
//        if (isDropping)
//        {
//            isGrounded = false;
//            return;
//        }

//        bool wasGrounded = isGrounded;

//        isGrounded =
//            Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

//        if (isGrounded)
//        {
//            if (!wasGrounded)
//                jumpCount = 0;

//            isJumping = false;
//            anim.SetBool("isJumping", false);
//        }
//    }


//    // ----------------------------------------------------------
//    // 아래로 떨어지기
//    // ----------------------------------------------------------
//    private System.Collections.IEnumerator DropDownFromPlatform()
//    {
//        isDropping = true;

//        int playerLayerIndex = LayerMask.NameToLayer("Player");
//        int groundLayerIndex = LayerMask.NameToLayer("Ground");

//        Physics2D.IgnoreLayerCollision(playerLayerIndex, groundLayerIndex, true);
//        yield return new WaitForSeconds(dropDownDisableTime);
//        Physics2D.IgnoreLayerCollision(playerLayerIndex, groundLayerIndex, false);

//        isDropping = false;
//    }


//    // ----------------------------------------------------------
//    // 화면 밖 이동 제한
//    // ----------------------------------------------------------
//    private void LateUpdate()
//    {
//        if (!allowClamp) return;

//        Vector3 pos = transform.position;
//        pos.x = Mathf.Clamp(pos.x, minX, maxX);
//        pos.y = Mathf.Clamp(pos.y, minY, maxY);
//        transform.position = pos;
//    }


//    void OnDrawGizmosSelected()
//    {
//        if (groundCheck != null)
//        {
//            Gizmos.color = Color.yellow;
//            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
//        }
//    }


//    public bool IsGrounded => isGrounded;
//}

//using UnityEngine;
//using UnityEngine.InputSystem;

//[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
//public class PlatformerPlayer : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    [SerializeField] private float speed = 5f;
//    [SerializeField] private float jumpForce = 10f;

//    [Header("Ground Check Settings")]
//    [SerializeField] private Transform groundCheck;
//    [SerializeField] private LayerMask groundLayer;
//    [SerializeField] private float groundRadius = 0.1f;

//    [Header("Down Jump Settings")]
//    [SerializeField] private float dropDownDisableTime = 0.25f;

//    // Clamp ON/OFF
//    [HideInInspector] public bool allowClamp = true;

//    private Rigidbody2D rb;
//    private SpriteRenderer sr;
//    private Animator anim;
//    private PlayerControls controls;
//    private Collider2D playerCollider;

//    private Vector2 moveInput;
//    private bool isGrounded;
//    private bool isJumping;
//    private bool isDropping = false;

//    // ⭐ 더블 점프
//    private int jumpCount = 0;
//    private int maxJumps = 2;

//    [Header("Clamp Bounds")]
//    [SerializeField] private float minX;
//    [SerializeField] private float maxX;
//    [SerializeField] private float minY;
//    [SerializeField] private float maxY;


//    void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        sr = GetComponent<SpriteRenderer>();
//        anim = GetComponent<Animator>();
//        playerCollider = GetComponent<Collider2D>();

//        // controls = new PlayerControls(); // 이 부분은 프로젝트 설정에 따라 다를 수 있습니다.
//    }

//    void OnEnable()
//    {
//        // controls가 null일 때만 새로 생성하도록 수정
//        if (controls == null)
//            controls = new PlayerControls();
//        controls.Enable();
//    }

//    void OnDisable()
//    {
//        if (controls != null)
//            controls.Disable();
//    }

//    void Start()
//    {
//        rb.gravityScale = 3f;
//        rb.freezeRotation = true;

//        anim.SetBool("isJumping", false);
//        anim.SetFloat("Speed", 0);

//        // ⭐ 플레이어 마찰 없애기
//        PhysicsMaterial2D mat = new PhysicsMaterial2D("PlayerFriction");
//        mat.friction = 0f;
//        mat.bounciness = 0f;
//        playerCollider.sharedMaterial = mat;
//    }


//    void Update()
//    {
//        moveInput = controls.Player.Move.ReadValue<Vector2>();

//        // ⭐ ↓ + 점프 → 발판 아래로 떨어지기
//        if (controls.Player.Jump.triggered && !isDropping)
//        {
//            if (moveInput.y < -0.5f)
//            {
//                StartCoroutine(DropDownFromPlatform());
//                return;
//            }
//        }

//        // ⭐ 점프 및 더블 점프
//        if (controls.Player.Jump.triggered)
//        {
//            TryJump();
//        }
//    }


//    void FixedUpdate()
//    {
//        Move();
//        CheckGround();
//    }


//    // ----------------------------------------------------------
//    // ⭐ 수정된 이동 로직: 플랫폼 속도 합산 (부모-자식 관계 없음)
//    // ----------------------------------------------------------
//    private void Move()
//    {
//        Vector2 v = rb.linearVelocity;

//        // 1) 플레이어 자신의 희망 속도
//        float playerSpeed = moveInput.x * speed;
//        float platformSpeed = 0f;

//        // 2) 발 밑 BoxCast로 이동 발판 감지
//        // GroundCheck는 플레이어 발 밑에 위치하는 Transform을 사용하는 것이 일반적입니다.
//        // 여기서는 BoxCast의 위치를 groundCheck.position으로 변경하여 정확도를 높입니다.
//        RaycastHit2D hit = Physics2D.BoxCast(
//            groundCheck.position, // ⭐ groundCheck 위치를 기준으로
//            new Vector2(playerCollider.bounds.size.x * 0.9f, 0.1f),
//            0f,
//            Vector2.down,
//            groundRadius * 2f, // ⭐ groundCheck Radius를 기반으로 거리 설정
//            groundLayer
//        );

//        // Debug.DrawRay(groundCheck.position, Vector2.down * groundRadius * 2f, Color.red); // 디버깅용

//        if (hit.collider != null)
//        {
//            // hit.collider에서 PlayerFollowPlatform 컴포넌트를 찾습니다.
//            PlayerFollowPlatform follow = hit.collider.GetComponent<PlayerFollowPlatform>();

//            // 발판 자체가 PlatformMover와 PlayerFollowPlatform을 가지고 있으므로,
//            // GetComponentInParent 대신 GetComponent로 시도합니다.
//            if (follow == null)
//                follow = hit.collider.GetComponentInParent<PlayerFollowPlatform>();


//            if (follow != null)
//            {
//                // 발판의 속도를 가져옵니다.
//                platformSpeed = follow.GetPlatformVelocity().x;
//                // Debug.Log($"Platform Velocity: {platformSpeed}");
//            }
//        }

//        // ⭐ 3) 핵심: 플레이어 속도 + 플랫폼 이동 속도 합산
//        v.x = playerSpeed + platformSpeed;

//        rb.linearVelocity = v;

//        // ======================================================
//        // 애니메이션 및 좌우 반전
//        // ======================================================
//        anim.SetFloat("Speed", Mathf.Abs(playerSpeed));

//        if (Mathf.Abs(moveInput.x) > 0.01f)
//            sr.flipX = moveInput.x < 0f;
//    }


//    // ----------------------------------------------------------
//    // 더블 점프 처리
//    // ----------------------------------------------------------
//    private void TryJump()
//    {
//        if (isGrounded)
//            jumpCount = 0;

//        if (jumpCount < maxJumps)
//        {
//            jumpCount++;
//            Jump();
//        }
//    }

//    private void Jump()
//    {
//        Vector2 v = rb.linearVelocity;
//        v.y = 0;
//        rb.linearVelocity = v;

//        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

//        isJumping = true;
//        anim.SetBool("isJumping", true);
//    }


//    // ----------------------------------------------------------
//    // 바닥 체크 (기존 OverlapCircle 유지)
//    // ----------------------------------------------------------
//    private void CheckGround()
//    {
//        if (isDropping)
//        {
//            isGrounded = false;
//            return;
//        }

//        bool wasGrounded = isGrounded;

//        isGrounded =
//            Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

//        if (isGrounded)
//        {
//            if (!wasGrounded)
//                jumpCount = 0;

//            isJumping = false;
//            anim.SetBool("isJumping", false);
//        }
//    }


//    // ----------------------------------------------------------
//    // 아래로 떨어지기
//    // ----------------------------------------------------------
//    private System.Collections.IEnumerator DropDownFromPlatform()
//    {
//        isDropping = true;

//        int playerLayerIndex = LayerMask.NameToLayer("Player");
//        int groundLayerIndex = LayerMask.NameToLayer("Ground");

//        Physics2D.IgnoreLayerCollision(playerLayerIndex, groundLayerIndex, true);
//        yield return new WaitForSeconds(dropDownDisableTime);
//        Physics2D.IgnoreLayerCollision(playerLayerIndex, groundLayerIndex, false);

//        isDropping = false;
//    }


//    // ----------------------------------------------------------
//    // 화면 밖 이동 제한
//    // ----------------------------------------------------------
//    private void LateUpdate()
//    {
//        if (!allowClamp) return;

//        Vector3 pos = transform.position;
//        pos.x = Mathf.Clamp(pos.x, minX, maxX);
//        pos.y = Mathf.Clamp(pos.y, minY, maxY);
//        transform.position = pos;
//    }


//    void OnDrawGizmosSelected()
//    {
//        if (groundCheck != null)
//        {
//            Gizmos.color = Color.yellow;
//            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
//        }
//    }


//    public bool IsGrounded => isGrounded;
//}

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlatformerPlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.1f;

    [Header("Down Jump Settings")]
    [SerializeField] private float dropDownDisableTime = 0.25f;

    // Clamp ON/OFF
    [HideInInspector] public bool allowClamp = true;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private PlayerControls controls;
    private Collider2D playerCollider;

    private Vector2 moveInput;
    private bool isGrounded;
    private bool isJumping;
    private bool isDropping = false;

    // 더블 점프
    private int jumpCount = 0;
    private int maxJumps = 2;

    [Header("Clamp Bounds")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

        // controls는 프로젝트 설정에 따라 초기화
        if (controls == null)
            controls = new PlayerControls();
    }

    void OnEnable()
    {
        if (controls == null)
            controls = new PlayerControls();
        controls.Enable();
    }

    void OnDisable()
    {
        if (controls != null)
            controls.Disable();
    }

    void Start()
    {
        rb.gravityScale = 3f;
        rb.freezeRotation = true;

        anim.SetBool("isJumping", false);
        anim.SetFloat("Speed", 0);

        // 플레이어 마찰 없애기
        PhysicsMaterial2D mat = new PhysicsMaterial2D("PlayerFriction");
        mat.friction = 0f;
        mat.bounciness = 0f;
        playerCollider.sharedMaterial = mat;

        // ⭐ Rigidbody 설정 확인: 떨림 방지를 위해 Interpolate와 Continuous로 설정해야 합니다.
        // rb.interpolation = RigidbodyInterpolation2D.Interpolate; 
        // rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; 
    }


    void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        // ↓ + 점프 → 발판 아래로 떨어지기
        if (controls.Player.Jump.triggered && !isDropping)
        {
            if (moveInput.y < -0.5f)
            {
                StartCoroutine(DropDownFromPlatform());
                return;
            }
        }

        // 점프 및 더블 점프
        if (controls.Player.Jump.triggered)
        {
            TryJump();
        }
    }


    void FixedUpdate()
    {
        Move();
        CheckGround();
    }


    // ----------------------------------------------------------
    // ⭐ 수정된 이동 로직: 플레이어 자신의 속도만 Rigidbody에 적용
    // ----------------------------------------------------------
    private void Move()
    {
        Vector2 v = rb.linearVelocity;

        // 1) 플레이어 자신의 희망 속도만 계산
        float playerSpeed = moveInput.x * speed;

        // 발판 속도 합산 로직은 PlayerFollowPlatform.cs가 강제 이동으로 처리하므로 제거합니다.

        // ⭐ 핵심: X축에 플레이어 희망 속도만 적용
        v.x = playerSpeed;

        // Y축은 Rigidbody가 중력, 점프 등으로 알아서 처리하도록 그대로 둡니다.

        rb.linearVelocity = v;

        // 애니메이션
        anim.SetFloat("Speed", Mathf.Abs(playerSpeed));

        // 좌우 반전
        if (Mathf.Abs(moveInput.x) > 0.01f)
            sr.flipX = moveInput.x < 0f;
    }


    // ----------------------------------------------------------
    // 더블 점프, 바닥 체크, 아래로 떨어지기, 화면 밖 이동 제한 로직은 기존대로 유지
    // ----------------------------------------------------------

    private void TryJump()
    {
        if (isGrounded)
            jumpCount = 0;

        if (jumpCount < maxJumps)
        {
            jumpCount++;
            Jump();
        }
    }

    private void Jump()
    {
        Vector2 v = rb.linearVelocity;
        v.y = 0;
        rb.linearVelocity = v;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        isJumping = true;
        anim.SetBool("isJumping", true);
    }


    private void CheckGround()
    {
        if (isDropping)
        {
            isGrounded = false;
            return;
        }

        bool wasGrounded = isGrounded;

        isGrounded =
            Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded)
        {
            if (!wasGrounded)
                jumpCount = 0;

            isJumping = false;
            anim.SetBool("isJumping", false);
        }
    }


    private System.Collections.IEnumerator DropDownFromPlatform()
    {
        isDropping = true;

        int playerLayerIndex = LayerMask.NameToLayer("Player");
        int groundLayerIndex = LayerMask.NameToLayer("Ground");

        Physics2D.IgnoreLayerCollision(playerLayerIndex, groundLayerIndex, true);
        yield return new WaitForSeconds(dropDownDisableTime);
        Physics2D.IgnoreLayerCollision(playerLayerIndex, groundLayerIndex, false);

        isDropping = false;
    }


    private void LateUpdate()
    {
        if (!allowClamp) return;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }


    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }


    public bool IsGrounded => isGrounded;
}