using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("TopDown Map Bounds")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4.5f;
    public float maxY = 4.5f;

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Animator anim;
    private PlayerControls controls;
    private Vector2 inputVec;
    private StageType stageType;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        controls = new PlayerControls();
    }

    void Start()
    {
        // StageManager에서 현재 스테이지 타입 가져오기
        stageType = StageManager.Instance != null ? StageManager.Instance.stageType : StageType.Platformer;

        if (stageType == StageType.TopDown)
        {
            // 중력 차단 및 회전 방지
            rigid.gravityScale = 0;
            rigid.freezeRotation = true;
        }
        else if (stageType == StageType.Platformer)
        {
            // 중력 복구
            rigid.gravityScale = 3f;
            rigid.freezeRotation = false;
        }
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
    }

    void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        inputVec = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0)
            spriter.flipX = inputVec.x < 0;

        // TopDown 맵일 때만 Clamp 적용
        if (stageType == StageType.TopDown)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            transform.position = pos;
        }
    }
}
