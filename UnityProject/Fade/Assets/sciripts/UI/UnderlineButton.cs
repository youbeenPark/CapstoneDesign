// Assets/Scripts/UI/UnderlineButton.cs
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UnderlineButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI buttonText;
    // ⭐ ⭐ 하드코딩 대신, 현재 텍스트를 저장할 변수로 변경 ⭐ ⭐
    private string originalText;

    void Awake()
    {
        buttonText = GetComponent<TextMeshProUGUI>();

        // ⭐ Awake 시점에 현재 TMP 컴포넌트의 텍스트를 읽어와 저장 ⭐
        originalText = buttonText.text;

        // 기본 색상 설정
        buttonText.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 1. 색상 변경
        buttonText.color = Color.yellow;

        // 2. 저장된 원본 텍스트에 밑줄 태그를 추가
        buttonText.text = "<u>" + originalText + "</u>";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 1. 색상 복귀
        buttonText.color = Color.white;

        // 2. 저장된 원본 텍스트로 복귀
        buttonText.text = originalText;
    }
}