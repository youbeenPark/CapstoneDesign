using UnityEngine;
using System.Collections;

public class GroupPlatformSet : MonoBehaviour
{
    public GameObject[] platforms;
    public float activeTime = 2f;   // 등장 후 유지 시간
    public float inactiveTime = 2f; // 사라진 후 대기 시간

    void Start()
    {
        StartCoroutine(GroupRoutine());
    }

    IEnumerator GroupRoutine()
    {
        while (true)
        {
            // 등장
            foreach (var p in platforms)
                p.SetActive(true);

            yield return new WaitForSeconds(activeTime);

            // 사라짐
            foreach (var p in platforms)
                p.SetActive(false);

            yield return new WaitForSeconds(inactiveTime);
        }
    }
}
