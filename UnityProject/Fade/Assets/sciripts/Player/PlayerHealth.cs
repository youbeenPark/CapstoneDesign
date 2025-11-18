using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 5f;
    public float currentHealth = 5f;

    public HeartUI heartUI;
    public Animator anim;

    public bool isInvincible = false;

    public static PlayerHealth instance;

    private bool isDead = false;
    private Vector3 startPosition;

    void Awake()
    {
        // 싱글톤 유지
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        startPosition = transform.position;
        ConnectHeartUI();
        UpdateHeartUI();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ===========================
    //     SCENE LOADED LOGIC
    // ===========================
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        // 1) TUTO 처리
        if (sceneName == "TUTO")
        {
            GameObject platformer = GameObject.Find("PlatformerPlayer");
            if (platformer != null)
                Destroy(platformer);

            anim = null;
            return;
        }

        // 2) Stage 처리
        if (sceneName.Contains("Stage"))
        {
            StartCoroutine(DelayedPlayerBinding());
        }
        else
        {
            anim = null;
        }

        // UI 재연결
        ConnectHeartUI();
        UpdateHeartUI();
    }

    // ======================================
    //  딜레이로 Player / Animator 안정 연결
    // ======================================
    IEnumerator DelayedPlayerBinding()
    {
        yield return null; // 한 프레임 대기 (플레이어 확실히 로드)

        GameObject playerObj = GameObject.Find("PlatformerPlayer");

        if (playerObj != null)
        {
            anim = playerObj.GetComponent<Animator>();

            // PlayerHealth가 플레이어 위치로 이동
            transform.position = playerObj.transform.position;
        }

        // UI 재연결
        ConnectHeartUI();
        UpdateHeartUI();
    }

    // ===========================
    //     UI / HEART PROCESS
    // ===========================
    private void ConnectHeartUI()
    {
        if (heartUI == null)
            heartUI = FindObjectOfType<HeartUI>();
    }

    private void UpdateHeartUI()
    {
        if (heartUI != null)
            heartUI.UpdateHearts(currentHealth);
    }

    // ===========================
    //       DAMAGE SYSTEM
    // ===========================
    public void TakeDamage(float amount)
    {
        if (isInvincible) return;
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        UpdateHeartUI();
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHeartUI();
    }

    // ===========================
    //         DEATH / RESPAWN
    // ===========================
    void Die()
    {
        isDead = true;

        if (anim != null)
            anim.SetTrigger("Die");

        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(2f);

        currentHealth = maxHealth;
        UpdateHeartUI();

        // 시작 위치로 리스폰
        transform.position = startPosition;

        if (anim != null)
            anim.Play("DaniIdle");

        isDead = false;
    }

    // ======================================================
    //             ⭐⭐  FALL (낙사 처리 기능 개선) ⭐⭐
    // ======================================================
    public void RespawnFromFall()
    {
        if (isDead || isInvincible) return;

        // ★ HP가 1일 때 낙사 → 바로 죽음 처리 (startPosition에서 부활)
        if (currentHealth - 1 <= 0)
        {
            TakeDamage(1f);
            return;
        }

        // ★ HP가 2 이상일 때만 낙사 리스폰 동작
        TakeDamage(1f);

        // 마지막 안전 위치로 이동
        transform.position = FallRespawnManager.lastSafePosition;

        // 잠깐 무적
        StartCoroutine(FallInvincibleRoutine());
    }

    IEnumerator FallInvincibleRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }
}
