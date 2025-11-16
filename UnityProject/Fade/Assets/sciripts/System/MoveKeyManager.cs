using UnityEngine;
using UnityEngine.UI;

public enum MoveKeyMode
{
    WASD,
    Arrow
}

public class MoveKeyManager : MonoBehaviour
{
    public static MoveKeyManager Instance;
    public HandleMove handleMove;

    public Toggle modeToggle;
    public MoveKeyMode currentMode;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Debug.Log("[MoveKeyManager] Start() 실행");

        // PlayerPrefs 로드
        currentMode = (MoveKeyMode)PlayerPrefs.GetInt("MoveKeyMode", 0);
        Debug.Log($"[MoveKeyManager] 저장된 MoveKeyMode = {currentMode}");

        // UI 초기 상태 반영
        modeToggle.isOn = (currentMode == MoveKeyMode.WASD);
        Debug.Log($"[MoveKeyManager] 토글 초기 상태 설정 isOn = {modeToggle.isOn}");

        // 이벤트 연결
        modeToggle.onValueChanged.AddListener(SetMode);

        // 초기 핸들 위치 설정
        handleMove.Move(modeToggle.isOn);
    }

    public void ApplyMode(MoveKeyMode mode)
    {
        Debug.Log($"[MoveKeyManager] ApplyMode() : {mode}");

        currentMode = mode;
        PlayerPrefs.SetInt("MoveKeyMode", (int)mode);
        PlayerPrefs.Save();

        Debug.Log("[MoveKeyManager] PlayerPrefs 저장 완료");
    }

    public void SetMode(bool isWASD)
    {
        Debug.Log($"[MoveKeyManager] SetMode() 호출됨 / isWASD = {isWASD}");

        MoveKeyMode newMode = isWASD ? MoveKeyMode.WASD : MoveKeyMode.Arrow;

        // 🔥 전환 가능 여부 체크
        if (!CanChangeTo(newMode))
        {
            Debug.Log("[MoveKeyManager] 모드 변경 실패 → 능력키 충돌 발생!");
            return;  // ❌ UI 강제 복구 제거 (핸들 오작동 원인)
        }

        // 🔥 성공 시 모드 적용
        Debug.Log("[MoveKeyManager] 모드 변경 성공 → ApplyMode 실행");
        ApplyMode(newMode);

        // 🔥 핸들 이동 적용 (이벤트 없이도 항상 이동되게)
        handleMove.Move(isWASD);
    }

    public bool CanChangeTo(MoveKeyMode mode)
    {
        Debug.Log($"[MoveKeyManager] CanChangeTo() 검사 시작. mode = {mode}");

        KeyCode[] modeKeys = GetKeysForMode(mode);
        Debug.Log($"[MoveKeyManager] 해당 모드 이동키 = {string.Join(", ", modeKeys)}");

        var abilityKeys = KeyBindingManager.Instance.GetAllKeys();

        foreach (var kv in abilityKeys)
        {
            foreach (var mk in modeKeys)
            {
                if (kv.Value == mk)
                {
                    Debug.Log($"[MoveKeyManager] 충돌! 능력키 '{kv.Key}' 가 이동키 '{mk}' 와 겹침");
                    return false;
                }
            }
        }

        Debug.Log("[MoveKeyManager] 충돌 없음 → 변경 가능");
        return true;
    }

    public KeyCode[] GetKeysForMode(MoveKeyMode mode)
    {
        if (mode == MoveKeyMode.WASD)
            return new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

        return new KeyCode[] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };
    }

    public KeyCode[] GetCurrentKeys()
    {
        return GetKeysForMode(currentMode);
    }
}
