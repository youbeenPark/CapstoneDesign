using UnityEngine;
using System.Collections;

public class DisappearPlatform : MonoBehaviour
{
    [Header("소멸 설정")]
    public float disappearDelay = 1f;   // 밟고 사라지는 시간
    public float respawnDelay = 2f;     // 다시 생기는 시간

    private bool isActivated = false;

    private Collider2D[] colliders;
    private SpriteRenderer[] sprites;

    void Start()
    {
        colliders = GetComponentsInChildren<Collider2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    // ==========================
    // 🔥 플레이어가 밟았을 때
    // ==========================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActivated && collision.gameObject.CompareTag("Player"))
        {
            isActivated = true;
            StartCoroutine(DisappearRoutine());
        }
    }

    // ==========================
    // 🔥 사라졌다가 다시 나타나는 루틴
    // ==========================
    IEnumerator DisappearRoutine()
    {
        // (1) 일정 시간 기다렸다가
        yield return new WaitForSeconds(disappearDelay);

        // (2) 발판 비활성화
        foreach (var col in colliders) col.enabled = false;
        foreach (var sr in sprites) sr.enabled = false;

        // (3) 일정 시간 뒤 다시 활성화
        yield return new WaitForSeconds(respawnDelay);

        foreach (var col in colliders) col.enabled = true;
        foreach (var sr in sprites) sr.enabled = true;

        isActivated = false;
    }
}
