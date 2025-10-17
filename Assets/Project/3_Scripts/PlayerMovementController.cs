using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
	private Rigidbody2D _rigidBody2D;
	[SerializeField] private InputActionReference _moveAction;
	private Vector2 _moveDirection2D;
	[SerializeField] private float _moveForce;

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
		_rigidBody2D.AddForce(_moveDirection2D * _moveForce);
	}
}
