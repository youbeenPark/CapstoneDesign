using UnityEngine;

public class DaniController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("DaniFall"); // ���� �� DaniFall �ִϸ��̼� ���
    }

    void Update()
    {
        // Y ��ǥ�� 0 ���Ϸ� �������� �ִϸ��̼� ����
        if (transform.position.y <= 0f)
        {
            animator.speed = 0;
        }
    }
}
