using UnityEngine;

public class GallerySpriteController : MonoBehaviour
{
    public GameObject GalleryRoot;
    public Collider2D galleryIconCollider;
    public Collider2D closeIconCollider;
    public Animator galleryAnimator;   // ★ 추가됨 (Animator 연결)

    private bool isOpen = false;

    void Start()
    {
        GalleryRoot.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 갤러리 아이콘 클릭 → OPEN
            if (galleryIconCollider == Physics2D.OverlapPoint(mousePos))
            {
                OpenGallery();
            }

            // 닫기 아이콘 클릭 → CLOSE
            if (closeIconCollider == Physics2D.OverlapPoint(mousePos))
            {
                CloseGallery();
            }
        }
    }

    void OpenGallery()
    {
        GalleryRoot.SetActive(true);
        isOpen = true;

        // ★ 애니메이션 재생
        if (galleryAnimator != null)
        {
            galleryAnimator.SetTrigger("Open");
        }
    }

    void CloseGallery()
    {
        // 닫힐 때는 애니메이션 없이 그냥 꺼짐
        GalleryRoot.SetActive(false);
        isOpen = false;
    }
}
