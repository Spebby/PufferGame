using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Suffocation : MonoBehaviour
{
    private Vector3 _startingPosition;
    private PlayerMovementController _playerState;
    private int timer;
    [SerializeField] private int _maxTime = 10;
    private bool _isSuffocating;

    private void Awake()
    {
        _startingPosition = transform.position;
        _playerState = GetComponent<PlayerMovementController>();
        timer = _maxTime;
        _isSuffocating = false;
    }

    void Update()
    {
        GameObject playerWaterState = _playerState.GetWaterPool();
        if (playerWaterState == null)
        {
            StartCoroutine(Suffocate());
        }
    }

    private void ReturnToStart()
    {
        transform.position = _startingPosition;
    }


    private IEnumerator Suffocate()
    {
        if (_isSuffocating) yield break;
        _isSuffocating = true;
        while (_playerState.GetWaterPool() == null)
        {
            yield return new WaitForSeconds(1f);
            
            timer--;
            if (timer <= 0)
            {
                timer = _maxTime;
                ReturnToStart();
            }
        }
        _isSuffocating = false;
    }
}
