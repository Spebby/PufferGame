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
    WaterPool _waterPool;

    AudioSource _audioSource; 
    
    [Header("Audio")]
    [SerializeField] AudioClip splashSound;
    [SerializeField] AudioClip movingInWaterSound;
    [SerializeField] AudioSource movementLoopSource; 

    void Awake() {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>(); 
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
        
        bool isMovingInWater = InWater && _moveDirection2D.sqrMagnitude > 0.01f;
        
        if (isMovingInWater && !movementLoopSource.isPlaying) { 
            movementLoopSource.clip = movingInWaterSound; 
            movementLoopSource.loop = true; 
            movementLoopSource.Play(); 
        } 
        else if (!isMovingInWater && movementLoopSource.isPlaying) { 
            movementLoopSource.Stop(); 
            movementLoopSource.loop = false; 
        }
        
        if (!_canMove) return;
        _rigidBody2D.AddForce(_moveDirection2D * moveForce);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Water Pool")) return;
        
        bool wasInAir = !InWater; 

        _rigidBody2D.gravityScale = 0f;
        _canMove                  = true;
        InWater                   = true;
        _waterPool                = collision.gameObject.GetComponentInParent<WaterPool>();

        if (wasInAir) {
            _audioSource.PlayOneShot(splashSound); 
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Water Pool")) return;
        _rigidBody2D.gravityScale = inAirGravity;
        _canMove                  = false;
        InWater                   = false;
        _waterPool                = null;
        
        if (movementLoopSource.isPlaying) { 
            movementLoopSource.Stop(); 
            movementLoopSource.loop = false; 
        }
    }

    public WaterPool GetWaterPool() {
        return _waterPool;
    }
}