using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;

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

    [SerializeField] private float minX ;
    [SerializeField] private float maxX ;
    [SerializeField] private float minY ;
    [SerializeField] private float maxY ;

    private void LateUpdate()
    {
        // 화면 밖으로 못 나가게 Clamp
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

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
            Debug.Log($"Jump pressed! isGrounded={isGrounded}");


        if (controls.Player.Jump.triggered && isGrounded)
            Jump();
    }


    void FixedUpdate()
    {
        Move();
        CheckGround();
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

        Debug.Log($"isGrounded={isGrounded}, isJumping={isJumping}");

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