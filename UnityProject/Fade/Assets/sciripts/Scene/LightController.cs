using UnityEngine;
using UnityEngine.Rendering.Universal; // Light2D 사용 시 필요

public class LightController : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;  // Global Light 2D 오브젝트
    [SerializeField] private float fadeSpeed = 1.0f; // 밝아지는 속도
    private bool shouldFadeIn = false;

    private void Start()
    {
        if (globalLight != null)
            globalLight.intensity = 0f; // 처음엔 완전 어둡게
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
