using UnityEngine;

public class DaniController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("DaniFall"); // 시작 시 DaniFall 애니메이션 재생
    }

    void Update()
    {
        // Y 좌표가 0 이하로 내려오면 애니메이션 멈춤
        if (transform.position.y <= 0f)
        {
            animator.speed = 0;
        }
    }
}
