using UnityEngine;
using UnityEngine.Rendering.Universal; // Light2D 사용 시 필요

public class DaniController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private bool hasLanded = false;

    [Header("Falling Path Settings")]
    [SerializeField] private float fallSpeed = -1f;           // 낙하 속도
    [SerializeField] private float horizontalDriftSpeed = 0.8f; // 오른쪽으로 전체 이동 속도
    [SerializeField] private float windStrength = 0.8f;         // 바람 세기 (좌우 흔들림 강도)
    [SerializeField] private float windChangeSpeed = 0.6f;      // 바람 변화 속도 (느릴수록 부드러움)

    [Header("Lighting Settings")]
    [SerializeField] private Light2D globalLight; // Global Light 2D 연결
    [SerializeField] private float lightFadeSpeed = 1.2f; // 밝아지는 속도

    private float noiseSeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // 물리 중력 비활성화
        animator.Play("DaniFall");

        noiseSeed = Random.Range(0f, 100f);

        // Light 자동 찾기 (직접 연결 안 했을 경우 대비)
        if (globalLight == null)
            globalLight = FindFirstObjectByType<Light2D>();

        if (globalLight != null)
            globalLight.intensity = 0f; // 처음엔 어둡게 시작
    }

    void Update()
    {
        if (!hasLanded)
        {
            float t = Time.time * windChangeSpeed;
            float wind = Mathf.PerlinNoise(noiseSeed, t) - 0.5f;

            // 좌우 흔들림 + 전체 오른쪽 이동
            float horizontalMove = (wind * windStrength + horizontalDriftSpeed) * Time.deltaTime;
            float verticalMove = fallSpeed * Time.deltaTime;

            transform.position += new Vector3(horizontalMove, verticalMove, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasLanded && collision.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;

            // 애니메이션 전환 (Trigger 방식)
            animator.SetTrigger("Landed");

            // 빛 페이드 인 시작
            if (globalLight != null)
                StartCoroutine(FadeInLight());
        }
    }

    private System.Collections.IEnumerator FadeInLight()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * lightFadeSpeed;
            globalLight.intensity = Mathf.Lerp(0f, 1.2f, t);
            yield return null;
        }
    }
}
