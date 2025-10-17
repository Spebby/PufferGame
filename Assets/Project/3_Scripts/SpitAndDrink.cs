using UnityEngine;
using UnityEngine.InputSystem;

public class SpitAndDrink : MonoBehaviour
{
    [SerializeField] private InputActionReference _spitAction;
	[SerializeField] private GameObject _waterBlobPrefab;
	[SerializeField] private float _spitForce = 5.0f;
	[SerializeField] private float _propulsionForce = 1.0f;
    private Rigidbody2D _rigidBody2D;

	private void Awake()
	{
		_rigidBody2D = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		_spitAction.action.Enable();
		_spitAction.action.performed += Spit;
	}
	private void OnDisable()
	{
		_spitAction.action.Disable();
		_spitAction.action.performed -= Spit;
	}

	private void Spit(InputAction.CallbackContext _)
	{
		Vector2 mousePosition2D = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		Vector2 spitDirection2D = (mousePosition2D - (Vector2)transform.position).normalized;

		GameObject waterBlobGameObject = Instantiate(_waterBlobPrefab, transform.position, Quaternion.identity);
		waterBlobGameObject.GetComponent<Rigidbody2D>().AddForce(spitDirection2D * _spitForce, ForceMode2D.Impulse);

		_rigidBody2D.AddForce(-spitDirection2D * _propulsionForce, ForceMode2D.Impulse);
	}
}
