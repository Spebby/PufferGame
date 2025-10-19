using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class SpitAndDrink : MonoBehaviour {
    [SerializeField] InputActionReference spitAction;
	[SerializeField] GameObject waterBlobPrefab;
	[SerializeField] float spitForce = 10f;
	[SerializeField] float propulsionForce = 5f;

	[SerializeField] float maxChargeTime;
	Rigidbody2D _rigidBody2D;

	bool _isCharging;
	float _chargeTime;
	
	void Awake() {
		_rigidBody2D = GetComponent<Rigidbody2D>();
	}

	void OnEnable() {
		spitAction.action.Enable();
		spitAction.action.performed += Charge;
		spitAction.action.canceled  += Release;
	}

	void OnDisable() {
		spitAction.action.Disable();
		spitAction.action.performed -= Charge;
		spitAction.action.canceled  -= Release;
	}

	void FixedUpdate() {
		if (!_isCharging) return;
		_chargeTime+= Time.fixedDeltaTime;
	}

	void Charge(InputAction.CallbackContext context) {
		_chargeTime = 0f;
		_isCharging = true;
	}

	void Release(InputAction.CallbackContext context) {
		_isCharging = false;
		float finalCharge = Mathf.Clamp(_chargeTime, 0.1f, maxChargeTime) / maxChargeTime;
		Spit(finalCharge);
	}
	
	void Spit(float charge) {
		Vector2 aim = (Camera.main!.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position).normalized;
		GameObject waterBlobGameObject = Instantiate(waterBlobPrefab, transform.position, Quaternion.identity);
		waterBlobGameObject.GetComponent<Rigidbody2D>().AddForce(aim * spitForce * charge, ForceMode2D.Impulse);

		_rigidBody2D.AddForce(-aim * propulsionForce * charge, ForceMode2D.Impulse);
	}
}
