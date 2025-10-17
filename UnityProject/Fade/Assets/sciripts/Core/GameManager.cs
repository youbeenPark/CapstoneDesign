using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject); // �� �Ѿ�� ����
    }

    // �ӽ�: ���� ���� �� Stage1�� �ڵ� �̵�
    void Start()
    {
        LoadScene("OR_01"); // �� �̸��� �װ� ���� �̸��� �����ؾ� ��
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
