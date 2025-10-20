using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovementController : MonoBehaviour {
    Rigidbody2D _rigidBody2D;
    [SerializeField] InputActionReference moveAction;
    Vector2 _moveDirection2D;
    [SerializeField] float moveForce = 1f;

    bool _canMove;
    [SerializeField] float inAirGravity = 1f;

    public bool InWater { get; private set; }
    WaterPool _waterPool; // :(

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
        // fyi layer is better than comparing to tag.
        if (!collision.gameObject.CompareTag("Water Pool")) return;
        _rigidBody2D.gravityScale = 0f;
        _canMove                  = true;
        InWater                   = true;
        _waterPool                = collision.gameObject.GetComponentInParent<WaterPool>();
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Water Pool")) return;
        _rigidBody2D.gravityScale = inAirGravity;
        _canMove                  = false;
        InWater                   = false;
        _waterPool                = null;
    }

    public WaterPool GetWaterPool() {
        return _waterPool;
    }
}