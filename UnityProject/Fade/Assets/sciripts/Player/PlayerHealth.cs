using System.Collections;
using Unity.Cinemachine;
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

    private Transform player;
    private Vector3 startPosition;

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
        FindPlayer();
        SaveStartPosition();
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

        // ⭐ 죽었을 때 = 시작 위치로 이동
        if (player != null)
            player.position = startPosition;

        // ⭐ 카메라 흔들림 제거 즉시 스냅
        StartCoroutine(ForceCameraSnap());

        if (anim != null)
            anim.Play("DaniIdle");

        isDead = false;
    }

    // =========================
    // FALL RESPAWN
    // =========================
    public void RespawnFromFall()
    {
        if (isDead || isInvincible) return;

        // 체력이 1이면 즉시 사망
        if (currentHealth - 1 <= 0)
        {
            TakeDamage(1f);
            return;
        }

        TakeDamage(1f);

        // 체력 >= 2 → 마지막 안전 위치로 이동
        if (player != null)
            player.position = FallRespawnManager.lastSafePosition;

        // ⭐ 카메라 즉시 위치 보정
        StartCoroutine(ForceCameraSnap());

        StartCoroutine(FallInvincibleRoutine());
    }

    IEnumerator FallInvincibleRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

    // ======================================================
    // ⭐ Cinemachine 3.x 즉시 카메라 스냅 (PositionDamping 사용)
    // ======================================================
    IEnumerator ForceCameraSnap()
    {
        // vcam 찾기
        var vcam = FindObjectOfType<CinemachineCamera>();
        if (vcam == null) yield break;

        // follow 컴포넌트 찾기 (네 프로젝트에 실제 존재함)
        var follow = vcam.GetComponent<CinemachineFollow>();
        if (follow == null) yield break;

        // ⭐ 카메라를 강제로 한 프레임 동안 비활성화
        vcam.enabled = false;

        // 1프레임 기다림 → 플레이어 위치 반영됨
        yield return null;

        // ⭐ 다시 활성화 → 즉시 스냅
        vcam.enabled = true;
    }

}
