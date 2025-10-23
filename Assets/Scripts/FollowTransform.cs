using UnityEngine;


public class FollowTransform : MonoBehaviour {
    [SerializeField] Transform followTransform;
    [SerializeField] float followStrength;
    [SerializeField] float followDamp;
    [SerializeField] float cap = 1;

    Vector3 _velocity;

    void OnEnable() {
        if (followTransform != transform.parent) return;
        transform.SetParent(null);
    }

    void Update() {
        if (!followTransform) {
            Destroy(gameObject);
            return;
        }
        
        if (!followTransform.gameObject.activeSelf) {
            transform.SetParent(followTransform);
        }

        _velocity += (-followStrength * (transform.position - followTransform.position) + -followDamp * _velocity) * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;

        if (!(Vector2.Distance(transform.position, followTransform.position) > cap)) return;
        Vector2 relativePos = transform.position - followTransform.position;
        transform.position = (Vector2)followTransform.position + (relativePos.normalized * cap);
    }
}
