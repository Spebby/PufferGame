using UnityEngine;

public class WaterPool : MonoBehaviour
{
	[Header("Size")]
	[SerializeField, Min(0f)] private float _width = 1f;
	[SerializeField, Min(0f)] private float _height = 1f;

	[Header("Volume")]
	[SerializeField, Min(0f)] private float _volume = 5f;
    [SerializeField] private float _volumeToHeightRatio = 0.2f;

	[Header("Dev")]
	[SerializeField] private Transform _spriteRootTransform;
	[SerializeField] private Transform _colliderTransform;
	[SerializeField, Min(0.1f)] private float _minColliderHeight = 0.1f;
	private float _prevHeight;
	private float _prevVolume;

	private void Reset()
	{
		_prevHeight = _height;
		_prevVolume = _volume;
	}

	private void OnValidate()
	{
		transform.localScale = Vector3.one;

		bool wasHeightChanged = !Mathf.Approximately(_height, _prevHeight);
		bool wasVolumeChanged = !Mathf.Approximately(_volume, _prevVolume);
		if (wasHeightChanged)
		{
			_volume = _height / _volumeToHeightRatio;
		}
		else if (wasVolumeChanged)
		{
			_height = _volume * _volumeToHeightRatio;
		}
		_prevHeight = _height;
		_prevVolume = _volume;

		_spriteRootTransform.localScale = new Vector3(_width, _height, 1f);
		_colliderTransform.localScale = new Vector3(_width, Mathf.Max(_minColliderHeight, _height), 1f);
	}

	public void AddVolume(float amount)
	{
		_volume += amount;
		SetHeight(_volume * _volumeToHeightRatio);
	}
	public void ReduceVolume(float amount)
	{
		if (_volume > 0)
		{
			_volume -= amount;
			SetHeight(_volume * _volumeToHeightRatio);
		}
	}
	public bool HasWater()
    {
		return _volume > 0;
    }

	private void SetHeight(float height)
	{
		_spriteRootTransform.localScale = new Vector3(_width, height, 1f);
		_colliderTransform.localScale = new Vector3(_width, Mathf.Max(_minColliderHeight, height), 1f);
	}

    void Update()
    {
        if (_volume < 0)
        {
			_volume = 0;
        }
    }
}
