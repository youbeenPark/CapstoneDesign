using UnityEngine;

public class HandleMove : MonoBehaviour
{
    public RectTransform handle;
    public Vector2 leftPos;
    public Vector2 rightPos;

    public void Move(bool isRight)
    {
        Debug.Log($"[HandleMove] Move() 호출됨 / isRight = {isRight}");
        Debug.Log($"[HandleMove] LeftPos = {leftPos}, RightPos = {rightPos}");

        Vector2 target = isRight ? rightPos : leftPos;
        Debug.Log($"[HandleMove] Target Position = {target}");

        handle.anchoredPosition = target;

        Debug.Log($"[HandleMove] 적용된 Handle Position = {handle.anchoredPosition}");
    }
}
