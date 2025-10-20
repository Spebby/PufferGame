using System.Runtime.CompilerServices;
using UnityEngine;

public class WaterPool : MonoBehaviour {
	[Header("Size")]
	[SerializeField, Min(0f)] float _width = 1f;
	[SerializeField, Min(0f)] float _height = 1f;

	[Header("Volume")]
	[SerializeField, Min(0f)] float _volume = 5f;
    [SerializeField] float _volumeToHeightRatio = 0.2f;
    // I assume that vTHR ratio 0.2 means 1 volume == 20% full?

	[Header("Dev")]
	[SerializeField]
	Transform _spriteRootTransform;
	[SerializeField] Transform _colliderTransform;
	[SerializeField, Min(0.1f)] float _minColliderHeight = 0.1f;
	float _prevHeight;
	float _prevVolume;

	void Reset() {
		_prevHeight = _height;
		_prevVolume = _volume;
	}

	void OnValidate() {
		transform.localScale = Vector3.one;

		bool wasHeightChanged = !Mathf.Approximately(_height, _prevHeight);
		bool wasVolumeChanged = !Mathf.Approximately(_volume, _prevVolume);
		if (wasHeightChanged) {
			_volume = _height / _volumeToHeightRatio;
		} else if (wasVolumeChanged) {
			_height = _volume * _volumeToHeightRatio;
		}
		_prevHeight = _height;
		_prevVolume = _volume;

		_spriteRootTransform.localScale = new Vector3(_width, _height, 1f);
		_colliderTransform.localScale = new Vector3(_width, Mathf.Max(_minColliderHeight, _height), 1f);
	}
	
	public bool HasWater(float amount = 0) => _volume >= amount;

	public void AddVolume(float delta) {
		_volume += delta;
		SetSpriteHeight(_volume * _volumeToHeightRatio);
	}

	public float ReduceVolume(float delta) {
		float reduction = Mathf.Min(delta, _volume);
		_volume -= reduction;
		SetSpriteHeight(_volume * _volumeToHeightRatio);
		return reduction;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	void SetSpriteHeight(float height) {
		_spriteRootTransform.localScale = new Vector3(_width, height, 1f);
		_colliderTransform.localScale   = new Vector3(_width, Mathf.Max(_minColliderHeight, height), 1f);
	}
}
