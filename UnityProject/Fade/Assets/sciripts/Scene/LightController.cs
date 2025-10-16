using UnityEngine;
using UnityEngine.Rendering.Universal; // Light2D ��� �� �ʿ�

public class LightController : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;  // Global Light 2D ������Ʈ
    [SerializeField] private float fadeSpeed = 1.0f; // ������� �ӵ�
    private bool shouldFadeIn = false;

    private void Start()
    {
        if (globalLight != null)
            globalLight.intensity = 0f; // ó���� ���� ��Ӱ�
    }

    public void StartFadeIn()
    {
        shouldFadeIn = true;
    }

    private void Update()
    {
        if (shouldFadeIn && globalLight != null)
        {
            globalLight.intensity = Mathf.MoveTowards(globalLight.intensity, 1f, fadeSpeed * Time.deltaTime);
        }
    }
}
