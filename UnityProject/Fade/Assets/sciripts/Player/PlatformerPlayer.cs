
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

//    // 더블 점프
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

//        // controls는 프로젝트 설정에 따라 초기화
//        if (controls == null)
//            controls = new PlayerControls();
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

//        // 플레이어 마찰 없애기
//        PhysicsMaterial2D mat = new PhysicsMaterial2D("PlayerFriction");
//        mat.friction = 0f;
//        mat.bounciness = 0f;
//        playerCollider.sharedMaterial = mat;

//        // ⭐ Rigidbody 설정 확인: 떨림 방지를 위해 Interpolate와 Continuous로 설정해야 합니다.
//        // rb.interpolation = RigidbodyInterpolation2D.Interpolate; 
//        // rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; 
//    }


//    void Update()
//    {
//        moveInput = controls.Player.Move.ReadValue<Vector2>();

//        // ↓ + 점프 → 발판 아래로 떨어지기
//        if (controls.Player.Jump.triggered && !isDropping)
//        {
//            if (moveInput.y < -0.5f)
//            {
//                StartCoroutine(DropDownFromPlatform());
//                return;
//            }
//        }

//        // 점프 및 더블 점프
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
//    // ⭐ 수정된 이동 로직: 플레이어 자신의 속도만 Rigidbody에 적용
//    // ----------------------------------------------------------
//    private void Move()
//    {
//        Vector2 v = rb.linearVelocity;

//        // 1) 플레이어 자신의 희망 속도만 계산
//        float playerSpeed = moveInput.x * speed;

//        // 발판 속도 합산 로직은 PlayerFollowPlatform.cs가 강제 이동으로 처리하므로 제거합니다.

//        // ⭐ 핵심: X축에 플레이어 희망 속도만 적용
//        v.x = playerSpeed;

//        // Y축은 Rigidbody가 중력, 점프 등으로 알아서 처리하도록 그대로 둡니다.

//        rb.linearVelocity = v;

//        // 애니메이션
//        anim.SetFloat("Speed", Mathf.Abs(playerSpeed));

//        // 좌우 반전
//        if (Mathf.Abs(moveInput.x) > 0.01f)
//            sr.flipX = moveInput.x < 0f;
//    }


//    // ----------------------------------------------------------
//    // 더블 점프, 바닥 체크, 아래로 떨어지기, 화면 밖 이동 제한 로직은 기존대로 유지
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

    // ⭐ BLAbility에서 사용할 public 속성 추가
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

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
