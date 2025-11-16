

//using System.Collections.Generic;
//using UnityEngine;

//public class KeyBindingManager : MonoBehaviour
//{
//    public static KeyBindingManager Instance;

//    // 실제 저장된 키들
//    private Dictionary<string, KeyCode> keyDict = new();

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }

//        LoadAllKeys();
//    }

//    // 🔥 모든 키 불러오기 (없으면 기본키)
//    public void LoadAllKeys()
//    {
//        keyDict.Clear();

//        foreach (var ability in AbilityManager.Instance.abilities)
//        {
//            string keyName = "Key_" + ability.abilityName;

//            if (PlayerPrefs.HasKey(keyName))
//            {
//                string saved = PlayerPrefs.GetString(keyName);
//                keyDict[ability.abilityName] = (KeyCode)System.Enum.Parse(typeof(KeyCode), saved);
//            }
//            else
//            {
//                keyDict[ability.abilityName] = ability.defaultKey;
//            }
//        }
//    }

//    // 🔥 키 저장하기
//    public void SaveKey(string abilityName, KeyCode newKey)
//    {
//        keyDict[abilityName] = newKey;
//        PlayerPrefs.SetString("Key_" + abilityName, newKey.ToString());
//        PlayerPrefs.Save();
//    }

//    // 🔥 해당 능력의 현재 키 반환
//    public KeyCode GetKey(string abilityName)
//    {
//        return keyDict[abilityName];
//    }

//    // 🔥 전체 기본값으로 초기화
//    public void ResetAllToDefault()
//    {
//        foreach (var ability in AbilityManager.Instance.abilities)
//        {
//            string keyName = "Key_" + ability.abilityName;

//            keyDict[ability.abilityName] = ability.defaultKey;
//            PlayerPrefs.SetString(keyName, ability.defaultKey.ToString());
//        }

//        PlayerPrefs.Save();
//        LoadAllKeys(); // 다시 로드해서 메모리도 갱신
//    }

//    // ⭐ 중복 키 방지용 함수 추가
//    public bool IsKeyAlreadyUsed(KeyCode key, string exceptAbilityName)
//    {
//        foreach (var kv in keyDict)
//        {
//            if (kv.Key == exceptAbilityName)
//                continue;

//            if (kv.Value == key)
//                return true;
//        }
//        return false;
//    }
//}
using System.Collections.Generic;
using UnityEngine;

public class KeyBindingManager : MonoBehaviour
{
    public static KeyBindingManager Instance;

    // 저장된 능력 키들
    private Dictionary<string, KeyCode> keyDict = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadAllKeys();
    }

    // 🔥 모든 능력키 로드 (없으면 기본값)
    public void LoadAllKeys()
    {
        keyDict.Clear();

        foreach (var ability in AbilityManager.Instance.abilities)
        {
            string keyName = "Key_" + ability.abilityName;

            if (PlayerPrefs.HasKey(keyName))
            {
                string saved = PlayerPrefs.GetString(keyName);
                keyDict[ability.abilityName] = (KeyCode)System.Enum.Parse(typeof(KeyCode), saved);
            }
            else
            {
                keyDict[ability.abilityName] = ability.defaultKey;
            }
        }
    }

    // 🔥 키 저장
    public void SaveKey(string abilityName, KeyCode newKey)
    {
        keyDict[abilityName] = newKey;
        PlayerPrefs.SetString("Key_" + abilityName, newKey.ToString());
        PlayerPrefs.Save();
    }

    // 🔥 능력키 가져오기
    public KeyCode GetKey(string abilityName)
    {
        return keyDict[abilityName];
    }

    // 🔥 전체 초기화
    public void ResetAllToDefault()
    {
        foreach (var ability in AbilityManager.Instance.abilities)
        {
            string keyName = "Key_" + ability.abilityName;

            keyDict[ability.abilityName] = ability.defaultKey;
            PlayerPrefs.SetString(keyName, ability.defaultKey.ToString());
        }

        PlayerPrefs.Save();
        LoadAllKeys();
    }

    // 🔥 능력 키 중복 체크
    public bool IsKeyAlreadyUsed(KeyCode key, string exceptAbility)
    {
        foreach (var kv in keyDict)
        {
            if (kv.Key == exceptAbility) continue;
            if (kv.Value == key) return true;
        }
        return false;
    }

    // 🔥 MoveKeyManager가 호출할 전체 능력키 반환 함수
    public Dictionary<string, KeyCode> GetAllKeys()
    {
        return keyDict;
    }

    // 🔥 능력키가 이동키인지 체크 (능력키 리바인드 방지용)
    public bool IsMoveKey(KeyCode key)
    {
        KeyCode[] moveKeys = MoveKeyManager.Instance.GetCurrentKeys();
        foreach (var mk in moveKeys)
        {
            if (mk == key) return true;
        }
        return false;
    }
}
