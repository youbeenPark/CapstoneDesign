using UnityEngine;

public class FallRespawnManager : MonoBehaviour
{
    public static Vector3 lastSafePosition;

    public Transform player;
    private PlatformerPlayer playerController;

    private void Start()
    {
        // 플레이어 자동 연결
        if (player == null)
            player = PlayerHealth.instance.transform;

        // 플레이어 이동/점프 제어 스크립트
        playerController = player.GetComponent<PlatformerPlayer>();

        // 게임 시작 시 초기 안전 지점 = 플레이어 시작 위치
        lastSafePosition = player.position;
    }

    private void Update()
    {
        if (playerController == null) return;

        // ⭐ 조건: 플레이어가 "지면 위에 있을 때만" 안전 지점 갱신
        if (playerController.IsGrounded)
        {
            lastSafePosition = player.position;
        }
    }
}
