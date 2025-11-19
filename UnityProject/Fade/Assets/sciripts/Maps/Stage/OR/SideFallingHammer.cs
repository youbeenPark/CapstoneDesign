using UnityEngine;
using System.Collections;

public class SideFallingHammer: MonoBehaviour
{
    [Header("Position Settings")]
    public float leftX = -4f;        // 시작 (대기/순간이동) 위치
    public float rightX = 4f;       // 도착 (데미지 판정 끝) 위치

    [Header("Speed Settings")]
    public float moveSpeed = 12f;  // 이동 속도 (기존 fallSpeed와 유사하게 빠르게)

    [Header("Timing Settings")]
    public float startDelay = 0.5f;   // 시작 전 대기 (leftX에서)
    public float rightDelay = 0.3f;   // 도착 후 대기 (rightX에서)
    public float cycleDelay = 0.5f;   // 다음 사이클 전 대기

    [Header("Damage")]
    public float damage = 0.5f;

    // isFalling 대신 isMoving으로 대체 (데미지 판정용)
    private bool isMoving = false;

    private void Start()
    {
        // 시작 위치를 왼쪽 끝으로 고정
        Vector3 pos = transform.position;
        pos.x = leftX;
        transform.position = pos;

        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            // 1) 왼쪽 끝 (leftX)에서 대기 (기존 startDelay 역할)
            yield return new WaitForSeconds(startDelay);

            // 2) 오른쪽 끝 (rightX)으로 이동
            isMoving = true; // 이동 시작: 데미지 판정 활성화
            while (transform.position.x < rightX)
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                yield return null;
            }

            // 위치 보정
            Vector3 pos = transform.position;
            pos.x = rightX;
            transform.position = pos;

            // 3) 오른쪽 끝 (rightX)에서 대기 (기존 bottomDelay 역할)
            isMoving = false; // 데미지 판정 비활성화
            yield return new WaitForSeconds(rightDelay);

            // 4) 왼쪽 끝 (leftX)으로 순간 이동 (기존 순간이동 역할)
            pos.x = leftX;
            transform.position = pos;

            // 5) 다음 사이클 전 잠깐 대기 (기존 cycleDelay 역할)
            yield return new WaitForSeconds(cycleDelay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이동 중(isMoving이 true)일 때만 데미지 들어감 (기존 !isFalling 조건과 동일)
        if (!isMoving) return;

        if (collision.CompareTag("Player"))
        {
            // 플레이어 데미지 처리 로직 (기존 코드와 동일)
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
}