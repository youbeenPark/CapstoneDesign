using UnityEngine;
using System.Collections;
using System.Linq;

public class PurpleAbility : MonoBehaviour
{
    [Header("보라 능력 설정")]
    public float freezeDuration = 2f;   // 정지 지속 시간 (초)
    public float cooldown = 8f;         // 쿨타임 (초)

    private bool isCooldown = false;

    void Update()
    {
        // X키 입력 + 쿨타임 아닐 때만 발동
        if (Input.GetKeyDown(KeyCode.X) && !isCooldown)
        {
            StartCoroutine(ActivateFreeze());
        }
    }


    private IEnumerator ActivateFreeze()
    {
        isCooldown = true;

        // 1) Freeze 가능한 모든 대상에게 Freeze() 호출
        FreezeAll();

        // 2) 정지 지속시간 동안 기다림
        yield return new WaitForSeconds(freezeDuration);

        // 3) 정지 해제
        UnfreezeAll();

        // 4) 쿨타임 대기
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }


    // ============================================================
    // Freeze / Unfreeze 전체 처리
    // ============================================================

    private void FreezeAll()
    {
        // 현재 씬 안에 있는 모든 IFreezable 오브젝트 찾기
        var freezables = FindObjectsOfType<MonoBehaviour>().OfType<IFreezable>();

        foreach (var f in freezables)
        {
            f.Freeze();
        }
    }

    private void UnfreezeAll()
    {
        var freezables = FindObjectsOfType<MonoBehaviour>().OfType<IFreezable>();

        foreach (var f in freezables)
        {
            f.Unfreeze();
        }
    }
}
