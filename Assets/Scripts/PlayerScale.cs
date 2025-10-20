using UnityEngine;


public class PlayerScale : MonoBehaviour {
    [SerializeField] float _maxScale = 2f;
    [SerializeField] float _minScale = 1f;

    void Update() {
        float clampX = Mathf.Clamp(transform.localScale.x, _minScale, _maxScale);
        transform.localScale = new Vector3(1, 1, 0) * clampX;
    }


    public void ModifyScale(float delta) {
        float d = delta * 0.1f;
        transform.localScale = new Vector3(transform.localScale.x + d, transform.localScale.y + d, transform.localScale.z + d);
    }
}
