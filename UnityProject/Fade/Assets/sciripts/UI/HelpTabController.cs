using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HelpTabController : MonoBehaviour
{
    [TextArea(2, 10)]
    public string[] pages;              // 페이지 텍스트들

    public TextMeshProUGUI contentText; // 내용 표시 텍스트
    public Button leftButton;           // ← 버튼
    public Button rightButton;          // → 버튼

    private int currentPage = 0;

    void Start()
    {
        leftButton.onClick.AddListener(PrevPage);
        rightButton.onClick.AddListener(NextPage);

        UpdatePage();
    }

    void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    void UpdatePage()
    {
        contentText.text = pages[currentPage];

        // 왼쪽 버튼: 첫 페이지에서는 숨김
        leftButton.gameObject.SetActive(currentPage > 0);

        // 오른쪽 버튼: 마지막 페이지에서는 숨김
        rightButton.gameObject.SetActive(currentPage < pages.Length - 1);
    }
}
