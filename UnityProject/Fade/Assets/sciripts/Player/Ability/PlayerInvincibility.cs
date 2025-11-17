using UnityEngine;
using System.Collections;

public class PlayerInvincibility : MonoBehaviour
{
    public float invincibleDuration = 1f;    // 지속시간 1초
    public float cooldown = 2.5f;            // 쿨타임 2.5초

    private bool isCooldown = false;

    private SpriteRenderer sr;
    private PlayerHealth health;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // ⭐ PlayerHealth는 다른 오브젝트에 있으므로 Find로 가져와야 함
        health = FindObjectOfType<PlayerHealth>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryInvincible();
        }
    }

    void TryInvincible()
    {
        if (isCooldown) return;           // 쿨타임 중이면 X
        StartCoroutine(InvincibleRoutine());
    }

    IEnumerator InvincibleRoutine()
    {
        isCooldown = true;
        health.isInvincible = true;       // 무적 ON

        // 👉 깜빡임 효과
        float timer = 0f;
        while (timer < invincibleDuration)
        {
            sr.enabled = !sr.enabled;     // 스프라이트 on/off로 깜빡임
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        sr.enabled = true;                // 다시 정상 표시
        health.isInvincible = false;      // 무적 OFF

        // 👉 쿨타임 대기
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }
}
