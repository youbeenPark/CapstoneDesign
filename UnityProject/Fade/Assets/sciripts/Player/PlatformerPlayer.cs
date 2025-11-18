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
//    [SerializeField] private float dropDownDisableTime = 0.25f;   // 발판 아래로 떨어지기 시간

//    private Rigidbody2D rb;
//    private SpriteRenderer sr;
//    private Animator anim;
//    private PlayerControls controls;
//    private Vector2 moveInput;

//    private bool isGrounded;
//    private bool isJumping;
//    private bool isDropping = false;

//    [SerializeField] private float minX;
//    [SerializeField] private float maxX;
//    [SerializeField] private float minY;
//    [SerializeField] private float maxY;

//    private Collider2D playerCollider;

//    private void LateUpdate()
//    {
//        // 화면 밖으로 못 나가게 Clamp
//        Vector3 pos = transform.position;
//        pos.x = Mathf.Clamp(pos.x, minX, maxX);
//        pos.y = Mathf.Clamp(pos.y, minY, maxY);
//        transform.position = pos;
//    }

//    void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        sr = GetComponent<SpriteRenderer>();
//        anim = GetComponent<Animator>();
//        controls = new PlayerControls();
//        playerCollider = GetComponent<Collider2D>();
//    }

//    void OnEnable() => controls.Enable();
//    void OnDisable() => controls.Disable();

//    void Start()
//    {
//        rb.gravityScale = 3f;
//        rb.freezeRotation = true;
//        anim.SetBool("isJumping", false);
//        anim.SetFloat("Speed", 0f);
//    }

//    void Update()
//    {
//        moveInput = controls.Player.Move.ReadValue<Vector2>();

//        if (controls.Player.Jump.triggered)
//            Debug.Log($"Jump pressed! isGrounded={isGrounded}");

//        // ⭐ 아래로 떨어지기 : ↓ + Jump
//        if (controls.Player.Jump.triggered && !isDropping)
//        {
//            if (moveInput.y < -0.5f)
//            {
//                StartCoroutine(DropDownFromPlatform());
//                return;
//            }
//        }

//        // ⭐ 일반 점프
//        if (controls.Player.Jump.triggered && isGrounded)
//            Jump();
//    }

//    void FixedUpdate()
//    {
//        Move();
//        CheckGround();
//    }

//    private void Move()
//    {
//        // 좌우 이동
//        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

//        // 애니메이션
//        anim.SetFloat("Speed", Mathf.Abs(moveInput.x));

//        // 좌우 반전
//        if (Mathf.Abs(moveInput.x) > 0.01f)
//            sr.flipX = moveInput.x < 0f;
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

//        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

//        Debug.Log($"isGrounded={isGrounded}, isJumping={isJumping}");

//        if (isGrounded && isJumping)
//        {
//            isJumping = false;
//            anim.SetBool("isJumping", false);
//        }
//    }


//    // ⭐ 수정된 One-Way Platform 아래로 떨어지기 기능 (Collider OFF 제거)
//    private System.Collections.IEnumerator DropDownFromPlatform()
//    {
//        isDropping = true;

//        // Player와 Ground Layer 충돌 무시 (플레이어는 Collider를 계속 켠 상태)
//        int playerLayer = LayerMask.NameToLayer("Player");
//        int groundLayerIndex = LayerMask.NameToLayer("Ground");

//        Physics2D.IgnoreLayerCollision(playerLayer, groundLayerIndex, true);

//        yield return new WaitForSeconds(dropDownDisableTime);

//        // 다시 충돌 활성화
//        Physics2D.IgnoreLayerCollision(playerLayer, groundLayerIndex, false);

//        isDropping = false;
//    }

//    private void OnDrawGizmosSelected()
//    {
//        if (groundCheck != null)
//        {
//            Gizmos.color = Color.yellow;
//            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
//        }
//    }

//    // 길우진이 추가함 - 다른 스크립트에서 플레이어가 땅에 닿아있는지 확인할 수 있도록
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

    // ⭐ Clamp ON/OFF 스위치 추가 — PlayerHealth가 이걸 제어함
    [HideInInspector] public bool allowClamp = true;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private PlayerControls controls;
    private Vector2 moveInput;

    private bool isGrounded;
    private bool isJumping;
    private bool isDropping = false;

    // ⭐ 더블 점프
    private int jumpCount = 0;
    private int maxJumps = 2;

    // ⭐ 화면 밖 못 나가게
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private Collider2D playerCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        controls = new PlayerControls();
        playerCollider = GetComponent<Collider2D>();
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

        // 플레이어 마찰 설정
        PhysicsMaterial2D mat = new PhysicsMaterial2D("PlayerFriction");
        mat.friction = 0f;
        mat.bounciness = 0f;

        playerCollider.sharedMaterial = mat;
    }

    void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        if (controls.Player.Jump.triggered)
        {
            Debug.Log($"Jump Pressed / Grounded={isGrounded} / Jumps={jumpCount}");
        }

        // ↓ + Jump
        if (controls.Player.Jump.triggered && !isDropping)
        {
            if (moveInput.y < -0.5f)
            {
                StartCoroutine(DropDownFromPlatform());
                return;
            }
        }

        // 점프 & 더블 점프
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

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
        anim.SetFloat("Speed", Mathf.Abs(moveInput.x));

        if (Mathf.Abs(moveInput.x) > 0.01f)
            sr.flipX = moveInput.x < 0;
    }

    private void TryJump()
    {
        if (isGrounded)
        {
            jumpCount = 0;
        }

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

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        Debug.Log($"isGrounded={isGrounded}, isJumping={isJumping}, jumpCount={jumpCount}");

        if (isGrounded)
        {
            if (!wasGrounded)
            {
                jumpCount = 0;
            }

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
        // ⭐ PlayerHealth가 이동시키는 1~2프레임 동안은 Clamp 꺼짐
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
