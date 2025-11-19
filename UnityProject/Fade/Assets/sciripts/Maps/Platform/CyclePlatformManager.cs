using UnityEngine;
using System.Collections;

public class CyclePlatformManager : MonoBehaviour
{
    public GameObject[] platforms;
    public float showTime = 1f;

    void Start()
    {
        StartCoroutine(CycleRoutine());
    }

    IEnumerator CycleRoutine()
    {
        while (true)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                // 전체 끄고 현재만 켜기
                for (int j = 0; j < platforms.Length; j++)
                    platforms[j].SetActive(j == i);

                yield return new WaitForSeconds(showTime);
            }
        }
    }
}
