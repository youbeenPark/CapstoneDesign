// Assets/Scripts/UI/MainMenuUIController.cs

using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    public GameObject MainMenuPanel;

    public static MainMenuUIController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (MainMenuPanel != null)
        {
            MainMenuPanel.SetActive(false);
        }
    }

    public void ShowUIOnLanding()
    {
        if (MainMenuPanel != null)
        {
            if (!MainMenuPanel.activeSelf)
            {
                MainMenuPanel.SetActive(true);
            }
        }
    }
}