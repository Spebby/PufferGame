using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] float panSpeed = 2f;
    
    GameObject _ref;
    
    void Awake() {
        _ref = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Update() {
        Vector3 target = Vector3.Lerp(transform.position, _ref.transform.position, panSpeed * Time.deltaTime);
        target.z = transform.position.z;
        transform.position = target;
    }
}
