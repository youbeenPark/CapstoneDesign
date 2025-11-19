using UnityEngine;

public class SlimeMovement : MonoBehaviour, IFreezable
{
    [Header("이동 관련")]
    public float moveSpeed = 1.5f;

    public float rightDistance = 1.5f;
    public float leftDistance = -1.5f;

    private float leftLimit;
    private float rightLimit;

    private int direction = 1;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    [Header("데미지 관련")]
    public float damage = 0.5f;

    // ⭐ Freeze 상태 저장
    private bool isFrozen = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;

        float startX = transform.position.x;

        rightLimit = startX + rightDistance;
        leftLimit = startX + leftDistance;
    }

    void Update()
    {
        // ⭐ 멈춰있으면 이동·애니·반전 처리 전부 중지
        if (isFrozen) return;

        rb.linearVelocity = new Vector2(direction * moveSpeed, 0);

        if (direction == 1 && transform.position.x >= rightLimit)
            direction = -1;
        else if (direction == -1 && transform.position.x <= leftLimit)
            direction = 1;

        sr.flipX = direction == -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFrozen) return;   // ⭐ 정지 중이면 데미지도 안 들어감

        if (collision.CompareTag("Player"))
        {
            PlayerHealth hp = collision.GetComponent<PlayerHealth>();
            if (hp == null)
            {
                hp = FindObjectOfType<PlayerHealth>();
            }

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }

    // =======================================================================
    //  ⭐⭐⭐ Freeze / Unfreeze 구현부
    // =======================================================================

    public void Freeze()
    {
        isFrozen = true;

        rb.linearVelocity = Vector2.zero;

        if (anim != null)
            anim.speed = 0f;
    }

    public void Unfreeze()
    {
        isFrozen = false;

        if (anim != null)
            anim.speed = 1f;
    }
}
