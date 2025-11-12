using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToWorldMap : MonoBehaviour
{
    private void OnMouseDown()
    {
        // 마우스로 클릭했을 때 실행
        SceneManager.LoadScene("WorldMap");
    }
}
