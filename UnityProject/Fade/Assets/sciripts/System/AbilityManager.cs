//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class AbilityData
//{
//    public string abilityName;      // 능력 ID (Red, Blue, Yellow...)
//    public string abilityDisplay;   // UI에 표시될 텍스트
//    public KeyCode defaultKey;      // 추천 기본 키
//}

//public class AbilityManager : MonoBehaviour
//{
//    public static AbilityManager Instance;

//    [Header("전체 능력 목록")]
//    public List<AbilityData> abilities = new List<AbilityData>();

//    private void Awake()
//    {
//        // 싱글톤
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    // 능력 이름으로 데이터 찾기
//    public AbilityData GetAbility(string name)
//    {
//        return abilities.Find(a => a.abilityName == name);
//    }
//}

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityData
{
    public string abilityName;      // 능력 ID (Red, Blue, Yellow...)
    public string abilityDisplay;   // UI에 표시될 텍스트
    public KeyCode defaultKey;      // 추천 기본 키
}

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    [Header("전체 능력 목록")]
    public List<AbilityData> abilities = new List<AbilityData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 🔥 abilityName으로 기본 키 찾기
    public KeyCode GetDefaultKey(string abilityName)
    {
        var ability = abilities.Find(a => a.abilityName == abilityName);
        return ability != null ? ability.defaultKey : KeyCode.None;
    }
}
