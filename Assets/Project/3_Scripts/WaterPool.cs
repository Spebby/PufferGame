using UnityEngine;

public class WaterPool : MonoBehaviour
{
    [SerializeField] float volume = 5.0f;
    [SerializeField] float volumeToScaleRatio = 0.2f;

    public void AddVolume(float amount)
    {
        volume += amount;
        transform.localScale += new Vector3(0, amount * volumeToScaleRatio, 0);
    }
}
