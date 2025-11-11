using UnityEditor;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("현재 스테이지 타입 설정")]
    public StageType stageType = StageType.Platformer;

    public static StageManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
