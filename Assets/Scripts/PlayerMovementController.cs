using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
	private Rigidbody2D _rigidBody2D;
	[SerializeField] private InputActionReference _moveAction;
	private Vector2 _moveDirection2D;
	[SerializeField] private float _moveForce;

	private bool _canMove = false;
	[SerializeField] private bool _inWater = false;
	[SerializeField] private float _inAirGravity = 1f;
	private GameObject _waterPool;

	private void Awake()
	{
		_rigidBody2D = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		_moveAction.action.Enable();
		_moveAction.action.performed += OnMove;
		_moveAction.action.canceled += OnMove;
	}
	private void OnDisable()
	{
		_moveAction.action.Disable();
		_moveAction.action.performed -= OnMove;
		_moveAction.action.canceled -= OnMove;
	}

	private void OnMove(InputAction.CallbackContext context)
	{
		_moveDirection2D = context.ReadValue<Vector2>();
	}

	private void FixedUpdate()
	{
		if (!_canMove)
		{
			return;
		}

		_rigidBody2D.AddForce(_moveDirection2D * _moveForce);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Water Pool"))
		{
			_rigidBody2D.gravityScale = 0f;
			_canMove = true;
			_inWater = true;
			_waterPool = collision.gameObject;
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Water Pool"))
		{
			_rigidBody2D.gravityScale = _inAirGravity;
			_canMove = false;
			_inWater = false;
			_waterPool = null;
		}
	}

	public bool isInWater()
	{
		return _inWater;
	}
	
	public GameObject getWaterPool()
    {
		return _waterPool;
    }
}
