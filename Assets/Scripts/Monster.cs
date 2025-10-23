using System;
using System.Collections; 
using UnityEngine;

public class Monster : MonoBehaviour {
    [SerializeField] float turnRadius;
    [SerializeField] float speed;
    
    Transform _target;
    Vector3 _velocity;

    [Header("Audio")] 
    [SerializeField] private AudioSource audio_Close; 
    [SerializeField] private AudioSource audio_Far; 
    [SerializeField] private float roarInterval = 4.0f; 

    void Awake() {
        _target   = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();
        Vector2 dir = (_target.position - transform.position).normalized;
        float desired = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, desired);
        _velocity = Vector3.right * speed;
    }

    
    void Start() 
    {
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

    public void PlayRoar() 
    {
        audio_Close.Play();
        audio_Far.Play();
    }

    private IEnumerator RoarLoop() 
    {
        while (true)
        {
            yield return new WaitForSeconds(roarInterval);
            PlayRoar();
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, _velocity + transform.position);
    }
}