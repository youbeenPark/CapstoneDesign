using UnityEngine;

public class PlayerFollowPlatform : MonoBehaviour
{
    private Transform originalParent;

    void Awake()
    {
        originalParent = transform.parent;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
        {
            if (collision.contacts[0].normal.y > 0.5f)
                transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
        {
            if (collision.contacts[0].normal.y > 0.5f)
            {
                if (transform.parent != collision.transform)
                    transform.SetParent(collision.transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
            transform.SetParent(originalParent);
    }
}
