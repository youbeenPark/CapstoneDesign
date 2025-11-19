using UnityEngine;

public class GRAbilityPlatform : MonoBehaviour
{
    [Header("소멸 설정")]
    public float destroyDelay = 1f;
    private bool isActivated = false;

    private GRAbility abilityScript;

    // 플레이어를 부모로 붙였다 떼기 위해 저장
    private Transform playerOnPlatform = null;

    public void Initialize(GRAbility spawner)
    {
        abilityScript = spawner;
    }

    // ==========================
    // 🔥 2D 충돌 감지
    // ==========================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            playerOnPlatform = collision.transform;
            playerOnPlatform.SetParent(this.transform);

            Invoke("DissolveAndDestroy", destroyDelay);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform == playerOnPlatform)
        {
            playerOnPlatform.SetParent(null);
            playerOnPlatform = null;
        }
    }

    // ==========================
    // 🔥 발판 소멸
    // ==========================
    private void DissolveAndDestroy()
    {
        // 부모 관계 해제
        if (playerOnPlatform != null && playerOnPlatform.parent == this.transform)
            playerOnPlatform.SetParent(null);

        // 2D Collider 비활성화
        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        // 자식 포함 모든 SpriteRenderer 비활성화
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            sr.enabled = false;

        // 🔥 Destroy 호출되는지 확인 로그
        Debug.Log("[Platform] Destroy 호출됨!!", this);

        // 발판 삭제
        Destroy(gameObject);
    }

    // ==========================
    // 🔥 Destroy 시 호출되는 함수
    // ==========================
    private void OnDestroy()
    {
        Debug.Log("[Platform] OnDestroy 실행됨 -> PlatformDestroyed 호출!", this);

        if (abilityScript != null)
            abilityScript.PlatformDestroyed();
    }
}
