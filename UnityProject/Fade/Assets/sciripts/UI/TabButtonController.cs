using UnityEngine;
using UnityEngine.UI;

public class TabButtonController : MonoBehaviour
{
    public Image[] tabButtons; // 버튼들의 Image
    public Sprite normalSprite;
    public Sprite selectedSprite;

    public GameObject keyGuidePanel;
    public GameObject gameInfoPanel;
    public GameObject stageInfoPanel;
    public GameObject tipsPanel;

    public void OpenKeyGuide()
    {
        keyGuidePanel.SetActive(true);
        gameInfoPanel.SetActive(false);
        stageInfoPanel.SetActive(false);
        tipsPanel.SetActive(false);

        SetTab(0);
    }

    public void OpenGameInfo()
    {
        keyGuidePanel.SetActive(false);
        gameInfoPanel.SetActive(true);
        stageInfoPanel.SetActive(false);
        tipsPanel.SetActive(false);

        SetTab(1);
    }

    public void OpenStageInfo()
    {
        keyGuidePanel.SetActive(false);
        gameInfoPanel.SetActive(false);
        stageInfoPanel.SetActive(true);
        tipsPanel.SetActive(false);

        SetTab(2);
    }

    public void OpenTips()
    {
        keyGuidePanel.SetActive(false);
        gameInfoPanel.SetActive(false);
        stageInfoPanel.SetActive(false);
        tipsPanel.SetActive(true);

        SetTab(3);
    }

    private void SetTab(int index)
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].sprite = (i == index) ? selectedSprite : normalSprite;
        }
    }
}
