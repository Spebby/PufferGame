using UnityEngine;

public class WaterBlob : MonoBehaviour
{
    [SerializeField] float volume = 1.0f;

    void OnTriggerEnter2D(Collider2D collision) {
	    if (!collision.gameObject.CompareTag("Water Pool")) return;
	    collision.gameObject.GetComponentInParent<WaterPool>().AddVolume(volume);
	    Destroy(gameObject);
    }
    // this is fine but it does not conserve mass. Not really possible for us to solve this unless we
    // actually simulate fluids
}
