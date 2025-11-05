using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool jumpPressed;
    private bool isDead = false;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void OnMove(InputValue value)
    {
        if (isDead) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (isDead) return;
        if (value.isPressed)
            jumpPressed = true;
    }

    void FixedUpdate()
    {
        if (isDead) return;

        // 이동
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        // 방향 전환
        if (moveInput.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1);

        // 점프
        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
        }

        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 애니메이션 갱신
        animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
        animator.SetBool("isJumping", !isGrounded);
    }

    void Update()
    {
        // 사망 시 ESC키로 재시작
        if (isDead && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            RestartStage();
        }
    }

    // --- 사망 처리 ---
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        animator.SetBool("isDead", true);
        Debug.Log("💀 다니 사망! (ESC키로 재시작 가능)");
    }

    private void RestartStage()
    {
        string scene = SceneManager.GetActiveScene().name;
        Debug.Log($"🔁 스테이지 재시작: {scene}");
        SceneManager.LoadScene(scene);
    }
}
