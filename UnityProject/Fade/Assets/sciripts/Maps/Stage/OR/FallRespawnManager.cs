using UnityEngine;

public class FallRespawnManager : MonoBehaviour
{
    public static Vector3 lastSafePosition;

    public Transform player;

    private PlatformerPlayer playerController;

    private void Start()
    {
        if (player == null)
            player = PlayerHealth.instance.transform;

        playerController = player.GetComponent<PlatformerPlayer>();

        lastSafePosition = player.position;
    }

    private void Update()
    {
        if (playerController == null) return;

        // 플레이어가 바닥에 있을 때마다 안전지점 갱신
        if (playerController.IsGrounded)
        {
            lastSafePosition = player.position;
        }
    }
}
