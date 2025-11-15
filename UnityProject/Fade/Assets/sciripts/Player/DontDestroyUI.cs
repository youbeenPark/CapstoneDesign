using UnityEngine;

public class DontDestroyUI : MonoBehaviour
{
    private static DontDestroyUI instance;

    void Awake()
    {
        // 중복 생성 방지
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // ⭐ 핵심 포인트 ⭐
        // 이 스크립트를 HeartPanel이 아닌 Canvas에 붙이므로
        // gameObject(=Canvas)를 DontDestroyOnLoad 처리하면 됨.
        DontDestroyOnLoad(gameObject);
    }
}
