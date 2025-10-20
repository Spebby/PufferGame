using UnityEngine;

public class PlayerScale : MonoBehaviour
{
    [SerializeField] private float _maxScale;
    [SerializeField] private float _minScale;

    void Update()
    {
        if (transform.localScale.x > _maxScale)
        {
            transform.localScale = new Vector3(1, 1, 0) * _maxScale;
        }
        if (transform.localScale.x < _minScale)
        {
            transform.localScale = new Vector3(1, 1, 0) * _minScale;
        }
    }

    public void IncreaseScale(float amount)
    {
        float increment = amount / 100.0f;
        transform.localScale += new Vector3(1, 1, 0) * increment;
    }
    
    public void DecreaseScale(float amount)
    {
        float increment = amount / 100.0f;
        transform.localScale -= new Vector3(1, 1, 0) * increment;
    }
}
