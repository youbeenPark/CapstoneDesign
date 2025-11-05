using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 게임 전체의 진행도(월드/에피소드/스테이지 클리어, 잠금 해제 등)를 관리하는 매니저.
/// 전역 싱글톤으로 작동하며, 씬 전환 간에도 파괴되지 않는다.
/// </summary>
[System.Serializable]
public class WorldProgress
{
    public string worldName;      // 예: "TUTO", "GR", "YL", ...
    public bool unlocked;         // 잠금 해제 여부
    public bool cleared;          // 클리어 여부
}

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;
    [Header("World Progress Data")]
    public List<WorldProgress> worlds = new List<WorldProgress>();

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 월드(에피소드) 해금 처리
    /// </summary>
    public void UnlockWorld(string worldName)
    {
        var world = worlds.Find(w => w.worldName == worldName);
        if (world != null)
        {
            world.unlocked = true;
            SaveProgress();
        }
        else
        {
            Debug.LogWarning($" UnlockWorld 실패: {worldName}를 찾을 수 없음");
        }
    }

    /// <summary>
    /// 월드(에피소드) 클리어 처리
    /// </summary>
    public void MarkWorldCleared(string worldName)
    {
        var world = worlds.Find(w => w.worldName == worldName);
        if (world != null)
        {
            world.cleared = true;
            SaveProgress();
        }
        else
        {
            Debug.LogWarning($"MarkWorldCleared 실패: {worldName}를 찾을 수 없음");
        }
    }

    /// <summary>
    /// 해당 월드가 해금되어 있는가?
    /// </summary>
    public bool IsWorldUnlocked(string worldName)
    {
        var world = worlds.Find(w => w.worldName == worldName);
        return world != null && world.unlocked;
    }

    /// <summary>
    /// 해당 월드가 클리어되어 있는가?
    /// </summary>
    public bool IsWorldCleared(string worldName)
    {
        var world = worlds.Find(w => w.worldName == worldName);
        return world != null && world.cleared;
    }

    /// <summary>
    /// 진행도 저장 (PlayerPrefs 사용)
    /// </summary>
    public void SaveProgress()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("GameProgress", json);
        PlayerPrefs.Save();
        Debug.Log("Game progress saved!");
    }

    /// <summary>
    /// 진행도 불러오기 (없으면 초기화)
    /// </summary>
    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey("GameProgress"))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("GameProgress"), this);
            Debug.Log(" Game progress loaded!");
        }
        else
        {
            InitializeDefaultProgress();
        }
    }

    /// <summary>
    /// 첫 실행 시 기본 상태 초기화
    /// </summary>
    private void InitializeDefaultProgress()
    {
        worlds.Clear();

        //  실제 존재하는 모든 에피소드(색상 맵) 추가
        worlds.Add(new WorldProgress { worldName = "TUTO", unlocked = true, cleared = false });
        worlds.Add(new WorldProgress { worldName = "GR", unlocked = false, cleared = false });
        worlds.Add(new WorldProgress { worldName = "YL", unlocked = false, cleared = false });
        worlds.Add(new WorldProgress { worldName = "RD", unlocked = false, cleared = false });
        worlds.Add(new WorldProgress { worldName = "BL", unlocked = false, cleared = false });
        worlds.Add(new WorldProgress { worldName = "OR", unlocked = false, cleared = false });
        worlds.Add(new WorldProgress { worldName = "PR", unlocked = false, cleared = false });
        worlds.Add(new WorldProgress { worldName = "SK", unlocked = false, cleared = false });
        worlds.Add(new WorldProgress { worldName = "BOSE", unlocked = false, cleared = false });
        worlds.Add(new WorldProgress { worldName = "RAINBOW", unlocked = false, cleared = false });

        SaveProgress();
        Debug.Log(" Game progress initialized!");
    }
}
