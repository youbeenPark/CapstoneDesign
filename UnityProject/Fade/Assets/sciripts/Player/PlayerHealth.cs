using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 5f;
    public float currentHealth = 5f;

    public HeartUI heartUI;
    public Animator anim;

    private static PlayerHealth instance;
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

    // --------------------------------------------------------
    // ⭐ 씬 로딩 시 자동 처리
    // --------------------------------------------------------
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        //-----------------------------------------------------
        // ⭐ 1) EpisodeMap(TUTO) → PlatformerPlayer 강제 제거
        //-----------------------------------------------------
        if (sceneName == "TUTO")
        {
            GameObject platformer = GameObject.Find("PlatformerPlayer");
            if (platformer != null)
                Destroy(platformer);

            anim = null;    // EpisodeMap에서는 플레이어 애니 끊음
            return;
        }

        //-----------------------------------------------------
        // ⭐ 2) Stage 씬일 때만 PlatformerPlayer 연결
        //-----------------------------------------------------
        if (sceneName.Contains("Stage"))
        {
            GameObject playerObj = GameObject.Find("PlatformerPlayer");

            if (playerObj != null)
            {
                anim = playerObj.GetComponent<Animator>();
                transform.position = playerObj.transform.position;
            }
        }
        else
        {
            // 스테이지가 아닌 씬에서는 애니 끊기 (불필요한 연결 방지)
            anim = null;
        }

        //-----------------------------------------------------
        // ⭐ Heart UI 다시 연결
        //-----------------------------------------------------
        ConnectHeartUI();

        if (heartUI != null)
            heartUI.UpdateHearts(currentHealth);
    }

    // --------------------------------------------------------
    // ⭐ HeartUI 자동 연결
    // --------------------------------------------------------
    private void ConnectHeartUI()
    {
        if (heartUI == null)
            heartUI = FindObjectOfType<HeartUI>();
    }

    // --------------------------------------------------------
    // 데미지 처리
    // --------------------------------------------------------
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        if (heartUI != null)
            heartUI.UpdateHearts(currentHealth);
    }

    // --------------------------------------------------------
    // 회복 처리
    // --------------------------------------------------------
    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (heartUI != null)
            heartUI.UpdateHearts(currentHealth);
    }

    // --------------------------------------------------------
    // 죽음 처리
    // --------------------------------------------------------
    void Die()
    {
        isDead = true;

        if (anim != null)
            anim.SetTrigger("Die");

        StartCoroutine(RespawnRoutine());
    }

    // --------------------------------------------------------
    // 리스폰 루틴
    // --------------------------------------------------------
    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(2f);

        currentHealth = maxHealth;

        if (heartUI != null)
            heartUI.UpdateHearts(currentHealth);

        transform.position = startPosition;

        if (anim != null)
            anim.Play("DaniIdle");

        isDead = false;
    }
}
