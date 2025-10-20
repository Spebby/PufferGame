using UnityEngine;


public class PlayerScale : MonoBehaviour {
    [SerializeField] float _maxScale = 2f;
    [SerializeField] float _minScale = 1f;

    void Update() {
        float clampX = Mathf.Clamp(transform.localScale.x, _minScale, _maxScale);
        transform.localScale = new Vector3(1, 1, 0) * clampX;
    }
    
    public void IncreaseScale(float amount) {
        float increment = amount / 100.0f;
        transform.localScale += new Vector3(1, 1, 0) * increment;
    }

    public void DecreaseScale(float amount) {
        float increment = amount / 100.0f;
        transform.localScale -= new Vector3(1, 1, 0) * increment;
    }
}
