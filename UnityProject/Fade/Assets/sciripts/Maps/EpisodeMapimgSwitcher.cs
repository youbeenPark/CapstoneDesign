using UnityEngine;
using UnityEngine.UI;

public class EpisodeMapImageSwitcher : MonoBehaviour
{
    [Header("Stage Name (Cleared Key 기준)")]
    public string episodeName;  // 예: "TUTO_Stage1", "GR_Stage1"

    [Header("Sprites")]
    public Sprite defaultSprite;   // 클리어 전 이미지
    public Sprite clearedSprite;   // 클리어 후 이미지

    private SpriteRenderer img;

    private void Awake()
    {
        img = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        UpdateMapImage();
    }

    public void UpdateMapImage()
    {
        // 월드맵과 동일한 PlayerPrefs 규칙 사용
        //bool isCleared = PlayerPrefs.GetInt("Cleared_" + stageName, 0) == 1;
        bool isCleared = PlayerPrefs.GetInt("Cleared_" + episodeName, 0) == 1;

        if (isCleared)
            img.sprite = clearedSprite;
        else
            img.sprite = defaultSprite;

        Debug.Log($"[EpisodeMap] {episodeName} Cleared={isCleared}, 이미지 변경됨");
    }
}
