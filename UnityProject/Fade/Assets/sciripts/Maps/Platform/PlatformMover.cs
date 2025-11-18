using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [Header("좌우 이동 설정")]
    public float moveDistance = 2f;   // 좌우 이동 범위
    public float moveSpeed = 2f;      // 이동 속도

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Sin 곡선 방식으로 왔다갔다
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        transform.position = new Vector3(
            startPos.x + offset,
            startPos.y,
            startPos.z
        );
    }
}
