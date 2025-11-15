using UnityEngine;

public class HealItem : MonoBehaviour
{
    public int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = collision.GetComponent<PlayerHealth>();

            if (ph != null)
            {
                ph.Heal(healAmount);
            }

            Destroy(gameObject); // ∏‘¿∏∏È ªÁ∂Û¡¸
        }
    }
}
