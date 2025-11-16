using UnityEngine;
using TMPro;

public class GalleryController : MonoBehaviour
{
    [Header("UI 요소")]
    public TMP_Text currentIslandText;
    public GameObject galleryRoot;

    [Header("클릭 감지용 Collider")]
    public Collider2D galleryIconCollider;   // HUD의 갤러리 아이콘
    public Collider2D closeIconCollider;     // 책 안의 닫기 버튼

    private bool isOpen = false;

    private void Start()
    {
        // 현재 섬 텍스트 표시
        if (currentIslandText != null)
            currentIslandText.text = "현재 섬 : " + ConvertIslandName(IslandInfo.currentIsland);

        // 갤러리는 기본적으로 꺼진 상태로 시작
        if (galleryRoot != null)
            galleryRoot.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 갤러리 아이콘 클릭 → 열기
            if (galleryIconCollider != null &&
                galleryIconCollider == Physics2D.OverlapPoint(mousePos))
            {
                OpenGallery();
            }

            // 닫기 아이콘 클릭 → 닫기
            if (closeIconCollider != null &&
                closeIconCollider == Physics2D.OverlapPoint(mousePos))
            {
                CloseGallery();
            }
        }
    }

    public void OpenGallery()
    {
        if (galleryRoot == null) return;

        galleryRoot.SetActive(true);
        isOpen = true;
    }

    public void CloseGallery()
    {
        if (galleryRoot == null) return;

        galleryRoot.SetActive(false);
        isOpen = false;
    }

    private string ConvertIslandName(IslandType type)
    {
        switch (type)
        {
            case IslandType.TUTO: return "잊혀진 기억의 땅";
            case IslandType.GR: return "포근한 새싹의 들판";
            case IslandType.YL: return "햇살 같은 동심의 마을";
            case IslandType.BL: return "요동치는 불안의 심연";
            case IslandType.OR: return "새로운 도전의 언덕";
            case IslandType.RD: return "따스한 온기의 울타리";
            case IslandType.SK: return "잃어버린 빛의 하늘";
            case IslandType.PR: return "쓸쓸한 그림자의 성";
            case IslandType.BOSE: return "흩어진 기억의 파편";
            case IslandType.RAINBOW: return "색의 정원";
            default: return "???";
        }
    }
}
