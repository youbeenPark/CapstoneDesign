//using UnityEngine;

//public class UDPlatformerMover : MonoBehaviour // ⭐ 새 스크립트
//{
//    [Header("상하 이동 설정")]
//    public float moveDistance = 2f;
//    public float moveSpeed = 2f;

//    private Vector3 startPos;

//    void Start()
//    {
//        startPos = transform.position;
//    }

//    void Update()
//    {
//        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;

//        transform.position = new Vector3(
//            startPos.x,
//            startPos.y + offset, // ⭐ Y축만 변경
//            startPos.z
//        );
//    }
//}

using UnityEngine;

public class UDPlatformerMover : MonoBehaviour // 상하 발판 전용 스크립트
{
    [Header("상하 이동 설정")]
    public float moveDistance = 2f;
    public float moveSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    // ⭐ 핵심 수정: Update() 대신 FixedUpdate()를 사용하여 물리 주기에 맞춰 발판을 움직입니다.
    void FixedUpdate()
    {
        // Sin 곡선 방식으로 왔다갔다
        // Time.time은 FixedUpdate에서도 월드 시간을 반환합니다.
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        transform.position = new Vector3(
            startPos.x,
            startPos.y + offset, // Y축만 변경
            startPos.z
        );
    }
}