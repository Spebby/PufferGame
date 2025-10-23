using UnityEngine;


public class NoisyScaleChanger : MonoBehaviour {
    [SerializeField] float Amplitude;
    [SerializeField] float frequency;

    Vector2 _startingScale;
    float   _randomOffset;

    void Start() {
        _startingScale = transform.localScale;
        _randomOffset = Random.value * 100;
    }

    void Update() {
        float noise = (Mathf.PerlinNoise(Time.time * frequency, _randomOffset) - 0.5f) * 2;
        float scaleModifier = noise * Amplitude;
        transform.localScale = _startingScale + _startingScale * scaleModifier;
    }
}
