//using UnityEngine;
//using UnityEngine.InputSystem;

//[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
//public class Player : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float speed = 5f;

//    private Rigidbody2D rigid;
//    private SpriteRenderer spriter;
//    private Animator anim;

//    private Vector2 inputVec;

//    // Input System용
//    private PlayerControls controls;

//    void Awake()
//    {
//        rigid = GetComponent<Rigidbody2D>();
//        spriter = GetComponent<SpriteRenderer>();
//        anim = GetComponent<Animator>();

//        // 새 Input System 세팅
//        controls = new PlayerControls();
//    }

//    void OnEnable()
//    {
//        controls.Enable();
//        controls.Player.Move.performed += OnMove;
//        controls.Player.Move.canceled += OnMove;
//    }

//    void OnDisable()
//    {
//        controls.Player.Move.performed -= OnMove;
//        controls.Player.Move.canceled -= OnMove;
//        controls.Disable();
//    }

//    private void OnMove(InputAction.CallbackContext context)
//    {
//        inputVec = context.ReadValue<Vector2>();
//    }

//    void FixedUpdate()
//    {
//        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
//        rigid.MovePosition(rigid.position + nextVec);
//    }

//    void LateUpdate()
//    {
//        anim.SetFloat("Speed", inputVec.magnitude);

//        if (inputVec.x != 0)
//            spriter.flipX = inputVec.x < 0;
//    }
//}
