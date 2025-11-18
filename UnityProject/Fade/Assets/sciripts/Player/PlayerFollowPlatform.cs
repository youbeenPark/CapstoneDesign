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
//using UnityEngine;

//public class PlayerFollowPlatform : MonoBehaviour
//{
//    private Vector3 lastPos;
//    private Vector2 platformVelocity;
//    private bool playerOnPlatform = false;

//    void Start()
//    {
//        lastPos = transform.position;
//    }

//    void Update()
//    {
//        Vector3 currentPos = transform.position;
//        platformVelocity = (currentPos - lastPos) / Time.deltaTime;
//        lastPos = currentPos;
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.collider.CompareTag("Player"))
//        {
//            playerOnPlatform = true;
//        }
//    }

//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        if (collision.collider.CompareTag("Player"))
//        {
//            playerOnPlatform = false;
//        }
//    }

//    public Vector2 GetPlatformVelocity()
//    {
//        if (playerOnPlatform)
//            return platformVelocity;

//        return Vector2.zero;
//    }
//}

//using UnityEngine;

//public class PlayerFollowPlatform : MonoBehaviour
//{
//    private Vector3 lastPos;
//    private Vector2 platformVelocity;
//    // private bool playerOnPlatform = false; // ❌ 제거

//    void Start()
//    {
//        lastPos = transform.position;
//    }

//    void Update()
//    {
//        Vector3 currentPos = transform.position;
//        // ⭐ Update에서 발판이 이동하므로, Update에서 속도를 계산합니다.
//        platformVelocity = (currentPos - lastPos) / Time.deltaTime;
//        lastPos = currentPos;
//    }

//    // ❌ OnCollisionEnter2D 및 OnCollisionExit2D 함수 제거

//    public Vector2 GetPlatformVelocity()
//    {
//        // ⭐️ 감지 상태와 관계없이 계산된 속도를 무조건 반환합니다.
//        return platformVelocity;
//    }
//}

using UnityEngine;

public class PlayerFollowPlatform : MonoBehaviour
{
    private Vector3 lastPos;
    private Vector2 platformVelocity;

    // ⭐ 추가: 현재 발판 위에 있는 플레이어를 참조할 변수 (강제 이동 대상)
    private Transform currentRider;

    void Start()
    {
        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 currentPos = transform.position;

        // ⭐ 발판의 실제 이동량 (변위 벡터) 계산
        Vector3 displacement = currentPos - lastPos;

        // 속도 계산 (다른 발판 속도 합산 등의 백업 용도로 유지)
        platformVelocity = displacement / Time.fixedDeltaTime;

        // ⭐ 핵심: 플레이어가 발판 위에 있다면, 발판의 이동량만큼 플레이어를 옮깁니다.
        if (currentRider != null)
        {
            // Rigidbody를 건드리지 않고 Transform 위치를 직접 조정하여 떨림을 제거합니다.
            currentRider.position += displacement;
        }

        lastPos = currentPos;
    }

    // ⭐ 추가: 충돌 진입 시 플레이어를 currentRider로 설정
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentRider = collision.transform;
            // 참고: 여기서 SetParent 코드는 사용하지 않습니다.
        }
    }

    // ⭐ 추가: 충돌 해제 시 currentRider를 해제
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어가 발판에서 벗어나면 참조를 해제합니다.
            currentRider = null;
        }
    }

    public Vector2 GetPlatformVelocity()
    {
        return platformVelocity;
    }
}