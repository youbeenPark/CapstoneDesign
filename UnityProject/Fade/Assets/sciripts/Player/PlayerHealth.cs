using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

        // 1) TUTO 처리 (원래 로직 유지)
        if (sceneName == "TUTO")
        {
            GameObject platformer = GameObject.Find("PlatformerPlayer");
            if (platformer != null)
                Destroy(platformer);

            anim = null;
            return;
        }

        // 2) Stage 씬이면 플레이어 찾고 Animator 연결
        if (sceneName.Contains("Stage"))
        {
            StartCoroutine(DelayedPlayerBinding());
        }
        else
        {
            anim = null;
        }

        // 3) UI 재연결
        ConnectHeartUI();
        UpdateHeartUI();
    }

    // ======================================
    //  딜레이로 Player / Animator 안정 연결
    // ======================================
    IEnumerator DelayedPlayerBinding()
    {
        // 한 프레임 기다리면 PlatformerPlayer가 확실히 로드됨
        yield return null;

        GameObject playerObj = GameObject.Find("PlatformerPlayer");

        if (playerObj != null)
        {
            anim = playerObj.GetComponent<Animator>();

            // PlayerHealth가 플레이어 위치로 이동
            transform.position = playerObj.transform.position;
        }

        // UI 재연결 한 번 더
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
    //       DEATH / RESPAWN
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
}
