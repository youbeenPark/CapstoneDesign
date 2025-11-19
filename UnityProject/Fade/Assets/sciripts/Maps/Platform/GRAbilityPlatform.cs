using UnityEngine;

public class GRAbilityPlatform : MonoBehaviour
{
    [Header("소멸 설정")]
    public float destroyDelay = 1f; // 밟힌 후 사라질 시간
    private bool isActivated = false; // 밟혔는지 여부

    // ⭐ GRAbility 스크립트와 통신하기 위한 변수 ⭐
    private GRAbility abilityScript;

    // GRAbility 스크립트가 호출하여 자신을 초기화하는 함수
    public void Initialize(GRAbility spawner)
    {
        abilityScript = spawner;
    }

    // 충돌이 발생했을 때 (플레이어가 밟았을 때)
    private void OnCollisionEnter(Collision collision)
    {
        // 'Player' 태그를 가진 오브젝트에 의해 밟혔는지 확인
        if (collision.gameObject.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            // 지정된 시간 뒤 파괴 예약
            Destroy(gameObject, destroyDelay);
        }
    }

    // 오브젝트가 파괴되기 직전에 호출되는 함수
    private void OnDestroy()
    {
        // 발판이 사라지면, 플레이어 스크립트에게 알립니다.
        if (abilityScript != null)
        {
            abilityScript.PlatformDestroyed();
        }
    }
}