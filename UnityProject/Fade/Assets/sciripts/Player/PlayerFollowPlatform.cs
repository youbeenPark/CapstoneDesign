//using UnityEngine;

//public class PlayerFollowPlatform : MonoBehaviour
//{
//    private Transform player;

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            player = collision.transform;
//            player.SetParent(transform.parent);   // PlatformLR을 따라가도록 설정
//        }
//    }

//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            if (player != null)
//                player.SetParent(null);           // 다시 원래대로
//        }
//    }
//}
using UnityEngine;

public class PlayerFollowPlatform : MonoBehaviour
{
    private Vector3 lastPos;
    private Vector2 platformVelocity;
    private bool playerOnPlatform = false;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        platformVelocity = (currentPos - lastPos) / Time.deltaTime;
        lastPos = currentPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }

    public Vector2 GetPlatformVelocity()
    {
        if (playerOnPlatform)
            return platformVelocity;

        return Vector2.zero;
    }
}

