//using UnityEngine;
//using UnityEngine.InputSystem;

//[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
//public class Player : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float speed = 5f;

//    [Header("TopDown Map Bounds")]
//    public float minX = -17f;
//    public float maxX = 17f;
//    public float minY = -9f;
//    public float maxY = 9f;

//    private Rigidbody2D rigid;
//    private SpriteRenderer spriter;
//    private Animator anim;
//    private PlayerControls controls;
//    private Vector2 inputVec;
//    private StageType stageType;

//    void Awake()
//    {
//        rigid = GetComponent<Rigidbody2D>();
//        spriter = GetComponent<SpriteRenderer>();
//        anim = GetComponent<Animator>();
//        controls = new PlayerControls();
//    }

//    void Start()
//    {
//        // StageManager에서 현재 스테이지 타입 가져오기
//        stageType = StageManager.Instance != null ? StageManager.Instance.stageType : StageType.Platformer;

//        if (stageType == StageType.TopDown)
//        {
//            // 중력 차단 및 회전 방지
//            rigid.gravityScale = 0;
//            rigid.freezeRotation = true;
//        }
//        else if (stageType == StageType.Platformer)
//        {
//            // 중력 복구
//            rigid.gravityScale = 3f;
//            rigid.freezeRotation = false;
//        }
//    }

//    void OnEnable()
//    {
//        controls.Enable();
//        controls.Player.Move.performed += OnMove;
//        controls.Player.Move.canceled += OnMove;
//    }

//    void OnDisable()
//    {
//        controls.Player.Move.performed -= OnMove;
//        controls.Player.Move.canceled -= OnMove;
//        controls.Disable();
//    }

//    private void OnMove(InputAction.CallbackContext context)
//    {
//        inputVec = context.ReadValue<Vector2>();
//    }

//    void FixedUpdate()
//    {
//        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
//        rigid.MovePosition(rigid.position + nextVec);
//    }

//    void LateUpdate()
//    {
//        anim.SetFloat("Speed", inputVec.magnitude);
//        if (inputVec.x != 0)
//            spriter.flipX = inputVec.x < 0;

//        // TopDown 맵일 때만 Clamp 적용
//        if (stageType == StageType.TopDown)
//        {
//            Vector3 pos = transform.position;
//            pos.x = Mathf.Clamp(pos.x, minX, maxX);
//            pos.y = Mathf.Clamp(pos.y, minY, maxY);
//            transform.position = pos;
//        }
//    }
//}
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRadius = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private PlayerControls controls;
    private Vector2 moveInput;

    private bool isGrounded;
    private bool isJumping;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        controls = new PlayerControls();
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        rb.gravityScale = 3f;
        rb.freezeRotation = true;
        anim.SetBool("isJumping", false);
        anim.SetFloat("Speed", 0f);
    }

    void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        if (controls.Player.Jump.triggered)
            Debug.Log($"Jump Input: triggered, isGrounded={isGrounded}");

        if (controls.Player.Jump.triggered && isGrounded)
            Jump();
    }


    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // 좌우 이동
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        // 애니메이션
        anim.SetFloat("Speed", Mathf.Abs(moveInput.x));

        // 좌우 반전
        if (Mathf.Abs(moveInput.x) > 0.01f)
            sr.flipX = moveInput.x < 0f;
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded && isJumping)
        {
            isJumping = false;
            anim.SetBool("isJumping", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
