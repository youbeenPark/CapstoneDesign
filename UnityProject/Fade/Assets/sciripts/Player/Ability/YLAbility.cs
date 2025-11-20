using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class YLAbility : MonoBehaviour
{
    [Header("능력 키 설정")]
    public KeyCode abilityKey = KeyCode.F;

    [Header("라이트 설정")]
    public Light2D yellowLight;
    public float duration = 5f;     // 유지 시간
    public float cooldown = 3f;     // 쿨타임

    private bool isRunning = false;
    private bool isCooldown = false;

    private void Start()
    {
        if (yellowLight != null)
        {
            yellowLight.enabled = false;
            Debug.Log("[YellowAbility] 시작: 라이트 OFF 상태로 시작");
        }
        else
        {
            Debug.LogError("[YellowAbility] yellowLight가 Inspector에 연결되지 않음!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(abilityKey))
        {
            Debug.Log("[YellowAbility] 능력키(F) 입력됨");
            TryActivate();
        }
    }

    private void TryActivate()
    {
        if (isRunning)
        {
            Debug.Log("[YellowAbility] 이미 능력 사용 중 → 실행 불가");
            return;
        }

        if (isCooldown)
        {
            Debug.Log("[YellowAbility] 쿨타임 진행 중 → 실행 불가");
            return;
        }

        Debug.Log("[YellowAbility] 능력 발동 시작!");
        StartCoroutine(AbilityRoutine());
    }

    private IEnumerator AbilityRoutine()
    {
        isRunning = true;
        isCooldown = true;

        // 능력 켜기
        yellowLight.enabled = true;
        Debug.Log("[YellowAbility] 라이트 ON");

        // 유지 시간
        Debug.Log($"[YellowAbility] 지속시간 {duration}초 유지");
        yield return new WaitForSeconds(duration);

        // 능력 종료
        yellowLight.enabled = false;
        Debug.Log("[YellowAbility] 라이트 OFF");

        isRunning = false;

        // 쿨타임 시작
        Debug.Log($"[YellowAbility] 쿨타임 시작: {cooldown}초");
        yield return new WaitForSeconds(cooldown);

        isCooldown = false;
        Debug.Log("[YellowAbility] 쿨타임 종료 → 능력 다시 사용 가능!");
    }
}
