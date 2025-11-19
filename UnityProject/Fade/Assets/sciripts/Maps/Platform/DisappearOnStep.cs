using UnityEngine;
using System.Collections;

public class DisappearOnStep : MonoBehaviour
{
    public float disappearDelay = 1f;
    public float respawnDelay = 2f;

    private Collider2D col;
    private SpriteRenderer sr;

    void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(DisappearRoutine());
    }

    IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(disappearDelay);

        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        sr.enabled = true;
        col.enabled = true;
    }
}
