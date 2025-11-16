//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class KeySettingPanelController : MonoBehaviour
//{
//    [Header("UI References")]
//    public Transform contentParent;     // ScrollView → Viewport → Content
//    public GameObject keySlotPrefab;    // AbilityKeySlot 프리팹

//    private List<GameObject> spawnedSlots = new List<GameObject>();

//    private void Start()
//    {
//        GenerateUI();
//    }

//    // 🔥 능력 목록 기반으로 UI 자동 생성
//    public void GenerateUI()
//    {
//        // 기존 슬롯 제거
//        foreach (var slot in spawnedSlots)
//            Destroy(slot);

//        spawnedSlots.Clear();

//        // 능력 목록 불러오기
//        var abilities = AbilityManager.Instance.abilities;

//        foreach (var ability in abilities)
//        {
//            // 슬롯 생성
//            GameObject slot = Instantiate(keySlotPrefab, contentParent);
//            spawnedSlots.Add(slot);

//            // 슬롯 안의 UI 가져오기
//            var texts = slot.GetComponentsInChildren<TextMeshProUGUI>();
//            var abilityNameText = texts[0]; // Text_AbilityName
//            var keyText = texts[1];         // Text_CurrentKey

//            // Change 버튼
//            var button = slot.GetComponentInChildren<Button>();

//            // 표시되는 텍스트 넣기
//            abilityNameText.text = ability.abilityDisplay;
//            keyText.text = KeyBindingManager.Instance.GetKey(ability.abilityName).ToString();

//            // 클릭 이벤트 등록
//            string name = ability.abilityName; // 클로저 문제 방지
//            button.onClick.AddListener(() => StartRebind(name, keyText));
//        }
//    }

//    // 🔥 리바인드 시작
//    private void StartRebind(string abilityName, TextMeshProUGUI keyText)
//    {
//        StartCoroutine(WaitForKeyPress(abilityName, keyText));
//    }

//    // 🔥 실제로 키 입력 받는 코루틴
//    private System.Collections.IEnumerator WaitForKeyPress(string abilityName, TextMeshProUGUI keyText)
//    {
//        keyText.text = "Press key...";

//        bool gotKey = false;
//        KeyCode newKey = KeyCode.None;

//        while (!gotKey)
//        {
//            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
//            {
//                if (Input.GetKeyDown(key))
//                {
//                    newKey = key;
//                    gotKey = true;
//                    break;
//                }
//            }

//            yield return null;
//        }

//        // 키 저장
//        KeyBindingManager.Instance.SaveKey(abilityName, newKey);

//        // UI 갱신
//        keyText.text = newKey.ToString();
//    }
//}


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeySettingPanelController : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentParent;     // ScrollView → Viewport → Content
    public GameObject keySlotPrefab;    // AbilityKeySlot 프리팹
    public Button resetAllButton;       // ⭐ 전체 초기화 버튼

    private List<GameObject> spawnedSlots = new List<GameObject>();

    private void Start()
    {
        GenerateUI();
        resetAllButton.onClick.AddListener(ResetAllKeys);
    }

    // 🔥 능력 목록 기반으로 UI 자동 생성
    public void GenerateUI()
    {
        // 기존 슬롯 제거
        foreach (var slot in spawnedSlots)
            Destroy(slot);

        spawnedSlots.Clear();

        var abilities = AbilityManager.Instance.abilities;

        foreach (var ability in abilities)
        {
            GameObject slot = Instantiate(keySlotPrefab, contentParent);
            spawnedSlots.Add(slot);

            var texts = slot.GetComponentsInChildren<TextMeshProUGUI>();
            var abilityNameText = texts[0];
            var keyText = texts[1];

            var button = slot.GetComponentInChildren<Button>();

            abilityNameText.text = ability.abilityDisplay;
            keyText.text = KeyBindingManager.Instance.GetKey(ability.abilityName).ToString();

            string name = ability.abilityName;

            // 🔥 Change 버튼 리스너 추가
            button.onClick.AddListener(() => StartRebind(name, keyText));
        }
    }

    // 🔥 키 변경 시작
    private void StartRebind(string abilityName, TextMeshProUGUI keyText)
    {
        StartCoroutine(WaitForKeyPress(abilityName, keyText));
    }

    // 🔥 실제 키 입력 받기 + 중복 감지 추가
    private System.Collections.IEnumerator WaitForKeyPress(string abilityName, TextMeshProUGUI keyText)
    {
        keyText.text = "키 선택중";

        bool gotKey = false;
        KeyCode newKey = KeyCode.None;

        while (!gotKey)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    // ⭐ 중복 체크
                    if (KeyBindingManager.Instance.IsKeyAlreadyUsed(key, abilityName))
                    {
                        keyText.text = "사용중인 키";
                        yield return new WaitForSeconds(0.7f);
                        keyText.text = "키 선택중";
                        continue;
                    }

                    newKey = key;
                    gotKey = true;
                    break;
                }
            }

            yield return null;
        }

        // 저장
        KeyBindingManager.Instance.SaveKey(abilityName, newKey);
        keyText.text = newKey.ToString();
    }

    // 🔥 전체 기본값으로 초기화
    private void ResetAllKeys()
    {
        KeyBindingManager.Instance.ResetAllToDefault();
        GenerateUI();
    }
}
