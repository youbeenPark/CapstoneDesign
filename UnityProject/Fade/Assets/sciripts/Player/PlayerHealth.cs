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

    private Transform player;      // 실제 PlatformerPlayer
    private Vector3 startPosition; // 스테이지 시작 위치

    void Awake()
    {
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
        FindPlayer();
        SaveStartPosition();
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();        // 씬 바뀌면 새 플레이어 찾기
        SaveStartPosition(); // 스테이지 시작 위치 갱신
        ConnectHeartUI();
        UpdateHeartUI();
    }

    // =========================
    // 플레이어 찾기
    // =========================
    private void FindPlayer()
    {
        GameObject obj = GameObject.Find("PlatformerPlayer");

        if (obj != null)
        {
            player = obj.transform;
            anim = obj.GetComponent<Animator>();
        }
    }

    // =========================
    // 스테이지 시작 위치 저장
    // =========================
    private void SaveStartPosition()
    {
        if (player != null)
            startPosition = player.position;
    }

    // =========================
    // UI 연결
    // =========================
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

    // =========================
    // DAMAGE
    // =========================
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

    // =========================
    // DEATH
    // =========================
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

        // ⭐ 죽었을 때 → 스테이지 시작 위치로 이동
        if (player != null)
            player.position = startPosition;

        if (anim != null)
            anim.Play("DaniIdle");

        isDead = false;
    }

    public void ForceDie()
    {
        if (!isDead)
        {
            Die();   // private Die() 내부 호출
        }
    }

    // =========================
    // FALL RESPAWN
    // =========================
    public void RespawnFromFall()
    {
        if (isDead || isInvincible) return;

        // 체력이 1이면 즉시 죽음 처리
        if (currentHealth - 1 <= 0)
        {
            TakeDamage(1f);
            return;
        }

        // 체력 2 이상이면 → 이전 안전위치로 이동
        TakeDamage(1f);

        if (player != null)
            player.position = FallRespawnManager.lastSafePosition;

        StartCoroutine(FallInvincibleRoutine());
    }

    IEnumerator FallInvincibleRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }
}
