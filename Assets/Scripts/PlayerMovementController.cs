using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
	private Rigidbody2D _rigidBody2D;
	[SerializeField] private InputActionReference _moveAction;
	private Vector2 _moveDirection2D;
	[SerializeField] private float _moveForce;

	private bool _canMove = false;
	[SerializeField] private float _inAirGravity = 1f;

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
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Water Pool"))
		{
			_rigidBody2D.gravityScale = _inAirGravity;
			_canMove = false;
		}
	}
}
