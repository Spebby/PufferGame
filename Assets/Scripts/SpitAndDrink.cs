using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpitAndDrink : MonoBehaviour
{
	[SerializeField] private InputActionReference _spitAction;
	[SerializeField] private InputActionReference _drinkAction;
	[SerializeField] private GameObject _waterBlobPrefab;
	private bool _isDrinking = false;
	[SerializeField] private float _waterAmount = 0.0f;
	[SerializeField] private float _waterIncrease = 1.0f;
	[SerializeField] private float _maxWater = 100.0f;
	private float timer = 0.0f;
	private float incrementDelay = 0.05f;
	[SerializeField] private float _spitForce = 5.0f;
	[SerializeField] private float _propulsionForce = 1.0f;
	private Rigidbody2D _rigidBody2D;
	private PlayerMovementController _player;
	private PlayerScale _scale;

	private void Awake()
	{
		_rigidBody2D = GetComponent<Rigidbody2D>();
		_player = GetComponent<PlayerMovementController>();
		_scale = GetComponent<PlayerScale>();
	}

    private void Update()
    {
		if (_isDrinking)
		{
			Drink();
		}
		if (_waterAmount > _maxWater)
        {
			_waterAmount = _maxWater;
        }
    }

    private void OnEnable()
	{
		_spitAction.action.Enable();
		_spitAction.action.performed += Spit;

		_drinkAction.action.Enable();
		_drinkAction.action.performed += ToggleDrink;
		_drinkAction.action.canceled += ToggleDrink;
	}
	private void OnDisable()
	{
		_spitAction.action.Disable();
		_spitAction.action.performed -= Spit;

		_drinkAction.action.Disable();
		_drinkAction.action.performed -= ToggleDrink;
		_drinkAction.action.canceled -= ToggleDrink;
	}

	private void Spit(InputAction.CallbackContext _)
	{
		Vector2 mousePosition2D = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		Vector2 spitDirection2D = (mousePosition2D - (Vector2)transform.position).normalized;

		GameObject waterBlobGameObject = Instantiate(_waterBlobPrefab, transform.position, Quaternion.identity);
		waterBlobGameObject.GetComponent<Rigidbody2D>().AddForce(spitDirection2D * _spitForce, ForceMode2D.Impulse);

		_rigidBody2D.AddForce(-spitDirection2D * _propulsionForce, ForceMode2D.Impulse);
	}

	private void ToggleDrink(InputAction.CallbackContext _)
	{
		_isDrinking = !_isDrinking;
	}
	
	private void Drink()
	{
		GameObject toDrink = _player.GetWaterPool();
		if (toDrink == null) return;
		if (toDrink.CompareTag("Water Pool") == false) return;
		WaterPool pool = toDrink.GetComponentInParent<WaterPool>();
		if (pool != null && _waterAmount < _maxWater && pool.HasWater() == true)
		{
			timer += Time.deltaTime;
			if (timer > incrementDelay)
			{
				_waterAmount += _waterIncrease * 5.0f;
				pool.ReduceVolume(_waterIncrease);
				_scale.IncreaseScale(_waterIncrease * 5.0f);
				timer = 0.0f;
				Debug.Log(_waterAmount);
			}
		}
		else
        {
			if (_waterAmount == _maxWater)
			{
				Debug.Log("Water limit reached");
			}
			else
            {
				Debug.Log("Pool not found");
            }
        }	
    }
}
