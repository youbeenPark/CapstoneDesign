//using UnityEngine;
//// using UnityEngine.SceneManagement; // 씬 재시작 기능을 사용하지 않으므로 필요 없습니다.

//public class DeadZoneHandler : MonoBehaviour
//{
//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        // ⭐ 충돌한 오브젝트가 'Player' 태그를 가졌는지 확인
//        if (other.CompareTag("Player"))
//        {
//            // PlayerHealth 스크립트의 싱글톤 인스턴스가 있는지 확인
//            if (PlayerHealth.instance != null)
//            {
//                // ⭐⭐⭐ 낭떠러지(DeadZone)에 떨어졌을 때 처리 함수 호출 ⭐⭐⭐
//                // 이 함수가 체력을 1 감소시키고, lastSafePosition으로 이동시키거나, 죽음 처리(Die())를 담당합니다.
//                PlayerHealth.instance.RespawnFromFall();

//                Debug.Log("플레이어가 Dead Zone에 닿아 낙하 부활 로직을 호출합니다.");
//            }
//            // 씬을 직접 재시작하는 RestartCurrentScene() 함수는 더 이상 필요 없습니다.
//        }
//    }
//}

using UnityEngine;

public class DeadZoneHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerHealth.instance != null)
            {
                PlayerHealth.instance.currentHealth = 0f;

                // private Die() 대신 public ForceDie() 호출
                PlayerHealth.instance.ForceDie();

                Debug.Log("플레이어가 Dead Zone에 닿아 즉시 사망 처리 로직 호출");
            }
        }
    }
}
