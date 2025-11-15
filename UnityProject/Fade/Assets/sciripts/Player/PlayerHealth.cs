using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 5f;
    public float currentHealth = 5f;

    public HeartUI heartUI;
    public Animator anim;

    private bool isDead = false;

    private Vector3 startPosition;   // 시작 위치 저장

    void Start()
    {
        currentHealth = maxHealth;
        heartUI.UpdateHearts(currentHealth);

        startPosition = transform.position;  // 시작 위치 기록
    }

    // --- 데미지 받기 ---
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        heartUI.UpdateHearts(currentHealth);
    }

    // --- 체력 회복 ---
    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        heartUI.UpdateHearts(currentHealth);
    }

    // --- 죽음 처리 ---
    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        StartCoroutine(RespawnRoutine());
    }

    // --- 리스폰 코루틴 ---
    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(2f);

        // 체력 회복
        currentHealth = maxHealth;
        heartUI.UpdateHearts(currentHealth);

        // 시작 위치로 이동
        transform.position = startPosition;

        // ⭐ Idle 애니메이션으로 되돌리기
        anim.Play("DaniIdle");

        // 사망 상태 해제
        isDead = false;
    }
}
