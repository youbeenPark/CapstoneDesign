//using UnityEngine;

///// <summary>
///// 스테이지 해금 상태를 전역적으로 관리하는 매니저.
///// PlayerPrefs에 저장되어, 다음 실행 시에도 유지됨.
///// </summary>
//public static class StageProgressManager
//{
//    /// <summary>
//    /// 스테이지 해금 처리
//    /// </summary>
//    public static void UnlockStage(string stageName)
//    {
//        PlayerPrefs.SetInt($"Unlocked_{stageName}", 1);
//        PlayerPrefs.Save();
//        Debug.Log($"🔓 스테이지 해금됨: {stageName}");
//    }

//    /// <summary>
//    /// 스테이지가 해금되어 있는지 확인
//    /// </summary>
//    public static bool IsStageUnlocked(string stageName)
//    {
//        return PlayerPrefs.GetInt($"Unlocked_{stageName}", 0) == 1;
//    }
//}

using UnityEngine;

/// <summary>
/// 스테이지 해금 및 클리어 상태를 전역적으로 관리.
/// PlayerPrefs에 저장되어, 다음 실행 시에도 유지됨.
/// </summary>
public static class StageProgressManager
{
    /// 🔓 스테이지 해금 처리
    public static void UnlockStage(string stageName)
    {
        PlayerPrefs.SetInt($"Unlocked_{stageName}", 1);
        PlayerPrefs.Save();
        Debug.Log($"🔓 스테이지 해금됨: {stageName}");
    }

    /// 🎯 스테이지 클리어 처리
    public static void ClearStage(string stageName)
    {
        PlayerPrefs.SetInt($"Cleared_{stageName}", 1);
        PlayerPrefs.Save();
        Debug.Log($"🏁 스테이지 클리어됨: {stageName}");
    }

    /// 🔍 스테이지 해금 여부 확인
    public static bool IsStageUnlocked(string stageName)
    {
        return PlayerPrefs.GetInt($"Unlocked_{stageName}", 0) == 1;
    }

    /// 🔍 스테이지 클리어 여부 확인
    public static bool IsStageCleared(string stageName)
    {
        return PlayerPrefs.GetInt($"Cleared_{stageName}", 0) == 1;
    }

    /// 🔄 모든 진행 초기화 (디버그용)
    public static void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("🧹 모든 스테이지 진행 상태 초기화됨");
    }
}
