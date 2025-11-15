using UnityEngine;

public class SlimeMovement : MonoBehaviour
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

    [Header("데미지 관련")]
    public float damage = 0.5f;   // 🔥 반칸 데미지

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;

        float startX = transform.position.x;

        rightLimit = startX + rightDistance;
        leftLimit = startX + leftDistance;
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, 0);

        if (direction == 1 && transform.position.x >= rightLimit)
            direction = -1;
        else if (direction == -1 && transform.position.x <= leftLimit)
            direction = 1;

        sr.flipX = direction == -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth hp = collision.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }
}
