using UnityEngine;
using System.Collections;

public class NeonHammerTrap : MonoBehaviour
{
    [Header("Position Settings")]
    public float topY = 4f;        // 시작 위치
    public float bottomY = -2f;    // 떨어질 위치

    [Header("Speed Settings")]
    public float fallSpeed = 12f;  // 떨어지는 속도

    [Header("Timing Settings")]
    public float startDelay = 0.5f;
    public float bottomDelay = 0.3f;
    public float cycleDelay = 0.5f;

    [Header("Damage")]
    public float damage = 0.5f;

    private bool isFalling = false;

    private void Start()
    {
        // 시작 위치 고정
        Vector3 pos = transform.position;
        pos.y = topY;
        transform.position = pos;

        StartCoroutine(HammerRoutine());
    }

    IEnumerator HammerRoutine()
    {
        while (true)
        {
            // 1) 위에서 대기
            yield return new WaitForSeconds(startDelay);

            // 2) 아래로 낙하
            isFalling = true;
            while (transform.position.y > bottomY)
            {
                transform.position += Vector3.down * fallSpeed * Time.deltaTime;
                yield return null;
            }

            // 위치 보정
            Vector3 pos = transform.position;
            pos.y = bottomY;
            transform.position = pos;

            // 3) 바닥에서 대기
            isFalling = false;
            yield return new WaitForSeconds(bottomDelay);

            // 4) 순간 이동 (올라가는 애니 없음)
            pos.y = topY;
            transform.position = pos;

            // 5) 다음 사이클 전 잠깐 대기
            yield return new WaitForSeconds(cycleDelay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFalling) return;  // 떨어질 때만 데미지 들어감

        if (collision.CompareTag("Player"))
        {
            // 🔥 슬라임 방식 그대로 적용
            PlayerHealth hp = collision.GetComponent<PlayerHealth>();
            if (hp == null)
            {
                hp = FindObjectOfType<PlayerHealth>();  // 항상 PlayerHealth 싱글톤 찾음
            }

            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }

    }

}
