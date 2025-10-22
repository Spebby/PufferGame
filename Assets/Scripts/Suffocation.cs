using System.Collections;
using UnityEngine;


public class Suffocation : MonoBehaviour {
    Vector3 _startingPosition;
    PlayerMovementController _playerState;
    [SerializeField] int maxTime = 10;
    bool _isSuffocating;
    [SerializeField] private GameObject suffocationText;

    void Awake() {
        _startingPosition = transform.position;
        _playerState      = GetComponent<PlayerMovementController>();
        _isSuffocating    = false;
    }

    void FixedUpdate() {
        // this is indirectly a physics function whether you like it or not.
        if (!_playerState.InWater)
        {
            suffocationText.SetActive(true);
            StartCoroutine(Suffocate());
        }
        else
        {
            suffocationText.SetActive(false);
        }
    }

    void ReturnToStart() {
        transform.position = _startingPosition;
    }

    IEnumerator Suffocate() {
        if (_isSuffocating) yield break;
        _isSuffocating = true;
        float timer = maxTime;
        
        // Pre-condition means player *will* be in water on first frame. Skip the duplicate
        // function call for at least the first step.
        do {
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        } while (timer > 0 && !_playerState.InWater);

        _isSuffocating = false;
        if (timer <= 0f) { // death
            ReturnToStart();
        }
    }
}