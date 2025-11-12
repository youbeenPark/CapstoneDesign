using UnityEngine;

public class Debugpref : MonoBehaviour
{
    [Header("개발 테스트용 초기화 옵션")]
    [Tooltip("true면 실행 시 PlayerPrefs 전체 초기화 (테스트 후 반드시 false로 바꾸세요!)")]
    public bool clearOnStart = true;  // 기본은 false

    [Tooltip("특정 키만 초기화할 경우 이름을 지정 (비워두면 전체 삭제)")]
    public string specificKey = "";

    void Start()
    {
        if (clearOnStart)
        {
            if (!string.IsNullOrEmpty(specificKey))
            {
                PlayerPrefs.DeleteKey(specificKey);
                Debug.Log($"🧹 PlayerPrefs 키 삭제: {specificKey}");
            }
            else
            {
                PlayerPrefs.DeleteAll();
                Debug.Log("🧹 PlayerPrefs 전체 초기화 완료!");
            }

            PlayerPrefs.Save();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var col in FindObjectsOfType<Collider2D>())
        {
            if (col.enabled)
            {
                Bounds b = col.bounds;
                Gizmos.DrawWireCube(b.center, b.size);
            }
        }
    }
}
