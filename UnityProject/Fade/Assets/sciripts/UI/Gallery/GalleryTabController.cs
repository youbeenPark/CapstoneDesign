using UnityEngine;

public class GalleryTabController : MonoBehaviour
{
    [Header("탭 버튼 Colliders")]
    public Collider2D tab1Collider;
    public Collider2D tab2Collider;
    public Collider2D tab3Collider;

    [Header("페이지 오브젝트")]
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;

    private void Start()
    {
        ShowPage(1);  // 기본 Page1 켜두기
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (tab1Collider == Physics2D.OverlapPoint(mousePos))
                ShowPage(1);

            if (tab2Collider == Physics2D.OverlapPoint(mousePos))
                ShowPage(2);

            if (tab3Collider == Physics2D.OverlapPoint(mousePos))
                ShowPage(3);
        }
    }

    private void ShowPage(int index)
    {
        page1.SetActive(index == 1);
        page2.SetActive(index == 2);
        page3.SetActive(index == 3);
    }
}
