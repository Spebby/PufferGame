using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class SpitAndDrink : MonoBehaviour {
	PlayerMovementController _player;
	PlayerScale _scale;
	Rigidbody2D _rigidBody2D;
	
    [SerializeField] InputActionReference spitAction;
    [SerializeField] InputActionReference drinkAction;

    [Header("Shared Behaviour")]
    float _waterAmount;
    [SerializeField] float maxWater = 100f;
    [SerializeField, Range(0f, 1f)] float startWaterRatio = 0.25f;
    
    [Header("Spitting Behaviour")]
	[SerializeField] GameObject waterBlobPrefab;
	[SerializeField] float spitForce = 10f;
	[SerializeField] float propulsionForce = 5f;
	[SerializeField] float spitCost = 0.1f;

	[SerializeField] float maxChargeTime;

	bool _isCharging;
	float _chargeTime;

	
	[Header("Drinking Behaviour")]
	[SerializeField, Tooltip("Units per-second")] float waterGainRate = 5f;
	bool _isDrinking;

	[Header("UI")]
	[SerializeField] Image chargeSlider;
	
	void Awake() {
		_player      = GetComponent<PlayerMovementController>();
		_scale       = GetComponent<PlayerScale>();
		_rigidBody2D = GetComponent<Rigidbody2D>();
		chargeSlider.fillAmount = 0f;
		_waterAmount = maxWater * startWaterRatio;
		
		// this is rigid but should work ok. the scale script is the main problem here.
		_scale.ModifyScale(_waterAmount);
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
		spitAction.action.performed += StartCharge;
		spitAction.action.canceled  += ReleaseCharge;

		drinkAction.action.Enable();
		drinkAction.action.performed += ToggleDrink;
		drinkAction.action.canceled  += ToggleDrink;
	}

	void OnDisable() {
		spitAction.action.Disable();
		spitAction.action.performed -= StartCharge;
		spitAction.action.canceled  -= ReleaseCharge;
		
		drinkAction.action.Disable();
		drinkAction.action.performed -= ToggleDrink;
		drinkAction.action.canceled  -= ToggleDrink;
	}

	void FixedUpdate() {
		if (!_isCharging) return;
		_chargeTime += Time.fixedDeltaTime;
		chargeSlider.fillAmount = _chargeTime / maxChargeTime;
	}

	void StartCharge(InputAction.CallbackContext context) {
		_isCharging             = true;
		_chargeTime             = 0f;
		chargeSlider.fillAmount = 0f;
	}

	void ReleaseCharge(InputAction.CallbackContext context) {
		if (!_isCharging) return;
		_isCharging = false;
		chargeSlider.fillAmount = 0;
		float finalCharge = Mathf.Clamp(_chargeTime, 0.1f, maxChargeTime) / maxChargeTime;
		Spit(finalCharge);
	}
	
	void Spit(float charge) {
		float cost = charge * maxWater * spitCost;
		if (_waterAmount < cost) return;
		
		Vector2 aim = (Camera.main!.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position).normalized;
		GameObject blob = Instantiate(waterBlobPrefab, transform.position + (Vector3)aim * gameObject.transform.localScale.x * 1f, Quaternion.identity);
		
		blob.GetComponent<WaterBlob>().Initialise(cost);
		blob.GetComponent<Rigidbody2D>().AddForce(aim * spitForce * charge, ForceMode2D.Impulse);

		_waterAmount -= cost;
		_rigidBody2D.AddForce(-aim * propulsionForce * charge, ForceMode2D.Impulse);
		_scale.ModifyScale(-cost);
	}

	void ToggleDrink(InputAction.CallbackContext _) {
		_isDrinking = !_isDrinking;
	}

	void Drink() {
		float cap = maxWater - _waterAmount;
		if (cap < Mathf.Epsilon) return;
		
		WaterPool pool = _player.GetWaterPool();
		// do a check on if player can actually drink more water
		if (!pool || !pool.HasWater()) return;
		float amount = pool.ReduceVolume(Mathf.Min(waterGainRate * Time.deltaTime, cap));
		_waterAmount += amount;
		_scale.ModifyScale(amount);
	}
}
