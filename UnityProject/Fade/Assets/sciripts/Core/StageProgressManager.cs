
//using UnityEngine;

///// <summary>
///// 스테이지 해금 및 클리어 상태를 전역적으로 관리.
///// PlayerPrefs에 저장되어, 다음 실행 시에도 유지됨.
///// </summary>
//public static class StageProgressManager
//{
//    /// 🔓 스테이지 해금 처리
//    public static void UnlockStage(string stageName)
//    {
//        PlayerPrefs.SetInt($"Unlocked_{stageName}", 1);
//        PlayerPrefs.Save();
//        Debug.Log($"🔓 스테이지 해금됨: {stageName}");
//    }

//    /// 🎯 스테이지 클리어 처리
//    public static void ClearStage(string stageName)
//    {
//        PlayerPrefs.SetInt($"Cleared_{stageName}", 1);
//        PlayerPrefs.Save();
//        Debug.Log($"🏁 스테이지 클리어됨: {stageName}");
//    }

//    /// 🔍 스테이지 해금 여부 확인
//    public static bool IsStageUnlocked(string stageName)
//    {
//        return PlayerPrefs.GetInt($"Unlocked_{stageName}", 0) == 1;
//    }

//    /// 🔍 스테이지 클리어 여부 확인
//    public static bool IsStageCleared(string stageName)
//    {
//        return PlayerPrefs.GetInt($"Cleared_{stageName}", 0) == 1;
//    }

//    /// 🔄 모든 진행 초기화 (디버그용)
//    public static void ResetAllProgress()
//    {
//        PlayerPrefs.DeleteAll();
//        PlayerPrefs.Save();
//        Debug.Log("🧹 모든 스테이지 진행 상태 초기화됨");
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

    /// 🏁 스테이지 클리어 처리 (클리어 저장 + 다음 스테이지 해금)
    public static void ClearStage(string stageName)
    {
        PlayerPrefs.SetInt($"Cleared_{stageName}", 1);
        PlayerPrefs.SetInt($"Unlocked_{stageName}", 1); // 자기 자신도 항상 해금된 상태로 보장
        PlayerPrefs.Save();

        Debug.Log($"🏁 스테이지 클리어됨: {stageName}");

        // ✅ 다음 스테이지 자동 해금
        string nextStage = GetNextStageName(stageName);
        if (!string.IsNullOrEmpty(nextStage))
        {
            UnlockStage(nextStage);
            Debug.Log($"➡️ 다음 스테이지 자동 해금됨: {nextStage}");
        }
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

    /// ▶ 다음 스테이지 이름 계산 (예: GR_Stage1 → GR_Stage2)
    private static string GetNextStageName(string stage)
    {
        int idx = stage.LastIndexOf("Stage");
        if (idx < 0) return null;

        string prefix = stage.Substring(0, idx + 5); // "GR_Stage"
        string numberPart = stage.Substring(idx + 5);
        if (int.TryParse(numberPart, out int stageNum))
            return $"{prefix}{stageNum + 1}";
        return null;
    }
}
