using UnityEngine;
using UnityEngine.InputSystem;

public class SpitAndDrink : MonoBehaviour
{
    [SerializeField] InputActionReference spitAction;
	[SerializeField] GameObject waterBlobPrefab;
	[SerializeField] float spitForce = 5.0f;
	[SerializeField] float propulsionForce = 1.0f;
    Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		spitAction.action.Enable();
		spitAction.action.performed += Spit;
	}
	private void OnDisable()
	{
		spitAction.action.Disable();
		spitAction.action.performed -= Spit;
	}

	private void Spit(InputAction.CallbackContext context)
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		Vector2 spitDirection = (mousePosition - (Vector2)transform.position).normalized;

		GameObject waterBlob = Instantiate(waterBlobPrefab, transform.position, Quaternion.identity);
		waterBlob.GetComponent<Rigidbody2D>().AddForce(spitDirection * spitForce, ForceMode2D.Impulse);

		rb.AddForce(-spitDirection * propulsionForce, ForceMode2D.Impulse);
	}
}
