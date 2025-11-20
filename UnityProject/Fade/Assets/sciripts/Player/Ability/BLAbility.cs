//using UnityEngine;
//using System.Collections;

//public class BLAbility : MonoBehaviour
//{
//    [Header("능력 키 설정")]
//    public KeyCode abilityKey = KeyCode.G;

//    [Header("스피드 설정")]
//    public float swimSpeedMultiplier = 1.5f;   // 헤엄칠 때 속도 증가 비율
//    public float duration = 5f;                // 유지 시간
//    public float cooldown = 3f;                // 쿨타임

//    [Header("애니메이션")]
//    public Animator animator;
//    public string swimTriggerName = "Swim";

//    private bool isRunning = false;
//    private bool isCooldown = false;

//    private PlatformerPlayer playerMove;
//    private float originalSpeed;

//    private void Start()
//    {
//        playerMove = GetComponent<PlatformerPlayer>();

//        if (playerMove != null)
//            originalSpeed = playerMove.speed;
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(abilityKey))
//        {
//            TryActivate();
//        }
//    }

//    private void TryActivate()
//    {
//        if (isRunning || isCooldown)
//            return;

//        StartCoroutine(AbilityRoutine());
//    }

//    private IEnumerator AbilityRoutine()
//    {
//        isRunning = true;
//        isCooldown = true;

//        Debug.Log("[BLAbility] 헤엄치기 능력 시작!");

//        // 애니메이션 실행
//        if (animator != null)
//            animator.SetTrigger(swimTriggerName);

//        // 이동 속도 증가
//        if (playerMove != null)
//            playerMove.speed *= swimSpeedMultiplier;

//        yield return new WaitForSeconds(duration);

//        // 속도 원상복귀
//        if (playerMove != null)
//            playerMove.speed = originalSpeed;

//        Debug.Log("[BLAbility] 헤엄치기 종료");

//        isRunning = false;

//        yield return new WaitForSeconds(cooldown);
//        isCooldown = false;

//        Debug.Log("[BLAbility] 쿨타임 종료 → 다시 사용 가능!");
//    }
//}

using UnityEngine;
using System.Collections;

public class BLAbility : MonoBehaviour
{
    [Header("능력 키 설정")]
    public KeyCode abilityKey = KeyCode.G;

    [Header("스피드 설정")]
    public float swimSpeedMultiplier = 1.5f;   // 헤엄칠 때 속도 증가 비율
    public float duration = 5f;                // 유지 시간
    public float cooldown = 3f;                // 쿨타임

    [Header("애니메이션")]
    public Animator animator;
    public string swimTriggerName = "Swim";

    private bool isRunning = false;
    private bool isCooldown = false;

    private PlatformerPlayer playerMove;
    private float originalSpeed;

    private void Start()
    {
        playerMove = GetComponent<PlatformerPlayer>();

        if (playerMove != null)
            originalSpeed = playerMove.Speed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            TryActivate();
        }
    }

    private void TryActivate()
    {
        if (isRunning || isCooldown)
            return;

        StartCoroutine(AbilityRoutine());
    }

    private IEnumerator AbilityRoutine()
    {
        isRunning = true;
        isCooldown = true;

        Debug.Log("[BLAbility] 헤엄치기 능력 시작!");

        // 애니메이션 실행
        if (animator != null)
            animator.SetTrigger(swimTriggerName);

        // ⭐ 이동 속도 증가
        originalSpeed = playerMove.Speed;
        playerMove.Speed = playerMove.Speed * swimSpeedMultiplier;

        // 지속 시간 유지
        yield return new WaitForSeconds(duration);

        // ⭐ 속도 원상복귀
        playerMove.Speed = originalSpeed;

        Debug.Log("[BLAbility] 헤엄치기 종료");

        isRunning = false;

        // 쿨타임
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;

        Debug.Log("[BLAbility] 쿨타임 종료 → 다시 사용 가능!");
    }
}
