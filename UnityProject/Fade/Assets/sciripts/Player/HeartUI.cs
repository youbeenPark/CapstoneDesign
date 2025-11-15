using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public void UpdateHearts(float currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            float value = currentHealth - i;

            if (value >= 1f)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (value >= 0.5f)
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
