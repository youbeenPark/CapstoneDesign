using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // PlayerHealth는 싱글톤으로 메인메뉴에서 이미 존재
            if (PlayerHealth.instance != null)
            {
                PlayerHealth.instance.Heal(healAmount);
            }

            Destroy(gameObject); // 먹으면 사라짐
        }
    }
}
