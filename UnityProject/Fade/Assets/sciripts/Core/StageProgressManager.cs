using UnityEngine;

/// <summary>
/// 스테이지 해금 상태를 전역적으로 관리하는 매니저.
/// PlayerPrefs에 저장되어, 다음 실행 시에도 유지됨.
/// </summary>
public static class StageProgressManager
{
    /// <summary>
    /// 스테이지 해금 처리
    /// </summary>
    public static void UnlockStage(string stageName)
    {
        PlayerPrefs.SetInt($"Unlocked_{stageName}", 1);
        PlayerPrefs.Save();
        Debug.Log($"🔓 {stageName} 해금됨");
    }

    /// <summary>
    /// 스테이지가 해금되어 있는지 확인
    /// </summary>
    public static bool IsStageUnlocked(string stageName)
    {
        return PlayerPrefs.GetInt($"Unlocked_{stageName}", 0) == 1;
    }
}
