// LandingCleanup.cs 파일 수정
using UnityEngine;

public class LandingCleanup : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 1. isLanded 파라미터를 다시 False로 설정
        animator.SetBool("isLanded", false);

        // ⭐ 2. StandUp 트리거를 발동하여 DaniStand로의 전환을 강제 실행 ⭐
        animator.SetTrigger("StandUp");
    }
}