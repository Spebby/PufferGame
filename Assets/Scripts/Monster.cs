using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class Monster : MonoBehaviour {
    [SerializeField] float turnRadius;
    [SerializeField] float speed;
    
    Transform _target;
    Vector3 _velocity;

    [Header("Audio")] 
    [SerializeField, FormerlySerializedAs("audio_Close")] AudioSource audioClose; 
    [SerializeField, FormerlySerializedAs("audio_Far")]   AudioSource audioFar; 
    [SerializeField] float roarInterval = 4.0f; 

    void Awake() {
        _target   = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        Vector2 dir = (_target.position - transform.position).normalized;
        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, desired);
        _velocity = Vector3.right * speed;
    }

    
    void Start() {
        StartCoroutine(RoarLoop());
    }

    void FixedUpdate() {
        Vector2 dir     = (_target.position - transform.position).normalized;
        float   desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float   current = transform.eulerAngles.z;

        float angleDiff = Mathf.DeltaAngle(current, desired);
        float maxDelta  = turnRadius * Time.fixedDeltaTime;
        float change    = Mathf.Clamp(angleDiff, -maxDelta, maxDelta);
        float newAngle  = current + change;

        transform.rotation = Quaternion.Euler(0, 0, newAngle);
        _velocity          = transform.right * speed;
        transform.position += _velocity * Time.fixedDeltaTime;
    }

    void PlayRoar() {
        audioClose.Play();
        audioFar.Play();
    }

    IEnumerator RoarLoop() {
        while (true) {
            yield return new WaitForSeconds(roarInterval);
            PlayRoar();
        }
        // ReSharper disable once IteratorNeverReturns
    }

    void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, _velocity + transform.position);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;  // ass
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
