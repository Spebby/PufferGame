using UnityEngine;
using UnityEngine.InputSystem;


public class SpitAndDrink : MonoBehaviour {
	PlayerMovementController player;
	PlayerScale scale;
	Rigidbody2D _rigidBody2D;
	
    [SerializeField] InputActionReference spitAction;
    [SerializeField] InputActionReference drinkAction;

    float _waterAmount;
    [SerializeField, Tooltip("Units per-second")] float waterGainRate = 5f;
    [SerializeField] float maxWater = 100f;
    
    [Header("Spitting Behaviour")]
	[SerializeField] GameObject waterBlobPrefab;
	[SerializeField] float spitForce = 10f;
	[SerializeField] float propulsionForce = 5f;

	[SerializeField] float maxChargeTime;

	bool _isCharging;
	float _chargeTime;

	[Header("Drinking Behaviour")]
	bool _isDrinking;
	
	
	void Awake() {
		player       = GetComponent<PlayerMovementController>();
		_rigidBody2D = GetComponent<Rigidbody2D>();
	}

	void Update() {
		if (!_isDrinking || _waterAmount >= maxWater) return;
		
		Drink();
		if (_waterAmount > maxWater) {
			_waterAmount = maxWater;
		}
	}

	void OnEnable() {
		spitAction.action.Enable();
		spitAction.action.performed += Charge;
		spitAction.action.canceled  += Release;

		drinkAction.action.Enable();
		drinkAction.action.performed += ToggleDrink;
		drinkAction.action.canceled  += ToggleDrink;
	}

	void OnDisable() {
		spitAction.action.Disable();
		spitAction.action.performed -= Charge;
		spitAction.action.canceled  -= Release;
		
		drinkAction.action.Disable();
		drinkAction.action.performed -= ToggleDrink;
		drinkAction.action.canceled  -= ToggleDrink;
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

	void ToggleDrink(InputAction.CallbackContext _) {
		_isDrinking = !_isDrinking;
	}

	void Drink() {
		WaterPool pool = player.GetWaterPool();
		if (pool && !pool.HasWater()) return;
		float amount = pool.ReduceVolume(waterGainRate * Time.deltaTime);
		_waterAmount += amount;
		scale.IncreaseScale(amount);
	}
}
