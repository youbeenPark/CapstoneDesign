using UnityEngine;
using System.Collections;

public class GallerySpriteController : MonoBehaviour
{
    [Header("연결할 UI 패널 (책 UI)")]
    public GameObject galleryPanel;  // Canvas 안의 GalleryPanel

    [Header("닫기 아이콘 (선택사항)")]
    public GameObject closeIcon;     // 닫기 버튼 (UI Button)

    [Header("책 애니메이터 (선택사항)")]
    public Animator galleryAnimator; // 책 열림/닫힘 애니메이션 담당

    private bool isGalleryOpen = false;

    void Start()
    {
        if (galleryPanel != null)
            galleryPanel.SetActive(false);

        if (galleryAnimator != null)
            galleryAnimator.SetBool("IsOpen", false);
    }

    // === 버튼에서 호출할 함수 ===
    public void OpenGallery()
    {
        if (isGalleryOpen) return;
        isGalleryOpen = true;

        if (galleryPanel != null)
            galleryPanel.SetActive(true);

        if (galleryAnimator != null)
            galleryAnimator.SetBool("IsOpen", true);
    }

    public void CloseGallery()
    {
        if (!isGalleryOpen) return;
        isGalleryOpen = false;

        if (galleryAnimator != null)
            galleryAnimator.SetBool("IsOpen", false);

        // 닫힘 애니메이션 끝나면 패널 비활성화
        StartCoroutine(HideAfterDelay(0.5f));
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isGalleryOpen && galleryPanel != null)
            galleryPanel.SetActive(false);
    }
}
