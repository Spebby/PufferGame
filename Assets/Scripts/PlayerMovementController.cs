using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovementController : MonoBehaviour {
    Rigidbody2D _rigidBody2D;
    [SerializeField] InputActionReference moveAction;
    Vector2 _moveDirection2D;
    [SerializeField] float moveForce;

    bool _canMove;
    [SerializeField] float inAirGravity = 1f;

    public bool inWater { get; private set; }
    WaterPool waterPool; // :(

    void Awake() {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void OnEnable() {
        moveAction.action.Enable();
        moveAction.action.performed += OnMove;
        moveAction.action.canceled  += OnMove;
    }

    void OnDisable() {
        moveAction.action.Disable();
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled  -= OnMove;
    }

    void OnMove(InputAction.CallbackContext context) {
        _moveDirection2D = context.ReadValue<Vector2>();
    }

    void FixedUpdate() {
        if (!_canMove) return;
        _rigidBody2D.AddForce(_moveDirection2D * moveForce);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Water Pool")) return;
        _rigidBody2D.gravityScale = 0f;
        _canMove                  = true;
        inWater                   = true;
        waterPool                 = collision.gameObject.GetComponent<WaterPool>();
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Water Pool")) return;
        _rigidBody2D.gravityScale = inAirGravity;
        _canMove                  = false;
        inWater                   = false;
        waterPool                 = null; // why
    }

    public WaterPool GetWaterPool() {
        return waterPool;
    }
}