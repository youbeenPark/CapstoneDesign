using UnityEngine;
using System.Collections;

public class GallerySpriteController : MonoBehaviour
{
    [Header("연결할 UI 패널 (책 UI)")]
    public GameObject galleryPanel;  // Canvas 안의 GalleryPanel

    [Header("닫기 아이콘 (선택사항)")]
    public GameObject closeIcon;     // 닫기용 스프라이트 (없으면 null)

    [Header("책 애니메이터 (선택사항)")]
    public Animator galleryAnimator; // 책 열림/닫힘 애니메이션 담당

    private bool isGalleryOpen = false;

    void Start()
    {
        // 처음엔 갤러리 숨김
        if (galleryPanel != null)
            galleryPanel.SetActive(false);
    }

    void Update()
    {
        // 마우스 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            // 갤러리 아이콘 클릭
            if (hit != null && hit.gameObject == gameObject)
            {
                OpenGallery();
            }

            // 닫기 아이콘 클릭
            if (closeIcon != null && hit != null && hit.gameObject == closeIcon)
            {
                CloseGallery();
            }
        }
    }

    void OpenGallery()
    {
        if (isGalleryOpen) return;

        isGalleryOpen = true;

        // 패널 활성화
        if (galleryPanel != null)
            galleryPanel.SetActive(true);

        // 애니메이션 실행
        if (galleryAnimator != null)
            galleryAnimator.SetBool("IsOpen", true);
    }

    void CloseGallery()
    {
        if (!isGalleryOpen) return;

        isGalleryOpen = false;

        // 닫힘 애니메이션 실행
        if (galleryAnimator != null)
            galleryAnimator.SetBool("IsOpen", false);

        // 닫히는 애니메이션 끝난 뒤 패널 비활성화
        StartCoroutine(HideAfterDelay(0.3f));
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (galleryPanel != null && !isGalleryOpen)
            galleryPanel.SetActive(false);
    }
}
