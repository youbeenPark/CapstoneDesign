using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 5f;
    public float currentHealth = 5f;

    public HeartUI heartUI;
    public Animator anim;

    private static PlayerHealth instance;   // 씬 전환에서 유지되는 싱글톤
    private bool isDead = false;

    private Vector3 startPosition;

    void Awake()
    {
        // PlayerHealth 중복 생성 방지
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);   // 씬 넘어가도 유지
    }

    void Start()
    {
        startPosition = transform.position;

        if (heartUI == null)
            heartUI = FindObjectOfType<HeartUI>();

        if (heartUI != null)
            heartUI.UpdateHearts(currentHealth);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // --- 씬 넘어갈 때 자동 연결 ---
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬에서 새 Player 찾기
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            anim = playerObj.GetComponent<Animator>();
            transform.position = playerObj.transform.position;  // 위치도 새 플레이어 기준으로
        }

        // HeartUI 자동 재연결
        if (heartUI == null)
            heartUI = FindObjectOfType<HeartUI>();

        if (heartUI != null)
            heartUI.UpdateHearts(currentHealth);
    }

    // --- 데미지 ---
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

    // --- 회복 ---
    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        heartUI.UpdateHearts(currentHealth);
    }

    // --- 죽음 ---
    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");

        // 리스폰 코루틴 실행
        StartCoroutine(RespawnRoutine());
    }

    // --- 리스폰 ---
    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(2f);

        // 체력 풀 회복
        currentHealth = maxHealth;
        heartUI.UpdateHearts(currentHealth);

        // 시작 위치로 이동
        transform.position = startPosition;

        // Idle로 복귀
        anim.Play("DaniIdle");

        // 죽음 상태 해제
        isDead = false;
    }
}
