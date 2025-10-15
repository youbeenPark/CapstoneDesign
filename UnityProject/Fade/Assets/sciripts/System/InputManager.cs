using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static PlayerControls Controls { get; private set; }

    private void Awake()
    {
        if (Controls == null)
            Controls = new PlayerControls();
    }

    private void OnEnable() => Controls.Enable();
    private void OnDisable() => Controls.Disable();
}
