//using UnityEngine;
//using TMPro;
//using UnityEngine.UI;

//public class HelpPageController : MonoBehaviour
//{
//    [TextArea(2, 10)]
//    public string[] pages;              // 페이지 텍스트들

//    public TextMeshProUGUI contentText; // 내용 표시 텍스트
//    public Button leftButton;           // ← 버튼
//    public Button rightButton;          // → 버튼

//    private int currentPage = 0;

//    void Start()
//    {
//        leftButton.onClick.AddListener(PrevPage);
//        rightButton.onClick.AddListener(NextPage);

//        UpdatePage();
//    }

//    void PrevPage()
//    {
//        if (currentPage > 0)
//        {
//            currentPage--;
//            UpdatePage();
//        }
//    }

//    void NextPage()
//    {
//        if (currentPage < pages.Length - 1)
//        {
//            currentPage++;
//            UpdatePage();
//        }
//    }

//    void UpdatePage()
//    {
//        contentText.text = pages[currentPage];

//        // 왼쪽 버튼: 첫 페이지에서는 숨김
//        leftButton.gameObject.SetActive(currentPage > 0);

//        // 오른쪽 버튼: 마지막 페이지에서는 숨김
//        rightButton.gameObject.SetActive(currentPage < pages.Length - 1);
//    }
//}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageinfoController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI contentText;
    public Image contentImage;

    public Button leftButton;               // ← 버튼
    public Button rightButton;              // → 버튼

    [Header("Page Data")]
    [TextArea(5, 20)]
    public string[] pagesText;
    public Sprite[] pagesImage;

    private int currentPage = 0;

    void Start()
    {
        UpdatePage();

        // 버튼 이벤트 연결
        if (leftButton != null)
            leftButton.onClick.AddListener(PrevPage);

        if (rightButton != null)
            rightButton.onClick.AddListener(NextPage);
    }

    public void NextPage()
    {
        if (currentPage < pagesText.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    private void UpdatePage()
    {
        // 텍스트 교체
        if (pagesText != null && pagesText.Length > 0)
            contentText.text = pagesText[currentPage];

        // 이미지 교체
        if (contentImage != null &&
            pagesImage != null &&
            pagesImage.Length > currentPage &&
            pagesImage[currentPage] != null)
        {
            contentImage.sprite = pagesImage[currentPage];
            contentImage.enabled = true;
        }
        else
        {
            if (contentImage != null)
                contentImage.enabled = false;
        }

        // 🔥 화살표 표시/숨김
        if (leftButton != null)
            leftButton.gameObject.SetActive(currentPage > 0);

        if (rightButton != null)
            rightButton.gameObject.SetActive(currentPage < pagesText.Length - 1);
    }
}
