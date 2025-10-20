using UnityEngine;

public class WaterBlob : MonoBehaviour {
	float _volume;
    public void Initialise(float volume) {
	    _volume = volume;
	    gameObject.transform.localScale *= volume;
    }
    
    void OnTriggerEnter2D(Collider2D collision) {
	    if (!collision.gameObject.CompareTag("Water Pool")) return;
	    collision.gameObject.GetComponentInParent<WaterPool>().AddVolume(_volume);
	    Destroy(gameObject);
    }
    // this is fine but it does not conserve mass. Not really possible for us to solve this unless we
    // actually simulate fluids
}
