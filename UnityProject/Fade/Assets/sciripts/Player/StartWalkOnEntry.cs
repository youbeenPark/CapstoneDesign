using UnityEngine;

public class StartWalkOnEntry : StateMachineBehaviour
{
    // DaniWalk 상태에 진입할 때 호출됩니다.
    // StartWalkOnEntry.cs 파일 수정
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ⭐ 추가: 애니메이션 속도를 기본값 (1.0)으로 복구 ⭐
        animator.speed = 1.0f;

        DaniController daniController = animator.GetComponent<DaniController>();
        if (daniController != null)
        {
            daniController.SetIsWalkingToWall(true);
        }
    }
}