using UnityEngine;
using System.Collections;

public class NeonHammerTrap : MonoBehaviour
{
    public float topY = 4f;           // 해머 시작 위치 (위)
    public float bottomY = -2f;       // 내려찍는 위치 (바닥)
    public float fallSpeed = 12f;     // 떨어질 때 속도
    public float riseSpeed = 3f;      // 올라갈 때 속도
    public float waitTime = 0.6f;     // 바닥 도달 후 대기 시간
    public float damage = 1f;         // 데미지

    private bool isFalling = false;

    private void Start()
    {
        // 시작 위치를 topY로 고정
        Vector3 pos = transform.position;
        pos.y = topY;
        transform.position = pos;

        // 반복 실행 시작
        StartCoroutine(HammerRoutine());
    }

    IEnumerator HammerRoutine()
    {
        while (true)
        {
            // ----- 1) 위에서 대기 -----
            yield return new WaitForSeconds(0.5f);

            // ----- 2) 아래로 떨어짐 -----
            isFalling = true;
            while (transform.position.y > bottomY)
            {
                transform.position += Vector3.down * fallSpeed * Time.deltaTime;
                yield return null;
            }

            // 도달 보정
            Vector3 pos = transform.position;
            pos.y = bottomY;
            transform.position = pos;

            // ----- 3) 바닥에서 약간 대기 -----
            isFalling = false;
            yield return new WaitForSeconds(waitTime);

            // ----- 4) 위로 천천히 올라감 -----
            while (transform.position.y < topY)
            {
                transform.position += Vector3.up * riseSpeed * Time.deltaTime;
                yield return null;
            }

            // 보정
            pos = transform.position;
            pos.y = topY;
            transform.position = pos;
        }
    }

    // 플레이어 충돌 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFalling) return; // 떨어지는 동안만 데미지

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
