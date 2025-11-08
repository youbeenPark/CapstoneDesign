using UnityEngine;

public class DebugPref : MonoBehaviour
{
    void Start()
    {
        Debug.Log("현재 저장된 키 목록 확인");
        Debug.Log($"Unlocked_TUTO_Stage2 = {PlayerPrefs.GetInt("Unlocked_TUTO_Stage2", 0)}");
    }
}
